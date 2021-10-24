using System;
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
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
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
                            i++;
                            textBox2.Invoke(mydelegate, new object[] { textBox2, string.Format("第{0}轮完成！", i) });
                            button1_Click(null, null);
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
