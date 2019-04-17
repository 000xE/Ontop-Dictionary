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
using Newtonsoft.Json;

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
                string fullLink = url + word + "?format=json";

                WebClient wc = new WebClient();
                string rawText = wc.DownloadString(fullLink);

                //definition = getFullDefinition(rawText);
                //textBox2.Text = definition;

                textBox2.Clear();
                makeObject(rawText);
            }
        }

        private void makeObject(string raw)
        {
            string[] definitions = raw.Split('}');
            char[] charsToTrim = { '[', ']'};

            foreach (string rawDef in definitions)
            {
                if (rawDef.Length > 2)
                {
                    string trimmed = rawDef.Trim(charsToTrim) + "}";


                    if (trimmed[0] == ',')
                    {
                        trimmed = trimmed.Remove(0, 1);
                    }

                    //Console.WriteLine(trimmed);
                    lookup jsonWord = JsonConvert.DeserializeObject<lookup>(trimmed);
                    findDefinition(jsonWord);
                }
                else
                {
                    textBox2.AppendText("No definition found");
                }
            }
        }

        private void findDefinition(lookup jsonWord)
        {
            textBox2.AppendText("Type: " + jsonWord.type + "\r\n" + "Definition: " + jsonWord.definition + "\r\n" + "Example: " +  jsonWord.example + "\r\n\r\n");
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
    }
}
