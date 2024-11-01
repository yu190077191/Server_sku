using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SKU.Email.Tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string bodyText = body.Text.Replace("{url}", "<a href=\"" + url.Text + "\">" + url.Text + "</a>").Replace("\r\n", "<br />");
                EmailHelper.SendByOutlook(to.Text, subject.Text, bodyText, cc.Text, null);
                DisplayData("Email_" + to.Text + "|" + comboBoxStep.SelectedItem.ToString() + "|" + subject);
                MessageBox.Show("Email sent successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DisplayData(string typeCode = "GetStep")
        {
            try
            {
                string str = url.Text;
                if (!string.IsNullOrEmpty(str))
                {
                    int id = 0;
                    int a = str.LastIndexOf("=") + 1;
                    if (a > 0)
                    {
                        string root = str.Substring(0, str.IndexOf("/", str.IndexOf("/", 10) + 2)) + "/";
                        str = str.Substring(a);
                        if (Int32.TryParse(str, out id))
                        {
                            if (id > 0)
                            {
                                string _url = string.Format(root + "Home/SKU_Email_Tool_API/?typeCode={0}&id={1}&xml={2}", typeCode, id, "");
                                string xml = EmailHelper.GetHtml(_url);
                                List<APIInfo> li = EmailHelper.GetByXmlString<APIInfo>(xml);
                                if (comboBoxStep.Items.Count == 0)
                                {
                                    string[] steps = li[0].Step.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                                    comboBoxStep.Items.Clear();
                                    foreach (string item in steps)
                                    {
                                        comboBoxStep.Items.Add(item);
                                    }
                                }

                                if (string.IsNullOrEmpty(subject.Text)) subject.Text = li[0].Subject;
                                if (string.IsNullOrEmpty(to.Text)) to.Text = li[0].To;
                                if (string.IsNullOrEmpty(cc.Text)) cc.Text = li[0].Cc;
                                if (string.IsNullOrEmpty(body.Text)) body.Text = li[0].Body.Replace("<br />", "\r\n").Replace("{url}", "<a href=\"" + url.Text + "\">" + url.Text + "</a>");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DisplayData();
        }

        private void comboBoxStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayData("Step_" + comboBoxStep.Text);
        }
    }
}
