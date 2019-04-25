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
using System.Text.RegularExpressions;

namespace OntopDictionary
{
    public partial class Form1 : Form
    {
        //true -- OFFLINE, false -- ONLINE
        bool offline = false;

        Dictionary<string, string> dictionary = new Dictionary<string, string>(); //Dictionary to store the dictionary!

        string offlinePath = Application.StartupPath + @"/Resources/dictionary.json"; //CHANGE THIS TO WHEREVER THE dictionary.js IS!

        public Form1()
        {
            InitializeComponent();

            using (StreamReader r = new StreamReader(offlinePath)) //Reads the dictionary
            {
                string json = r.ReadToEnd(); //from start to end
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json); //Deserializes into the Dictionary
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear(); //Clears the textbox first
            textBox2.Text = getDefinition(); //To get the definition
        }

        private string getDefinition()
        { //To get the definition via offline or online

            string definition = "";

            if (textBox1.Text.Length > 0) //Ensure the user typed something
            {
                string word = textBox1.Text; //Grab the word 

                if (offline) //For offline (Inaccurate, sorry)
                {
                    definition = offlineAccess(word.ToUpper()); //Convert to uppercase since the dictionary is in uppercase
                }
                else //For online
                {
                    string language = "en"; //language to use
                    string url = "http://googledictionaryapi.eu-gb.mybluemix.net/?define=" + word + "&lang=" + language;
                    //Credit: https://googledictionaryapi.eu-gb.mybluemix.net

                    using (WebClient wc = new WebClient())
                    {
                        try
                        { //Try get the definition
                            var rawText = wc.DownloadData(url); //Download the raw Json (As data)
                            var encoded = Encoding.UTF8.GetString(rawText); //Encodes to UTF-8 to prevent invalid characters
                            definition = onlineAccess(encoded); //Get the definition
                        }
                        catch (WebException e)
                        { //Definition not found
                            definition = "No definition!";
                            Console.WriteLine("Error: " + e);
                        }
                        catch (Newtonsoft.Json.JsonReaderException e)
                        { //Parser error
                            definition = "ERROR: Unfortunately, this definition page contains special characters which are difficult to remove to be parsed.";
                            Console.WriteLine("Error: " + e);
                        }
                    }                  
                }
            }

            return definition; //Return the fixed definition
        }

        private string onlineAccess(string raw)
        { //To get the definition via online
            string definition = ""; //Default

            raw = @"{ ""words"":" + raw + "\n}"; //To allow deserializing the list of words 
            //(Added "words:" to let it detect it intiially as a list)

            Console.WriteLine(raw); //-- For testing

            Json jsonWord = JsonConvert.DeserializeObject<Json>(raw); //Deserialize

            for (int i = 0; i < jsonWord.words.Count; i++)
            {//Go through each word, from the list
                jsonWord.words[i].meaning.addLists(); //Add all the lists containing the types (exclamation, verbs, etc.) into one dictionary

                definition += findDefinition(jsonWord, i); //Find each definition using the object and the index
                //Console.WriteLine(jsonWord.words.Count); //-- For testing
            }

            return definition; //Return the definition
        }

        private string offlineAccess(string word)
        { //To get the definition via offline
            string definition = "No definition!"; //Default

            if (dictionary.ContainsKey(word)) //If the dictionary has the word
            {
                definition = dictionary[word]; //Set the definiton
            }

            return definition; //Return the definition
        }
        
        private string findDefinition(Json jsonWord, int i)
        { //Online access only
            //To print the definition along with type and example

            string definition = ""; //Default

            foreach (KeyValuePair<string, List<Type>> key in jsonWord.words[i].meaning.allWords)
            {//Go through each type (exclamation, verbs, etc.) in the given word/from index 'i'
                if (key.Value != null) //If the type list contains any definitions
                {
                    definition += ("Type: ") + (key.Key + "\r\n").ToUpper(); //Append the type itself as a header to the definitions
                    foreach (Type type in key.Value)
                    {//Go through each type in for the given word/from index 'i'
                        definition += ("Definition: " + type.definition + "\r\n" + "Example: " + type.example + "\r\n")
                        + ("--------------------------------------------------------------------------------------------------------------------------------------\r\n");
                    } //Append the definitions and their examples to the definition itself
                }
            }

            return definition; //Return the definition
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
