using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace AAgOpenGPS;

//public class Registry
//{
  //  public string KeyName { get; set; }
//}

public class ConfigManager
{
    public Dictionary<string, string> LoadFromJson(string sFilePath)
    {
        var json = File.ReadAllText(sFilePath);
        return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
    }
}
