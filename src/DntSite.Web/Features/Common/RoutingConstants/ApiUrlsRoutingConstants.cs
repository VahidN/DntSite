namespace DntSite.Web.Features.Common.RoutingConstants;

internal static class ApiUrlsRoutingConstants
{
    internal static class Feed
    {
        internal static class HttpAny
        {
            internal const string Index = "/rss.xml";
        }
    }

    internal static class OpenSearch
    {
        internal static class HttpGet
        {
            internal const string RenderOpenSearch = "/OpenSearch/RenderOpenSearch";
        }
    }

    /// <summary>
    ///     DntSite.Web.Features.UserFiles.Controllers.FileController
    /// </summary>
    internal static class File
    {
        /// <summary>
        ///     The supported HTTP method
        /// </summary>
        internal static class HttpAny
        {
            /// <summary>
            ///     File
            ///     <para>DntSite.Web.Features.UserFiles.Controllers.FileController Microsoft.AspNetCore.Mvc.IActionResult Index()</para>
            /// </summary>
            internal const string Index = "/File";

            /// <summary>
            ///     File/Avatar
            ///     <para>
            ///         DntSite.Web.Features.UserFiles.Controllers.FileController Microsoft.AspNetCore.Mvc.IActionResult
            ///         Avatar(System.String)
            ///     </para>
            /// </summary>
            internal const string Avatar = "/File/Avatar";

            /// <summary>
            ///     File/Image
            ///     <para>
            ///         DntSite.Web.Features.UserFiles.Controllers.FileController Microsoft.AspNetCore.Mvc.IActionResult
            ///         Image(System.String)
            ///     </para>
            /// </summary>
            internal const string Image = "/File/Image";

            /// <summary>
            ///     File/MessagesImages
            ///     <para>
            ///         DntSite.Web.Features.UserFiles.Controllers.FileController Microsoft.AspNetCore.Mvc.IActionResult
            ///         MessagesImages(System.String)
            ///     </para>
            /// </summary>
            internal const string MessagesImages = "/File/MessagesImages";

            /// <summary>
            ///     File/UserFile
            ///     <para>
            ///         DntSite.Web.Features.UserFiles.Controllers.FileController Microsoft.AspNetCore.Mvc.IActionResult
            ///         UserFile(System.String)
            ///     </para>
            /// </summary>
            internal const string UserFile = "/File/UserFile";

            /// <summary>
            ///     File/ProjectFile
            ///     <para>
            ///         DntSite.Web.Features.UserFiles.Controllers.FileController Microsoft.AspNetCore.Mvc.IActionResult
            ///         ProjectFile(System.String)
            ///     </para>
            /// </summary>
            internal const string ProjectFile = "/File/ProjectFile";

            /// <summary>
            ///     File/Messages
            ///     <para>
            ///         DntSite.Web.Features.UserFiles.Controllers.FileController Microsoft.AspNetCore.Mvc.IActionResult
            ///         Messages(System.String)
            ///     </para>
            /// </summary>
            internal const string Messages = "/File/Messages";

            /// <summary>
            ///     File/NewsThumb
            ///     <para>
            ///         DntSite.Web.Features.UserFiles.Controllers.FileController Microsoft.AspNetCore.Mvc.IActionResult
            ///         NewsThumb(System.String)
            ///     </para>
            /// </summary>
            internal const string NewsThumb = "/File/NewsThumb";

            /// <summary>
            ///     File/CommonFiles
            ///     <para>
            ///         DntSite.Web.Features.UserFiles.Controllers.FileController Microsoft.AspNetCore.Mvc.IActionResult
            ///         CommonFiles(System.String)
            ///     </para>
            /// </summary>
            internal const string CommonFiles = "/File/CommonFiles";

            /// <summary>
            ///     File/CourseFiles
            ///     <para>
            ///         DntSite.Web.Features.UserFiles.Controllers.FileController Microsoft.AspNetCore.Mvc.IActionResult
            ///         CourseFiles(System.String)
            ///     </para>
            /// </summary>
            internal const string CourseFiles = "/File/CourseFiles";

            /// <summary>
            ///     File/CourseImages
            ///     <para>
            ///         DntSite.Web.Features.UserFiles.Controllers.FileController Microsoft.AspNetCore.Mvc.IActionResult
            ///         CourseImages(System.String)
            ///     </para>
            /// </summary>
            internal const string CourseImages = "/File/CourseImages";

            /// <summary>
            ///     File/EmailToImage
            ///     <para>
            ///         DntSite.Web.Features.UserFiles.Controllers.FileController
            ///         System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult]
            ///         EmailToImage(System.Nullable`1[System.Int32])
            ///     </para>
            /// </summary>
            internal const string EmailToImage = "/File/EmailToImage";
        }
    }

    /// <summary>
    ///     DntSite.Web.Features.UserFiles.Controllers.UploadFileController
    /// </summary>
    internal static class UploadFile
    {
        /// <summary>
        ///     The supported HTTP method
        /// </summary>
        internal static class HttpPost
        {
            /// <summary>
            ///     api/UploadFile/ImageUpload
            ///     <para>
            ///         DntSite.Web.Features.UserFiles.Controllers.UploadFileController
            ///         System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult]
            ///         ImageUpload(DntSite.Web.Features.UserFiles.Models.ImageFileDataModel)
            ///     </para>
            /// </summary>
            internal const string ImageUpload = "/api/UploadFile/ImageUpload";

            /// <summary>
            ///     api/UploadFile/MessagesImagesUpload
            ///     <para>
            ///         DntSite.Web.Features.UserFiles.Controllers.UploadFileController
            ///         System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult]
            ///         MessagesImagesUpload(DntSite.Web.Features.UserFiles.Models.ImageFileDataModel)
            ///     </para>
            /// </summary>
            internal const string MessagesImagesUpload = "/api/UploadFile/MessagesImagesUpload";

            /// <summary>
            ///     api/UploadFile/CourseImagesUpload
            ///     <para>
            ///         DntSite.Web.Features.UserFiles.Controllers.UploadFileController
            ///         System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult]
            ///         CourseImagesUpload(DntSite.Web.Features.UserFiles.Models.ImageFileDataModel)
            ///     </para>
            /// </summary>
            internal const string CourseImagesUpload = "/api/UploadFile/CourseImagesUpload";

            /// <summary>
            ///     api/UploadFile/CourseFileUpload
            ///     <para>
            ///         DntSite.Web.Features.UserFiles.Controllers.UploadFileController
            ///         System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult]
            ///         CourseFileUpload(DntSite.Web.Features.UserFiles.Models.NormalFileDataModel)
            ///     </para>
            /// </summary>
            internal const string CourseFileUpload = "/api/UploadFile/CourseFileUpload";

            /// <summary>
            ///     api/UploadFile/FileUpload
            ///     <para>
            ///         DntSite.Web.Features.UserFiles.Controllers.UploadFileController
            ///         System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult]
            ///         FileUpload(DntSite.Web.Features.UserFiles.Models.NormalFileDataModel)
            ///     </para>
            /// </summary>
            internal const string FileUpload = "/api/UploadFile/FileUpload";

            /// <summary>
            ///     api/UploadFile/CommonFilesUpload
            ///     <para>
            ///         DntSite.Web.Features.UserFiles.Controllers.UploadFileController
            ///         System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult]
            ///         CommonFilesUpload(DntSite.Web.Features.UserFiles.Models.NormalFileDataModel)
            ///     </para>
            /// </summary>
            internal const string CommonFilesUpload = "/api/UploadFile/CommonFilesUpload";

            /// <summary>
            ///     api/UploadFile/MessagesFilesUpload
            ///     <para>
            ///         DntSite.Web.Features.UserFiles.Controllers.UploadFileController
            ///         System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult]
            ///         MessagesFilesUpload(DntSite.Web.Features.UserFiles.Models.NormalFileDataModel)
            ///     </para>
            /// </summary>
            internal const string MessagesFilesUpload = "/api/UploadFile/MessagesFilesUpload";
        }
    }

    /// <summary>
    ///     DntSite.Web.Features.Seo.Controllers.SitemapController
    /// </summary>
    internal static class Sitemap
    {
        /// <summary>
        ///     The supported HTTP method
        /// </summary>
        internal static class HttpAny
        {
            /// <summary>
            ///     Sitemap
            ///     <para>
            ///         DntSite.Web.Features.Seo.Controllers.SitemapController
            ///         System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Get()
            ///     </para>
            /// </summary>
            internal const string Get = "/Sitemap";
        }
    }

    /// <summary>
    ///     DntSite.Web.Features.Common.Controllers.Gen204Controller
    /// </summary>
    internal static class Gen204
    {
        /// <summary>
        ///     The supported HTTP method
        /// </summary>
        internal static class HttpAny
        {
            /// <summary>
            ///     Gen204
            ///     <para>DntSite.Web.Features.Common.Controllers.Gen204Controller Microsoft.AspNetCore.Mvc.IActionResult Get()</para>
            /// </summary>
            internal const string Get = "/Gen204";
        }
    }

    /// <summary>
    ///     DntSite.Web.Features.Common.Controllers.JavaScriptErrorsReportController
    /// </summary>
    internal static class JavaScriptErrorsReport
    {
        /// <summary>
        ///     The supported HTTP method
        /// </summary>
        internal static class HttpPost
        {
            /// <summary>
            ///     api/JavaScriptErrorsReport/Log
            ///     <para>
            ///         DntSite.Web.Features.Common.Controllers.JavaScriptErrorsReportController
            ///         Microsoft.AspNetCore.Mvc.IActionResult Log(System.String)
            ///     </para>
            /// </summary>
            internal const string Log = "/api/JavaScriptErrorsReport/Log";
        }
    }

    /// <summary>
    ///     DNTCommon.Web.Core.CspReportController
    /// </summary>
    internal static class CspReport
    {
        /// <summary>
        ///     The supported HTTP method
        /// </summary>
        internal static class HttpPost
        {
            /// <summary>
            ///     api/CspReport/Log
            ///     <para>
            ///         DNTCommon.Web.Core.CspReportController System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult]
            ///         Log()
            ///     </para>
            /// </summary>
            internal const string Log = "/api/CspReport/Log";
        }
    }

    internal static class Fts
    {
        internal static class HttpGet
        {
            internal const string Search = "/api/Fts/Search";
        }

        internal static class HttpPost
        {
            internal const string Log = "/api/Fts/Log";
        }
    }
}
