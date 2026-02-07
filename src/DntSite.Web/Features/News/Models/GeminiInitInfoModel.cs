using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.News.Models;

public record GeminiInitInfoModel(
    AppSetting AppSetting,
    User? AiUser,
    List<DailyNewsItemAIBacklog>? DailyNewsItemAiBacklogs,
    bool Proceed);
