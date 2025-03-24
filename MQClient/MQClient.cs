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

        /// <summary>
        /// Constructor para inicializar el cliente y conectarse al broker.
        /// </summary>
        /// <param name="serverIp">Dirección IP del broker.</param>
        /// <param name="port">Puerto en el que el broker escucha.</param>
        /// <param name="appId">Identificador único del cliente.</param>
        public MQClient(string serverIp, int port, Guid appId)
        {
            this.appId = appId;
            client = new TcpClient();
            client.Connect(serverIp, port);
            stream = client.GetStream();
        }

        /// <summary>
        /// Solicita una suscripción a un topic específico.
        /// </summary>
        /// <param name="topic">El topic al que se desea suscribir.</param>
        /// <returns>True si la suscripción fue exitosa, False en caso contrario.</returns>
        public bool Subscribe(Topic topic)
        {
            return SendCommand($"SUBSCRIBE {appId} {topic.Name}");
        }

        /// <summary>
        /// Cancela la suscripción a un topic específico.
        /// </summary>
        /// <param name="topic">El topic del cual se desea desuscribir.</param>
        /// <returns>True si la desuscripción fue exitosa, False en caso contrario.</returns>
        public bool Unsubscribe(Topic topic)
        {
            return SendCommand($"UNSUBSCRIBE {appId} {topic.Name}");
        }

        /// <summary>
        /// Publica un mensaje en un topic específico.
        /// </summary>
        /// <param name="message">El mensaje a publicar.</param>
        /// <param name="topic">El topic en el que se publicará el mensaje.</param>
        /// <returns>True si el mensaje fue publicado con éxito, False en caso contrario.</returns>
        public bool Publish(Message message, Topic topic)
        {
            return SendCommand($"PUBLISH {appId} {topic.Name} {message.Content}");
        }

        /// <summary>
        /// Solicita un mensaje desde un topic específico.
        /// </summary>
        /// <param name="topic">El topic desde el cual se desea recibir un mensaje.</param>
        /// <returns>Un objeto Message si hay mensajes disponibles, Null si no hay mensajes.</returns>
        public Message Receive(Topic topic)
        {
            if (SendCommand($"RECEIVE {appId} {topic.Name}"))
            {
                byte[] data = new byte[1024];
                int bytesRead = stream.Read(data, 0, data.Length);
                string response = Encoding.UTF8.GetString(data, 0, bytesRead);
                return new Message(response);
            }
            return null;
        }

        /// <summary>
        /// Envía un comando al broker y espera una respuesta.
        /// </summary>
        /// <param name="command">El comando a enviar.</param>
        /// <returns>True si la respuesta es positiva, False en caso de error.</returns>
        private bool SendCommand(string command)
        {
            try
            {
                // Convertir y enviar el comando
                byte[] data = Encoding.UTF8.GetBytes(command);
                stream.Write(data, 0, data.Length);

                // Leer la respuesta del broker
                byte[] responseBuffer = new byte[1024];
                int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
                string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);

                // Validar si la respuesta fue exitosa
                return response.StartsWith("OK", StringComparison.OrdinalIgnoreCase) ||
                       response.StartsWith("SUBSCRIBE_OK", StringComparison.OrdinalIgnoreCase) ||
                       response.StartsWith("MESSAGE_RECEIVED", StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Cierra la conexión con el broker.
        /// </summary>
        public void Close()
        {
            SendCommand("CLOSE");
            stream.Close();
            client.Close();
        }
    }
}
