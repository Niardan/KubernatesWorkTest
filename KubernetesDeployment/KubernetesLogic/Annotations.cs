using System.Collections.Generic;
using System.Text.Json;

namespace KubernetesWorkTest.KubernetesDeployment.KubernetesLogic
{
    public class KubernetesAnnotations
    {
        public IDictionary<string, string> GetNegPorts(IEnumerable<KubernetesPort> ports)
        {
            return new Dictionary<string, string>
            {
                {"cloud.google.com/neg", GetExposedPorts(ports)},
                {"cloud.google.com/network-tier", "Standard"}
            };
        }

        public static string GetExposedPorts(IEnumerable<KubernetesPort> ports)
        {
            var portsDict = new Dictionary<string, IDictionary<string, string>>();

            foreach (var port in ports)
            {
                portsDict.Add(port.Port.ToString(), new Dictionary<string, string> { { "name", port.Name } });
            }

            var dict = new Dictionary<string, IDictionary<string, IDictionary<string, string>>>
                {{"exposed_ports", portsDict}};
            return JsonSerializer.Serialize(dict);
        }
    }
}