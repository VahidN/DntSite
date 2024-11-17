using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.UserProfiles.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DntSite.Web.Features.Stats.Services;

public class YearStatService(IUnitOfWork uow) : IEfYearStatService
{
    public async Task<AnnualStatisticsInfo> GetYearArticlesAsync(string persianYear)
    {
        var posts = uow.DbSet<BlogPost>()
            .AsNoTracking()
            .Where(x => x.Audit.CreatedAtPersian.StartsWith(persianYear) && !x.IsDeleted);

        return new AnnualStatisticsInfo
        {
            Item = AnnualStatisticsItem.Articles,
            AuthorsCount = await posts.Where(x => x.User != null).Select(x => x.User!.Id).Distinct().CountAsync(),
            Count = await posts.CountAsync(),
            Feedbacks = await posts.SumAsync(x => (int?)x.EntityStat.NumberOfComments) ?? 0
        };
    }

    public async Task<IList<SelectListItem>> PersianYearsOfWritingAsync()
    {
        var years = await uow.DbSet<BlogPost>()
            .AsNoTracking()
            .Select(x => x.Audit.CreatedAtPersian.Substring(0, 4))
            .Distinct()
            .ToListAsync();

        return years.Select(year => new SelectListItem
            {
                Text = year,
                Value = year
            })
            .ToList();
    }

    public async Task<AnnualStatisticsInfo> GetRegisteredUsersAsync(string persianYear)
    {
        var users = uow.DbSet<User>()
            .AsNoTracking()
            .Where(x => x.Audit.CreatedAtPersian.StartsWith(persianYear) && x.IsActive);

        return new AnnualStatisticsInfo
        {
            Item = AnnualStatisticsItem.RegisteredUsers,
            AuthorsCount = 0,
            Count = await users.CountAsync(),
            Feedbacks = 0
        };
    }

    public async Task<IList<AnnualStatisticsInfo>> GetAnnualStatisticsAsync(string persianYear)
        =>
        [
            await GetYearArticlesAsync(persianYear), await GetYearCoursesAsync(persianYear),
            await GetProjectsAsync(persianYear), await GetLinksAsync(persianYear),
            await GetSurveysAsync(persianYear), await GetRegisteredUsersAsync(persianYear)
        ];

    public async Task<AnnualStatisticsInfo> GetSurveysAsync(string persianYear)
    {
        var surveys = uow.DbSet<Survey>()
            .AsNoTracking()
            .Where(x => x.Audit.CreatedAtPersian.StartsWith(persianYear) && !x.IsDeleted);

        return new AnnualStatisticsInfo
        {
            Item = AnnualStatisticsItem.Surveys,
            AuthorsCount = await surveys.Where(x => x.User != null).Select(x => x.User!.Id).Distinct().CountAsync(),
            Count = await surveys.CountAsync(),
            Feedbacks = await surveys.SumAsync(x => (int?)x.EntityStat.NumberOfComments) ?? 0
        };
    }

    public async Task<AnnualStatisticsInfo> GetLinksAsync(string persianYear)
    {
        var links = uow.DbSet<DailyNewsItem>()
            .AsNoTracking()
            .Where(x => x.Audit.CreatedAtPersian.StartsWith(persianYear) && !x.IsDeleted);

        return new AnnualStatisticsInfo
        {
            Item = AnnualStatisticsItem.Links,
            AuthorsCount = await links.Where(x => x.User != null).Select(x => x.User!.Id).Distinct().CountAsync(),
            Count = await links.CountAsync(),
            Feedbacks = await links.SumAsync(x => (int?)x.EntityStat.NumberOfComments) ?? 0
        };
    }

    public async Task<AnnualStatisticsInfo> GetProjectsAsync(string persianYear)
    {
        var projects = uow.DbSet<Project>()
            .AsNoTracking()
            .Where(x => x.Audit.CreatedAtPersian.StartsWith(persianYear) && !x.IsDeleted);

        return new AnnualStatisticsInfo
        {
            Item = AnnualStatisticsItem.Projects,
            AuthorsCount = await projects.Where(x => x.User != null).Select(x => x.User!.Id).Distinct().CountAsync(),
            Count = await projects.CountAsync(),
            Feedbacks = (await projects.SumAsync(x => (int?)x.NumberOfIssues) ?? 0) +
                        (await projects.SumAsync(x => (int?)x.NumberOfIssuesComments) ?? 0)
        };
    }

    public async Task<AnnualStatisticsInfo> GetYearCoursesAsync(string persianYear)
    {
        var courses = uow.DbSet<Course>()
            .AsNoTracking()
            .Where(x => x.Audit.CreatedAtPersian.StartsWith(persianYear) && x.IsReadyToPublish);

        return new AnnualStatisticsInfo
        {
            Item = AnnualStatisticsItem.Courses,
            AuthorsCount = await courses.Where(x => x.User != null).Select(x => x.User!.Id).Distinct().CountAsync(),
            Count = await courses.CountAsync(),
            Feedbacks = (await courses.SumAsync(x => (int?)x.NumberOfComments) ?? 0) +
                        (await courses.SumAsync(x => (int?)x.NumberOfQuestions) ?? 0) +
                        (await courses.SumAsync(x => (int?)x.NumberOfQuestionsComments) ?? 0)
        };
    }
}
