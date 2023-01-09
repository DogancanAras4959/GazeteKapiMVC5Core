using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;

namespace GazeteKapiMVC5Core.WEB.ViewModels.News
{
    public class NewsBaseViewModelWeb
    {
        public string MetaTitle { get; set; }
        public string Title { get; set; }
        public bool IsTitle { get; set; }
        public string Spot { get; set; }
        public string NewsContent { get; set; }
        public int RowNo { get; set; }
        public string VideoSlug { get; set; }
        public string VideoUploaded { get; set; }
        public bool doublePlace { get; set; }
        public bool fourthPlace { get; set; }
        public bool isArchive { get; set; }
        public int Sorted { get; set; }
        public string Image { get; set; }
        public string Tag { get; set; }
        public int ParentNewsId { get; set; }
        public DateTime UpdatedTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime PublishedTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsLock { get; set; }
        public bool IsOpenNotifications { get; set; }
        public bool IsSlide { get; set; }
        public bool IsCommentActive { get; set; }
        public int Views { get; set; }
        public string Sound { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public int GuestId { get; set; }
        public int PublishTypeId { get; set; }

        public Guest guest { get; set; }
        public Users users { get; set; }
        public DataAccessLayer.Models.Categories categories { get; set; }
        public PublishType publishtype { get; set; }
    }
}
