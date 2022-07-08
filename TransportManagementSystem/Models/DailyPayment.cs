using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransportManagementSystem.Models
{
    public class DailyPayment
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        public string Date { get; set; }
        public int IsPaid { get; set; }
    }
}