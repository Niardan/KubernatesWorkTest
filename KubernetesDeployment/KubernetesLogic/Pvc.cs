using System.Collections.Generic;
using k8s.Models;

namespace KubernetesWorkTest.KubernetesDeployment.KubernetesLogic
{
    public class Pvc
    {
        public V1PersistentVolume BuildPv(ServerDescription serverDescription)
        {
            V1PersistentVolume pv = new()
            {
                Metadata = new V1ObjectMeta
                {
                    Name = "rsc-storage",
                    Labels = new Dictionary<string, string>
                    {
                        {"id-project",serverDescription.IdProject}
                    }
                },
                ApiVersion = "v1",
                Kind = "PersistentVolume",
                Spec = new V1PersistentVolumeSpec
                {
                    StorageClassName = "rsc-local",
                    AccessModes = new List<string>
                    {
                        "ReadWriteOnce"
                    },
                    Capacity = new Dictionary<string, ResourceQuantity>
                    {
                        {"storage", new ResourceQuantity("5Gi")}
                    },
                    HostPath = new V1HostPathVolumeSource
                    {
                        Path = "/e/Minikube/Data"
                    }
                }
            };
            return pv;

        }


        public V1PersistentVolumeClaim BasePvc(ServerDescription serverDescription)
        {
            var pvc = new V1PersistentVolumeClaim
            {
                ApiVersion = "v1",
                Kind = "PersistentVolumeClaim",
                Metadata = new V1ObjectMeta
                {
                    Name = "rsc-volume",
                    Labels = new Dictionary<string, string>
                    {
                        {"id-project", serverDescription.IdProject}
                    }
                },
                Spec = new V1PersistentVolumeClaimSpec
                {
                    StorageClassName = "rsc-local",
                    AccessModes = new List<string>
                    {
                        "ReadWriteOnce"
                    },
                    Resources = new V1ResourceRequirements
                    {
                        Requests = new Dictionary<string, ResourceQuantity>
                        {
                            {"storage", new ResourceQuantity("5Gi")}
                        }
                    }
                }
            };
            return pvc;
        }
    }
}