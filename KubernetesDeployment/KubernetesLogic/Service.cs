using System.Collections.Generic;
using System.Linq;
using k8s.Models;

namespace KubernetesWorkTest.KubernetesDeployment.KubernetesLogic
{
    public class ServiceFactory
    {
        public V1Service BuildService(KubernetesMetadata metadata, IDictionary<string, string> annotations,
            IDictionary<string, string> selector, IEnumerable<KubernetesPort> ports)
        {
            var service = BaseService;
            service.Metadata = new V1ObjectMeta()
            {
                Name = metadata.Name,
                Labels = metadata.Label,
                //Annotations = annotations,
            };
            //service.Spec = new V1ServiceSpec
            //{
            //    Ports = ports.Select(n => new V1ServicePort(n.Port, null, n.Name, null, n.Protocol.ToString(), new IntstrIntOrString(n.TargetPort.ToString()))).ToList(),
            //    Selector = selector,
            //    LoadBalancerIP = ""
            //};
            return service;
        }

        private V1Service BaseService =>
            new()
            {
                ApiVersion = "apps/v1",
                Kind = "Service"
            };
    }
}