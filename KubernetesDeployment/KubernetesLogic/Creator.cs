using System.Collections.Generic;
using System.Linq;
using k8s.Models;
using KubernetesWorkTest.KubernetesDeployment.Descriptions;

namespace KubernetesWorkTest.KubernetesDeployment.KubernetesLogic
{
    public class Creator
    {
        private readonly ServiceFactory _serviceFactory = new ServiceFactory();
        private readonly KubernetesAnnotations _kubernetesAnnotations = new KubernetesAnnotations();
        private readonly LabelsDescriptions _labelsDescriptions = new LabelsDescriptions("tpso");
        public V1Service ServiceCenterHeadless(string id, ServerType serverType, IList<KubernetesPort> ports)
        {
            var metadata = new KubernetesMetadata
            {
                Name = $"{id}-btl -{serverType}-headless",
                Label = _labelsDescriptions.GetLabels(id, serverType.ToString())
            };

            var service = _serviceFactory.BuildService(metadata, _kubernetesAnnotations.GetNegPorts(ports),
                _labelsDescriptions.GetLabels(id, serverType.ToString()), ports);
            return service;
        }

        public V1Service ServiceCenterNetwork(string id, ServerType serverType, IList<KubernetesPort> ports, Protocol protocol)
        {
            var metadata = new KubernetesMetadata
            {
                Name = $"{id}-btl -{serverType}-headless",
                Label = _labelsDescriptions.GetLabels(id, serverType.ToString())
            };
            var customPorts = ports.Where(n => n.Protocol == protocol).ToList();
            var service = _serviceFactory.BuildService(metadata, _kubernetesAnnotations.GetNegPorts(customPorts),
                _labelsDescriptions.GetLabels(id, serverType.ToString()), customPorts);
            return service;
        }
    }

    public enum ServerType
    {

    }
}