using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MQBroker
{
    // Clase que implementa el servidor MQBroker.
    // - Escucha conexiones TCP en el puerto indicado.
    // - Gestiona suscripciones a través de la lista enlazada.
    // - Procesa los comandos SUBSCRIBE y UNSUBSCRIBE.
    public class MQBroker
    {
        // TcpListener para escuchar conexiones en un puerto.
        private TcpListener listener;

        // Lista enlazada que guarda todas las suscripciones.
        private SubscriptionLinkedList subscriptions;

        // Bandera para controlar el bucle de escucha.
        private bool isRunning;

        // Constructor: Inicializa la lista y el listener en IPAddress.Any, puerto recibido.
        public MQBroker(int port)
        {
            subscriptions = new SubscriptionLinkedList();
            listener = new TcpListener(IPAddress.Any, port);
        }

        // Inicia el servidor:
        // 1. listener.Start() para comenzar a escuchar.
        // 2. Bucle while (isRunning) que acepta clientes.
        public void Start()
        {
            listener.Start();
            isRunning = true;
            Console.WriteLine("MQBroker: Servidor iniciado. Escuchando conexiones...");

            while (isRunning)
            {
                try
                {
                    // Espera a que un cliente se conecte (bloqueante).
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Cliente conectado.");

                    // Crea un hilo para procesar a cada cliente sin bloquear la escucha de nuevos.
                    Thread clientThread = new Thread(() => ProcessClient(client));
                    clientThread.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al aceptar cliente: " + ex.Message);
                }
            }
        }

        // Método que procesa la comunicación con un cliente particular.
        // - Lee el mensaje del cliente.
        // - Determina si es SUBSCRIBE o UNSUBSCRIBE.
        // - Llama a los métodos apropiados y envía respuesta.
        private void ProcessClient(TcpClient client)
        {
            // Usamos "using" para asegurarnos de cerrar la conexión al final.
            using (client)
            {
                // Obtenemos el stream para leer/escribir datos.
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];

                try
                {
                    // Leemos datos enviados por el cliente.
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    // Convertimos el buffer en string (UTF8).
                    string request = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                    Console.WriteLine("Mensaje recibido: " + request);

                    // Dividimos el mensaje en partes (ej: "SUBSCRIBE 1234 deportes").
                    string[] parts = request.Split(' ');
                    if (parts.Length >= 3)
                    {
                        // El primer elemento es el comando, el segundo es AppID y el tercero es Topic.
                        string command = parts[0].ToUpper();
                        string appID = parts[1];
                        string topic = parts[2];

                        // Evaluamos el comando.
                        if (command == "SUBSCRIBE")
                        {
                            Subscribe(appID, topic);
                            SendResponse(stream, $"Suscripción añadida: AppID={appID}, Topic={topic}");
                        }
                        else if (command == "UNSUBSCRIBE")
                        {
                            Unsubscribe(appID, topic);
                            SendResponse(stream, $"Suscripción eliminada: AppID={appID}, Topic={topic}");
                        }
                        else
                        {
                            SendResponse(stream, "Comando desconocido.");
                        }
                    }
                    else
                    {
                        // Si no hay suficientes partes, enviamos un mensaje de error al cliente.
                        SendResponse(stream, "Formato incorrecto. Se espera: COMMAND AppID Topic");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al procesar cliente: " + ex.Message);
                }
            }
        }

        // Envía una respuesta al cliente a través del stream.
        private void SendResponse(NetworkStream stream, string message)
        {
            byte[] responseBytes = Encoding.UTF8.GetBytes(message);
            stream.Write(responseBytes, 0, responseBytes.Length);
        }

        // Método para suscribir un AppID a un Topic.
        // Agrega la suscripción a la lista si no existe ya.
        public void Subscribe(string appID, string topic)
        {
            Subscription newSub = new Subscription(appID, topic);
            if (!subscriptions.Contains(newSub))
            {
                subscriptions.Add(newSub);
                Console.WriteLine("Nueva suscripción agregada: " + newSub.ToString());
            }
            else
            {
                Console.WriteLine("La suscripción ya existe: " + newSub.ToString());
            }
        }

        // Método para desuscribir un AppID de un Topic.
        // Elimina la suscripción de la lista, si existe.
        public void Unsubscribe(string appID, string topic)
        {
            Subscription sub = new Subscription(appID, topic);
            if (subscriptions.Remove(sub))
            {
                Console.WriteLine("Suscripción eliminada: " + sub.ToString());
            }
            else
            {
                Console.WriteLine("No se encontró la suscripción para eliminar: " + sub.ToString());
            }
        }

        // Detiene el servidor:
        // - Cambia isRunning a false para romper el bucle.
        // - Cierra el listener.
        public void Stop()
        {
            isRunning = false;
            listener.Stop();
        }
    }
}