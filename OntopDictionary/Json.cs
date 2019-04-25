using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace OntopDictionary
{
    //For each type
    public class Type
    {
        public string definition { get; set; }
        public string example { get; set; }
        public List<string> synonyms { get; set; }
    }

    //For each meaning
    public class Meaning
    {
        //List for each type (Keep adding more as you go along)
        public List<Type> exclamation { get; set; }
        public List<Type> noun { get; set; }
        public List<Type> verb { get; set; }
        public List<Type> adverb { get; set; }
        public List<Type> abbreviation { get; set; }
        public List<Type> contraction { get; set; }
        public List<Type> adjective { get; set; }
        public List<Type> proNoun { get; set; }

        //For spaces (Keep adding more as you go along)
        [JsonProperty("exclamation & noun")]
        public List<Type> exclamationNoun { get; set; }
        [JsonProperty("proper noun")]
        public List<Type> properNoun { get; set; }

        //For adding all the types into one big list for ease of use
        public Dictionary<string, List<Type>> allWords = new Dictionary<string, List<Type>>();
        public void addLists()
        { //(Keep adding more as you go along)
            allWords.Add("exclamation", exclamation);
            allWords.Add("noun", noun);
            allWords.Add("verb", verb);
            allWords.Add("adverb", adverb);
            allWords.Add("abbreviation", abbreviation);
            allWords.Add("contraction", contraction);
            allWords.Add("adjective", adjective);
            allWords.Add("exclamation & noun", exclamationNoun);
            allWords.Add("proper noun", properNoun);
            allWords.Add("pronoun", proNoun);
        }
    }

    //For each word
    public class Word
    {
        public string word { get; set; }
        public string phonetic { get; set; }
        public string pronunciation { get; set; }
        public Meaning meaning { get; set; }
    }

    //For list of words
    public class Json
    {
        public List<Word> words { get; set; }
    }
}
