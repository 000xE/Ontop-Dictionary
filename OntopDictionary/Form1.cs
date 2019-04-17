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
using System.IO;

namespace OntopDictionary
{
    public partial class Form1 : Form
    {
        //true -- OFFLINE, false -- ONLINE
        bool offline = false;

        Dictionary<string, string> dictionary = new Dictionary<string, string>(); //Dictionary to store the dictionary!

        public Form1()
        {
            InitializeComponent();

            using (StreamReader r = new StreamReader(Application.StartupPath + @"/Resources/dictionary.json")) //Reads the dictionary
            {
                string json = r.ReadToEnd(); //from start to end
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json); //Deserializes into the Dictionary
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear(); //Clears the textbox first
            textBox2.Text = findDefinition(); //To find the definition
        }

        private string findDefinition()
        {
            string definition = "No definition!";

            if (textBox1.Text.Length > 0) //Ensure the user typed something
            {
                string word = textBox1.Text; //Grab the word 

                if (offline) //For offline
                {
                    definition = offlineAccess(word.ToUpper()); //Convert to uppercase since the dictionary is in uppercase
                }
                else //For online (Inaccurate, sorry!)
                {
                    string url = "https://owlbot.info/api/v2/dictionary/"; //API
                    string fullLink = url + word + "?format=json"; //API in use

                    using (WebClient wc = new WebClient())
                    {
                        string rawText = wc.DownloadString(fullLink); //Download the json file as a string
                        definition = onlineAccess(rawText); //Send it to be deserialized
                    }                  
                }
            }

            return definition;
        }

        private string onlineAccess(string raw)
        {
            string definition = "";

            string[] definitions = raw.Split('}'); //Splits the raw data into seperate definitions
            char[] charsToTrim = { '[', ']'}; //To trim additional characters

            foreach (string rawDef in definitions) //Go through each definition from the raw data
            {
                string trimmed = rawDef.Trim(charsToTrim); //Remove additional characters

                if (rawDef.Length > 2) //To ensure there's a definition
                {
                    trimmed += "}"; //Since it initially removes it when splitting

                    if (trimmed[0] == ',') //Check if there's a comma at start
                    {
                        trimmed = trimmed.Remove(0, 1); //Remove it
                    }

                    try
                    {
                        lookup jsonWord = JsonConvert.DeserializeObject<lookup>(trimmed); //Make a new object using the definition
                        definition += findDefinition(jsonWord); //Add to the definition
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }

            return definition;
        }

        private string offlineAccess(string word)
        {
            string definition = "";

            if (dictionary.ContainsKey(word)) //If the dictionary has the word
            {
                definition = dictionary[word]; //Set the definiton
            }

            return definition;
        }

        private string findDefinition(lookup jsonWord)
        { //Online access only
            //To print the definition along with type and example
            string definition = ("Type: " + jsonWord.type.ToUpper() + "\r\n" + "Definition: " + jsonWord.definition + "\r\n" + "Example: " +  jsonWord.example + "\r\n")
            + ("--------------------------------------------------------------------------------------------------------------------------------------\r\n");

            return definition;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            textBox1.Focus(); //To perma forcus the textbox
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            offline = checkBox1.Checked; //To set state
        }
    }
}
