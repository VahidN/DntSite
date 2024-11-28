using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Models;

namespace DntSite.Web.Features.Courses.Services.Contracts;

public interface ICoursesEmailsService : IScopedService
{
    public Task NewCourseEmailToAdminsAsync(int id, CourseModel data);

    public Task NewCourseEmailToUserAsync(int id, CourseModel data, int userId);

    public Task AccessAddedEmailToUserAsync(Course data, string userName);

    public Task AccessAddedEmailToAdminAsync(Course data, string userName, string operation);

    public Task WriteCourseTopicSendEmailAsync(CourseTopic data);

    public Task CourseTopicCommentSendEmailToAdminsAsync(CourseTopicComment comment);

    public Task CourseTopicCommentSendEmailToWritersAsync(CourseTopicComment comment);

    public Task CourseTopicCommentSendEmailToPersonAsync(CourseTopicComment comment);

    public Task NewQuestionSendEmailToAdminsAsync(CourseQuestion courseQuestion);

    public Task NewQuestionSendEmailToCourseWritersAsync(CourseQuestion data);

    public Task CourseQuestionCommentSendEmailToAdminsAsync(CourseQuestionComment comment);

    public Task CourseQuestionCommentSendEmailToWritersAsync(CourseQuestionComment comment);

    public Task CourseQuestionCommentSendEmailToPersonAsync(CourseQuestionComment comment);
}
