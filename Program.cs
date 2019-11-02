using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Net.Mail;
using System.Configuration;

namespace Refresh_ID
{
    class Program
    {
        static void Main(string[] args)
        {
            Program obj = new Program();
            obj.refreshid();
            obj.performMaintenance();
            obj.NH_ESCC_EMS();
            obj.getWorkItem();
            obj.getDuplicateWorkItems();
        }
        public void refreshid()
        {
            
            if ((DateTime.Now.ToString("hh:mm").Equals("08:00")) || (DateTime.Now.ToString("hh:mm").Equals("09:00")) || (DateTime.Now.ToString("hh:mm").Equals("10:00")))
            {
                DAL temp = new DAL();
                DataSet ds = temp.GetTasks();

                foreach (DataTable table in ds.Tables)
                {

                    foreach (DataRow dr in table.Rows)
                    {
                        if (dr[2].ToString() == "")
                        {
                            Console.WriteLine("still running");
                            //Console.ReadKey();
                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress("noreply@example.com");
                            
                            mail.To.Add("example@example.com");
                            mail.CC.Add("example@example.com");
                            
                            mail.IsBodyHtml = true;
                            mail.Subject = "Refresh Identity Cubes - still running - Terminations May be Delayed";
                            mail.Body = "HI," + "<br /><br />" + "This is an automated email to inform you that the task: Refresh Identity Cubes is still running in IIQ." + "<br /><br />" + "Terminations May be Delayed" + "<br /><br />" + "For any clarifications send an email to example@example.com";

                            SmtpClient client = new SmtpClient();
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;
                            client.UseDefaultCredentials = false;
                            client.Host = ConfigurationManager.AppSettings["MailHost"].ToString();
                            client.Send(mail);
                        }
                        else
                        {
                            Console.WriteLine("Task Completed");
                        }
                    }
                }
            }
        }
        public void performMaintenance()
        {
            DAL temp = new DAL();
            DataSet dsm = temp.GetMaintenance();
            foreach (DataTable table in dsm.Tables)
            {

                foreach (DataRow dr in table.Rows)
                {
                    string Name = dr[0].ToString();
                    string Created = dr[1].ToString();
                    string Completed = dr[2].ToString();
                    string Status = dr[3].ToString();
                    string endTime1 = DateTime.Now.ToString();

                    TimeSpan duration = DateTime.Parse(endTime1).Subtract(DateTime.Parse(Created));
                    if (duration.TotalMinutes >= 30)
                    {
                        MailMessage mail = new MailMessage();
                        mail.From = new MailAddress("noreply@example.com");
                        mail.To.Add("example@example.com");
                        mail.CC.Add("example@example.com");
                        mail.IsBodyHtml = true;
                        if (Name.ToLower() == "perform maintenance")
                        {
                            mail.Subject = "Perform Maintenance - stuck";
                            mail.Body = "HI," + "<br /><br />" + "This is an automated email to inform you that the task: Perform Maintenance is pending for more than thirty minutes in IIQ." + "<br /><br />" + "For any clarifications send an email to example@example.com";
                            
                            SmtpClient client = new SmtpClient();
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;
                            client.UseDefaultCredentials = false;
                            client.Host = ConfigurationManager.AppSettings["MailHost"].ToString();
                            client.Send(mail);
                        }
                        if (Name.ToLower() == "perform maintenance background workflows")
                        {
                            mail.Subject = "Perform Maintenance Background Workflows - stuck";
                            mail.Body = "HI," + "<br /><br />" + "This is an automated email to inform you that the task: Perform Maintenance Background Workflows is pending for more than thirty minutes in IIQ." + "<br /><br />" + "For any clarifications send an email to example@example.com";
                            SmtpClient client = new SmtpClient();
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;
                            client.UseDefaultCredentials = false;
                            client.Host = ConfigurationManager.AppSettings["MailHost"].ToString();
                            client.Send(mail);
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine("all okay with Perform Maintenance!");
                    }
                }
            }
        }

        public static string Application { get; set; }

        public void NH_ESCC_EMS()
        {

            if (DateTime.Now.ToString("hh:mm").Equals("01:00"))
            {
                DAL temp = new DAL();

                Dictionary<string, string> dictionary = new Dictionary<string, string>();
				//Add application ids and applications
                dictionary.Add("id1", "Application1");
                dictionary.Add("id2", "Application2");
                dictionary.Add("id3", "Application3");
                dictionary.Add("id4", "Application4");
                dictionary.Add("id5", "Application5");
                dictionary.Add("id6", "Application6");
                dictionary.Add("id7", "Application7");
                dictionary.Add("id8", "Application8");
                dictionary.Add("id9", "Application9");
                dictionary.Add("id10", "Application10");
                dictionary.Add("id11", "Application11");
                dictionary.Add("id12", "Application12");
                dictionary.Add("id13", "Application13");
                dictionary.Add("id14", "Application14");


                
                foreach (string i in dictionary.Keys)
                {
                    Application = i;

                    DataSet dsnhescc = temp.GetNHESCCAccounts();

                    foreach (DataTable table in dsnhescc.Tables)
                    {

                        foreach (DataRow dr in table.Rows)
                        {
                            string Count = dr[0].ToString();

                            if (Count == "0")
                            {
                                
                                MailMessage mail = new MailMessage();
                                mail.From = new MailAddress("noreply@example.com");

                                mail.To.Add("example@example.com");
								mail.CC.Add("example@example.com");

                                mail.IsBodyHtml = true;
                                mail.Subject = dictionary[i] + " accounts not getting aggregated correctly";
                                mail.Body = "HI," + "<br /><br />" + "This is an automated email to inform you that the application accounts are not getting aggregated correctly for the application "+ dictionary[i] + " in IIQ." + "<br /><br />" + "For any clarifications send an email to example@example.com";
                                
                                SmtpClient client = new SmtpClient();
                                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                                client.UseDefaultCredentials = false;
                                client.Host = ConfigurationManager.AppSettings["MailHost"].ToString();
                                client.Send(mail);
                            }
                            else
                            {
                                Console.WriteLine("all okay with "+ dictionary[i] + " number of accounts: "+ Count);
                            }
                        }
                    }
                }
            }
        }

        public void getWorkItem()
        {
            
            if (DateTime.Now.ToString("hh:mm").Equals("06:00"))
            {
                DAL temp = new DAL();
                DataSet ds = temp.GetWorkitem();
                foreach (DataTable table in ds.Tables)
                {
                    if (table.Rows.Count != 0)
                    {
                        foreach (DataRow dr in table.Rows)
                        {
                            string created = dr[0].ToString();
                            string modified = dr[1].ToString();
                            string name = dr[3].ToString().TrimStart('0');

                            string description = dr[4].ToString();
                            string severity = dr[5].ToString();
                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress("noreply@example.com");
                            mail.To.Add("example@example.com");
							mail.CC.Add("example@example.com");

                            mail.IsBodyHtml = true;
                            mail.Subject = "WebServices Executor Work Item " + name;
                            mail.Body = "HI," + "<br><br>" + "This is an automated email to inform you about the workitem: " + name + " with details: " + "<br><br>" + "Description: " + description + "<br>" + " severity: " + severity + "<br>" + "Owner: WebServices Executor" + "<br>" + "Created: " + created + "<br>" + "Modified: " + modified + "<br><br><br>" + "For any clarifications send an email to example@example.com";

                            SmtpClient client = new SmtpClient();
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;
                            client.UseDefaultCredentials = false;
                            client.Host = ConfigurationManager.AppSettings["MailHost"].ToString();
                            client.Send(mail);
                        }
                    }

                    else
                    {
                        Console.WriteLine("No workitem exec");
                    }
                }
            }
            }

        public void getDuplicateWorkItems()
        { 
        DAL temp = new DAL();
        DataSet ds = temp.GetDuplicateWorkitem();
            foreach (DataTable table in ds.Tables)
            {
                if (table.Rows.Count!=0)
                {
                    
                        MailMessage mail = new MailMessage();
                        mail.From = new MailAddress("noreply@example.com");
                        mail.To.Add("example@example.com");
                        mail.CC.Add("example@example.com");

                        mail.IsBodyHtml = true;
                        mail.Subject = "Duplicate workitem Alert - Notification!";
                        mail.Body = "Alert!!" + "<br><br>" + "This is an automated email to inform you that there are duplicate workitems in the queue. Please take necessary actions to delete them.";
                        
                        SmtpClient client = new SmtpClient();
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Host = ConfigurationManager.AppSettings["MailHost"].ToString();
                        client.Send(mail);
                    
                }

                else
                {
                    Console.WriteLine("No workitem");
                }
                }}
    }
}
