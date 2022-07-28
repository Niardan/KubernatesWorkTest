namespace KubernetesWorkTest.KubernetesDeployment
{
    public class RoleDescription
    {
        public RoleDescription(string name, int size, int startPort)
        {
            Name = name;
            Size = size;
            StartPort = startPort;
        }

        public string Name { get; }
        public int Size { get; }
        public int StartPort { get; }
    }
}