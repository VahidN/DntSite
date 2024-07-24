using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Backlogs.Entities;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.RoadMaps.Entities;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.Stats.Services;

public class StatService(IUnitOfWork uow) : IStatService
{
    public async Task<RecalculatePostsModel> GetStatAsync()
        => new()
        {
            NumberOfAdmins = await NumberOfAdminsAsync(),
            NumberOfPosts = await NumberOfPostsAsync(),
            NumberOfTags = await NumberOfTagsAsync(),
            NumberOfUsers = await NumberOfUsersAsync(),
            NumberOfWriters = await NumberOfWritersAsync(),
            NumberOfLinks = await NumberOfLinksAsync(),
            BrowsersList = BrowsersList(),
            NumberOfTodayBirthdays = await GetTodayBirthdayCountAsync(),
            NumberOfAds = await GetNumberOfAdsAsync(),
            NumberOfComingSoon = await GetNumberOfComingSoonAsync(),
            NumberOfCourses = await GetNumberOfCoursesAsync(),
            NumberOfProjects = await GetNumberOfProjectsAsync(),
            NumberOfVotes = await GetNumberOfVotesAsync(),
            NumberOfJobsSeekers = await GetNumberOfJobsSeekersAsync(),
            NumberOfLearningPaths = await GetNumberOfLearningPathsAsync(),
            NumberOfBacklogs = await GetNumberOfBacklogsAsync(),
            NumberOfQuestions = await GetNumberOfQuestionsAsync()
        };

    public Task<int> NumberOfLinksAsync(bool showDeletedItems = false)
        => uow.DbSet<DailyNewsItem>().CountAsync(x => x.IsDeleted == showDeletedItems);

    public Task<int> NumberOfPostsAsync(bool showDeletedItems = false)
        => uow.DbSet<BlogPost>().CountAsync(x => x.IsDeleted == showDeletedItems);

    public async Task UpdateNumberOfDraftsStatAsync(int userId)
    {
        var user = await uow.DbSet<User>().FindAsync(userId);

        if (user is null)
        {
            return;
        }

        user.UserStat.NumberOfDrafts = await uow.DbSet<BlogPostDraft>().CountAsync(x => x.UserId == userId);
        await uow.SaveChangesAsync();
    }

    public Task<int> NumberOfCommentsAsync(bool showDeletedItems = false)
        => uow.DbSet<BlogPostComment>().CountAsync(x => x.IsDeleted == showDeletedItems);

    public Task<int> NumberOfTagsAsync(bool showActives = true)
        => uow.DbSet<BlogPostTag>().CountAsync(x => x.IsDeleted != showActives);

    public Task<int> NumberOfAdminsAsync(bool showActives = true)
    {
        var query = from role in uow.DbSet<Role>()
            from user in role.AssociatedEntities
            where role.Name == CustomRoles.Admin && user.IsActive == showActives
            select user;

        return query.CountAsync();
    }

    public Task<int> NumberOfUsersAsync(bool showActives = true)
        => uow.DbSet<User>().CountAsync(x => x.IsActive == showActives);

    public Task<int> NumberOfWritersAsync(bool showActives = true)
        => uow.DbSet<User>().CountAsync(user => user.IsActive == showActives && user.UserStat.NumberOfPosts > 0);

    public async Task RecalculateAllUsersNumberOfPostsAndCommentsAsync(bool showDeletedItems = false)
    {
        var users = await uow.DbSet<User>().Where(x => x.IsActive).ToListAsync();

        foreach (var user in users)
        {
            await UpdateUserStatAsync(showDeletedItems, user);
        }

        await uow.SaveChangesAsync();
    }

    public async Task UpdateNumberOfLearningPathsAsync(int userId)
    {
        var user = await uow.DbSet<User>().FindAsync(userId);

        if (user is null)
        {
            return;
        }

        var count = await uow.DbSet<LearningPath>().CountAsync(x => !x.IsDeleted && x.UserId == userId);
        user.UserStat.NumberOfLearningPaths = count;
        await uow.SaveChangesAsync();
    }

    public async Task RecalculateBlogPostTagsInUseCountsAsync(IList<BlogPostTag> tags,
        bool showActives = true,
        bool showDeletedItems = false)
    {
        ArgumentNullException.ThrowIfNull(tags);

        foreach (var tag in tags)
        {
            tag.InUseCount = await uow.DbSet<BlogPostTag>()
                .Where(x => x.IsDeleted != showActives && x.Id == tag.Id)
                .SelectMany(x => x.AssociatedEntities)
                .CountAsync(x => x.IsDeleted == showDeletedItems);
        }

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateAllBlogPostTagsInUseCountsAsync(bool showActives = true, bool showDeletedItems = false)
    {
        var tags = await uow.DbSet<BlogPostTag>().ToListAsync();

        foreach (var tag in tags)
        {
            tag.InUseCount = await uow.DbSet<BlogPostTag>()
                .Where(x => x.IsDeleted != showActives && x.Id == tag.Id)
                .SelectMany(x => x.AssociatedEntities)
                .CountAsync(x => x.IsDeleted == showDeletedItems);
        }

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateAllLinkTagsInUseCountsAsync(IList<string> tags,
        bool showActives = true,
        bool showDeletedItems = false)
    {
        var listOfActualTagNames = await uow.DbSet<DailyNewsItemTag>().Where(x => tags.Contains(x.Name)).ToListAsync();

        foreach (var tag in listOfActualTagNames)
        {
            tag.InUseCount = await uow.DbSet<DailyNewsItemTag>()
                .Where(x => x.IsDeleted != showActives && x.Id == tag.Id)
                .SelectMany(x => x.AssociatedEntities)
                .CountAsync(x => x.IsDeleted == showDeletedItems);
        }

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateAllQuestionTagsInUseCountsAsync(IList<string> tags,
        bool showActives = true,
        bool showDeletedItems = false)
    {
        var listOfActualTagNames =
            await uow.DbSet<StackExchangeQuestionTag>().Where(x => tags.Contains(x.Name)).ToListAsync();

        foreach (var tag in listOfActualTagNames)
        {
            tag.InUseCount = await uow.DbSet<StackExchangeQuestionTag>()
                .Where(x => x.IsDeleted != showActives && x.Id == tag.Id)
                .SelectMany(x => x.AssociatedEntities)
                .CountAsync(x => x.IsDeleted == showDeletedItems);
        }

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateAllBacklogTagsInUseCountsAsync(IList<string> backlogTags,
        bool showActives = true,
        bool showDeletedItems = false)
    {
        var listOfActualTagNames = await uow.DbSet<BacklogTag>().Where(x => backlogTags.Contains(x.Name)).ToListAsync();

        foreach (var tag in listOfActualTagNames)
        {
            tag.InUseCount = await uow.DbSet<BacklogTag>()
                .Where(x => x.IsDeleted != showActives && x.Id == tag.Id)
                .SelectMany(x => x.AssociatedEntities)
                .CountAsync(x => x.IsDeleted == showDeletedItems);
        }

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateAllLearningPathTagsInUseCountsAsync(IList<LearningPathTag> tags,
        bool showActives = true,
        bool showDeletedItems = false)
    {
        if (tags is null)
        {
            return;
        }

        foreach (var tag in tags)
        {
            tag.InUseCount = await uow.DbSet<LearningPathTag>()
                .Where(x => x.IsDeleted != showActives && x.Id == tag.Id)
                .SelectMany(x => x.AssociatedEntities)
                .CountAsync(x => x.IsDeleted == showDeletedItems);
        }

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateAllProjectTagsInUseCountsAsync(IList<string> tags,
        bool showActives = true,
        bool showDeletedItems = false)
    {
        var listOfActualTagNames = await uow.DbSet<ProjectTag>().Where(x => tags.Contains(x.Name)).ToListAsync();

        foreach (var tag in listOfActualTagNames)
        {
            tag.InUseCount = await uow.DbSet<ProjectTag>()
                .Where(x => x.IsDeleted != showActives && x.Id == tag.Id)
                .SelectMany(x => x.AssociatedEntities)
                .CountAsync(x => x.IsDeleted == showDeletedItems);
        }

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateAllVoteTagsInUseCountsAsync(string[] tags,
        bool showActives = true,
        bool showDeletedItems = false)
    {
        var listOfActualTagNames = await uow.DbSet<SurveyTag>().Where(x => tags.Contains(x.Name)).ToListAsync();

        foreach (var tag in listOfActualTagNames)
        {
            tag.InUseCount = await uow.DbSet<SurveyTag>()
                .Where(x => x.IsDeleted != showActives && x.Id == tag.Id)
                .SelectMany(x => x.AssociatedEntities)
                .CountAsync(x => x.IsDeleted == showDeletedItems);
        }

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(int userId,
        bool showDeletedItems = false)
    {
        var user = await uow.DbSet<User>().FindAsync(userId);

        if (user is null)
        {
            return;
        }

        await UpdateUserStatAsync(showDeletedItems, user);
        await uow.SaveChangesAsync();
    }

    public async Task UpdateAllUsersRatingsAsync()
    {
        var users = await uow.DbSet<User>().Where(x => x.IsActive).ToListAsync();

        foreach (var user in users)
        {
            await UpdateUserRatingsAsync(user);
        }
    }

    public async Task RecalculateThisTagInUseCountsAsync(string tagName,
        bool showActives = true,
        bool showDeletedItems = false)
    {
        var tag = await uow.DbSet<BlogPostTag>().OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.Name == tagName);

        if (tag is null)
        {
            return;
        }

        tag.InUseCount = await uow.DbSet<BlogPostTag>()
            .Where(x => x.IsDeleted != showActives && x.Id == tag.Id)
            .SelectMany(x => x.AssociatedEntities)
            .CountAsync(x => x.IsDeleted == showDeletedItems);

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateThisTagInUseCountsAsync(int tagId,
        bool showActives = true,
        bool showDeletedItems = false)
    {
        var tag = await uow.DbSet<BlogPostTag>().FindAsync(tagId);

        if (tag is null)
        {
            return;
        }

        tag.InUseCount = await uow.DbSet<BlogPostTag>()
            .Where(x => x.IsDeleted != showActives && x.Id == tagId)
            .SelectMany(x => x.AssociatedEntities)
            .CountAsync(x => x.IsDeleted == showDeletedItems);

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateThisBlogPostCommentsCountsAsync(int postId, bool showDeletedItems = false)
    {
        var post = await uow.DbSet<BlogPost>().FindAsync(postId);

        if (post is null)
        {
            return;
        }

        post.PingbackSent = false;

        post.EntityStat.NumberOfComments = await uow.DbSet<BlogPost>()
            .Where(x => x.Id == postId)
            .SelectMany(x => x.Comments)
            .CountAsync(x => x.IsDeleted == showDeletedItems);

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateAllBlogPostsCommentsCountsAsync(bool showDeletedItems = false)
    {
        var list = await uow.DbSet<BlogPost>().ToListAsync();

        foreach (var item in list)
        {
            item.EntityStat.NumberOfComments = await uow.DbSet<BlogPost>()
                .Where(x => x.Id == item.Id)
                .SelectMany(x => x.Comments)
                .CountAsync(x => x.IsDeleted == showDeletedItems);
        }

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateThisNewsPostCommentsCountsAsync(int postId, bool showDeletedItems = false)
    {
        var post = await uow.DbSet<DailyNewsItem>().FindAsync(postId);

        if (post is null)
        {
            return;
        }

        post.PingbackSent = false;

        post.EntityStat.NumberOfComments = await uow.DbSet<DailyNewsItem>()
            .Where(x => x.Id == postId)
            .SelectMany(x => x.Comments)
            .CountAsync(x => x.IsDeleted == showDeletedItems);

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateThisStackExchangeQuestionCommentsCountsAsync(int postId, bool showDeletedItems = false)
    {
        var post = await uow.DbSet<StackExchangeQuestion>().FindAsync(postId);

        if (post is null)
        {
            return;
        }

        post.EntityStat.NumberOfComments = await uow.DbSet<StackExchangeQuestion>()
            .Where(x => x.Id == postId)
            .SelectMany(x => x.Comments)
            .CountAsync(x => x.IsDeleted == showDeletedItems);

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateThisProjectIssuesCountsAsync(int projectId, bool showDeletedItems = false)
    {
        var project = await uow.DbSet<Project>().FindAsync(projectId);

        if (project is null)
        {
            return;
        }

        project.NumberOfIssues = await uow.DbSet<Project>()
            .Where(x => x.Id == projectId)
            .SelectMany(x => x.ProjectIssues)
            .CountAsync(x => x.IsDeleted == showDeletedItems);

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateThisProjectIssueCommentsCountsAsync(int issueId, bool showDeletedItems = false)
    {
        var projectIssue = await uow.DbSet<ProjectIssue>().FindAsync(issueId);

        if (projectIssue is null)
        {
            return;
        }

        projectIssue.EntityStat.NumberOfComments = await uow.DbSet<ProjectIssue>()
            .Where(x => x.Id == issueId)
            .SelectMany(x => x.Comments)
            .CountAsync(x => x.IsDeleted == showDeletedItems);

        await uow.SaveChangesAsync();
    }

    public async Task UpdateProjectNumberOfReleasesStatAsync(int projectId, bool showDeletedItems = false)
    {
        var project = await uow.DbSet<Project>().FindAsync(projectId);

        if (project is null)
        {
            return;
        }

        project.NumberOfReleases = await uow.DbSet<Project>()
            .Where(x => x.Id == projectId)
            .SelectMany(x => x.ProjectReleases)
            .CountAsync(x => x.IsDeleted != showDeletedItems);

        await uow.SaveChangesAsync();
    }

    public async Task UpdateProjectNumberOfIssuesCommentsAsync(int projectId, bool showDeletedItems = false)
    {
        var project = await uow.DbSet<Project>().FindAsync(projectId);

        if (project is null)
        {
            return;
        }

        project.NumberOfIssuesComments = await uow.DbSet<ProjectIssue>()
            .Where(x => x.ProjectId == projectId && x.IsDeleted == showDeletedItems)
            .SelectMany(x => x.Comments)
            .CountAsync(x => x.IsDeleted == showDeletedItems);

        await uow.SaveChangesAsync();
    }

    public async Task UpdateProjectNumberOfFaqsAsync(int projectId, bool showDeletedItems = false)
    {
        var project = await uow.DbSet<Project>().FindAsync(projectId);

        if (project is null)
        {
            return;
        }

        project.NumberOfFaqs = await uow.DbSet<ProjectFaq>()
            .Where(x => x.ProjectId == projectId)
            .CountAsync(x => x.IsDeleted == showDeletedItems);

        await uow.SaveChangesAsync();
    }

    public async Task UpdateProjectNumberOfViewsOfFaqAsync(int faqId, bool fromFeed)
    {
        var faq = await uow.DbSet<ProjectFaq>().FindAsync(faqId);

        if (faq is null)
        {
            return;
        }

        faq.EntityStat.NumberOfViews++;

        if (fromFeed)
        {
            faq.EntityStat.NumberOfViewsFromFeed++;
        }

        await uow.SaveChangesAsync();
    }

    public async Task UpdateProjectFileNumberOfDownloadsAsync(int fileId)
    {
        var file = await uow.DbSet<ProjectRelease>().OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.Id == fileId);

        if (file is null)
        {
            return;
        }

        file.NumberOfDownloads++;
        await uow.SaveChangesAsync();
    }

    public async Task UpdateTotalVotesCountAsync(int voteId)
    {
        var vote = await uow.DbSet<Survey>().FindAsync(voteId);

        if (vote is null)
        {
            return;
        }

        vote.TotalRaters = await uow.DbSet<Survey>()
            .Where(x => x.Id == voteId)
            .SelectMany(x => x.SurveyItems)
            .Where(v => !v.IsDeleted)
            .SumAsync(v => (int?)v.TotalSurveys) ?? 0;

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateThisVoteCommentsCountsAsync(int voteId, bool showDeletedItems = false)
    {
        var vote = await uow.DbSet<Survey>().FindAsync(voteId);

        if (vote is null)
        {
            return;
        }

        vote.EntityStat.NumberOfComments = await uow.DbSet<Survey>()
            .Where(x => x.Id == voteId)
            .SelectMany(x => x.Comments)
            .CountAsync(x => x.IsDeleted == showDeletedItems);

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateThisAdvertisementCommentsCountsAsync(int id, bool showDeletedItems = false)
    {
        var advertisement = await uow.DbSet<Advertisement>().FindAsync(id);

        if (advertisement is null)
        {
            return;
        }

        advertisement.EntityStat.NumberOfComments = await uow.DbSet<Advertisement>()
            .Where(x => x.Id == id)
            .SelectMany(x => x.Comments)
            .CountAsync(x => x.IsDeleted == showDeletedItems);

        await uow.SaveChangesAsync();
    }

    public async Task UpdateNumberOfAdvertisementsOfActiveUsersAsync(bool showDeletedItems = false)
    {
        var now = DateTime.UtcNow;

        foreach (var user in uow.DbSet<User>().Where(x => x.IsActive && x.UserStat.NumberOfAdvertisements > 0))
        {
            user.UserStat.NumberOfAdvertisements = await GetUserStatNumberOfAdvertisementsAsync(user,
                showDeletedItems, now);
        }

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateAllCourseTagsInUseCountsAsync(IList<string> tags, bool onlyIsActive = true)
    {
        var listOfActualTagNames = await uow.DbSet<CourseTag>().Where(x => tags.Contains(x.Name)).ToListAsync();

        foreach (var tag in listOfActualTagNames)
        {
            tag.InUseCount = await uow.DbSet<CourseTag>()
                .Where(x => x.IsDeleted != onlyIsActive && x.Id == tag.Id)
                .SelectMany(x => x.AssociatedEntities)
                .CountAsync(x => x.IsDeleted != onlyIsActive);
        }

        await uow.SaveChangesAsync();
    }

    public async Task UpdateNumberOfCourseTopicsStatAsync(int courseId)
    {
        var course = await uow.DbSet<Course>().FindAsync(courseId);

        if (course is null)
        {
            return;
        }

        course.NumberOfTopics = await uow.DbSet<Course>()
            .Where(x => x.Id == courseId && !x.IsDeleted)
            .SelectMany(x => x.CourseTopics)
            .CountAsync(x => !x.IsDeleted && x.IsMainTopic);

        course.NumberOfHelperTopics = await uow.DbSet<Course>()
            .Where(x => x.Id == courseId && !x.IsDeleted)
            .SelectMany(x => x.CourseTopics)
            .CountAsync(x => !x.IsDeleted && !x.IsMainTopic);

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateThisCourseTopicCommentsCountsAsync(int postId)
    {
        var topic = await uow.DbSet<CourseTopic>().FindAsync(postId);

        if (topic is null)
        {
            return;
        }

        topic.EntityStat.NumberOfComments = await uow.DbSet<CourseTopic>()
            .Where(x => x.Id == postId && !x.IsDeleted)
            .SelectMany(x => x.Comments)
            .CountAsync(x => !x.IsDeleted);

        await uow.SaveChangesAsync();
    }

    public async Task UpdateCourseNumberOfTopicsCommentsAsync(int courseId)
    {
        var course = await uow.DbSet<Course>().FindAsync(courseId);

        if (course is null)
        {
            return;
        }

        course.NumberOfComments = await uow.DbSet<CourseTopic>()
            .Where(x => x.CourseId == courseId && !x.IsDeleted)
            .SelectMany(x => x.Comments)
            .CountAsync(x => !x.IsDeleted);

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateThisCourseQuestionsCountsAsync(int courseId)
    {
        var course = await uow.DbSet<Course>().FindAsync(courseId);

        if (course is null)
        {
            return;
        }

        course.NumberOfQuestions = await uow.DbSet<Course>()
            .Where(x => x.Id == courseId && !x.IsDeleted)
            .SelectMany(x => x.CourseQuestions)
            .CountAsync(x => !x.IsDeleted);

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateThisCourseQuestionCommentsCountsAsync(int questionId)
    {
        var courseQuestion = await uow.DbSet<CourseQuestion>().FindAsync(questionId);

        if (courseQuestion is null)
        {
            return;
        }

        courseQuestion.EntityStat.NumberOfComments = await uow.DbSet<CourseQuestion>()
            .Where(x => x.Id == questionId && !x.IsDeleted)
            .SelectMany(x => x.Comments)
            .CountAsync(x => !x.IsDeleted);

        await uow.SaveChangesAsync();
    }

    public async Task UpdateNumberOfCourseQuestionsCommentsAsync(int courseId)
    {
        var course = await uow.DbSet<Course>().FindAsync(courseId);

        if (course is null)
        {
            return;
        }

        course.NumberOfQuestionsComments = await uow.DbSet<CourseQuestion>()
            .Where(x => x.CourseId == courseId && !x.IsDeleted)
            .SelectMany(x => x.Comments)
            .CountAsync(x => !x.IsDeleted);

        await uow.SaveChangesAsync();
    }

    public async Task<CoursesStatModel> GetCoursesStatAsync(bool onlyActives = true)
        => new()
        {
            CoursesAdditinalTopicsNumber =
                await uow.DbSet<Course>()
                    .SelectMany(x => x.CourseTopics)
                    .CountAsync(x => x.IsDeleted != onlyActives && !x.IsMainTopic),
            CoursesNumber =
                await uow.DbSet<Course>()
                    .CountAsync(x => x.IsDeleted != onlyActives && x.IsReadyToPublish == onlyActives),
            CoursesTagsNumber = await uow.DbSet<CourseTag>().CountAsync(x => x.IsDeleted != onlyActives),
            CoursesTopicsNumber = await uow.DbSet<Course>()
                .SelectMany(x => x.CourseTopics)
                .CountAsync(x => x.IsDeleted != onlyActives && x.IsMainTopic)
        };

    public async Task<ProjectsStatModel> GetProjectsStatAsync(bool showDeletedItems = false)
        => new()
        {
            ProjectsFilesNumber =
                await uow.DbSet<ProjectRelease>()
                    .Include(x => x.Project)
                    .CountAsync(x => x.IsDeleted != showDeletedItems && x.Project.IsDeleted == showDeletedItems),
            ProjectsIssuesNumber =
                await uow.DbSet<ProjectIssue>()
                    .Include(x => x.Project)
                    .AsNoTracking()
                    .CountAsync(x => x.IsDeleted == showDeletedItems && x.Project.IsDeleted == showDeletedItems),
            ProjectsNumber = await uow.DbSet<Project>().CountAsync(x => x.IsDeleted == showDeletedItems),
            ProjectsIssuesCommentsNumber =
                await uow.DbSet<ProjectIssueComment>()
                    .Include(x => x.Parent)
                    .CountAsync(x => x.IsDeleted == showDeletedItems && x.Parent.IsDeleted == showDeletedItems),
            ProjectsTagsNumber = await uow.DbSet<ProjectTag>().CountAsync(x => x.IsDeleted != showDeletedItems)
        };

    public async Task<ProjectsStatModel> GetProjectStatAsync(int projectId, bool showDeletedItems = false)
        => new()
        {
            ProjectsFilesNumber =
                await uow.DbSet<ProjectRelease>()
                    .CountAsync(x => x.IsDeleted != showDeletedItems && x.ProjectId == projectId),
            ProjectsIssuesNumber =
                await uow.DbSet<ProjectIssue>()
                    .AsNoTracking()
                    .CountAsync(x => x.IsDeleted == showDeletedItems && x.ProjectId == projectId),
            ProjectsNumber = 1
        };

    public Task<int> GetTodayBirthdayCountAsync()
    {
        var day = DateTime.UtcNow.Day;
        var month = DateTime.UtcNow.Month;

        return uow.DbSet<User>()
            .AsNoTracking()
            .CountAsync(x => x.DateOfBirth.HasValue && x.IsActive && x.DateOfBirth.Value.Day == day &&
                             x.DateOfBirth.Value.Month == month);
    }

    public Task<List<User>> GetTodayBirthdayListAsync()
    {
        var day = DateTime.UtcNow.Day;
        var month = DateTime.UtcNow.Month;

        return uow.DbSet<User>()
            .Include(x => x.Roles)
            .Include(x => x.UserSocialNetwork)
            .AsNoTracking()
            .Where(x => x.DateOfBirth.HasValue && x.IsActive && x.DateOfBirth.Value.Day == day &&
                        x.DateOfBirth.Value.Month == month)
            .ToListAsync();
    }

    public async Task UpdateUserRatingsAsync(User user)
    {
        if (user is null)
        {
            return;
        }

        var userId = user.Id;

        user.Rating.TotalRaters =
            await uow.DbSet<ParentReactionEntity>().CountAsync(userRating => userRating.ForUserId == userId);

        if (user.Rating.TotalRaters == 0)
        {
            return;
        }

        user.Rating.TotalRating = await uow.DbSet<ParentReactionEntity>()
            .Where(userRating => userRating.ForUserId == userId)
            .SumAsync(userRating => (int?)userRating.Reaction) ?? 0;

        user.Rating.AverageRating = (decimal)user.Rating.TotalRating / user.Rating.TotalRaters;

        await uow.SaveChangesAsync();
    }

    public async Task<AgeStatModel?> GetAverageAgeAsync()
    {
        var users = await uow.DbSet<User>()
            .AsNoTracking()
            .Where(x => x.DateOfBirth.HasValue && x.IsActive)
            .OrderBy(x => x.DateOfBirth)
            .ToListAsync();

        var count = users.Count;

        if (count == 0)
        {
            return null;
        }

        var sum = users.Where(user => user.DateOfBirth != null).Sum(user => (int?)user.DateOfBirth!.Value.GetAge()) ??
                  0;

        return new AgeStatModel
        {
            AverageAge = sum / count,
            MaxAgeUser = users[index: 0],
            MinAgeUser = users[^1],
            UsersCount = count
        };
    }

    public async Task RecalculateAllAdvertisementTagsInUseCountsAsync(ICollection<AdvertisementTag> tags,
        bool showActives = true,
        bool showDeletedItems = false)
    {
        ArgumentNullException.ThrowIfNull(tags);
        var now = DateTime.UtcNow;

        foreach (var tag in tags)
        {
            tag.InUseCount = await uow.DbSet<AdvertisementTag>()
                .Where(x => x.IsDeleted != showActives && x.Id == tag.Id)
                .SelectMany(x => x.AssociatedEntities)
                .CountAsync(x => x.IsDeleted == showDeletedItems && (!x.DueDate.HasValue || x.DueDate.Value >= now));
        }

        await uow.SaveChangesAsync();
    }

    public async Task RecalculateAllAdvertisementTagsInUseCountsAsync(bool onlyInUseItems,
        bool showActives = true,
        bool showDeletedItems = false)
    {
        var now = DateTime.UtcNow;

        var advertisementTags = uow.DbSet<AdvertisementTag>().AsQueryable();

        if (onlyInUseItems)
        {
            advertisementTags = advertisementTags.Where(x => x.InUseCount > 0);
        }

        var tags = await advertisementTags.ToListAsync();

        foreach (var tag in tags)
        {
            tag.InUseCount = await uow.DbSet<AdvertisementTag>()
                .Where(x => x.IsDeleted != showActives && x.Id == tag.Id)
                .SelectMany(x => x.AssociatedEntities)
                .CountAsync(x => x.IsDeleted == showDeletedItems && (!x.DueDate.HasValue || x.DueDate.Value >= now));
        }

        await uow.SaveChangesAsync();
    }

    private Task<int> GetNumberOfQuestionsAsync()
        => uow.DbSet<StackExchangeQuestion>().CountAsync(x => !x.IsAnswered && !x.IsDeleted);

    private Task<int> GetNumberOfBacklogsAsync()
        => uow.DbSet<Backlog>().CountAsync(x => !x.IsDeleted && !x.DoneByUserId.HasValue);

    private Task<int> GetNumberOfLearningPathsAsync() => uow.DbSet<LearningPath>().CountAsync(x => !x.IsDeleted);

    private Task<int> GetNumberOfJobsSeekersAsync() => uow.DbSet<User>().CountAsync(x => x.IsActive && x.IsJobsSeeker);

    private Task<int> GetNumberOfComingSoonAsync()
        => uow.DbSet<BlogPostDraft>().CountAsync(x => !x.IsConverted && x.User!.UserStat.NumberOfPosts > 0);

    private Task<int> GetNumberOfAdsAsync()
    {
        var now = DateTime.UtcNow;

        return uow.DbSet<Advertisement>()
            .CountAsync(x => !x.IsDeleted && (!x.DueDate.HasValue || x.DueDate.Value >= now));
    }

    private Task<int> GetNumberOfCoursesAsync()
        => uow.DbSet<Course>().CountAsync(x => !x.IsDeleted && x.IsReadyToPublish);

    private Task<int> GetNumberOfProjectsAsync() => uow.DbSet<Project>().CountAsync(x => !x.IsDeleted);

    private Task<int> GetNumberOfVotesAsync() => uow.DbSet<Survey>().CountAsync(x => !x.IsDeleted);

    private static List<Browser> BrowsersList() => new();

    private async Task UpdateUserStatAsync(bool showDeletedItems, User user)
    {
        var now = DateTime.UtcNow;

        user.UserStat.NumberOfStackExchangeQuestions =
            await GetUserStatNumberOfStackExchangeQuestionsAsync(user, showDeletedItems);

        user.UserStat.NumberOfStackExchangeQuestionsComments =
            await GetUserStatStackExchangeQuestionsCommentsAsync(user, showDeletedItems);

        user.UserStat.NumberOfComments = await GetUserStatNumberOfCommentsAsync(user, showDeletedItems);

        user.UserStat.NumberOfPosts = await GetUserStatNumberOfPostsAsync(user, showDeletedItems);

        user.UserStat.NumberOfLinks = await GetUserStatNumberOfLinksAsync(user, showDeletedItems);

        user.UserStat.NumberOfProjects = await GetUserStatNumberOfProjectsAsync(showDeletedItems, user);

        user.UserStat.NumberOfProjectsFeedbacks =
            await GetUserStatNumberOfProjectsFeedbacksAsync(user, showDeletedItems);

        user.UserStat.NumberOfProjectsComments = await GetUserStatNumberOfProjectsCommentsAsync(user, showDeletedItems);

        user.UserStat.NumberOfLinksComments = await GetUserStatNumberOfLinksCommentsAsync(user, showDeletedItems);

        user.UserStat.NumberOfSurveys = await GetUserStatNumberOfSurveysAsync(user, showDeletedItems);

        user.UserStat.NumberOfSurveyComments = await GetUserStatNumberOfSurveyCommentsAsync(user, showDeletedItems);

        user.UserStat.NumberOfAdvertisements = await GetUserStatNumberOfAdvertisementsAsync(user,
            showDeletedItems, now);

        user.UserStat.NumberOfAdvertisementComments =
            await GetUserStatNumberOfAdvertisementCommentsAsync(user, showDeletedItems, now);

        user.UserStat.NumberOfCourses = await UserStatNumberOfCoursesAsync(user, showDeletedItems);

        user.UserStat.NumberOfBacklogs = await GetUserStatNumberOfBacklogsAsync(user, showDeletedItems);
    }

    private Task<int> GetUserStatStackExchangeQuestionsCommentsAsync(User user, bool showDeletedItems)
        => uow.DbSet<StackExchangeQuestionComment>()
            .Include(x => x.Parent)
            .CountAsync(x => x.UserId == user.Id && x.IsDeleted == showDeletedItems &&
                             x.Parent.IsDeleted == showDeletedItems);

    private Task<int> GetUserStatNumberOfStackExchangeQuestionsAsync(User user, bool showDeletedItems)
        => uow.DbSet<StackExchangeQuestion>().CountAsync(x => x.UserId == user.Id && x.IsDeleted == showDeletedItems);

    private Task<int> GetUserStatNumberOfBacklogsAsync(User user, bool showDeletedItems)
        => uow.DbSet<Backlog>().CountAsync(x => x.UserId == user.Id && x.IsDeleted == showDeletedItems);

    private Task<int> UserStatNumberOfCoursesAsync(User user, bool showDeletedItems)
        => uow.DbSet<Course>()
            .CountAsync(x => x.UserId == user.Id && x.IsDeleted == showDeletedItems && x.IsReadyToPublish);

    private Task<int> GetUserStatNumberOfAdvertisementCommentsAsync(User user, bool showDeletedItems, DateTime now)
        => uow.DbSet<AdvertisementComment>()
            .Include(x => x.Parent)
            .CountAsync(x => x.UserId == user.Id && x.IsDeleted == showDeletedItems &&
                             x.Parent.IsDeleted == showDeletedItems &&
                             (!x.Parent.DueDate.HasValue || x.Parent.DueDate.Value >= now));

    private Task<int> GetUserStatNumberOfAdvertisementsAsync(User user, bool showDeletedItems, DateTime now)
        => uow.DbSet<Advertisement>()
            .CountAsync(x
                => x.UserId == user.Id && x.IsDeleted == showDeletedItems &&
                   (!x.DueDate.HasValue || x.DueDate.Value >= now));

    private Task<int> GetUserStatNumberOfSurveyCommentsAsync(User user, bool showDeletedItems)
        => uow.DbSet<SurveyComment>()
            .Include(x => x.Parent)
            .CountAsync(x
                => x.UserId == user.Id && x.IsDeleted == showDeletedItems && x.Parent.IsDeleted == showDeletedItems);

    private Task<int> GetUserStatNumberOfSurveysAsync(User user, bool showDeletedItems)
        => uow.DbSet<Survey>().CountAsync(x => x.UserId == user.Id && x.IsDeleted == showDeletedItems);

    private Task<int> GetUserStatNumberOfLinksCommentsAsync(User user, bool showDeletedItems)
        => uow.DbSet<DailyNewsItemComment>()
            .Include(x => x.Parent)
            .CountAsync(x
                => x.UserId == user.Id && x.IsDeleted == showDeletedItems && x.Parent.IsDeleted == showDeletedItems);

    private Task<int> GetUserStatNumberOfProjectsCommentsAsync(User user, bool showDeletedItems)
        => uow.DbSet<ProjectIssueComment>()
            .Include(x => x.Parent)
            .CountAsync(x
                => x.UserId == user.Id && x.IsDeleted == showDeletedItems && x.Parent.IsDeleted == showDeletedItems);

    private Task<int> GetUserStatNumberOfProjectsFeedbacksAsync(User user, bool showDeletedItems)
        => uow.DbSet<ProjectIssue>()
            .Include(x => x.Project)
            .CountAsync(x
                => x.UserId == user.Id && x.IsDeleted == showDeletedItems && x.Project.IsDeleted == showDeletedItems);

    private Task<int> GetUserStatNumberOfProjectsAsync(bool showDeletedItems, User user)
        => uow.DbSet<Project>().CountAsync(x => x.UserId == user.Id && x.IsDeleted == showDeletedItems);

    private Task<int> GetUserStatNumberOfLinksAsync(User user, bool showDeletedItems)
        => uow.DbSet<DailyNewsItem>().CountAsync(x => x.UserId == user.Id && x.IsDeleted == showDeletedItems);

    private Task<int> GetUserStatNumberOfPostsAsync(User user, bool showDeletedItems)
        => uow.DbSet<BlogPost>().CountAsync(x => x.UserId == user.Id && x.IsDeleted == showDeletedItems);

    private Task<int> GetUserStatNumberOfCommentsAsync(User user, bool showDeletedItems)
        => uow.DbSet<BlogPostComment>()
            .Include(x => x.Parent)
            .CountAsync(x
                => x.UserId == user.Id && x.IsDeleted == showDeletedItems && x.Parent.IsDeleted == showDeletedItems);
}
