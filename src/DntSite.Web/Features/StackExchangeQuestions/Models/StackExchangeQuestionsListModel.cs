using DntSite.Web.Features.StackExchangeQuestions.Entities;

namespace DntSite.Web.Features.StackExchangeQuestions.Models;

public class StackExchangeQuestionsListModel
{
    public IList<StackExchangeQuestion> StackExchangeQuestions { set; get; } = new List<StackExchangeQuestion>();

    public int AllItemsCount { set; get; }

    public int DoneItemsCount { set; get; }

    public int NewItemsCount { set; get; }
}
