using NSMQBroker;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NSMQBroker
{
    public class MQBroker
    {
        private TcpListener listener;

        // Lista enlazada de suscripciones (AppID, Topic).
        private SubscriptionLinkedList subscriptions;

        // Lista enlazada de colas de mensajes para cada suscriptor (AppID, Topic).
        private SubscriptionQueueLinkedList subscriptionQueues;

        private bool isRunning;

        /// <summary>
        /// Constructor que inicializa el servidor en el puerto especificado,
        /// e inicializa las listas enlazadas de suscripciones y colas.
        /// </summary>
        /// <param name="port">Puerto en el que se escucharán conexiones entrantes.</param>
        public MQBroker(int port)
        {
            // Inicializamos ambas listas (suscripciones y colas).
            subscriptions = new SubscriptionLinkedList();
            subscriptionQueues = new SubscriptionQueueLinkedList();

            // Creamos el TcpListener para escuchar en todas las interfaces de red en el puerto indicado.
            listener = new TcpListener(IPAddress.Any, port);
        }

        /// <summary>
        /// Inicia el servidor: comienza a escuchar conexiones y, por cada cliente,
        /// lanza un hilo que ejecuta ProcessClient.
        /// </summary>
        public void Start()
        {
            listener.Start();
            isRunning = true;
            Console.WriteLine("MQBroker: Servidor iniciado. Escuchando conexiones...");

            while (isRunning)
            {
                try
                {
                    // Acepta de forma bloqueante la conexión de un cliente.
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Cliente conectado.");

                    // Crea un hilo para atender al cliente sin bloquear la escucha principal.
                    Thread clientThread = new Thread(() => ProcessClient(client));
                    clientThread.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al aceptar cliente: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Atiende a un cliente específico, leyendo múltiples comandos
        /// en la misma conexión hasta que el cliente cierre o se produzca un error.
        /// </summary>
        /// <param name="client">TcpClient conectado al cliente.</param>
        private void ProcessClient(TcpClient client)
        {
            using (client)
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[2048];

                try
                {
                    // Bucle para leer múltiples comandos
                    while (true)
                    {
                        // Bloquea hasta que el cliente envíe datos o cierre la conexión
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);

                        // Si bytesRead es 0, significa que el cliente cerró la conexión
                        if (bytesRead == 0)
                        {
                            Console.WriteLine("El cliente cerró la conexión.");
                            break;
                        }

                        // Convertimos lo recibido en string
                        string request = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                        Console.WriteLine("Mensaje recibido: " + request);

                        // Formato esperado:
                        // SUBSCRIBE <AppID> <Topic>
                        // UNSUBSCRIBE <AppID> <Topic>
                        // PUBLISH <AppID> <Topic> <Mensaje...>
                        // RECEIVE <AppID> <Topic>
                        string[] parts = request.Split(' ');

                        if (parts.Length >= 3)
                        {
                            string command = parts[0].ToUpper();
                            string appID = parts[1];
                            string topic = parts[2];

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
                            else if (command == "PUBLISH")
                            {
                                if (parts.Length >= 4)
                                {
                                    // El mensaje puede tener espacios, lo reconstruimos
                                    string messageContent = string.Join(" ", parts, 3, parts.Length - 3);
                                    Publish(appID, topic, messageContent);
                                    SendResponse(stream, $"Mensaje publicado en Topic={topic}");
                                }
                                else
                                {
                                    SendResponse(stream, "Formato incorrecto para PUBLISH. Se espera: PUBLISH <AppID> <Topic> <Mensaje>");
                                }
                            }
                            else if (command == "RECEIVE")
                            {
                                string msgContent = ReceiveMessage(appID, topic);
                                if (string.IsNullOrEmpty(msgContent))
                                {
                                    SendResponse(stream, "No hay mensajes disponibles o no está suscrito.");
                                }
                                else
                                {
                                    SendResponse(stream, $"Mensaje recibido: {msgContent}");
                                }
                            }
                            else
                            {
                                SendResponse(stream, "Comando desconocido.");
                            }
                        }
                        else
                        {
                            SendResponse(stream, "Formato incorrecto. Se espera: COMMAND AppID Topic [Mensaje]");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al procesar cliente: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Envía una respuesta al cliente a través del NetworkStream.
        /// </summary>
        private void SendResponse(NetworkStream stream, string message)
        {
            byte[] responseBytes = Encoding.UTF8.GetBytes(message);
            stream.Write(responseBytes, 0, responseBytes.Length);
        }

        /// <summary>
        /// Registra una suscripción (AppID, Topic).
        /// Si ya existe, no la vuelve a crear. Además, crea la cola de mensajes correspondiente.
        /// </summary>
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

            // Creamos también la cola de mensajes para este suscriptor (si no existe).
            subscriptionQueues.Add(newSub);
        }

        /// <summary>
        /// Elimina la suscripción (AppID, Topic) de la lista y su cola de mensajes.
        /// </summary>
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

            subscriptionQueues.Remove(sub);
        }

        /// <summary>
        /// Publica un mensaje en un topic. Si hay suscriptores para ese topic,
        /// se encola el mensaje en la cola de cada uno. Si no hay suscriptores, se ignora.
        /// </summary>
        public void Publish(string appID, string topic, string messageContent)
        {
            var queues = subscriptionQueues.FindAllByTopic(topic);
            if (queues.Length == 0)
            {
                Console.WriteLine($"Publish -> No hay suscriptores para el topic '{topic}'. Se ignora.");
                return;
            }

            Message msg = new Message(appID, messageContent);
            foreach (var sub in queues)
            {
                sub.Messages.Enqueue(msg);
            }

            Console.WriteLine($"Publish -> Se encoló mensaje '{messageContent}' para {queues.Length} suscriptores del topic '{topic}'.");
        }

        /// <summary>
        /// Desencola el siguiente mensaje de (AppID, Topic), si existe.
        /// Retorna el contenido del mensaje o cadena vacía si no hay mensajes o no está suscrito.
        /// </summary>
        public string ReceiveMessage(string appID, string topic)
        {
            Subscription sub = new Subscription(appID, topic);

            if (!subscriptions.Contains(sub))
            {
                Console.WriteLine($"Receive -> (AppID={appID}, Topic={topic}) NO está suscrito.");
                return string.Empty;
            }

            var queue = subscriptionQueues.Find(sub);
            if (queue == null)
            {
                Console.WriteLine($"Receive -> No existe cola para (AppID={appID}, Topic={topic}).");
                return string.Empty;
            }

            Message msg = queue.Messages.Dequeue();
            if (msg == null)
            {
                Console.WriteLine($"Receive -> Cola vacía para (AppID={appID}, Topic={topic}).");
                return string.Empty;
            }

            Console.WriteLine($"Receive -> (AppID={appID}, Topic={topic}) recibió mensaje: '{msg.Content}'.");
            return msg.Content;
        }

        /// <summary>
        /// Detiene el servidor, cerrando el TcpListener.
        /// </summary>
        public void Stop()
        {
            isRunning = false;
            listener.Stop();
        }
    }
}