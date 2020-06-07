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
using Newtonsoft.Json.Linq;

namespace OntopDictionary
{
    public partial class Form1 : Form
    {
        //true -- OFFLINE, false -- ONLINE
        bool offline = false;

        //Ofline access
        Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>(); //Dictionary to store the dictionary!
        string offlinePath = Application.StartupPath + @"/Resources/076 data.json"; //CHANGE THIS TO WHEREVER THE OFFLINE DICTIONARY (076 data) IS!

        //Online access
        static List<string> sourceDictionaries = new List<string>() {"all", "ahd", "century", "cmu", "macmillan", "wiktionary", "webster", "wordnet"};

        int resultLimit = 10; //The limit of definitions
        string currentSource = sourceDictionaries[7]; //The current online source of definitions
        bool useCanonical = false; //To fix spelling mistakes in typed (Inaccurate)
        string currentWord = ""; //The currently entered word

        string apiKey = ""; //The API key
        string onlineURL = ""; //The full online URL (set later)

        //Auto-opacity
        double focusOpacity = 0.8; //Opacity when the form is focused
        double passiveOpacity = 0.3; //Opacity when the form isn't focused

        public Form1()
        {
            InitializeComponent();
            //richTextBox1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular);

            using (StreamReader r = new StreamReader(offlinePath)) //Reads the dictionary
            {
                string json = r.ReadToEnd(); //from start to end
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json); //Deserializes into the Dictionary
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            richTxtDefinition.Clear(); //Clears the textbox first
            addToText();
        }

        private void setOnlineURL()
        {
            //Set the settings for the online API
            onlineURL = "https://api.wordnik.com/v4/word.json/" + currentWord + "/definitions?limit=" + resultLimit + "&includeRelated=false&sourceDictionaries=" + currentSource + "&useCanonical=" + useCanonical + "&includeTags=false&api_key=" + apiKey;
        }

        private async void addToText()
        { //To add to the text itself
          //richTextBox1.Text = getDefinition(); //To get the definition
            Task<string> task = new Task<string>(getDefinition);
            task.Start();

            string definition = await task;

            string[] lines = definition.Split(Environment.NewLine.ToCharArray()); //Splits all new lines

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
                    richTxtDefinition.AppendText(line + "\r\n"); //Appends the line
                }
            }
        }

        private void colorText(string line, string keyWord, Color color, FontStyle style)
        { //To color the text
            if (line.Contains(keyWord)) //If the line contains the keyword
            {
                richTxtDefinition.SelectionColor = color; //Colours it
                richTxtDefinition.SelectionFont = new Font(richTxtDefinition.SelectionFont, style); //Styles it
            }
        }

        private string getDefinition()
        { //To get the definition via offline or online
            string definition = "";

            if (txtWord.Text.Length > 0) //Ensure the user typed something
            {
                currentWord = txtWord.Text; //Grab the word 

                if (offline) //For offline (Inaccurate, sorry)
                {
                    definition = offlineAccess(currentWord.ToLower()); //Convert to uppercase since the dictionary is in uppercase
                }
                else //For online
                {
                    setOnlineURL();
                    definition = onlineAccess(onlineURL); //Get the definition from online
                }
            }

            return definition; //Return the fixed definition
        }

        private static dynamic getJson(string url)
        {
            try
            {
                var request = WebRequest.Create(url);
                string text;
                var response = (HttpWebResponse)request.GetResponse();

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    text = sr.ReadToEnd(); //Read the url
                }

                dynamic json = JsonConvert.DeserializeObject(text); //Get the json format

                return json; //Return the json
            }
            catch (WebException e)
            {
                var response = (HttpWebResponse) e.Response;
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return 0;
                }
                else
                {
                    return null;
                }
            }
        }

        private string onlineAccess(string url)
        {
            string definition = ""; //Default

            dynamic json = getJson(url); //Get json from URL

            if (!(json is int))
            {
                if (json == null)
                {
                    definition = "API error";
                }
                else
                {
                    foreach (JObject result in json)
                    { //Go through each word, from the list
                        definition += getOnlineDefinition(result); //Find each definition using the object and the index
                    }
                }
            }
            else
            {
                if (json == 0)
                {
                    definition = "No definition!";
                }
            }

            return definition; //Return the definition
        }

        private string offlineAccess(string word)
        { //To get the definition via offline
            string definition = "No definition!"; //Default

            if (dictionary.ContainsKey(word)) //If the dictionary has the word
            {
                definition = dictionary[word][0]; //Set the definiton
            }

            return definition; //Return the definition
        }
        
        private string getOnlineDefinition(JObject jsonWord)
        { //Online access only
            //To print the definition along with type and example
            string definition = ""; //Default

            if (jsonWord != null) //If the type list contains any definitions
            {
                definition += ("Type: ") + (jsonWord["partOfSpeech"] + "\r\n").ToUpper(); //Append the type itself as a header to the definitions
                definition += ("Definition: ") + (jsonWord["text"]); //Append the definition

                //Append each example (If they exist)
                if (jsonWord["exampleUses"].Count() > 0)
                {
                    foreach (JObject example in jsonWord["exampleUses"])
                    {
                        definition += "\nExample: " + example["text"];
                    }
                }
                else
                {
                    definition += "\r\n"; //Append blank line
                }
            }

            return definition; //Return the definition
        }

        private void txtWord_Leave(object sender, EventArgs e)
        {
            //textBox1.Focus(); //To perma forcus the textbox
        }

        private void checkOffline_CheckedChanged(object sender, EventArgs e)
        {
            offline = checkOffline.Checked; //To set offline/online
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            txtWord.Focus(); //To re-focus the textbox

            if (checkDynamicOpacity.Checked)
            {
                Opacity = passiveOpacity; //Low opacity (Less visible)
            }
            else
            {
                Opacity = (double)trackOpacity.Value / 10; //Set to trackbar opacity
            }

            //FormBorderStyle = FormBorderStyle.None; //Remove control border
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            if (checkDynamicOpacity.Checked)
            {
                Opacity = focusOpacity; //Higher opacity (More visible)
            }
            else
            {
                Opacity = (double)trackOpacity.Value / 10; //Set to trackbar opacity
            }

            //FormBorderStyle = FormBorderStyle.Sizable; //Add control border
        }

        private void trackOpacity_ValueChanged(object sender, EventArgs e)
        {
            Opacity = (double)trackOpacity.Value / 10; //Set to trackbar opacity
        }

        private void checkDynamicOpacity_CheckedChanged(object sender, EventArgs e)
        {
            trackOpacity.Enabled = !checkDynamicOpacity.Checked;

            if (checkDynamicOpacity.Checked)
            {
                Opacity = focusOpacity; //Higher opacity (More visible)
            }
            else
            {
                Opacity = (double)trackOpacity.Value / 10; //Set to trackbar opacity
            }
        }
    }
}
