namespace DntSite.Web.Features.Common.Utils.Security;

public static class SecurityHeadersBuilder
{
    public static HeaderPolicyCollection GetCsp(bool isDevelopment)
    {
        var policy = new HeaderPolicyCollection().AddFrameOptionsDeny()
            .AddXssProtectionBlock()
            .AddContentTypeOptionsNoSniff()
            .AddReferrerPolicyStrictOriginWhenCrossOrigin()
            .AddCrossOriginOpenerPolicy(builder => builder.SameOrigin())
            .AddCrossOriginResourcePolicy(builder => builder.SameOrigin())
            .AddCrossOriginEmbedderPolicy(builder => builder.RequireCorp())
            .AddContentSecurityPolicy(builder =>
            {
                builder.AddBaseUri().Self();
                builder.AddDefaultSrc().Self().From(uri: "blob:");
                builder.AddObjectSrc().Self().From(uri: "blob:");
                builder.AddBlockAllMixedContent();
                builder.AddImgSrc().Self().From(uri: "data:").From(uri: "blob:").From(uri: "https:");
                builder.AddFontSrc().Self();

                builder.AddStyleSrc().UnsafeInline().Self();

                builder.AddFrameAncestors().None();
                builder.AddFrameSrc().Self();
                builder.AddConnectSrc().Self();
                builder.AddMediaSrc().Self();

                builder.AddScriptSrc().Self();

                // NOTE: it's part of the DNTCommon.Web.Core\Middlewares\CspReportController.cs
                builder.AddReportUri().To(uri: "/api/CspReport/Log");

                if (!isDevelopment)
                {
                    builder.AddUpgradeInsecureRequests();
                }
            })
            .RemoveServerHeader()
            .AddPermissionsPolicy(builder =>
            {
                builder.AddAccelerometer().None();
                builder.AddAutoplay().None();
                builder.AddCamera().None();
                builder.AddEncryptedMedia().None();
                builder.AddFullscreen().All();
                builder.AddGeolocation().None();
                builder.AddGyroscope().None();
                builder.AddMagnetometer().None();
                builder.AddMicrophone().None();
                builder.AddMidi().None();
                builder.AddPayment().None();
                builder.AddPictureInPicture().None();
                builder.AddSyncXHR().None();
                builder.AddUsb().None();
            });

        if (!isDevelopment)
        {
            // Default maxAge => one year in seconds
            policy.AddStrictTransportSecurityMaxAgeIncludeSubDomains();
        }

        policy.ApplyDocumentHeadersToAllResponses();

        return policy;
    }
}
