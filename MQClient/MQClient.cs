using System;
using System.Net.Sockets;
using System.Text;

namespace MQClientLibrary // Espacio de nombres para evitar conflictos con otros archivos
{
    /// <summary>
    /// Clase que representa un cliente para conectarse a MQBroker mediante sockets.
    /// </summary>
    public class MQClient
    {
        // Variables privadas para almacenar la configuración del cliente
        private string ip;   // Dirección IP del servidor MQBroker
        private int port;    // Puerto en el que MQBroker está escuchando conexiones
        private Guid appID;  // Identificador único del cliente
        private TcpClient client;  // Objeto para manejar la conexión TCP
        private NetworkStream stream;  // Flujo de datos para enviar y recibir información

        /// <summary>
        /// Constructor de MQClient: inicializa los valores y se conecta al servidor.
        /// </summary>
        /// <param name="ip">Dirección IP donde se encuentra el MQBroker.</param>
        /// <param name="port">Puerto donde está escuchando el MQBroker.</param>
        /// <param name="appID">Identificador único de la aplicación cliente.</param>
        public MQClient(string ip, int port, Guid appID)
        {
            this.ip = ip;   // Guarda la dirección IP del servidor
            this.port = port;   // Guarda el puerto del servidor
            this.appID = appID;   // Guarda el identificador único del cliente
            this.client = new TcpClient(); // Crea el cliente TCP

            Connect(); // Llama al método para conectarse con MQBroker
        }

        /// <summary>
        /// Método que establece la conexión con el servidor MQBroker.
        /// </summary>
        private void Connect()
        {
            try
            {
                client.Connect(ip, port); // Intenta conectar con el servidor en la IP y puerto dados
                stream = client.GetStream(); // Obtiene el flujo de datos para comunicación
                Console.WriteLine("Conectado al MQBroker."); // Mensaje de éxito en la conexión
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Error al conectar con MQBroker: {ex.Message}"); // Captura y muestra el error en caso de fallo
            }
        }

        /// <summary>
        /// Método que cierra la conexión con el servidor MQBroker.
        /// </summary>
        public void Close()
        {
            try
            {
                if (stream != null)
                    stream.Close(); // Cierra el flujo de datos si está abierto

                if (client != null)
                    client.Close(); // Cierra la conexión TCP si está abierta

                Console.WriteLine("Conexión con MQBroker cerrada."); // Mensaje de confirmación
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cerrar la conexión: {ex.Message}"); // Captura y muestra el error si falla el cierre
            }
        }
    }
}
