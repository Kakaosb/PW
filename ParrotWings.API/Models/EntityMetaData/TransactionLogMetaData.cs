using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ParrotWings.API.Models.EntityMetaData
{
    public class TransactionLogMetaData
    {
        [Required]
        [DisplayName("Sender")]
        public int SenderId { get; set; }

        [Required]
        [DisplayName("Recipient")]
        public int RecipientId { get; set; }

        [Required]
        public decimal Sum { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        [DisplayName("Create Time")]
        public DateTime CreateDateTime { get; set; }
    }
}