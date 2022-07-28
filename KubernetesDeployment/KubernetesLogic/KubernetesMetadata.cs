using System.Collections.Generic;

namespace KubernetesWorkTest.KubernetesDeployment.KubernetesLogic
{
    public class KubernetesMetadata
    {
        public string Name { set; get; }
        public IDictionary<string, string> Label { set; get; }
    }
}