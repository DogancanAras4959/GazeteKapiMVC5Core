using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CORE.ApplicationCommon.DTOS.MenuDTO.ItemsDto
{
    public class ItemBaseDto
    {
        public string ItemName { get; set; }
        public string slug { get; set; }

        [ForeignKey("type")]
        public int TypeId { get; set; }
        public bool IsActive { get; set; }

        public MenuTypes type { get; set; }
    }
}
