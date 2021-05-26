using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrowserWebSite
{
    public partial class Form1 : Form
    {
        int current = 0;
        System.Windows.Forms.Timer timeDown = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer timeUp = new System.Windows.Forms.Timer();
 

        public Form1()
        {
            InitializeComponent();
            this.webBrowser1.ScriptErrorsSuppressed = true;
        }
        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool SetProcessWorkingSetSize(IntPtr pProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);

        [DllImport("KERNEL32.DLL", EntryPoint = "GetCurrentProcess", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetCurrentProcess();

        private void ReadMoreEventHandler(object sender, HtmlElementEventArgs e)
        {

        }
        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                timeDown.Enabled = true;
                timeUp.Enabled = false;
                this.webBrowser1.Document.Window.Error += OnWebBrowserDocumentWindowError;
            }
            catch (Exception EX)
            {

            }
        }
        private void OnWebBrowserDocumentWindowError(object sender, HtmlElementErrorEventArgs e)
        {
            e.Handled = true;

        }
        void timeDown_Tick(object sender, EventArgs e)
        {
            try
            {
                HtmlDocument doc = webBrowser1.Document;
                int height = webBrowser1.Document.Body.ScrollRectangle.Height;
                current += height / (101-(int)numericUpDown1.Value);
                if (current >= height)
                {
                    current = height;
                    timeDown.Enabled = false;
                    timeUp.Enabled = true;
                    Thread.Sleep(500);
                    readMore();
                }
                doc.Window.ScrollTo(new Point(0, current));
            }
            catch (Exception EX)
            {

            }
        }
        void timeUp_Tick(object sender, EventArgs e)
        {
            try
            {
                HtmlDocument doc = webBrowser1.Document;
                int height = webBrowser1.Document.Body.ScrollRectangle.Height;
                current -= height / (101 - (int)numericUpDown1.Value);
                if (current <= 0)
                {
                    current = 0;
                    timeUp.Enabled = false;
                    readMore();
                }
                doc.Window.ScrollTo(new Point(0, current));
            }
            catch(Exception EX)
            {

            }
        }
        void readMore()
        {
            try
            {
                var objDoc = webBrowser1.Document;
                var objSpan = objDoc.GetElementsByTagName("span");
                for (var i = 0; i < objSpan.Count;i++ )
                {
                    if (objSpan[i].InnerText == "刷新该页面。")
                    {
                        Application.Exit();
                    }
                }
                for (var i = 0; i < objSpan.Count; i++)
                {
                    if (objSpan[i].InnerText == "展开阅读全部")
                    {
                        objSpan[i].Click += ReadMoreEventHandler;
                        objSpan[i].InvokeMember("click");
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }
        List<string> listSites = new List<string>();
        List<string> listVistedSites = new List<string>();
        int loopCount = 0;
        Action<Label, string> mydelegate = (label, txt) =>
        {
            label.Text = txt;
        };
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
                timeDown.Interval = 120;
                timeDown.Tick += new EventHandler(timeDown_Tick);
                timeUp.Interval = 120;
                timeUp.Tick += new EventHandler(timeUp_Tick);
                foreach (var str in System.IO.File.ReadAllLines(Application.StartupPath + "\\WebSites.txt", Encoding.Default))
                {
                    if (!listSites.Contains(str))
                        listSites.Add(str);
                }
                label6.Text = listSites.Count.ToString();
                Task.Run(() =>
                {
                    while (true)
                    {
                        try
                        {
                            if (listSites.Count > 0)
                            {
                                label4.Invoke(mydelegate, new object[] { label4, "" });
                                var randomNum = new Random(Guid.NewGuid().GetHashCode()).Next(0, listSites.Count - 1);
                                if (listVistedSites.Contains(listSites[randomNum]))
                                {
                                    listSites.Remove(listSites[randomNum]);
                                    continue;
                                }
                                webBrowser1.Navigate(listSites[randomNum]);
                                listVistedSites.Add(listSites[randomNum]);
                                listSites.Remove(listSites[randomNum]);
                                loopCount++;
                                label2.Invoke(mydelegate, new object[] { label2, loopCount.ToString() });
                                //IntPtr pHandle = GetCurrentProcess();
                                //SetProcessWorkingSetSize(pHandle, -1, -1);
                            }
                            else
                            {
                                label4.Invoke(mydelegate, new object[] { label4, "所有地址都已经访问一遍啦，请点击初始化按钮，换个ip再来一遍" });
                                Thread.Sleep(1000);
                                button1_Click(null, null);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        Thread.Sleep(new Random(Guid.NewGuid().GetHashCode()).Next(7, 12) * 1000);
                    }
                });
            }
            catch (Exception ex)
            {

            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var str in System.IO.File.ReadAllLines(Directory.GetCurrentDirectory() + "\\WebSites.txt", Encoding.Default))
                {
                    if (!listSites.Contains(str))
                        listSites.Add(str);
                }
                listVistedSites.Clear();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
