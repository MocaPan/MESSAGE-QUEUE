namespace NSMQClient
{
    public class Message
    {
        public string Content { get; set; }

        public Message(string content)
        {
            Content = content;
        }

        public override string ToString()
        {
            return Content;
        }
    }
}