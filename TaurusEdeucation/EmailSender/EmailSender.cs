using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace TaurusEdeucation.Email_Sender
{
  public class EmailSender
  {
    SmtpClient SmtpServer;
    public EmailSender(string to, string subject, string text)
    {
      SmtpServer = new SmtpClient
      {
        Host = config.AppSettings.EmailSmtp,
        Port = config.AppSettings.EmailPort,
        EnableSsl = true,
        DeliveryMethod = SmtpDeliveryMethod.Network,
        UseDefaultCredentials = false,
        Credentials = new System.Net.NetworkCredential(config.AppSettings.EmailFrom, config.AppSettings.EmailPassword)
      };
      
      SendEmail(to, subject, text);
    }
    private void SendEmail(string to, string subject, string text)
    {
      using (MailMessage mail = new MailMessage())
      {
        mail.From = new MailAddress(config.AppSettings.EmailFrom);
        mail.To.Add(to);
        mail.Subject = subject;
        mail.Body = text;
        mail.IsBodyHtml = true;

        SmtpServer.Send(mail);
      }
    }
  }
}
