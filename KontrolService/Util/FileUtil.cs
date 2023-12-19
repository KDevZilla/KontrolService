using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KontrolService.Util
{
    public class FileUtil
    {
        public static string AppInfoPath
        {
            get
            {
                String ExePath = new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                return Path.GetDirectoryName(ExePath) + @"\AppInfo";

            }
        }

        public static string ProjectFilesPath => AppInfoPath + @"\ProjectFiles\";
        public static string SettingsPath => AppInfoPath + @"\Settings.bin";
        public static List<String> ListAllFiles(String path, String searchPattern)
        {

            DirectoryInfo d = new DirectoryInfo(path); //Assuming Test is your Folder

            FileInfo[] Files = d.GetFiles(searchPattern); //Getting Text files
            return Files.Select(x => x.Name).ToList();

        }
        public static List<String> ListAllProjectXMLContent(String path)
        {
            List<String> listFile = ListAllFiles(path, "*.xml");
            int i;
            List<String> listContent = new List<string>();
            for (i = 0; i < listFile.Count; i++)
            {
                string XMLContent = GetXMLFile(path + listFile[i]);
                listContent.Add(XMLContent);
            }
            return listContent;
        }
        public static String GetXMLFile(String fileName)
        {
            String fileContent = "";
            using (System.IO.StreamReader SR = new StreamReader(fileName))
            {
                fileContent = SR.ReadToEnd();

            }
            return fileContent;
        }
        public static void SaveFile(String fileName, String stringXML)
        {
            using (System.IO.StreamWriter SW = new StreamWriter(fileName))
            {
                SW.Write(stringXML);
                SW.Close();
            }

        }

    }
}
