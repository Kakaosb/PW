using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ParrotWings.API.Models.EntityMetaData;

namespace ParrotWings.API.Models
{
    [Table("TransactionLog")]
    [MetadataType(typeof(TransactionLogMetaData))]
    public class TransactionLog
    {
        [Key]
        public int Id { get; set; }

        public int SenderId { get; set; }
        public virtual User Sender { get; set; }

        [Required]
        public int RecipientId { get; set; }
        public virtual User Recipient { get; set; }

        [Required]
        [Range(1.00, Double.MaxValue)]
        public decimal Sum { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}