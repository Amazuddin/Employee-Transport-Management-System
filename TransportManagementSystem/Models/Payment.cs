using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransportManagementSystem.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int Ammount { get; set; }
        public string PaymentDate { get; set; }
    }
}