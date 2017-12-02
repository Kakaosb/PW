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
        [DisplayName("Sender")]
        public int SenderId { get; set; }

        [Required]
        [DisplayName("Recipient")]
        public int RecipientId { get; set; }

        [Required]
        public decimal Sum { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("Create Time")]
        public DateTime CreateDateTime { get; set; }

        [DisplayName("Total (Sender)")]
        public decimal TotalSender { get; set; }

        [DisplayName("Total (Recipient)")]
        public decimal TotalRecipient { get; set; }
    }
}