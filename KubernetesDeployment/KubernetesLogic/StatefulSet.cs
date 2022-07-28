using System.Collections.Generic;
using System.Linq;
using k8s.Models;

namespace KubernetesWorkTest.KubernetesDeployment.KubernetesLogic
{
    public class StatefulSet
    {
        public V1StatefulSet BuildService(ServerDescription serverDescription, IDictionary<string, string> annotations,
            IDictionary<string, string> selector, string serviceName)
        {
            var service = BaseService;
            service.Metadata = new V1ObjectMeta()
            {
                Name = serverDescription.Name,
                Labels = serverDescription.Labels,
                Annotations = annotations,
            };
            service.Spec = new V1StatefulSetSpec()
            {
                Replicas = 1,
                Selector = new V1LabelSelector()
                {
                    MatchLabels = new Dictionary<string, string>
                    {
                        { "app", "nepomucen" }
                    }
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
                    Labels = new Dictionary<string, string>
                    {
                        { "app", "nepomucen" }
                    }},
                Spec = new V1PodSpec
                {
                    RestartPolicy = "Always",
                    Containers = new List<V1Container> { BuildContainer(serverDescription) }
                }
            };
            return template;
        }

        private V1Container BuildContainer(ServerDescription serverDescription)
        {
            var container = new V1Container
            {
                Name = $"{serverDescription.IdProject}-center.{serverDescription.Domain}",
                Image = "gcr.io/kefirdev/btl:0.13.21-9d92e881-303",
                ImagePullPolicy = "IfNotPresent",
                Command = new List<string> { "/opt/btl/bin/btl foreground" },
                Ports = serverDescription.Ports.Select(n => new V1ContainerPort(n.Port, null, null, n.Name, n.Protocol.ToString()))
                    .ToList(),
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
                Env = Env(serverDescription.IdProject, serverDescription.Domain),
                VolumeMounts = new List<V1VolumeMount>
                {
                    new()
                    {
                        MountPath = "/opt/server/",
                        SubPath = "./server_build/"
                    },
                    new()
                    {
                        MountPath = "/opt/references/",
                        SubPath = "./server_refs/"
                    }
                }
            };
            return container;
        }

        private List<V1EnvVar> Env(string id, string domain)
        {
            var list = new List<V1EnvVar>
            {
                new("CUSTOM_MAIN_CONF", "True"),
                new("IS_CHEAT_ENABLED", "True"),
                new("ENV", id),
                new("PROJECT", "tpso"),
                new("VERSION", "tpso_version"),
                new("HOST", $"{id}-center.{domain}"),
                new("GRAYLOG_SERVER", "127.0.0.1"),
                new("BTL_HOST", domain),
                new("RING_SIZE", "64"),
                new("RIAK_HOSTS", $"{id}-riak"),
                new("RIAK_CONNECTION_POOL_SIZE", "32"),
                new("AUTH_SALT", "pCsFG8XQfgD2m76hE1yFvzIAEbwf8Mtb"),
                new("ROLE_EXE", "/opt/server/Server"),
                new("ROLE_REFERENCES_PATH", "/opt/references/references_server.dat"),
                new("DATASETS_LIST", "users public_id_generators public_id_uids"),
                new("TZ", $"[eu]=$(btl@{id}-region-eu.{domain})"),
                new("REGIONS_LIST", "Europe/Volgograd"),
                new("TZ", @$"[user]='[ROLE_ARGS]=$(-type user -logger debug -profiler debug -regions eu) [SIZE]=1 [PORT]=45000' 
                [public_id_generator] = '[ROLE_ARGS]=$(-type user -logger debug -profiler debug) [SIZE]=1'
                [public_id_search] = '[ROLE_ARGS]=$(-type user -logger debug -profiler debug) [SIZE]=1' ")
            };
            return list;
        }
        private V1StatefulSet BaseService =>
            new()
            {
                ApiVersion = "apps/v1",
                Kind = "StatefulSet"
            };
    }
}