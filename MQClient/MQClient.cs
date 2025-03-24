using System;
using System.Net.Sockets;
using System.Text;

namespace NSMQClient
{
    public class MQClient
    {
        private TcpClient client;
        private NetworkStream stream;
        private Guid appId;

        public MQClient(string serverIp, int port, Guid appId)
        {
            this.appId = appId;
            client = new TcpClient();
            client.Connect(serverIp, port);
            stream = client.GetStream();
        }

        public bool Subscribe(Topic topic)
        {
            return SendCommand($"SUBSCRIBE {appId} {topic.Name}");
        }

        public bool Unsubscribe(Topic topic)
        {
            return SendCommand($"UNSUBSCRIBE {topic.Name}");
        }

        public bool Publish(Message message, Topic topic)
        {
            return SendCommand($"PUBLISH {topic.Name} {message.Content}");
        }

        public Message Receive(Topic topic)
        {
            if (SendCommand($"RECEIVE {topic.Name}"))
            {
                byte[] data = new byte[1024];
                int bytesRead = stream.Read(data, 0, data.Length);
                string response = Encoding.UTF8.GetString(data, 0, bytesRead);
                return new Message(response);
            }
            return null;
        }

        private bool SendCommand(string command)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(command);
                stream.Write(data, 0, data.Length);

                byte[] responseBuffer = new byte[1024];
                int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
                string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);

                return response.Equals("OK", StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public void Close()
        {
            SendCommand("CLOSE");
            stream.Close();
            client.Close();
        }
    }
}
