namespace DntSite.Web.Features.Advertisements.Models;

public enum JobType
{
    [Display(Name = "دور کاری")] RemoteWorking,

    [Display(Name = "تمام وقت")] FullTime,

    [Display(Name = "پاره وقت")] PartTime
}
