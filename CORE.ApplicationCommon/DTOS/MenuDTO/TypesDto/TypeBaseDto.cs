using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CORE.ApplicationCommon.DTOS.MenuDTO.TypesDto
{
    public class TypeBaseDto
    {
        public string MenuName { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
        public Users user { get; set; }
    }
}
