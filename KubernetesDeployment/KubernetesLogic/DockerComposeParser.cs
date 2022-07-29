using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KubernetesWorkTest.KubernetesDeployment.KubernetesLogic
{
    public static class  DockerComposeExtension
    {
        public static Dictionary<object, object> GetParam(this Dictionary<object, object> source, string key)
        {
            return source[key] as Dictionary<object, object>;
        }
        public static Dictionary<string, string> GetDictionary(this Dictionary<object, object> source)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var o in source)
            {
                dictionary.Add(o.Key.ToString(), o.Value.ToString());
            }
            return dictionary;
        }
        public static Dictionary<object, object> ParseYaml(this string source)
        {
            var deserializer = new YamlDotNet.Serialization.Deserializer();
            var dict = deserializer.Deserialize<Dictionary<object, object>>(source);
            return dict;
        }
    }
}