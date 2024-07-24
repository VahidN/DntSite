namespace DntSite.Web.Features.UserProfiles.Endpoints;

public static class ChangePasswordEndpoint
{
    /// <summary>
    ///     Improves chrome://settings/passwords page by managing `Change password` button next to a password.
    /// </summary>
    public static void AddChangePasswordEndpoint(this IEndpointRouteBuilder app)
        => app.MapGet("/.well-known/change-password", context =>
        {
            // `/.well-known/change-password` address will be called by the `Change password` button of the Chrome.
            // Now our Web-API app redirects the user to the `/change-password` address of the Blazor App.
            context.Response.Redirect("/change-password", true);

            return Task.CompletedTask;
        });
}
