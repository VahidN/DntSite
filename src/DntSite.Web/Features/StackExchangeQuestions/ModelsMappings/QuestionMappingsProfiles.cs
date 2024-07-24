using AutoMapper;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.StackExchangeQuestions.Models;

namespace DntSite.Web.Features.StackExchangeQuestions.ModelsMappings;

public class QuestionMappingsProfiles : Profile
{
    public QuestionMappingsProfiles()
    {
        MapQuestionToModel();
        MapModelToQuestion();
    }

    private void MapModelToQuestion()
        => CreateMap<QuestionModel, StackExchangeQuestion>(MemberList.None)
            .ForMember(question => question.Tags, opt => opt.Ignore())
            .AfterMap<AfterMapQuestionModel>();

    private void MapQuestionToModel()
        => CreateMap<StackExchangeQuestion, QuestionModel>(MemberList.None)
            .ForMember(model => model.Tags, opt => opt.MapFrom(post => post.Tags.Select(tag => tag.Name).ToList()));
}
