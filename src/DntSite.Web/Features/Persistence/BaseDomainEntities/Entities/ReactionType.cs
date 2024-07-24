namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

public enum ReactionType
{
    ShowList = -2,
    Cancel = -1,
    ThumbsDown = 0, // بیشتر جنبه‌ی احساسی دارد و سبب کسر امتیازی نمی‌شود
    ThumbsUp = 5
}
