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
        _standardTokenizer = new StandardTokenizer(matchVersion, reader);
        _standardFilter = new StandardFilter(matchVersion, _standardTokenizer);
        _lowerCaseFilter = new LowerCaseFilter(matchVersion, _standardFilter);
        var stopWords = new CharArraySet(matchVersion, PersianStopwords.List, ignoreCase: true);
        _tokenStream = new StopFilter(matchVersion, _lowerCaseFilter, stopWords);

        return new TokenStreamComponents(_standardTokenizer, _tokenStream);
    }

    protected override TextReader InitReader(string fieldName, TextReader reader)
    {
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
            if (!disposing)
            {
                return;
            }

            _standardFilter?.Dispose();
            _standardTokenizer?.Dispose();
            _tokenStream?.Dispose();
            _lowerCaseFilter?.Dispose();
            _htmlStripCharFilter?.Dispose();
        }
        finally
        {
            _isDisposed = true;
        }
    }
}
