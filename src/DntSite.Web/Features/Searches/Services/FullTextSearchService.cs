using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.RssFeeds.Models;
using DntSite.Web.Features.Searches.Models;
using DntSite.Web.Features.Searches.ModelsMappings;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.Searches.Utils;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.Miscellaneous;
using Lucene.Net.Index;
using Lucene.Net.Queries.Mlt;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Directory = System.IO.Directory;

namespace DntSite.Web.Features.Searches.Services;

public class FullTextSearchService : IFullTextSearchService
{
    private const LuceneVersion LuceneVersion = Lucene.Net.Util.LuceneVersion.LUCENE_48;

    private static readonly string[] DefaultMoreLikeThisFieldNames =
    [
        nameof(WhatsNewItemModel.Content), nameof(LuceneDocumentMapper.IndexedTitle),
        nameof(WhatsNewItemModel.Categories)
    ];

    private static readonly string[] DefaultSearchFieldNames =
    [
        ..DefaultMoreLikeThisFieldNames, nameof(WhatsNewItemModel.AuthorName)
    ];

    private readonly IAppAntiXssService _antiXssService;
    private readonly IAppFoldersService _appFoldersService;
    private readonly ILockerService _lockerService;
    private readonly ILogger<FullTextSearchService> _logger;

    private Analyzer? _analyzer;
    private FSDirectory? _fsDirectory;

    //  IndexWriter instances are completely thread safe, meaning multiple threads can call any of its methods, concurrently.
    private IndexWriter? _indexWriter;

    private bool _isDisposed;
    private KeywordAnalyzer? _keywordAnalyzer;
    private LowerCaseHtmlStripAnalyzer? _lowerCaseHtmlStripAnalyzer;

    // Safely shares IndexSearcher instances across multiple threads, while periodically reopening.
    private SearcherManager? _searcherManager;

    public FullTextSearchService(IAppFoldersService appFoldersService,
        IAppAntiXssService antiXssService,
        ILogger<FullTextSearchService> logger,
        ILockerService lockerService)
    {
        _appFoldersService = appFoldersService ?? throw new ArgumentNullException(nameof(appFoldersService));
        _antiXssService = antiXssService;
        _logger = logger;
        _lockerService = lockerService;
        InitializeSearchService();
    }

    private IndexWriter FtsIndexWrite
    {
        get
        {
            if (_indexWriter is not null)
            {
                return _indexWriter;
            }

            InitializeSearchService();

            return _indexWriter;
        }
    }

    private SearcherManager FtsSearcherManager
    {
        get
        {
            if (_searcherManager is not null)
            {
                return _searcherManager;
            }

            InitializeSearchService();

            return _searcherManager;
        }
    }

    public bool IsDatabaseIndexed => GetNumberOfDocuments() > 0;

    public void DeleteOldIndexFiles()
    {
        using var @lock = _lockerService.Lock<FullTextSearchService>();

        try
        {
            TryUnlockDirectory();
            CloseSearchService();

            foreach (var file in Directory.GetFiles(_appFoldersService.LuceneIndexFolderPath, searchPattern: "*.*"))
            {
                File.Delete(file);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Demystify(), message: "DeleteOldIndexFiles Error");
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public void AddOrUpdateLuceneDocument(WhatsNewItemModel? item)
    {
        if (item is null)
        {
            return;
        }

        using var @lock = _lockerService.Lock<FullTextSearchService>();

        try
        {
            AddOrUpdatePost(item);

            FtsIndexWrite.Flush(triggerMerge: true, applyAllDeletes: true);
            FtsIndexWrite.Commit();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Demystify(), message: "AddOrUpdateLuceneEntryItem Error");
        }
    }

    public async Task IndexTableAsync(IEnumerable<WhatsNewItemModel>? items, bool commitChanges = true)
    {
        if (items is null)
        {
            return;
        }

        try
        {
            var count = 0;

            foreach (var post in items)
            {
                AddOrUpdatePost(post);
                count++;

                if (count % 40 == 0)
                {
                    // To reduce the CPU usage. We don't want to get suspended!
                    await Task.Delay(TimeSpan.FromSeconds(value: 2));
                }
            }

            if (commitChanges)
            {
                using var @lock = await _lockerService.LockAsync<FullTextSearchService>();
                FtsIndexWrite.Flush(triggerMerge: true, applyAllDeletes: true);
                FtsIndexWrite.Commit();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Demystify(), message: "IndexDatabase Error");
        }
    }

    public void CommitChanges()
    {
        using var @lock = _lockerService.Lock<FullTextSearchService>();

        try
        {
            FtsIndexWrite.Flush(triggerMerge: true, applyAllDeletes: true);
            FtsIndexWrite.Commit();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Demystify(), message: "IndexDatabase Error");
        }
    }

    public PagedResultModel<LuceneSearchResult> FindPagedSimilarPosts(string documentTypeIdHash,
        int maxItems,
        int pageNumber,
        int pageSize,
        params string[]? moreLikeTheseFieldNames)
    {
        using var @lock = _lockerService.Lock<FullTextSearchService>();

        try
        {
            var docId = FindLuceneDocument(documentTypeIdHash)?.LuceneDocId;

            if (!docId.HasValue)
            {
                return new PagedResultModel<LuceneSearchResult>();
            }

            if (moreLikeTheseFieldNames is null || moreLikeTheseFieldNames.Length == 0)
            {
                moreLikeTheseFieldNames = DefaultMoreLikeThisFieldNames;
            }

            return DoSearch(indexSearcher =>
            {
                var query = new MoreLikeThis(indexSearcher.IndexReader)
                {
                    Analyzer = _analyzer,
                    ApplyBoost = true,
                    FieldNames = moreLikeTheseFieldNames,
                    MinDocFreq = 1,
                    MinTermFreq = 1
                }.Like(docId.Value);

                return DoQuery(query, parser: null, searchText: null, maxItems, pageNumber, pageSize,
                    convertToLowercase: true);
            }, new PagedResultModel<LuceneSearchResult>());
        }
        catch (FileNotFoundException)
        {
            // It's not indexed yet.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Demystify(), message: "FindSimilarPosts({Hash})", documentTypeIdHash);
        }

        return new PagedResultModel<LuceneSearchResult>();
    }

    public PagedResultModel<LuceneSearchResult> FindPagedPosts(string? searchText,
        int maxItems,
        int pageNumber,
        int pageSize,
        params string[]? searchInTheseFieldNames)
    {
        searchText = searchText?.Trim();

        if (string.IsNullOrWhiteSpace(searchText))
        {
            return new PagedResultModel<LuceneSearchResult>();
        }

        using var @lock = _lockerService.Lock<FullTextSearchService>();

        try
        {
            if (searchInTheseFieldNames is null || searchInTheseFieldNames.Length == 0)
            {
                searchInTheseFieldNames = DefaultSearchFieldNames;
            }

            var parser = new MultiFieldQueryParser(LuceneVersion, searchInTheseFieldNames, _analyzer);

            var query = parser.ParseQuery(searchText, convertToLowercase: true);

            return DoQuery(query, parser, searchText, maxItems, pageNumber, pageSize, convertToLowercase: true);
        }
        catch (FileNotFoundException)
        {
            // It's not indexed yet.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Demystify(), message: "FindPagedPosts({Terms})",
                _antiXssService.GetSanitizedHtml(searchText));
        }

        return new PagedResultModel<LuceneSearchResult>();
    }

    public int GetNumberOfDocuments() => DoSearch(indexSearcher => indexSearcher.IndexReader.NumDocs, defaultValue: 0);

    public PagedResultModel<LuceneSearchResult> GetAllPagedPosts(int pageNumber,
        int pageSize,
        string sortField,
        bool isDescending)
    {
        using var @lock = _lockerService.Lock<FullTextSearchService>();

        var query = new MatchAllDocsQuery();
        var maxItems = GetNumberOfDocuments();

        if (maxItems == 0)
        {
            return new PagedResultModel<LuceneSearchResult>();
        }

        return DoSearch(indexSearcher =>
        {
            var sort = new Sort(new SortField(sortField, SortFieldType.STRING, isDescending));
            var hits = indexSearcher.Search(query, maxItems, sort);

            var scoreNorm = 100.0f / hits.MaxScore;
            var startIndex = (pageNumber - 1) * pageSize;

            var results = hits.ScoreDocs.Skip(startIndex)
                .Take(pageSize)
                .Select(scoreDoc => indexSearcher.Doc(scoreDoc.Doc)
                    .MapToLuceneSearchResult(scoreDoc.Doc, scoreDoc.Score, scoreNorm))
                .ToList();

            return new PagedResultModel<LuceneSearchResult>
            {
                Data = results,
                TotalItems = hits.TotalHits
            };
        }, new PagedResultModel<LuceneSearchResult>());
    }

    public LuceneSearchResult? FindLuceneDocument(string? documentTypeIdHash)
    {
        if (string.IsNullOrEmpty(documentTypeIdHash))
        {
            return null;
        }

        documentTypeIdHash = documentTypeIdHash.ToUpperInvariant();

        return DoSearch(indexSearcher =>
        {
            var parser = new QueryParser(LuceneVersion, nameof(WhatsNewItemModel.DocumentTypeIdHash), _analyzer);
            var query = parser.ParseQuery(documentTypeIdHash, convertToLowercase: false);
            var hits = indexSearcher.Search(query, n: 1);

            if (hits.TotalHits == 0)
            {
                return null;
            }

            var scoreDoc = hits.ScoreDocs[0];
            var scoreNorm = 100.0f / hits.MaxScore;

            return indexSearcher.Doc(scoreDoc.Doc).MapToLuceneSearchResult(scoreDoc.Doc, scoreDoc.Score, scoreNorm);
        }, defaultValue: null);
    }

    public void DeleteLuceneDocument(string? documentTypeIdHash)
    {
        if (string.IsNullOrEmpty(documentTypeIdHash))
        {
            return;
        }

        using var @lock = _lockerService.Lock<FullTextSearchService>();

        FtsIndexWrite.DeleteDocuments(new Term(nameof(WhatsNewItemModel.DocumentTypeIdHash), documentTypeIdHash));

        FtsIndexWrite.Flush(triggerMerge: true, applyAllDeletes: true);
        FtsIndexWrite.Commit();
    }

    [MemberNotNull(nameof(_indexWriter), nameof(_searcherManager))]
    private void InitializeSearchService()
    {
        using var @lock = _lockerService.Lock<FullTextSearchService>();

        CloseSearchService();

        _keywordAnalyzer = new KeywordAnalyzer();

        _lowerCaseHtmlStripAnalyzer = new LowerCaseHtmlStripAnalyzer(LuceneVersion);

        _analyzer = new PerFieldAnalyzerWrapper(_lowerCaseHtmlStripAnalyzer, new Dictionary<string, Analyzer>
        {
            // Document StringField instances are sort of keywords, they are not analyzed, they indexed as is (in its original case).
            // But StandardAnalyzer applies lower case filter to a query.
            // We can fix this by using KeywordAnalyzer with our query parser.
            {
                nameof(WhatsNewItemModel.Id), _keywordAnalyzer
            },
            {
                nameof(WhatsNewItemModel.DocumentTypeIdHash), _keywordAnalyzer
            },
            {
                nameof(WhatsNewItemModel.DocumentContentHash), _keywordAnalyzer
            }
        });

        _fsDirectory = FSDirectory.Open(_appFoldersService.LuceneIndexFolderPath);
        TryUnlockDirectory();
        _indexWriter = new IndexWriter(_fsDirectory, new IndexWriterConfig(LuceneVersion, _analyzer));
        _searcherManager = new SearcherManager(_indexWriter, applyAllDeletes: true, searcherFactory: null);
    }

    private void TryUnlockDirectory()
    {
        if (_fsDirectory is null)
        {
            return;
        }

        if (IndexWriter.IsLocked(_fsDirectory))
        {
            IndexWriter.Unlock(_fsDirectory);
        }
    }

    private TResult DoSearch<TResult>(Func<IndexSearcher, TResult> action, TResult defaultValue)
    {
        FtsSearcherManager.MaybeRefreshBlocking();
        var indexSearcher = FtsSearcherManager.Acquire();

        try
        {
            return action(indexSearcher);
        }
        catch (FileNotFoundException)
        {
            // It's not indexed yet.
            return defaultValue;
        }
        finally
        {
            FtsSearcherManager.Release(indexSearcher);
        }
    }

    private void AddOrUpdatePost(WhatsNewItemModel post)
    {
        var doc = FindLuceneDocument(post.DocumentTypeIdHash);

        if (doc is null)
        {
            FtsIndexWrite.AddDocument(post.MapToLuceneDocument());
        }
        else
        {
            if (string.Equals(doc.DocumentContentHash, post.DocumentContentHash, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            FtsIndexWrite.UpdateDocument(
                new Term(nameof(WhatsNewItemModel.DocumentTypeIdHash), post.DocumentTypeIdHash),
                post.MapToLuceneDocument());
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        using var @lock = _lockerService.Lock<FullTextSearchService>();

        if (_isDisposed)
        {
            return;
        }

        try
        {
            if (!disposing)
            {
                return;
            }

            CloseSearchService();
        }
        finally
        {
            _isDisposed = true;
        }
    }

    private void CloseSearchService()
    {
        TryDispose(_lowerCaseHtmlStripAnalyzer);
        TryDispose(_keywordAnalyzer);
        TryDispose(_analyzer);
        TryDispose(_fsDirectory);
        TryDispose(_indexWriter);
        TryDispose(_searcherManager);
    }

    private void TryDispose(IDisposable? iDisposable)
    {
        try
        {
            iDisposable?.Dispose();
            iDisposable = null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Demystify(), message: "TryDispose Error");
        }
    }

    private PagedResultModel<LuceneSearchResult> DoQuery(Query query,
        QueryParser? parser,
        string? searchText,
        int maxItems,
        int pageNumber,
        int pageSize,
        bool convertToLowercase)
        => DoSearch(indexSearcher =>
        {
            var collector = TopScoreDocCollector.Create(maxItems, docsScoredInOrder: true);
            indexSearcher.Search(query, collector);
            var startIndex = (pageNumber - 1) * pageSize;
            var docs = collector.GetTopDocs(startIndex, pageSize);

            if (docs.TotalHits == 0 && !string.IsNullOrWhiteSpace(searchText) && parser is not null)
            {
                string[] matchingTypes = ["*", "~"];

                foreach (var matchingType in matchingTypes)
                {
                    query = parser.ParseQuery(searchText.SearchByPartialWords(matchingType, convertToLowercase),
                        convertToLowercase);

                    collector = TopScoreDocCollector.Create(maxItems, docsScoredInOrder: true);
                    indexSearcher.Search(query, collector);
                    docs = collector.GetTopDocs(startIndex, pageSize);

                    if (docs.TotalHits > 0)
                    {
                        break;
                    }
                }
            }

            if (docs.TotalHits == 0)
            {
                return new PagedResultModel<LuceneSearchResult>();
            }

            var scoreNorm = 100.0f / docs.MaxScore;

            var results = docs.ScoreDocs.Select(doc
                    => indexSearcher.Doc(doc.Doc).MapToLuceneSearchResult(doc.Doc, doc.Score, scoreNorm))
                .ToList();

            return new PagedResultModel<LuceneSearchResult>
            {
                Data = results,
                TotalItems = docs.TotalHits > maxItems ? maxItems : docs.TotalHits
            };
        }, new PagedResultModel<LuceneSearchResult>());
}
