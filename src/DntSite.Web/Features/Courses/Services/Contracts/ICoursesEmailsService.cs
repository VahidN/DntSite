using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Models;

namespace DntSite.Web.Features.Courses.Services.Contracts;

public interface ICoursesEmailsService : IScopedService
{
    Task NewCourseEmailToAdminsAsync(int id, CourseModel data);

    Task NewCourseEmailToUserAsync(int id, CourseModel data, int userId);

    Task AccessAddedEmailToUserAsync(Course data, string userName);

    Task AccessAddedEmailToAdminAsync(Course data, string userName, string operation);

    Task WriteCourseTopicSendEmailAsync(CourseTopic data);

    Task CourseTopicCommentSendEmailToAdminsAsync(CourseTopicComment comment);

    Task CourseTopicCommentSendEmailToWritersAsync(CourseTopicComment comment);

    Task CourseTopicCommentSendEmailToPersonAsync(CourseTopicComment comment);

    Task NewQuestionSendEmailToAdminsAsync(CourseQuestion courseQuestion);

    Task NewQuestionSendEmailToCourseWritersAsync(CourseQuestion data);

    Task CourseQuestionCommentSendEmailToAdminsAsync(CourseQuestionComment comment);

    Task CourseQuestionCommentSendEmailToWritersAsync(CourseQuestionComment comment);

    Task CourseQuestionCommentSendEmailToPersonAsync(CourseQuestionComment comment);
}
