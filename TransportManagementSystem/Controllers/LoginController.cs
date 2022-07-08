using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TransportManagementSystem.Context;
using TransportManagementSystem.Models;

namespace TransportManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        private TMSContext db = new TMSContext();

        //
        // GET: /Login/
        public ActionResult Register()
        {
          
            ViewBag.Register = "active";
            if (Session["EmployeeId"] != null)
            {
                return RedirectToAction("Index", "Tranport");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Register(Employee employee, HttpPostedFileBase Image)
        {
            ViewBag.Register = "active";
            if (Image != null && Image.ContentLength > 0)
            {
                try
                {
                    string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(Image.FileName);
                    string uploadUrl = Server.MapPath("~/picture");
                    Image.SaveAs(Path.Combine(uploadUrl, fileName));
                    employee.Image = "picture/" + fileName;
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "ERROR:" + ex.Message.ToString();
                }
            }
            int k = db.Employees.Count(r => r.Email == employee.Email);
            if (k == 0)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
            }
            else
            {
                ViewBag.Error = "Another Employee is Registered with this Email";
                return View();
            }

            return RedirectToAction("Login");
        }
        public ActionResult Login(string id)
        {
            ViewBag.Login = "active";
            if (Session["EmployeeId"] != null)
            {
                return RedirectToAction("Index", "Tranport");
            }
            ViewBag.Error = id;
            return View();
        }

        public ActionResult LoginUser(string username, string password)
        {
            Employee hs = db.Employees.FirstOrDefault(r => r.Email == username && r.Password == password);
            if (hs != null)
            {
                Session["EmployeeId"] = hs.Id;
                Session["EmployeeEmail"] = hs.Email;
                return RedirectToAction("Index", "Tranport");
            }
            else
                return RedirectToAction("Login", "Login", new { id = "Error" });

        }
        public ActionResult Logout()
        {
            Session["EmployeeId"] = null;
            return RedirectToAction("Login");
        }

        public ActionResult AdminLogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogIn(Admin admin)
        {
            string password = admin.Password;
            string email = admin.Email;
            using (var db = new TMSContext())
            {

                var p = db.Admins.Where(c => c.Email == email && c.Password == password).Select(c => new { c.Id, c.Email }).ToList();
                if (p.Any())
                {
                    foreach (var k in p)
                    {
                        Session["AdminId"] = k.Id;
                        Session["AdminEmail"] = k.Email;
                    }
                    return RedirectToAction("Index", "Tranport");
                }
                else
                {
                    ViewBag.Error = "Login Failed";
                }

            }
            return View();
        }

        public ActionResult AdminLogout()
        {
            Session["AdminId"] = null;
            Session["AdminEmail"] = null;
            return RedirectToAction("AdminLogIn", "Login");
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(string email)
        {
            using (var ctx = new TMSContext())
            {
                var employee = ctx.Employees.FirstOrDefault(r => r.Email == email);

                if (employee != null)
                {
                    var r = new Random();
                    var resetCode = r.Next(1000, 9999).ToString() + employee.Id;
                    var Messages = "Your verification code is: " + resetCode;

                    var searchReset = ctx.PasswordResets.FirstOrDefault(i => i.Email == employee.Email && i.UserId == employee.Id);
                    if(searchReset != null)
                    {
                        searchReset.ResetCode = resetCode;
                        ctx.SaveChanges();
                    }
                    else
                    {
                        PasswordReset passreset = new PasswordReset();
                        passreset.ResetCode = resetCode;
                        passreset.Email = employee.Email;
                        passreset.UserId = employee.Id;
                        ctx.PasswordResets.Add(passreset);
                        ctx.SaveChanges();
                    }

                    SendEmail(Messages, email);
                    Session["reset"] = "reset";
                    return RedirectToAction("ResetPassword", "Login");
                }

                return RedirectToAction("ForgetPassword", "Login", new { id = "Error" });
            }
        }

        public void SendEmail(string message, string email)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("medtodoorbgc@gmail.com", "TransportManagement");
                    var receiverEmail = new MailAddress(email, "Receiver");
                    var password = "@a123456z";
                    var body = message;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = "Verification Code",
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }                   
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.ToString();
            }
        }

        public ActionResult ResetPassword()
        {
            if(Session["reset"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
           
        }

        [HttpPost]
        public ActionResult ResetPassword(string resetCode, string password)
        {
            using (var ctx = new TMSContext())
            {
                var searchReset = ctx.PasswordResets.FirstOrDefault(i => i.ResetCode == resetCode);
                if(searchReset != null)
                {
                    var employee = ctx.Employees.FirstOrDefault(r => r.Email == searchReset.Email && r.Id == searchReset.UserId);
                    employee.Password = password;
                    searchReset.ResetCode = null;
                    ctx.SaveChanges();
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    ViewBag.Message = "Varifiaction Code Mismatch!";
                }
            }
            return View();
        }

    }
}