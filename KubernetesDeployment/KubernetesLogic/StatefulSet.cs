using System.Collections.Generic;
using System.IO;
using System.Linq;
using k8s.Models;

namespace KubernetesWorkTest.KubernetesDeployment.KubernetesLogic
{
    public class StatefulSet
    {
        public V1StatefulSet BuildService(ServerDescription serverDescription, IDictionary<string, string> annotations,
            IDictionary<string, string> selector, string serviceName)
        {
            var service = new V1StatefulSet
            {
                ApiVersion = "apps/v1",
                Kind = "StatefulSet"
            };
            service.Metadata = new V1ObjectMeta()
            {
                Name = serverDescription.Name,
                Annotations = annotations,
            };
            service.Spec = new V1StatefulSetSpec()
            {
                Replicas = 1,
                Selector = new V1LabelSelector()
                {
                    MatchLabels = serverDescription.Labels
                },
                Template = BuildTemplate(serverDescription)
            };
            service.Status = new V1StatefulSetStatus()
            {
                Replicas = 1
            };
            return service;
        }

        private V1PodTemplateSpec BuildTemplate(ServerDescription serverDescription)
        {
            var template = new V1PodTemplateSpec()
            {
                Metadata = new V1ObjectMeta()
                {
                    CreationTimestamp = null,
                    Labels = serverDescription.Labels
                },
                Spec = new V1PodSpec
                {
                    RestartPolicy = "Always",
                    Containers = new List<V1Container> { BuildContainer(serverDescription) },
                    Volumes = new List<V1Volume>
                    {
                        new()
                        {
                            Name = "server",
                            PersistentVolumeClaim = new V1PersistentVolumeClaimVolumeSource("rsc-volume",false)
                        }
                    },
                    SecurityContext = new V1PodSecurityContext
                    {
                        FsGroup = 1001
                    }
                }
            };
            return template;
        }

        private V1Container BuildContainer(ServerDescription serverDescription)
        {
            var container = new V1Container
            {
                Name = $"{serverDescription.IdProject}-center-{serverDescription.Domain}",
                Image = serverDescription.BtlImage,
                SecurityContext = new V1SecurityContext()
                {
                    Capabilities = new V1Capabilities { Drop = new List<string> { "ALL" } },
                    ReadOnlyRootFilesystem = false,
                    RunAsNonRoot = true,
                    RunAsUser = 1001
                },
                ImagePullPolicy = "Never",
                Ports = serverDescription.Ports.Select(n => new V1ContainerPort(n.Port, null, n.Port, n.Name, n.Protocol.ToString())).ToList(),
                LivenessProbe = new V1Probe
                {
                    HttpGet = new V1HTTPGetAction()
                    {
                        Port = 1337,
                        Path = "status"
                    },
                    FailureThreshold = 4,
                    InitialDelaySeconds = 5,
                    PeriodSeconds = 10,
                    SuccessThreshold = 1,
                    TimeoutSeconds = 5
                },
                ReadinessProbe = new V1Probe
                {
                    HttpGet = new V1HTTPGetAction()
                    {
                        Port = 1337,
                        Path = "status"
                    },
                    FailureThreshold = 4,
                    InitialDelaySeconds = 5,
                    PeriodSeconds = 10,
                    SuccessThreshold = 1,
                    TimeoutSeconds = 5
                },
                Args = new List<string>
                {
                    "/opt/btl/bin/btl",
                    "foreground"
                },
                Env = serverDescription.Env.Select(n => new V1EnvVar(n.Key, n.Value)).ToList(),
                VolumeMounts = new List<V1VolumeMount>
                {
                    new()
                    {
                        Name = "server",
                        MountPath = "/opt/server/",
                        SubPath = "server_build/"
                    }
                }
            };
            return container;
        }

        private V1StatefulSet BaseService =>
            new()
            {
            };
    }
}