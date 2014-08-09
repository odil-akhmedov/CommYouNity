using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using CommYouNity;

namespace CommYouNity.Models
{
    public class EmaiAlertNotification
    {
        private CommunityDataModel db = new CommunityDataModel();
        public String from{ get;set; }
        public String to {get; set;}
        public List<String> EmailList{ get;set; }
        public List<String> NameList  {get; set;} 
        //public void EmailTaskAlert();

    }      

    }
//    public void EmailTaskAlert(){
//    var x1 = db.Locations;
//            var x2 = x1.Where(i => i.Id == locationTask.LocationId);
//            var x3 = x2.Select(e => e.Email);
//            var fromLocationAddress = x3.FirstOrDefault();

//            var x4 = x2.Select(n => n.Name);
//            var fromLocationName = x4.FirstOrDefault();

//            var x5 = db.Members;
//            var x6 = db.Communities;

//            var fromAddress = new MailAddress(fromLocationAddress, fromLocationName);
//            string fromPassword = db.Locations
//                .Where(i => i.Id == locationTask.LocationId)
//                .Select(p => p.Password).
//                FirstOrDefault()
//                .ToString();
//            string subject = locationTask.Name;
//            string taskType;
//            if (locationTask.Flag == true)
//                taskType = "task";
//            else
//                taskType = "alert";

//            string body = "Location \"" + fromLocationName + "\" posted new " + taskType + ":\n";
//            body += "You can see that on <a href='/location'>";
//            var toCommunityEmailList = db.Communities.Where(i => i.LocationId == locationTask.LocationId).Select(e => e.Email).ToList();
//            var toNameList = db.Communities.Where(i => i.LocationId == locationTask.LocationId).Select(n => n.OfficerName).ToList();

//            for (int index = 0; index < toCommunityEmailList.Count; index++ )
//            {   
//                string toEmail = toCommunityEmailList[index];
//                string toName = toNameList[index];
//                if (toEmail != null)
//                {

//                    var toAddress = new MailAddress(toEmail, toName);
//                    var smtp = new SmtpClient
//                    {
//                        Host = "smtp.gmail.com",
//                        Port = 587,
//                        EnableSsl = true,
//                        DeliveryMethod = SmtpDeliveryMethod.Network,
//                        UseDefaultCredentials = false,
//                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
//                    };
//                    using (var message = new MailMessage(fromAddress, toAddress)
//                    {
//                        Subject = subject,
//                        Body = body
//                    })
//                    {
//                        smtp.Send(message);
//                    }
//                }
//            }
//}
//}

