using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ParrotWings.API.Models.EntityMetaData;

namespace ParrotWings.API.Models
{
    [Table("User")]
    [MetadataType(typeof(UserMetaData))]
    public sealed class User
    {
        public User()
        {
            TransactionLogs = new HashSet<TransactionLog>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(55, MinimumLength = 2)]
        public string Name { get; set; }
        
        public decimal Balance { get; set; }
      
        public bool Active { get; set; }

        public ICollection<TransactionLog> TransactionLogs { get; set; }
    }
}