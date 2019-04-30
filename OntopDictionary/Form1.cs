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
using System.Diagnostics;

namespace OntopDictionary
{
    public partial class Form1 : Form
    {
        //true -- OFFLINE, false -- ONLINE
        bool offline = false;
        string onlineDefinition = "";

        Dictionary<string, string> dictionary = new Dictionary<string, string>(); //Dictionary to store the dictionary!

        string offlinePath = Application.StartupPath + @"/Resources/dictionary.json"; //CHANGE THIS TO WHEREVER THE dictionary.js IS!

        public Form1()
        {
            InitializeComponent();
            //richTextBox1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular);

            using (StreamReader r = new StreamReader(offlinePath)) //Reads the dictionary
            {
                string json = r.ReadToEnd(); //from start to end
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json); //Deserializes into the Dictionary
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear(); //Clears the textbox first
            addToText();
        }

        private void addToText()
        { //To add to the text itself
            //richTextBox1.Text = getDefinition(); //To get the definition
            string[] lines = getDefinition().Split(Environment.NewLine.ToCharArray()); //Splits all new lines

            //TO ADD STYLE THE LINES
            if (lines[0] != String.Empty) //Removing the stupid empty line at start
            {
                foreach (string line in lines)
                { //Go through each line

                    //To color the text
                    colorText(line, "Type:", Color.LightSkyBlue, FontStyle.Bold);
                    colorText(line, "Definition:", Color.LightGreen, FontStyle.Bold);
                    colorText(line, "Example:", Color.Gray, FontStyle.Bold);

                    // AppendText is better than rtb.Text += ...
                    richTextBox1.AppendText(line + "\r\n"); //Appends the line
                }
            }
        }

        private void colorText(string line, string keyWord, Color color, FontStyle style)
        { //To color the text
            if (line.Contains(keyWord)) //If the line contains the keyword
            {
                richTextBox1.SelectionColor = color; //Colours it
                richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, style); //Styles it
            }
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
                    if (onlineDefinition == "ERROR") //If the definition returns an error
                    {
                        definition = "No definition!";
                        onlineDefinition = ""; //Resets the definition raw data for the next definition
                    }
                    else //If the definition isn't an error
                    {
                        if (onlineDefinition != "") //When this method is re-called, it will go here after it's downloaded asynchronously
                        {
                            definition = onlineAccess(onlineDefinition); //Get the definition from the raw data
                            onlineDefinition = ""; //Resets the definition raw data for the next definition
                        }
                        else //On a new definition
                        {
                            string language = "en"; //language to use
                            string url = "http://googledictionaryapi.eu-gb.mybluemix.net/?define=" + word + "&lang=" + language; //API                                                                                                                            
                            //Credit: https://googledictionaryapi.eu-gb.mybluemix.net

                            using (WebClient wc = new WebClient())
                            {
                                wc.Proxy = null; //Faster

                                //Old
                                //var rawText = wc.DownloadData(new Uri(url)); //Download the raw Json (As data)
                                //var encoded = Encoding.UTF8.GetString(rawText); //Encodes to UTF-8 to prevent invalid characters

                                wc.DownloadDataCompleted += DownloadDataCompleted; //When download is complete
                                wc.DownloadDataAsync(new Uri(url)); //Downloads asynchronously
                            }
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

            //Console.WriteLine(raw); //-- For testing

            Json jsonWord = JsonConvert.DeserializeObject<Json>(raw); //Deserialize

            for (int i = 0; i < jsonWord.words.Count; i++)
            { //Go through each word, from the list
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
            { //Go through each type (exclamation, verbs, etc.) in the given word/from index 'i'
                if (key.Value != null) //If the type list contains any definitions
                {
                    definition += ("Type: ") + (key.Key + "\r\n").ToUpper(); //Append the type itself as a header to the definitions
                    foreach (Type type in key.Value)
                    { //Go through each type in for the given word/from index 'i'
                        definition += ("Definition: " + type.definition + (type.example == null ? "\r\n" : "\nExample: " + type.example + "\r\n"));
                    } //Append the definitions and their examples (If they exist) to the definition itself
                }
            }

            return definition; //Return the definition
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            //textBox1.Focus(); //To perma forcus the textbox
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            offline = checkBox1.Checked; //To set state
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            textBox1.Focus(); //To re-focus the textbox
            Opacity = 0.3; //Low opacity (Less visible)
            //FormBorderStyle = FormBorderStyle.None; //Remove control border
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            Opacity = 0.8; //Higher opacity (More visible)
            //FormBorderStyle = FormBorderStyle.Sizable; //Add control border
        }

        private void DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            //For testing - Getting the time taken for this
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start(); //Start the stopwatch

            try
            { //Try get the definition
                byte[] raw = e.Result; //Gets the raw data
                onlineDefinition = Encoding.UTF8.GetString(raw); //Converts it to a string
            }
            catch (Exception ex)
            { //Definition not found
                Console.WriteLine("Error: " + ex);
                onlineDefinition = "ERROR";
            }

            addToText(); //Goes to the add function to recursively check for the definition that's found

            //For testing - Getting the time taken for this
            stopWatch.Stop(); //Stop the stopwatch
            Console.WriteLine("Time elapsed: {0}.{1}", stopWatch.Elapsed.Seconds, stopWatch.Elapsed.Milliseconds); //Print the time elapsed
        }
    }
}
