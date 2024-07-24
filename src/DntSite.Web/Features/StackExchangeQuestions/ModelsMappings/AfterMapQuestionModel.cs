using AutoMapper;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.StackExchangeQuestions.Models;

namespace DntSite.Web.Features.StackExchangeQuestions.ModelsMappings;

public class AfterMapQuestionModel(IAntiXssService antiXssService)
    : IMappingAction<QuestionModel, StackExchangeQuestion>
{
    public void Process(QuestionModel source, StackExchangeQuestion destination, ResolutionContext context)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        destination.Description = antiXssService.GetSanitizedHtml(source.Description);
    }
}
