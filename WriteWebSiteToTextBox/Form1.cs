﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WriteWebSiteToTextBox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<string> listSites = new List<string>();
        List<string> listVistedSites = new List<string>();
        Action<TextBox, string> mydelegate = (txtbox, txt) =>
        {
            txtbox.Text = txt;
        };
        Action<ListBox, string> listboxDelegate = (lbox, txt) =>
        {
            lbox.Items.Add(txt);
        };
        private void ReadAllWebsites()
        {
            try
            {
                beginTime = DateTime.Now;
                foreach (var str in System.IO.File.ReadAllLines(Directory.GetCurrentDirectory() + "\\WebSites.txt", Encoding.Default))
                {
                    if (!listSites.Contains(str))
                        listSites.Add(str);
                }
                textBox3.Text = listSites.Count.ToString();
                listVistedSites.Clear();
            }
            catch (Exception ex)
            {

            }
        }
        int i = -1;
        DateTime beginTime = DateTime.Now;
        DateTime endTime = DateTime.Now;
        private void Form1_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        if (listSites.Count > 0)
                        {
                            var randomNum = new Random(Guid.NewGuid().GetHashCode()).Next(0, listSites.Count - 1);
                            if (listVistedSites.Contains(listSites[randomNum]))
                            {
                                listSites.Remove(listSites[randomNum]);
                                continue;
                            }
                            textBox1.Invoke(mydelegate, new object[] { textBox1, listSites[randomNum] });
                            listVistedSites.Add(listSites[randomNum]);
                            listSites.Remove(listSites[randomNum]);
                        }
                        else
                        {
                            endTime = DateTime.Now;
                            i++;
                            listBox1.Invoke(listboxDelegate, new object[] { listBox1, string.Format("{2}:第{0}轮完成,耗时{1}分钟",i,endTime.Subtract(beginTime).TotalMinutes,DateTime.Now ) });
                            ReadAllWebsites();
                        }
                        
                    }
                    catch (Exception ex)
                    {

                    }
                    Thread.Sleep(9000);
                }
            });
        }
    }
}
