using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransportManagementSystem.Models
{
    public class DriverInfo
    {
        [Key]
        public int Id { get; set; }
        public string DriverName { get; set; }
        public string Phone { get; set; }
        public string VehicleNo { get; set; }
        public int RouteId { get; set; }
        public string Image { get; set; }

    }
}