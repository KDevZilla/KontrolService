using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KontrolService
{
    public class Global
    {
        private static  Setting setting = null;
        public static  String SettingFilePath { get; set; } = Util.FileUtil.SettingsPath;
        public static  Setting CurrentSetting
        {
            get
            {
                if(setting == null)
                {
                    if(!System.IO.File.Exists(SettingFilePath))
                    {
                        SerializeHelper.SerializeSetting(new Setting(), SettingFilePath);
                    }
                    SerializeHelper.DeserializeSetting(ref setting, SettingFilePath);
                }

                return setting;
            }
        }
        public static  void SaveSetting()
        {
            SerializeHelper.SerializeSetting(setting, SettingFilePath);
        }
    }
}
