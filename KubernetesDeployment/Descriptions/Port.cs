using System.Collections.Generic;
using k8s.Models;

namespace KubernetesWorkTest.KubernetesDeployment.Descriptions
{
    public class PortDescriptions
    {
        public V1ServicePort BtlStatus => GetPort(1337, Protocol.TCP, "btl-http", "btl-http");
        public V1ServicePort BtlMain => GetPort(8082, Protocol.TCP, "btl-main", "btl-main");
        public V1ServicePort BtlFluent => GetPort(22428, Protocol.UDP, "btl-fluent", "22428");

        public IList<V1ServicePort> BtlPortHeadless(ICollection<RoleDescription> roles)
        {
            var list = new List<V1ServicePort>
            {
                BtlMain,
                BtlStatus,
                BtlFluent
            };
            list.AddRange(RolesPort(roles));
            return list;
        }

        public IList<V1ServicePort> BtlPortUdp(ICollection<RoleDescription> roles)
        {
            var list = new List<V1ServicePort>
            {
                BtlFluent
            };
            list.AddRange(RolesPort(roles));
            return list;
        }

        public IList<V1ServicePort> BtlPortTcp()
        {
            var list = new List<V1ServicePort>
            {
                BtlMain,
                BtlStatus,
            };
            return list;
        }

        public ICollection<V1ServicePort> RolesPort(ICollection<RoleDescription> roles)
        {
            var list = new List<V1ServicePort>();
            foreach (var role in roles)
            {
                for (int i = 0; i < role.Size; i++)
                {
                    list.Add(GetPort(role.StartPort + i, Protocol.UDP,role.Name+i, (role.StartPort + i).ToString()));
                }
            }

            return list;
        }

        private V1ServicePort GetPort(int port, Protocol protocol, string name, string target)
        {
            return new V1ServicePort(port, protocol.ToString().ToUpperInvariant(), name, null,
                new IntstrIntOrString(target));
        }
    }

    public enum Protocol
    {
        TCP,
        UDP
    }
}