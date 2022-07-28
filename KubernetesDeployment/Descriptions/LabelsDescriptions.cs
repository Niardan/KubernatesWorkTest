using System.Collections.Generic;

namespace KubernetesWorkTest.KubernetesDeployment.Descriptions
{
    public class LabelsDescriptions
    {
        private string _idProject;

        public LabelsDescriptions(string idProject)
        {
            _idProject = idProject;
        }

        public IDictionary<string, string> GetLabels(string idUser, string idServer)
        {
            return new Dictionary<string, string>
            {
                {"id-user", idUser},
                {"id-server", idServer},
                {"id-project", _idProject},
            };
        }
    }
}