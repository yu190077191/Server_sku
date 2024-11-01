using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace SKU.Email.Tool
{
    public class EmailHelper
    {
        static Microsoft.Office.Interop.Outlook.Application app = new Microsoft.Office.Interop.Outlook.Application();

        public static void SendByOutlook(string to, string subject, string body, string cc = null, string attachments = null)
        {
            char semicolon = ';';
            if (string.IsNullOrWhiteSpace(to) && string.IsNullOrWhiteSpace(cc))
            {
                throw new ArgumentNullException("to");
            }

            var mail = app.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
            mail.HTMLBody = body;
            //Add an attachment.
            int attachPos = (int)mail.Body.Length + 1;
            int attachType = (int)Microsoft.Office.Interop.Outlook.OlAttachmentType.olByValue;
            //now attached the file

            if (!string.IsNullOrEmpty(attachments))
            {
                string[] attachmentArray = attachments.Split(new char[] { semicolon }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in attachmentArray)
                {
                    mail.Attachments.Add(s, attachType, attachPos, s.Substring(s.Replace("\\", "/").LastIndexOf("/") + 1));
                }
            }

            //Subject line
            mail.Subject = subject;
            // Add a recipient.


            //var oRecip = mail.Recipients.Add(to);
            //oRecip.Resolve();
            // Send.

            if (!string.IsNullOrWhiteSpace(to))
            {
                string[] toArray = to.Split(new char[] { semicolon }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var address in toArray)
                {
                    if (address.IndexOf("@") > 0)
                    {
                        Recipient c = mail.Recipients.Add(address);
                        c.Type = (int)OlMailRecipientType.olTo;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(cc))
            {
                string[] array = cc.Split(new char[] { semicolon }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var address in array)
                {
                    if (address.IndexOf("@") > 0)
                    {
                        Recipient c = mail.Recipients.Add(address);
                        c.Type = (int)OlMailRecipientType.olCC;
                    }
                }
            }

            mail.Send();
        }

        public static string DownloadFile(string URL)
        {
            string folder = "C:/SKURegistration/";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string filename = Path.Combine(folder, Guid.NewGuid().ToString() + ".txt");
            System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(URL);
            Myrq.Timeout = 1000000;
            Myrq.Headers.Set("Pragma", "no-cache");
            Myrq.Credentials = CredentialCache.DefaultNetworkCredentials;
            System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
            long totalBytes = myrp.ContentLength;
            System.IO.Stream st = myrp.GetResponseStream();
            System.IO.Stream so = new System.IO.FileStream(filename, System.IO.FileMode.Create);
            long totalDownloadedByte = 0;
            byte[] by = new byte[1024];
            int osize = st.Read(by, 0, (int)by.Length);
            while (osize > 0)
            {
                totalDownloadedByte = osize + totalDownloadedByte;
                so.Write(by, 0, osize);
                osize = st.Read(by, 0, (int)by.Length);
            }

            so.Close();
            st.Close();

            string xml = File.ReadAllText(filename, Encoding.UTF8).Trim();
            File.Delete(filename);
            return xml;
        }

        public static string GetHtml(string URL, Encoding code = null)
        {
            try
            {
                if (code == null)
                {
                    code = Encoding.UTF8;
                }

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Timeout = 30000;
                request.Headers.Set("Pragma", "no-cache");
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamReceive = response.GetResponseStream();
                if (response.ContentType.ToLower().IndexOf("utf") > 0) code = Encoding.UTF8;
                StreamReader sr = new StreamReader(streamReceive, code);
                string html = sr.ReadToEnd();
                sr.Close();

                int a = html.IndexOf("<!--");
                if(a > 0)
                {
                    int b = html.LastIndexOf("-->");
                    if(b > a)
                    {
                        html = html.Remove(a, b - a + 3);
                    }
                }

                return html;
            }
            catch (System.Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public static List<T> GetByXmlString<T>(string xmlString)
        {
            List<T> li = new List<T>();
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    string typeName = typeof(T).Name;
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//" + typeName);
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<T>(node.OuterXml));
                    }
                }
            }

            return li;
        }
    }
}
