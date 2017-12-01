using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParrotWings.API.Models.ModelHelpers
{
    public class OperationsLogTable
    {
        public decimal Sum { get; set; }
        public bool IsDebet{ get; set; }
        public string OperationDate { get; set; }
        public string UserName { get; set; }
    }
}