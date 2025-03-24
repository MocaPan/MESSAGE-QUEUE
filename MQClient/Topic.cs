namespace NSMQClient
{
    public class Topic
    {
        public string Name { get; private set; }

        public Topic(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("El nombre del topic no puede estar vacío.");
            }
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
