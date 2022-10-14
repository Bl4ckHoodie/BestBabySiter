using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Best_BabySitter.Models
{
    public class MailManager
    {


        public static void NotifySitters(Object obj)
        {
            if(obj is Advert)
            {
                List<String> emails = SqlDataAccess.getEmails(((Advert)obj).City);
                try
                {
                    string body = "Good Day, We would like to notify you of the recent job, the details are as follows:\n\t Date: "+((Advert)obj).StartDate+" - "+ ((Advert)obj).EndDate+ "\n\t Time: " + ((Advert)obj).StartTime+"-"+ ((Advert)obj).EndTime+ "\n\t Age Range: " + ((Advert)obj).AgeRange + "\n\t Specifications: " + ((Advert)obj).Specification+"\n\n We hope to hear from you soon...";
                    string subject = "New Job Available";
                    foreach(string mail in emails)
                    {
                       if(sendMail(mail, subject,body)==1)
                        {
                            Console.WriteLine("NOTIFY ....{0}", mail);
                        }
                        else
                        {
                            Console.WriteLine("FAILED!! TO NOTIFY....{0}", mail);
                        }
                    }

                }catch(Exception ex)
                {
                    Console.WriteLine("Notification Faild: "+ex.Message);
                }

            }
           
        }
        public static void NotifyParent(Object obj)
        {
            if (obj is Parent)
            {
                List<String> emails = new List<string> { SqlDataAccess.getParentData(((Parent)obj).parent_ID).email};
                try
                {
                    string body = "Good Day, We would like to notify you of the recent job, the details are as follows:\n\t Date: " + ((Advert)obj).StartDate + " - " + ((Advert)obj).EndDate + "\n\t Time: " + ((Advert)obj).StartTime + "-" + ((Advert)obj).EndTime + "\n\t Age Range: " + ((Advert)obj).AgeRange + "\n\t Specifications: " + ((Advert)obj).Specification + "\n\n We hope to hear from you soon...";
                    string subject = "New Job Available";
                    foreach (string mail in emails)
                    {
                        if (sendMail(mail, subject, body) == 1)
                        {
                            Console.WriteLine("NOTIFY ....{0}", mail);
                        }
                        else
                        {
                            Console.WriteLine("FAILED!! TO NOTIFY....{0}", mail);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Notification Faild: "+ex.Message);
                }

            }

        }
        private static int sendMail(string reciever, string subject, string body)
        {
            try
            {

                var senderEmail = new MailAddress("bestbabysitter2022@gmail.com", "runtime");
                var receiverEmail = new MailAddress(reciever, "Receiver");
                var password = "RuntimeTerror2022";
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
                    Subject = subject,
                    Body = body
                })

                {
                    smtp.Send(mess);
                    return 1;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}", ex.Message);
                return -1;
            }
        }
    }
   
}