using AlphaSolutions.Core.Diagnostics.TimeMeasures;
using System.Net;
using System.Net.Mail;

namespace Importer.Facade
{
    class HangfireController
    {
        CategoryFacade cf = new CategoryFacade();
        UrlController uf = new UrlController();
        ProductController pf = new ProductController();
        
        public void StatusMail(string StatusMessage)
        {
            using (new TimeMeasure("StatusMail"))
            {
                string fromaddr = "GengangereneHangFire@gmail.com";
                string toaddr = "adj@alpha-solutions.dk";
                string password = "Pr4ktikH4ngFir3";

                MailMessage msg = new MailMessage();
                msg.Subject = "Your Daily HangfireUpdate";
                msg.From = new MailAddress(fromaddr);
                msg.Body = StatusMessage;
                msg.To.Add(new MailAddress(toaddr));
                //msg.To.Add(new MailAddress("jhe@alpha-solutions.dk"));
                msg.To.Add(new MailAddress("oml@alpha-solutions.dk"));
                //msg.To.Add(new MailAddress("teb@alpha-solutions.dk"));
                //msg.To.Add(new MailAddress("fwp@alpha-solutions.dk"));
                using (SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    UseDefaultCredentials = false,
                    EnableSsl = true
                })
                {
                    NetworkCredential nc = new NetworkCredential(fromaddr, password);
                    smtp.Credentials = nc;
                    smtp.Send(msg);
                }
            }

        }
    }
}
