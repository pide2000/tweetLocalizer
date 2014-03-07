using System;
using System.Web.Script;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace TweetLocalizationEF
{
    public class jsonFileAccess
    {

       

        public List<dynamic> getJSON(string filepath)
        {
            using (StreamReader reader = new StreamReader(filepath))
            {
                string json = reader.ReadToEnd();
                List<dynamic> items = JsonConvert.DeserializeObject<List<dynamic>>(json);
                return items;

            }


        }

        public static void saveAsJsonToFile(String filepath, dynamic objectToSave)
        {
            using (FileStream fs = File.Open(filepath, FileMode.CreateNew))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jw, objectToSave);

            }
        }

    }
}
