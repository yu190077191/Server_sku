using System;
using System.IO;
using System.Net;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace Nestle.WriteOff.Tool
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class Form1 : Form
    {
        const string VERSION = "1.0";

#if !DEBUG
        const string url = "http://gcrintranet.cn.nestle.com";
#endif

#if DEBUG
        const string url = "http://localhost:57044";
#endif

        public Form1()
        {
            InitializeComponent();
            webBrowser1.ObjectForScripting = this;
        }

        public string DownloadFile(string URL)
        {
            string folder = "C:/Nestle.WriteOff.WorkStation/";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string filename = Path.Combine(folder, Guid.NewGuid().ToString() + ".xlsx");
            System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url + URL);
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

            System.Diagnostics.Process.Start(filename);
            return "ok";
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
                return html;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        void Window_Error(object sender, HtmlElementErrorEventArgs e)
        {
            e.Handled = true;
        }

        WebBrowser wb = new System.Windows.Forms.WebBrowser();
        string computerName = null;
        private void Form1_Load(object sender, EventArgs e)
        {
            wb = webBrowser1;
            this.Text = "Nestlé Write Off Workstation v" + VERSION;
#if !DEBUG
            string home = "http://gcrintranet.cn.nestle.com/WriteOff/";
#endif

#if DEBUG
        string home = "http://localhost:57044";
#endif

            webBrowser1.Navigate(home + "?version=" + VERSION + "&time=" + DateTime.Now.ToString().Replace("/", string.Empty));
            computerName = System.Net.Dns.GetHostName();
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
        }

        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.Document.Window.Error += new HtmlElementErrorEventHandler(Window_Error);
            webBrowser1.ScriptErrorsSuppressed = true;
            AutoFill("IsWorkstationUser", "1");
        }

        void AutoFill(string elementId, string value)
        {
            AutoFill(webBrowser1, new[] { elementId }, new[] { value });
        }

        void AutoFill(WebBrowser webBrowser, string[] ID, string[] values)
        {
            int Total = 0;

            HtmlDocument doc = webBrowser.Document;

            foreach (HtmlElement em in doc.All)
            {
                string str = em.Id;
                if (str == null || str == "") str = em.Name;
                for (int i = 0; i < ID.Length; i++)
                {
                    if (str == ID[i])
                    {
                        em.SetAttribute("value", values[i]);
                        try
                        {
                            em.InvokeMember("onblur");
                        }
                        catch { }
                        Total++;
                    }
                    if (Total >= ID.Length) break;
                }
                if (Total >= ID.Length) break;
            }
        }

        void runWebBrowserAPIScript(string typeCode)
        {
            webBrowser1.Navigate("javascript:WebBrowserAPI('" + typeCode.Trim() + "');");
        }


    }
}
