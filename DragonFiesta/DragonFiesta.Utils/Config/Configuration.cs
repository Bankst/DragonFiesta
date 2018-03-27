using System.IO;
using System.Xml.Serialization;

namespace DragonFiesta.Utils.Config
{
    public class Configuration<T>
    {
        public void CreateDefaultFolder()
        {
            string folder = "Configuration";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        public void WriteXml(string ConfigName = null)
        {
            CreateDefaultFolder();

            string path = Path.Combine(
                "Configuration", string.Format("{0}.xml",
                ConfigName == null ? typeof(T).Name : ConfigName));


            var writer = new XmlSerializer(GetType());
            var file = new StreamWriter(path);
            writer.Serialize(file, this);
            file.Close();
        }

        public static T ReadXml(string ConfigName = null)
        {

            string path = Path.Combine(
                     "Configuration", string.Format("{0}.xml", 
                     ConfigName == null ? typeof(T).Name : ConfigName));




            if (File.Exists(path))
            {
                var reader = new XmlSerializer(typeof(T));
                var file = new StreamReader(path);

                T xml = (T)reader.Deserialize(file);

                file.Close();
                return xml;
            }
            else return default(T);
        }
    }
}