using System;
using System.Collections.Generic;
using k8s;
using k8s.Models;
using KubernetesWorkTest.KubernetesDeployment;
using KubernetesWorkTest.KubernetesDeployment.Descriptions;
using KubernetesWorkTest.KubernetesDeployment.KubernetesLogic;

namespace KubernetesWorkTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            var client = new Kubernetes(config);
            var stateFull = new StatefulSet();
            var labels = new Dictionary<string, string>();
            var port = new KubernetesPort { Name = "8080", Port = 8080, Protocol = Protocol.Tcp, TargetPort = 8080 };
            var namespaces = client.CoreV1.ListNamespace();

            var serverDescription = new ServerDescription("localhost", "tpso");
            serverDescription.SetLabels("v.kotov", "center");

            var state = stateFull.BuildService(serverDescription, null, labels, "test");
            //  var service = new ServiceFactory();
            //var  s = service.BuildService(new KubernetesMetadata { Label = labels, Name = "testcenter" },
            //      new Dictionary<string, string>(), new Dictionary<string, string>(), new List<KubernetesPort> { port });

            //  client.CoreV1.CreateNamespacedService(s,"default");
            // client.CreateNamespacedStatefulSet(new V1StatefulSet(), "default");

            V1StatefulSet deployment = new V1StatefulSet()
            {
                ApiVersion = "apps/v1",
                Kind = "StatefulSet",
                Metadata = new V1ObjectMeta()
                {
                    Name = "nepomucen",
                    NamespaceProperty = null,
                    Labels = new Dictionary<string, string>()
                {
                    { "app", "nepomucen" }
                }
                },
                Spec = new V1StatefulSetSpec()
                {
                    Replicas = 1,
                    Selector = new V1LabelSelector()
                    {
                        MatchLabels = new Dictionary<string, string>
                    {
                        { "app", "nepomucen" }
                    }
                    },
                    Template = new V1PodTemplateSpec()
                    {
                        Metadata = new V1ObjectMeta()
                        {
                            CreationTimestamp = null,
                            Labels = new Dictionary<string, string>
                        {
                             { "app", "nepomucen" }
                        }
                        },
                        Spec = new V1PodSpec
                        {
                            Containers = new List<V1Container>()
                        {
                            new V1Container()
                            {
                                Name = "nginx",
                                Image = "nginx:1.7.9",
                                ImagePullPolicy = "Always",
                                Ports = new List<V1ContainerPort> { new V1ContainerPort(80) }
                            }
                        }
                        }
                    }
                },
                Status = new V1StatefulSetStatus()
                {
                    Replicas = 1
                }
            };

            var result = client.CreateNamespacedStatefulSet(state, "default");
            Console.ReadKey();
        }
    }
}
