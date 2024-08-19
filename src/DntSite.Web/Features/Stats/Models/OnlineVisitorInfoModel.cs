using UAParser;

namespace DntSite.Web.Features.Stats.Models;

public class OnlineVisitorInfoModel
{
    public required string Ip { set; get; }

    public required string ReferrerUrl { set; get; }

    public string? ReferrerUrlTitle { set; get; }

    public bool HasMissingReferrerUrlTitle => !string.IsNullOrWhiteSpace(ReferrerUrl) &&
                                              (string.IsNullOrWhiteSpace(ReferrerUrlTitle) ||
                                               string.Equals(ReferrerUrlTitle, ReferrerUrl,
                                                   StringComparison.OrdinalIgnoreCase));

    public DateTime VisitTime { set; get; }

    public bool IsSpider { set; get; }

    public required string UserAgent { set; get; }

    public ClientInfo? ClientInfo { set; get; }

    public required string VisitedUrl { set; get; }

    public string? VisitedUrlTitle { set; get; }

    public bool HasMissingVisitedUrlTitle => !string.IsNullOrWhiteSpace(VisitedUrl) &&
                                             (string.IsNullOrWhiteSpace(VisitedUrlTitle) ||
                                              string.Equals(VisitedUrlTitle, VisitedUrl,
                                                  StringComparison.OrdinalIgnoreCase));

    public bool IsStaticFileUrl => VisitedUrl.IsStaticFileUrl();

    public bool IsProtectedPage { set; get; }

    public bool IsValidReferrer => ReferrerUrl.IsValidUrl();

    public bool IsLocalReferrer => IsValidReferrer && ReferrerUrl.IsLocalReferrer(VisitedUrl);

    public required string RootUrl { set; get; }

    public bool IsReferrerToThisSite => IsValidReferrer && VisitedUrl.IsReferrerToThisSite(RootUrl);

    public string? DisplayName { set; get; }
}
