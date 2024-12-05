using AutoMapper;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.News.Models;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.Models;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Posts.Services;

public class BlogPostDraftsService(
    IUnitOfWork uow,
    ITagsService tagsService,
    IBlogPostsService blogPostsService,
    IBlogPostsEmailsService blogPostsEmailsService,
    IStatService statService,
    IDailyNewsItemsService dailyNewsItemsService,
    IMapper mapper) : IBlogPostDraftsService
{
    private readonly DbSet<BlogPostDraft> _blogPostDrafts = uow.DbSet<BlogPostDraft>();

    public async Task<BlogPostDraft> AddBlogPostDraftAsync(WriteDraftModel model)
    {
        var draft = mapper.Map<WriteDraftModel, BlogPostDraft>(model);
        _blogPostDrafts.Add(draft);
        await uow.SaveChangesAsync();

        return draft;
    }

    public async Task UpdateBlogPostDraftAsync(WriteDraftModel model, BlogPostDraft draft)
    {
        mapper.Map(model, draft);
        await uow.SaveChangesAsync();
    }

    public async Task MarkAsDeletedAsync(BlogPostDraft? draft)
    {
        if (draft is null)
        {
            return;
        }

        draft.IsConverted = true;
        draft.IsReady = false;
        await uow.SaveChangesAsync();
    }

    public ValueTask<BlogPostDraft?> FindBlogPostDraftAsync(int id) => _blogPostDrafts.FindAsync(id);

    public Task<BlogPostDraft?> FindBlogPostDraftIncludeUserAsync(int id)
        => _blogPostDrafts.Include(x => x.User).OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.Id == id);

    public async Task DeleteDraftAsync(BlogPostDraft draft)
    {
        _blogPostDrafts.Remove(draft);
        await uow.SaveChangesAsync();
    }

    public async Task DeleteConvertedBlogPostDraftsAsync()
    {
        var list = await _blogPostDrafts.Where(x => x.IsConverted).ToListAsync();
        _blogPostDrafts.RemoveRange(list);
        await uow.SaveChangesAsync();
    }

    public Task<List<BlogPostDraft>> FindUsersNotConvertedBlogPostDraftsAsync(int userId)
        => _blogPostDrafts.Include(x => x.User)
            .Where(x => !x.IsConverted && x.UserId!.Value == userId)
            .OrderByDescending(x => x.IsReady)
            .ThenBy(x => x.DateTimeToShow)
            .ToListAsync();

    public Task<List<BlogPostDraft>> ComingSoonItemsAsync()
    {
        var aMonth = DateTime.UtcNow.AddMonths(months: -1);

        return _blogPostDrafts.AsNoTracking()
            .Include(x => x.User)
            .Where(x => !x.IsConverted && x.User!.UserStat.NumberOfPosts > 0 &&
                        x.Audit.CreatedAt >= aMonth) // جلوگیری از ارسال مطالب بی‌ربط توسط تازه واردها
            .OrderByDescending(x => x.IsReady)
            .ThenBy(x => x.DateTimeToShow)
            .ToListAsync();
    }

    public Task<List<BlogPostDraft>> FindAllNotConvertedBlogPostDraftsAsync()
        => _blogPostDrafts.Include(x => x.User)
            .Where(x => !x.IsConverted)
            .OrderByDescending(x => x.IsReady)
            .ThenBy(x => x.DateTimeToShow)
            .ToListAsync();

    public async Task RunConvertDraftsToPostsJobAsync()
    {
        var draftsToConvert = await _blogPostDrafts.Where(x
                => !x.IsConverted && x.IsReady && x.DateTimeToShow.HasValue && x.DateTimeToShow <= DateTime.UtcNow)
            .ToListAsync();

        foreach (var draft in draftsToConvert)
        {
            if (!draft.DateTimeToShow.HasValue)
            {
                continue;
            }

            await SaveDraftToBlogPostAsync(draft);
        }
    }

    public async Task<bool> ConvertDraftToLinkAsync(DailyNewsItemModel data, int draftId)
    {
        ArgumentNullException.ThrowIfNull(data);

        var draft = await FindBlogPostDraftIncludeUserAsync(draftId);

        if (draft is null)
        {
            return false;
        }

        if ((await dailyNewsItemsService.CheckUrlHashAsync(data.Url, id: null, isAdmin: true)).Stat ==
            OperationStat.Failed)
        {
            return false;
        }

        await dailyNewsItemsService.AddNewsItemAsync(data, new User
        {
            IsRestricted = false,
            Id = draft.UserId!.Value
        });

        return true;
    }

    private async Task SaveDraftToBlogPostAsync(BlogPostDraft draft)
    {
        ArgumentNullException.ThrowIfNull(draft);

        var tagsList = draft.Tags.ToList();
        var listOfActualTags = await tagsService.SaveNewArticleTagsAsync(tagsList);

        var blogPost = new BlogPost
        {
            IsDeleted = false,
            Body = draft.Body,
            BriefDescription = draft.Body.GetBriefDescription(charLength: 450),
            Title = draft.Title,
            EmailsSent = false,
            UserId = draft.UserId,
            NumberOfRequiredPoints = draft.NumberOfRequiredPoints,
            ReadingTimeMinutes = draft.ReadingTimeMinutes
        };

        draft.IsConverted = true;
        await blogPostsService.SaveBlogPostAsync(blogPost, listOfActualTags);

        await statService.RecalculateTagsInUseCountsAsync<BlogPostTag, BlogPost>();

        if (blogPost.UserId is not null)
        {
            await statService.RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(blogPost.UserId.Value);
        }

        await blogPostsEmailsService.DraftConvertedEmailAsync(blogPost);
    }
}
