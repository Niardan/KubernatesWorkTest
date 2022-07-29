using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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
            var str = File.ReadAllText(@"e:\Minikube\docker-compose.yml");

            var yaml = str.ParseYaml().GetParam("services").GetParam("btl-center").GetParam("environment").GetDictionary();
            var btl = str.ParseYaml().GetParam("services").GetParam("btl-center")["image"].ToString();
            var ports = str.ParseYaml().GetParam("services").GetParam("btl-center")["ports"] as List<object>;

            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            var client = new Kubernetes(config);
            var stateFull = new StatefulSet();
            var pv = new Pvc();
            var labels = new Dictionary<string, string>();
            var port = new KubernetesPort { Name = "http", Port = 8080, Protocol = Protocol.TCP, TargetPort = 8080 };
            var namespaces = client.CoreV1.ListNamespace();

            var serverDescription = new ServerDescription("localhost", "tpso");
            serverDescription.Init("v-kotov", "center", new List<KubernetesPort> { port }, yaml);
            serverDescription.BtlImage = btl;
            //  var service = new ServiceFactory();
            //try
            //{
            //    client.DeletePersistentVolume("rsc-storage");
            //}
            //catch
            //{
            //}
            //try
            //{
            //    client.DeleteNamespacedPersistentVolumeClaim("rsc-volume", "default");
            //}
            //catch
            //{
            //}
            try
            {
                client.DeleteNamespacedStatefulSet("v-kotov-center-tpso-localhost", "default");
            }
            catch
            {
            }



            Thread.Sleep(2000);
            var state = stateFull.BuildService(serverDescription, null, labels, "test");
            var pvI = pv.BuildPv(serverDescription);
            //var resultPV = client.CreatePersistentVolume(pvI);
            //var resultPVS = client.CreateNamespacedPersistentVolumeClaim(pv.BasePvc(serverDescription), "default");
            var result = client.CreateNamespacedStatefulSet(state, "default");
            Console.WriteLine("Complete");
            Console.ReadKey();
        }
    }
}
