using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Asteroids
{
    public static class ConfigLoader
    {
        public static T LoadConfig<T>(string fileName)
        {
            var nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            var textAsset = Resources.Load<TextAsset>($"Configs/{nameWithoutExt}");
            if (textAsset == null)
            {
                Debug.LogError($"Config not found in Resources/Configs: {fileName}");
                return default;
            }

            return JsonConvert.DeserializeObject<T>(textAsset.text);
        }
    }
}
    