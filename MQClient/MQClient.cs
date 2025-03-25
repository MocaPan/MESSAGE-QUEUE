using System;
using System.Net.Sockets;
using System.Text;

namespace NSMQClient
{
    public class MQClient
    {
        private TcpClient client;
        private NetworkStream? stream; // Hacerlo nullable para evitar el error CS8618
        private Guid appId;

        public MQClient(string serverIp, int port, Guid appId)
        {
            this.appId = appId;
            client = new TcpClient();
            try
            {
                client.Connect(serverIp, port);
                stream = client.GetStream();
                Console.WriteLine("[Info] Conectado al servidor.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] No se pudo conectar al servidor: {ex.Message}");
            }
        }

        public bool Subscribe(string topic)
        {
            return SendCommand($"SUBSCRIBE {appId} {topic}");
        }

        public bool Unsubscribe(string topic)
        {
            return SendCommand($"UNSUBSCRIBE {appId} {topic}");
        }

        public bool Publish(string topic, string message)
        {
            return SendCommand($"PUBLISH {appId} {topic} {message}");
        }

        public bool Receive(string topic)
        {
            return SendCommand($"RECEIVE {appId} {topic}");
        }

        private bool SendCommand(string command)
        {
            try
            {
                if (!client.Connected || stream == null)
                {
                    Console.WriteLine("[Advertencia] El cliente no está conectado al servidor o la conexión es nula.");
                    return false;
                }

                byte[] data = Encoding.UTF8.GetBytes(command);
                stream.Write(data, 0, data.Length);
                Console.WriteLine($"[Info] Comando enviado: {command}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] No se pudo enviar el comando: {ex.Message}");
                return false;
            }
        }

        public void Close()
        {
            try
            {
                if (client.Connected && stream != null)
                {
                    SendCommand("CLOSE");
                    stream.Close();
                    client.Close();
                    Console.WriteLine("[Info] Conexión cerrada.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] No se pudo cerrar la conexión: {ex.Message}");
            }
        }
    }
}
