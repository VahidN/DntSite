using AutoMapper;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.Models;

namespace DntSite.Web.Features.Posts.ModelsMappings;

public class PostsMappingsProfiles : Profile
{
    public PostsMappingsProfiles()
    {
        MapDraftToModel();
        MapModelToDraft();

        MapPostToModel();
        MapModelToPost();
    }

    private void MapModelToPost()
        => CreateMap<WriteArticleModel, BlogPost>(MemberList.None)
            .ForMember(post => post.ReadingTimeMinutes, opt => opt.MapFrom(model => model.ArticleBody.MinReadTime(180)))
            .ForMember(post => post.BriefDescription,
                opt => opt.MapFrom(model => model.ArticleBody.GetBriefDescription(450)))
            .AfterMap<AfterMapWriteArticleModel>();

    private void MapPostToModel()
        => CreateMap<BlogPost, WriteArticleModel>(MemberList.None)
            .ForMember(model => model.ArticleBody, opt => opt.MapFrom(post => post.Body))
            .ForMember(model => model.ArticleTags,
                opt => opt.MapFrom(post => post.Tags.Select(tag => tag.Name).ToList()));

    private void MapModelToDraft()
        => CreateMap<WriteDraftModel, BlogPostDraft>(MemberList.None)
            .ForMember(draft => draft.IsConverted, opt => opt.MapFrom(_ => false))
            .ForMember(draft => draft.ReadingTimeMinutes,
                opt => opt.MapFrom(model => model.ArticleBody.MinReadTime(180)))
            .ForMember(draft => draft.DateTimeToShow,
                opt => opt.MapFrom(model => new PersianDateTime(model.PersianDateYear, model.PersianDateMonth,
                    model.PersianDateDay, model.Hour, model.Minute, 0).DateTime))
            .AfterMap<AfterMapWriteDraftModel>();

    private void MapDraftToModel()
        => CreateMap<BlogPostDraft, WriteDraftModel>(MemberList.None)
            .ForMember(model => model.ArticleBody, opt => opt.MapFrom(draft => draft.Body))
            .ForMember(model => model.ReadingTimeMinutes, opt => opt.MapFrom(draft => draft.Body.MinReadTime(180)))
            .ForMember(model => model.Hour,
                opt => opt.MapFrom(draft => draft.DateTimeToShow == null ? 23 : draft.DateTimeToShow.Value.Hour))
            .ForMember(model => model.Minute,
                opt => opt.MapFrom(draft => draft.DateTimeToShow == null ? 55 : draft.DateTimeToShow.Value.Minute))
            .ForMember(model => model.PersianDateYear, opt => opt.MapFrom(draft => GetYear(draft.DateTimeToShow)))
            .ForMember(model => model.PersianDateMonth, opt => opt.MapFrom(draft => GetMonth(draft.DateTimeToShow)))
            .ForMember(model => model.PersianDateDay, opt => opt.MapFrom(draft => GetDay(draft.DateTimeToShow)));

    private static int? GetYear(DateTime? draftDateTimeToShow) => draftDateTimeToShow.ToPersianYearMonthDay()?.Year;

    private static int? GetMonth(DateTime? draftDateTimeToShow) => draftDateTimeToShow.ToPersianYearMonthDay()?.Month;

    private static int? GetDay(DateTime? draftDateTimeToShow) => draftDateTimeToShow.ToPersianYearMonthDay()?.Day;
}
