using DntSite.Web.Features.PrivateMessages.Entities;

namespace DntSite.Web.Features.PrivateMessages.Services;

public static class PrivateMessagesSecurityExtensions
{
    public static bool CanUserReadMessage(this PrivateMessage? message, int? currentUserId)
    {
        if (message?.UserId is null)
        {
            return false;
        }

        if (!currentUserId.HasValue)
        {
            return false;
        }

        if (message.IsDeleted)
        {
            return false;
        }

        var isSender = message.UserId.Value == currentUserId.Value;
        var isReceiver = message.ToUserId == currentUserId.Value;

        return isSender || isReceiver;
    }
}
