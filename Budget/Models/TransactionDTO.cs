using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Budget
{
    public class TransactionDTO
    {
        public int TransactionId { get; set; }
        public int OperationId { get; set; }
        public string OperationName { get; set; }
        public string OperationIcon { get; set; }
        public float TransactionAmount { get; set; }
        public int TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionRemark { get; set; }
    }
}