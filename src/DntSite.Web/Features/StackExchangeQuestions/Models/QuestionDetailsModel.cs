using DntSite.Web.Features.StackExchangeQuestions.Entities;

namespace DntSite.Web.Features.StackExchangeQuestions.Models;

public class QuestionDetailsModel
{
    public StackExchangeQuestion? CurrentItem { set; get; }

    public StackExchangeQuestion? NextItem { set; get; }

    public StackExchangeQuestion? PreviousItem { set; get; }

    public StackExchangeQuestionsListModel? QuestionsListModel { set; get; }
}
