namespace DntSite.Web.Features.Surveys.RoutingConstants;

public static class SurveysRoutingConstants
{
    public const string SurveysComments = "/surveys-comments";
    public const string SurveysCommentsPageCurrentPage = $"{SurveysComments}/page/{{CurrentPage:int?}}";
    public const string SurveysCommentsUserFriendlyName = $"{SurveysComments}/{{UserFriendlyName}}";

    public const string SurveysCommentsUserFriendlyNamePageCurrentPage =
        $"{SurveysComments}/{{UserFriendlyName}}/page/{{CurrentPage:int?}}";

    public const string Votes = "/Votes";
    public const string VotesDetailsSurveyId = $"{Votes}/details/{{SurveyId:int}}";

    public const string SurveysArchive = "/surveys";
    public const string SurveysArchivePageCurrentPage = $"{SurveysArchive}/page/{{CurrentPage:int?}}";

    public const string SurveysArchiveFilterBase = $"{SurveysArchive}/filter";

    public const string SurveysArchiveFilterFilterPageCurrentPage =
        $"{SurveysArchiveFilterBase}/{{Filter}}/page/{{CurrentPage:int?}}";

    public const string SurveysArchiveDetailsBase = $"{SurveysArchive}/details";
    public const string SurveysArchiveDetailsSurveyId = $"{SurveysArchiveDetailsBase}/{{SurveyId:int}}";

    public const string SurveysTag = "/surveys-tag";
    public const string SurveysTagPageCurrentPage = $"{SurveysTag}/page/{{CurrentPage:int?}}";
    public const string SurveysTagTagName = $"{SurveysTag}/{{TagName}}";
    public const string SurveysTagTagNamePageCurrentPage = $"{SurveysTag}/{{TagName}}/page/{{CurrentPage:int?}}";
    public const string VotesTagTagName = $"{Votes}/tag/{{TagName}}";

    public const string SurveysWriters = "/surveys-writers";
    public const string SurveysWritersPageCurrentPage = $"{SurveysWriters}/page/{{CurrentPage:int?}}";
    public const string SurveysWritersUserFriendlyName = $"{SurveysWriters}/{{UserFriendlyName}}";

    public const string SurveysWritersUserFriendlyNamePageCurrentPage =
        $"{SurveysWriters}/{{UserFriendlyName}}/page/{{CurrentPage:int?}}";

    public const string WriteSurvey = "/write-survey";

    public const string WriteSurveyEditBase = $"{WriteSurvey}/edit";
    public const string WriteSurveyEditEditId = $"{WriteSurveyEditBase}/{{EditId:{EncryptedRouteConstraint.Name}}}";

    public const string WriteSurveyDeleteBase = $"{WriteSurvey}/delete";

    public const string WriteSurveyDeleteDeleteId =
        $"{WriteSurveyDeleteBase}/{{DeleteId:{EncryptedRouteConstraint.Name}}}";

    public const string CommentsUrlTemplate = $"{SurveysArchiveDetailsBase}/{{0}}#comments";
    public const string PostUrlTemplate = $"{SurveysArchiveDetailsBase}/{{0}}";
    public const string PostTagUrlTemplate = $"{SurveysTag}/{{0}}";
    public const string EditPostUrlTemplate = $"{WriteSurveyEditBase}/{{0}}";
    public const string DeletePostUrlTemplate = $"{WriteSurveyDeleteBase}/{{0}}";
}
