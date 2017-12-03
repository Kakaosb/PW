using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParrotWings.API.Models.ResponseModels
{
    public class TransactionResponse
    {
        public decimal Sum { get; set; }

        public decimal Total { get; set; }

        public string TransferType { get; set; }

        public string UserName { get; set; }

        public string OperationDate { get; set; }
    }
}