using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoStart
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 启动程序进程
        /// </summary>
        /// <param name="ProcessName"></param>
        public static void StartProcess(string ProcessPath)
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = ProcessPath;
                info.Arguments = "";
                info.WindowStyle = ProcessWindowStyle.Maximized;
                Process pro = Process.Start(info);
            }
            catch (Exception ex)
            {
                
            }
        }

        /// <summary>
        /// 结束后台程序进程
        /// </summary>
        /// <param name="ProcessName"></param>
        public static void KillProcess(string ProcessName)
        {
            try
            {
                Process[] processes = Process.GetProcessesByName(ProcessName);
                foreach (Process process in processes)
                {
                    process.Kill();
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        Action<ListBox, string> mydelegate = (lbox, txt) =>
        {
            lbox.Items.Add(txt);
        };
        int k = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    KillProcess("BrowserWebSite");
                    Thread.Sleep(5000);
                    if (DateTime.Now.Hour == int.Parse(textBox1.Text))
                    {
                        listBox1.Invoke(mydelegate, new object[] { listBox1, string.Format("凌晨{0}点退出循环，不再重启",textBox1.Text) });
                        break;
                    }
                    StartProcess(@"D:\GitPro\BrowserWebSite(2)\BrowserWebSite\bin\Debug\BrowserWebSite.exe");
                    
                    k++;
                    listBox1.Invoke(mydelegate, new object[] { listBox1, string.Format("at {0} 第 {1}次重启", DateTime.Now, k) });
                    Thread.Sleep(600000);
                }
            });
        }
    }
}
