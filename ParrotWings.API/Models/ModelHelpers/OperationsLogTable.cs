using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParrotWings.API.Models.ModelHelpers
{
    public class OperationsLogTable
    {
        public decimal Sum { get; set; }
        public string TransferType { get; set; }
        public string OperationDate { get; set; }
        public string UserName { get; set; }
        public decimal Total { get; set; }
    }

    public enum TransferType
    {
        Debit = 1,
        Credit = 2
    }
}