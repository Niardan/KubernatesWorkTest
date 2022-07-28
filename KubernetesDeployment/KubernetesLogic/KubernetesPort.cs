using KubernetesWorkTest.KubernetesDeployment.Descriptions;

namespace KubernetesWorkTest.KubernetesDeployment.KubernetesLogic
{
    public class KubernetesPort
    {
        public string Name { set; get; }
        public int Port { set; get; }
        public int TargetPort { set; get; }
        public Protocol Protocol { set; get; }
    }
}