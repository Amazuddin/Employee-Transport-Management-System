using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransportManagementSystem.Models
{
    public class PasswordReset
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string ResetCode { get; set; }
    }
}