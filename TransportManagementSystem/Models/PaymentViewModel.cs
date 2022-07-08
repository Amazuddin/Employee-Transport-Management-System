using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransportManagementSystem.Models
{
    public class PaymentViewModel
    {
        public string VehicleNo { get; set; }
        public int Amount { get; set; }
        public string PaymentDate { get; set; }
    }
}