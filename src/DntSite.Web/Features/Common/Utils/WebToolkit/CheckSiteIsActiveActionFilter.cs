using DntSite.Web.Features.AppConfigs.Services.Contracts;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DntSite.Web.Features.Common.Utils.WebToolkit;

public class CheckSiteIsActiveActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        var cachedAppSettingsProvider =
            context.HttpContext.RequestServices.GetRequiredService<ICachedAppSettingsProvider>();

        if ((await cachedAppSettingsProvider.GetAppSettingsAsync()).SiteIsActive)
        {
            await next(); // اکشن متد اجرا می‌شود
        }
        else
        {
            context.Result = new StatusCodeResult((int)HttpStatusCode.ServiceUnavailable);
        }
    }
}
