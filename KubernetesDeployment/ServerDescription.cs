using System.Collections;
using System.Collections.Generic;
using KubernetesWorkTest.KubernetesDeployment.KubernetesLogic;

namespace KubernetesWorkTest.KubernetesDeployment
{
    public class ServerDescription
    {
        private IDictionary<string, string> _labels;
        private List<KubernetesPort> _ports;
        private string _name;
        private readonly string _idProject;
        private readonly string _domain;
      
        public ServerDescription(string domain, string idProject)
        {
            _domain = domain;
            _idProject = idProject;
        }

        public IDictionary<string, string> Labels => _labels;

        public List<KubernetesPort> Ports => _ports;

        public string Name => _name;

        public string IdProject => _idProject;

        public string Domain => _domain;

        public void SetLabels(string idUser, string idServer)
        {
            _labels = new Dictionary<string, string>
            {
                {"id-user", idUser},
                {"id-server", idServer},
                {"id-project", _idProject},
                {"domain", _domain},
            };
            _name = $"{idUser}-{idServer}-{_idProject}-{_domain}";
        }

        public void SetPorts(List<KubernetesPort> ports)
        {
            _ports = ports;
           
        }
    }
}