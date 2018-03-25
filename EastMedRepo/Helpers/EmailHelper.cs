using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace EastMedRepo.Helpers
{
    public class EmailHelper
    {
        public static void SendEMail(string emailid, string subject, string body)
        {
            //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            //client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            //client.EnableSsl = true;
            //client.Host = "smtp.gmail.com";
            //client.Port = 587;            
            //System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("xxxxx@gmail.com", "password");
            //client.UseDefaultCredentials = false;
            //client.Credentials = credentials;
            //System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            //msg.From = new MailAddress("xxxxx@gmail.com");
            //msg.To.Add(new MailAddress(emailid));

            //msg.Subject = subject;
            //msg.IsBodyHtml = true;
            //msg.Body = body;

            MailMessage mail = new MailMessage();
            //set the addresses 
            mail.From = new MailAddress("info@emrecavunt.info"); 
            mail.To.Add(emailid); 
            //set the content 
            mail.Subject = subject; 
            mail.Body = body; 
                                                                                                //send the message 
            SmtpClient smtp = new SmtpClient("mail.emrecavunt.info"); 

            NetworkCredential Credentials = new NetworkCredential("info@emrecavunt.info", "your password!"); 
            smtp.Credentials = Credentials;
            smtp.Send(mail);           
        }
    }
}