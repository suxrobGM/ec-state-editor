using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace EC_StateEditor.Model
{
    enum Religions
    {
        christianity,
        islam,
        buddhism,
        judaism,
        hinduism,
        sintoism,
        taoism,
        confucianism,
        witoutReligion
    }

    enum StateCategories
    {
        city,
        enclave,
        large_city,
        large_town,
        megalopolis,
        metropolis,
        pastoral,
        rural,
        small_island,
        tiny_island,
        town,
        wasteland
    }

    class State
    {
        public string FileName { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string LocalizationToken { get; set; }
        public string Owner { get; set; }
        public int Manpower { get; set; }
        public Religions Religion { get; set; }
        public StateCategories StateCategory { get; set; }
        public int[] Provinces { get; set; }


        public void SaveContent(string pathToStatesFolder)
        {
            var buffer = File.ReadAllLines(pathToStatesFolder + "\\" + FileName);

            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i].Contains("id"))
                    buffer[i] = $"\tid = {Id}";
                if (buffer[i].Contains("name"))
                    buffer[i] = $"\tname = \"{LocalizationToken}\"";
                if (buffer[i].Contains("owner"))
                    buffer[i] = $"\t\towner = {Owner}";
                if (buffer[i].Contains("manpower"))
                    buffer[i] = $"\tmanpower = {Manpower}";
                if (buffer[i].Contains("set_state_flag"))
                    buffer[i] = $"\t\tset_state_flag = {Religion}";
                if (buffer[i].Contains("state_category"))
                    buffer[i] = $"\tstate_category = {StateCategory}";
            }

            File.WriteAllLines(pathToStatesFolder + "\\" + FileName, buffer.ToArray());
        }

        public static string ParseFileName(string pathToFileName)
        {
            return Path.GetFileName(pathToFileName);
        }

        public static string ParseName(string fileName)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            if (Regex.IsMatch(name, @"^\d+-\D+"))
            {
                var numberMatch = Regex.Match(name, @"^\d+-"); //delete numbers in start of substring
                name = name.Remove(numberMatch.Index, numberMatch.Length);
                //var txtExtension = Regex.Match(name, @"\.txt");
                //name = name.Remove(txtExtension.Index, txtExtension.Length); //delete .txt in end of substring
            }
            return name;
        }

        public static string ParseLocalizationToken(string[] fileContent)
        {
            string localizationToken = String.Empty;

            foreach (var content in fileContent)
            {
                if (content.Contains("name"))
                {
                    localizationToken = content.Trim();

                    if (CheckComment(localizationToken))
                        localizationToken = DeleteComment(localizationToken);

                    localizationToken = localizationToken.Replace(" ", "").Replace("\"", "").Remove(0, "name".Length + 1);
                    break;
                }
            }
            return localizationToken;
        }

        public static int ParseId(string[] fileContent)
        {
            int id = 0;
            foreach (var content in fileContent)
            {
                if(content.Contains("id"))
                {
                    string idString = content.Trim();

                    if (CheckComment(idString))
                        idString = DeleteComment(idString);

                    idString = idString.Replace(" ", "").Remove(0, "id".Length + 1);                                           
                    int.TryParse(idString, out id);
                    break;
                }        
            }
            return id;
        }

        public static string ParseOwner(string[] fileContent)
        {
            string owner = String.Empty;

            foreach (var content in fileContent)
            {
                if (content.Contains("owner"))
                {
                    owner = content.Trim();

                    if (CheckComment(owner))
                        owner = DeleteComment(owner);

                    owner = owner.Replace(" ", "").Remove(0, "owner".Length + 1);                   
                    break;
                }
            }
            return owner;
        }

        public static int ParseManpower(string[] fileContent)
        {
            int manpower = 0;
            foreach (var content in fileContent)
            {
                if (content.Contains("manpower"))
                {
                    string manpowerString = content.Trim();

                    if (CheckComment(manpowerString))
                        manpowerString = DeleteComment(manpowerString);

                    manpowerString = manpowerString.Replace(" ", "").Remove(0, "manpower".Length + 1);
                    int.TryParse(manpowerString, out manpower);
                    break;
                }
            }
            return manpower;
        }

        public static Religions ParseReligion(string[] fileContent)
        {
            foreach (var content in fileContent)
            {
                if(content.Contains("set_state_flag"))
                {
                    string religionString = content.Trim();

                    if (CheckComment(religionString))
                        religionString = DeleteComment(religionString);

                    religionString = religionString.Replace(" ", "").Remove(0, "set_state_flag".Length + 1);

                    switch (religionString)
                    {
                        case "christianity": return Religions.christianity;
                        case "islam": return Religions.islam;
                        case "buddhism": return Religions.buddhism;
                        case "judaism": return Religions.judaism;
                        case "hinduism": return Religions.hinduism;
                        case "sintoism": return Religions.sintoism;
                        case "taoism": return Religions.taoism;
                        case "confucianism": return Religions.confucianism;
                        default: return Religions.witoutReligion;
                    }
                }
            }
           
            return Religions.witoutReligion;
        }

        public static StateCategories ParseStateCategory(string[] fileContent)
        {
            foreach (var content in fileContent)
            {
                if (content.Contains("state_category"))
                {
                    string stateCategoryString = content.Trim();

                    if (CheckComment(stateCategoryString))
                        stateCategoryString = DeleteComment(stateCategoryString);

                    stateCategoryString = stateCategoryString.Replace(" ", "").Remove(0, "state_category".Length + 1);

                    switch (stateCategoryString)
                    {
                        case "city": return StateCategories.city;
                        case "enclave": return StateCategories.enclave;
                        case "large_city": return StateCategories.large_city;
                        case "large_town": return StateCategories.large_town;
                        case "megalopolis": return StateCategories.megalopolis;
                        case "metropolis": return StateCategories.metropolis;
                        case "pastoral": return StateCategories.pastoral;
                        case "rural": return StateCategories.rural;
                        case "small_island": return StateCategories.small_island;
                        case "tiny_island": return StateCategories.tiny_island;
                        case "town": return StateCategories.town;
                        default: return StateCategories.wasteland;
                    }
                }
            }

            return StateCategories.wasteland;
        }
        
        private static bool CheckComment(string str)
        {
            if (Regex.IsMatch(str, @".?#.*"))
                return true;
            return false;
        }

        private static string DeleteComment(string str)
        {
            var commentMatch = Regex.Match(str, @".?#.*");
            var strWithOutComment = str.Remove(commentMatch.Index, commentMatch.Length);
            return strWithOutComment;
        }
    }
}
