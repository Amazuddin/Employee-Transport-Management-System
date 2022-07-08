using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransportManagementSystem.Models
{
    public class RouteInformation
    {
        [Key]
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int DriverInfoId { get; set; }
        public string StartTime { get; set; }
    }
}