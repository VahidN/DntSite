namespace DntSite.Web.Features.PrivateMessages.RoutingConstants;

public static class PrivateMessagesRoutingConstants
{
    public const string MyPrivateMessages = "/my-private-messages";
    public const string MyPrivateMessagesPageCurrentPage = $"{MyPrivateMessages}/page/{{CurrentPage:int}}";

    public const string MyPrivateMessageBase = "/my-private-message";
    public const string MyPrivateMessagePrivateMessageId = $"{MyPrivateMessageBase}/{{PrivateMessageId:int}}";

    public const string PrivateMessagesViewer = "/private-messages-viewer";
    public const string PrivateMessagesViewerPageCurrentPage = $"{PrivateMessagesViewer}/page/{{CurrentPage:int}}";

    public const string PublicContactUs1 = "/PublicContactUs";
    public const string PublicContactUs2 = "/public-contact-us";

    public const string SendPrivateMessageBase = "/send-private-message";
    public const string SendPrivateMessageToUserId = $"{SendPrivateMessageBase}/{{ToUserId:int}}";

    public const string SendPrivateMessageEditEditId = $"{SendPrivateMessageBase}/edit/{{EditId:int}}";

    public const string SendPrivateMessageDeleteBase = $"{SendPrivateMessageBase}/delete";
    public const string SendPrivateMessageDeleteDeleteId = $"{SendPrivateMessageDeleteBase}/{{DeleteId:int}}";

    public const string CommentsUrlTemplate = $"{MyPrivateMessageBase}/{{0}}#comments";
    public const string PostUrlTemplate = $"{MyPrivateMessageBase}/{{0}}";
    public const string PostTagUrlTemplate = $"{MyPrivateMessageBase}/{{0}}";
    public const string EditPostUrlTemplate = $"{SendPrivateMessageBase}/edit/{{0}}";
    public const string DeletePostUrlTemplate = $"{SendPrivateMessageBase}/delete/{{0}}";
}
