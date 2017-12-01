using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ParrotWings.API.Models.EntityMetaData
{
    public class UserMetaData
    {
        [StringLength(55, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 2)]
        public string Name { get; set; }
    }
}