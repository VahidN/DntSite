namespace DntSite.Web.Features.StackExchangeQuestions.RoutingConstants;

public static class QuestionsRoutingConstants
{
    public const string Questions = "/questions";
    public const string QuestionsPageCurrentPage = $"{Questions}/page/{{CurrentPage:int?}}";

    public const string QuestionsFilterBase = $"{Questions}/filter";

    public const string QuestionsFilterFilterPageCurrentPage =
        $"{QuestionsFilterBase}/{{Filter}}/page/{{CurrentPage:int?}}";

    public const string QuestionsFilterOptionalFilterPageCurrentPage =
        $"{QuestionsFilterBase}/page/{{CurrentPage:int?}}";

    public const string QuestionsDetailsBase = $"{Questions}/details";
    public const string QuestionsDetailsQuestionId = $"{QuestionsDetailsBase}/{{QuestionId:int}}";

    public const string QuestionsDetailsOldQuestionId = "/questions/question/{QuestionId:int}";

    public const string QuestionsTag = "/questions-tag";
    public const string QuestionsTagPageCurrentPage = $"{QuestionsTag}/page/{{CurrentPage:int?}}";
    public const string QuestionsTagTagName = $"{QuestionsTag}/{{TagName}}";
    public const string QuestionsTagTagNamePageCurrentPage = $"{QuestionsTag}/{{TagName}}/page/{{CurrentPage:int?}}";

    public const string QuestionsWriters = "/questions-writers";
    public const string QuestionsWritersPageCurrentPage = $"{QuestionsWriters}/page/{{CurrentPage:int?}}";
    public const string QuestionsWritersUserFriendlyName = $"{QuestionsWriters}/{{UserFriendlyName}}";

    public const string QuestionsWritersUserFriendlyNamePageCurrentPage =
        $"{QuestionsWriters}/{{UserFriendlyName}}/page/{{CurrentPage:int?}}";

    public const string QuestionsComments = "/questions-comments";
    public const string QuestionsCommentsPageCurrentPage = $"{QuestionsComments}/page/{{CurrentPage:int?}}";
    public const string QuestionsCommentsUserFriendlyName = $"{QuestionsComments}/{{UserFriendlyName}}";

    public const string QuestionsCommentsUserFriendlyNamePageCurrentPage =
        $"{QuestionsComments}/{{UserFriendlyName}}/page/{{CurrentPage:int?}}";

    public const string WriteQuestion = "/write-question";

    public const string WriteQuestionEditBase = $"{WriteQuestion}/edit";
    public const string WriteQuestionEditEditId = $"{WriteQuestionEditBase}/{{EditId:{EncryptedRouteConstraint.Name}}}";

    public const string WriteQuestionDeleteBase = $"{WriteQuestion}/delete";

    public const string WriteQuestionDeleteDeleteId =
        $"{WriteQuestionDeleteBase}/{{DeleteId:{EncryptedRouteConstraint.Name}}}";

    public const string CommentsUrlTemplate = $"{QuestionsDetailsBase}/{{0}}#comments";
    public const string PostUrlTemplate = $"{QuestionsDetailsBase}/{{0}}";
    public const string PostTagUrlTemplate = $"{QuestionsTag}/{{0}}";
    public const string EditPostUrlTemplate = $"{WriteQuestionEditBase}/{{0}}";
    public const string DeletePostUrlTemplate = $"{WriteQuestionDeleteBase}/{{0}}";
}
