using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Web;

namespace OntopDictionary
{
    public partial class Form1 : Form
    {
        string url = "https://owlbot.info/api/v2/dictionary/";
        string word;
        string definition;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            findDefinition();
        }

        private void findDefinition()
        {
            if (textBox1.Text.Length > 0)
            {
                word = textBox1.Text;

                WebClient wc = new WebClient();

                definition = getFullDefinition(wc.DownloadString(url + word + "?format=json"));

                textBox2.Text = definition;
            }
        }

        private string getFullDefinition(string raw)
        {
            string definition = "";

            if (raw.IndexOf("definition") != -1)
            {
                char[] charsToTrim = { '[', ']', '{', '}', ','};
                string[] definitions = raw.Split('}');
                foreach (string full in definitions)
                {
                    string trimmed = full.Trim(charsToTrim);
                    definition += trimmed + "\r\n\r\n";
                }
                //definition = raw; //raw.Substring(raw.IndexOf("definition"));
            }
            else
            {
                definition = "No definition";
            }

            return definition;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                findDefinition();
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
    }
}
