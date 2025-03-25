using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NSMQBroker
{
    public class MQBroker
    {
        // TcpListener para escuchar conexiones entrantes de clientes
        private TcpListener listener;
        
        // Lista enlazada para gestionar las suscripciones (AppID, Topic)
        private SubscriptionLinkedList subscriptions;
        
        // Lista enlazada de colas de mensajes para cada suscriptor
        private SubscriptionQueueLinkedList subscriptionQueues;
        
        // Variable para controlar el estado del servidor
        private bool isRunning;

        public MQBroker(int port)
        {
            subscriptions = new SubscriptionLinkedList();
            subscriptionQueues = new SubscriptionQueueLinkedList();
            listener = new TcpListener(IPAddress.Any, port);
        }

        /// <summary>
        /// Inicia el servidor y comienza a escuchar conexiones entrantes.
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
                    // Acepta nuevas conexiones de clientes
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Cliente conectado.");
                    
                    // Crea un hilo independiente para procesar cada cliente
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
        /// Procesa las solicitudes del cliente mediante comandos recibidos.
        /// </summary>
        private void ProcessClient(TcpClient client)
        {
            using (client)
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[2048];

                try
                {
                    while (true)
                    {
                        // Lee los datos enviados por el cliente
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        if (bytesRead == 0)
                        {
                            Console.WriteLine("El cliente cerró la conexión.");
                            break;
                        }

                        string request = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                        Console.WriteLine("Mensaje recibido: " + request);

                        // Divide el mensaje en partes para interpretar el comando
                        string[] parts = request.Split(' ');

                        if (parts.Length >= 3)
                        {
                            string command = parts[0].ToUpper();
                            string appID = parts[1];
                            string topic = parts[2];

                            // Ejecuta el comando correspondiente
                            if (command == "SUBSCRIBE")
                            {
                                Subscribe(appID, topic);
                                Console.WriteLine($"Suscripción añadida: AppID={appID}, Topic={topic}");
                            }
                            else if (command == "UNSUBSCRIBE")
                            {
                                Unsubscribe(appID, topic);
                                Console.WriteLine($"Suscripción eliminada: AppID={appID}, Topic={topic}");
                            }
                            else if (command == "PUBLISH" && parts.Length >= 4)
                            {
                                string messageContent = string.Join(" ", parts, 3, parts.Length - 3);
                                Publish(appID, topic, messageContent);
                                Console.WriteLine($"Mensaje publicado en Topic={topic}");
                            }
                            else if (command == "RECEIVE")
                            {
                                string msgContent = ReceiveMessage(appID, topic);
                                if (string.IsNullOrEmpty(msgContent))
                                {
                                    Console.WriteLine("No hay mensajes disponibles o no está suscrito.");
                                }
                                else
                                {
                                    Console.WriteLine($"Mensaje recibido: {msgContent}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Comando desconocido.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Formato incorrecto. Se espera: COMMAND AppID Topic [Mensaje]");
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
        /// Agrega una nueva suscripción si no existe.
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
            subscriptionQueues.Add(newSub);
        }

        /// <summary>
        /// Elimina una suscripción existente.
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
        /// Publica un mensaje en un topic y lo encola para los suscriptores.
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
            Console.WriteLine($"Publish -> Mensaje encolado para {queues.Length} suscriptores del topic '{topic}'");
        }

        public string ReceiveMessage(string appID, string topic)
        {
            Subscription sub = new Subscription(appID, topic);
            if (!subscriptions.Contains(sub))
            {
                Console.WriteLine($"Receive -> (AppID={appID}, Topic={topic}) NO está suscrito.");
                return string.Empty;
            }

            var queue = subscriptionQueues.Find(sub);
            if (queue == null || queue.Messages.Count == 0)
            {
                Console.WriteLine($"Receive -> No hay mensajes para (AppID={appID}, Topic={topic}).");
                return string.Empty;
            }

            Message msg = queue.Messages.Dequeue();
            Console.WriteLine($"Receive -> (AppID={appID}, Topic={topic}) recibió mensaje: '{msg.Content}'");
            return msg.Content;
        }

        public void Stop()
        {
            isRunning = false;
            listener.Stop();
        }
    }
}
