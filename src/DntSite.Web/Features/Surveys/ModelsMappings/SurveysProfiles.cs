using AutoMapper;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.Surveys.Models;

namespace DntSite.Web.Features.Surveys.ModelsMappings;

public class SurveysProfiles : Profile
{
    public const string SurveyTags = $"{nameof(Survey)}_Tags";

    public SurveysProfiles()
    {
        MapSurveyToModel();
        MapModelToSurvey();
    }

    private void MapModelToSurvey()
        => CreateMap<VoteModel, Survey>(MemberList.None)
            .ForMember(survey => survey.SurveyItems, opt => opt.Ignore())
            .ForMember(survey => survey.DueDate, opt => opt.MapFrom(model => SetDueDate(model)))
            .ForMember(survey => survey.Tags, opt => opt.Ignore())
            .AfterMap<AfterMapVoteModel>();

    private static DateTime? SetDueDate(VoteModel model)
    {
        if (model.ExpirationDate is null)
        {
            return null;
        }

        if (model is { Hour: not null, Minute: not null })
        {
            return model.ExpirationDate.Value.Date.Add(new TimeSpan(model.Hour.Value, model.Minute.Value, seconds: 0));
        }

        return model.ExpirationDate;
    }

    private void MapSurveyToModel()
        => CreateMap<Survey, VoteModel>(MemberList.None)
            .ForMember(model => model.VoteItems, opt => opt.MapFrom(survey => GetVoteItems(survey)))
            .ForMember(model => model.Hour, opt => opt.MapFrom(survey => GetHour(survey)))
            .ForMember(model => model.Minute, opt => opt.MapFrom(survey => GetMinute(survey)))
            .ForMember(model => model.ExpirationDate,
                opt => opt.MapFrom(survey => survey.DueDate.ToShortPersianDateString(true)))
            .ForMember(model => model.Tags, opt => opt.MapFrom(survey => survey.Tags.Select(tag => tag.Name).ToList()));

    private static int? GetHour(Survey survey) => survey.DueDate?.ToIranTimeZoneDateTime().Hour;

    private static int? GetMinute(Survey survey) => survey.DueDate?.ToIranTimeZoneDateTime().Minute;

    private static string GetVoteItems(Survey survey)
        => survey.SurveyItems.Where(x => !x.IsDeleted)
            .OrderBy(x => x.Id)
            .Select(x => x.Title)
            .ToList()
            .ConvertListToMultiLineText();
}
