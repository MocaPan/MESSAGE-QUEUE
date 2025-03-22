using System;
using System.Net.Sockets;
using System.Text;
using EstructurasPersonalizadas;
using EstructurasParaMQBroker;


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
        private ListaDoble<Topic> temasSuscritos;  // Lista de tópicos a los que el cliente está suscrito


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
            this.temasSuscritos = new ListaDoble<Topic>(); // Inicializa la lista de temas suscritos

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

        /// <summary>
        /// Método que solicita una suscripción al servidor.
        /// </summary>
        public bool Subscribe(Topic topic)
        {
            // Verifica que la conexión está activa
            if (stream == null || !stream.CanWrite)
            {
                Console.WriteLine("❌ Error: No hay conexión con MQBroker.");
                return false;
            }

            try
            {
                // Construye el mensaje con el formato correcto
                Console.WriteLine($"📢 Enviando solicitud de suscripción para: {topic.Name}");
                string message = $"SUBSCRIBE {appID} {topic.Name}";

                // Convierte el mensaje a bytes utilizando Arreglo<byte>
                Arreglo<byte> data = new Arreglo<byte>(Encoding.UTF8.GetBytes(message + "\n"));

                // Envía los datos a MQBroker
                stream.Write(data.ObtenerDatos(), 0, data.Tamaño());
                Console.WriteLine("📤 Mensaje de suscripción enviado al servidor.");

                // Espera respuesta del servidor
                Arreglo<byte> buffer = new Arreglo<byte>(256);
                int bytesRead = stream.Read(buffer.ObtenerDatos(), 0, buffer.Tamaño());
                string response = Encoding.UTF8.GetString(buffer.ObtenerDatos(), 0, bytesRead).Trim();

                // Muestra la respuesta del servidor
                Console.WriteLine($"📩 Respuesta recibida del servidor: {response}");

                // Verifica si la suscripción fue exitosa
                if (response == "SUBSCRIBE_OK")
                {
                    temasSuscritos.Agregar(topic); // Guarda el tema en la lista personalizada
                    Console.WriteLine($"✅ Suscripción exitosa al tópico: {topic.Name}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"❌ Error al suscribirse a {topic.Name}: {response}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en Subscribe: {ex.Message}");
                return false;
            }
        }

    }
}
