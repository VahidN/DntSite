using Lucene.Net.Analysis;
using Lucene.Net.Analysis.CharFilters;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.Util;
using Lucene.Net.Util;

namespace DntSite.Web.Features.Searches.Utils;

public class LowerCaseHtmlStripAnalyzer(LuceneVersion matchVersion) : Analyzer
{
    private HTMLStripCharFilter? _htmlStripCharFilter;
    private bool _isDisposed;
    private LowerCaseFilter? _lowerCaseFilter;
    private StandardFilter? _standardFilter;
    private StandardTokenizer? _standardTokenizer;
    private StopFilter? _tokenStream;

    protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader)
    {
        DisposeTokenStreamComponents();
#pragma warning disable IDISP003
        _standardTokenizer = new StandardTokenizer(matchVersion, reader);
        _standardFilter = new StandardFilter(matchVersion, _standardTokenizer);
        _lowerCaseFilter = new LowerCaseFilter(matchVersion, _standardFilter);
        var stopWords = new CharArraySet(matchVersion, PersianStopwords.List, ignoreCase: true);
        _tokenStream = new StopFilter(matchVersion, _lowerCaseFilter, stopWords);
#pragma warning restore IDISP003
        return new TokenStreamComponents(_standardTokenizer, _tokenStream);
    }

    private void DisposeTokenStreamComponents()
    {
        _standardTokenizer?.Dispose();
        _standardFilter?.Dispose();
        _lowerCaseFilter?.Dispose();
        _tokenStream?.Dispose();
    }

    protected override TextReader InitReader(string fieldName, TextReader reader)
    {
        _htmlStripCharFilter?.Dispose();
        _htmlStripCharFilter = new HTMLStripCharFilter(reader);

        return base.InitReader(fieldName, _htmlStripCharFilter);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (_isDisposed)
        {
            return;
        }

        try
        {
            if (disposing)
            {
                DisposeTokenStreamComponents();
                _htmlStripCharFilter?.Dispose();
            }
        }
        finally
        {
            _isDisposed = true;
        }
    }
}
