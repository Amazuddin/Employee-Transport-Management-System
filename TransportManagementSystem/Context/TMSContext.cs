using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TransportManagementSystem.Models;

namespace TransportManagementSystem.Context
{
    public class TMSContext:DbContext
    {
        public DbSet<DriverInfo> Driver { get; set; }  
        public DbSet<Routes> Route { get; set; }
        public DbSet<RouteInformation> RoutesInformation { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<PasswordReset> PasswordResets { get; set; }
      
    }
}