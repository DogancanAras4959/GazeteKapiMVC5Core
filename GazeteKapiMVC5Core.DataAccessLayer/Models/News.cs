﻿using DOMAIN.DataAccessLayer.Models;
using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class News : IEntity
    {
        public News()
        {

        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Spot { get; set; }
        public string NewsContent { get; set; }
        public int Sorted { get; set; }
        public string Image { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public DateTime? PublishedTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsLock { get; set; }
        public bool IsOpenNotifications { get; set; }
        public bool IsCommentActive { get; set; }
        public bool IsSlide { get; set; }
        public int Views { get; set; }


        [ForeignKey("categories")]
        public int CategoryId { get; set; }

        [ForeignKey("users")]
        public int UserId { get; set; }

        [ForeignKey("guest")]
        public int GuestId { get; set; }

        [ForeignKey("publishtype")]
        public int PublishTypeId { get; set; }


        public virtual Guest guest { get; set; }
        public virtual Users users { get; set; }
        public virtual Categories categories { get; set; }
        public virtual PublishType publishtype { get; set; }
    }
}
