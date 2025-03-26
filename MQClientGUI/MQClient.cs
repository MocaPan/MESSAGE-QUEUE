using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms; // Necesario para usar MessageBox

namespace NSMQClient
{
    public class MQClient
    {
        private TcpClient client; // Instancia del cliente TCP
        private NetworkStream? stream; // Flujo de datos de la conexión. Nullable para evitar errores si la conexión falla.
        private Guid appId; // Identificador único de la aplicación

        // Constructor que recibe la IP del servidor, el puerto y el ID de la aplicación
        public MQClient(string serverIp, int port, Guid appId)
        {
            this.appId = appId; // Asignar el appId recibido
            client = new TcpClient(); // Crear una nueva instancia de TcpClient
            try
            {
                // Intentar conectar al servidor usando la IP y el puerto proporcionados
                client.Connect(serverIp, port);
                stream = client.GetStream(); // Obtener el flujo de red para la comunicación
                MessageBox.Show("[Info] Conectado al servidor."); // Mensaje de éxito en la conexión
            }
            catch (Exception ex)
            {
                // En caso de error, mostrar el mensaje con el error
                MessageBox.Show($"[Error] No se pudo conectar al servidor: {ex.Message}");
            }
        }

        // Método para suscribirse a un tema
        public bool Subscribe(string topic)
        {
            return SendCommand($"SUBSCRIBE {appId} {topic}"); // Enviar comando para suscribirse
        }

        // Método para desuscribirse de un tema
        public bool Unsubscribe(string topic)
        {
            return SendCommand($"UNSUBSCRIBE {appId} {topic}"); // Enviar comando para desuscribirse
        }

        // Método para publicar un mensaje en un tema
        public bool Publish(string topic, string message)
        {
            return SendCommand($"PUBLISH {appId} {topic} {message}"); // Enviar comando para publicar
        }

        // Método para recibir un mensaje de un tema
        public bool Receive(string topic)
        {
            return SendCommand($"RECEIVE {appId} {topic}"); // Enviar comando para recibir
        }

        // Método privado para enviar un comando al servidor
        private bool SendCommand(string command)
        {
            try
            {
                // Verificar que el cliente esté conectado y que el flujo de datos no sea nulo
                if (!client.Connected || stream == null)
                {
                    MessageBox.Show("[Advertencia] El cliente no está conectado al servidor o la conexión es nula.");
                    return false; // Retornar false si no está conectado
                }

                // Convertir el comando a un arreglo de bytes en formato UTF8
                byte[] data = Encoding.UTF8.GetBytes(command);
                stream.Write(data, 0, data.Length); // Enviar los datos al servidor
                MessageBox.Show($"[Info] Comando enviado: {command}"); // Mostrar el comando enviado
                return true; // Retornar true si el comando fue enviado exitosamente
            }
            catch (Exception ex)
            {
                // Mostrar el error si algo falla al enviar el comando
                MessageBox.Show($"[Error] No se pudo enviar el comando: {ex.Message}");
                return false; // Retornar false en caso de error
            }
        }

        // Método para cerrar la conexión al servidor
        public void Close()
        {
            try
            {
                // Verificar si el cliente está conectado y si el flujo no es nulo
                if (client.Connected && stream != null)
                {
                    SendCommand("CLOSE"); // Enviar comando para cerrar la conexión
                    stream.Close(); // Cerrar el flujo de datos
                    client.Close(); // Cerrar la conexión TCP
                    MessageBox.Show("[Info] Conexión cerrada."); // Mostrar mensaje de éxito
                }
            }
            catch (Exception ex)
            {
                // Mostrar el error si no se pudo cerrar la conexión
                MessageBox.Show($"[Error] No se pudo cerrar la conexión: {ex.Message}");
            }
        }
    }
}
