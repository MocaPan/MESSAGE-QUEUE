using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MQBroker
{
    public class MQBroker
    {
        private TcpListener listener;

        // Lista enlazada de suscripciones (AppID, Topic).
        private SubscriptionLinkedList subscriptions;

        // Lista enlazada de colas de mensajes para cada suscriptor (AppID, Topic).
        private SubscriptionQueueLinkedList subscriptionQueues;

        private bool isRunning;

        public MQBroker(int port)
        {
            // Inicializamos ambas listas (suscripciones y colas).
            subscriptions = new SubscriptionLinkedList();
            subscriptionQueues = new SubscriptionQueueLinkedList();

            listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            listener.Start();
            isRunning = true;
            Console.WriteLine("MQBroker: Servidor iniciado. Escuchando conexiones...");

            while (isRunning)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Cliente conectado.");

                    Thread clientThread = new Thread(() => ProcessClient(client));
                    clientThread.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al aceptar cliente: " + ex.Message);
                }
            }
        }

        private void ProcessClient(TcpClient client)
        {
            using (client)
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[2048];

                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
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
                            // Se requiere al menos 4 partes: PUBLISH <AppID> <Topic> <Mensaje...>
                            if (parts.Length >= 4)
                            {
                                // El mensaje puede tener espacios, lo reconstruimos.
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
                            // Retiramos el siguiente mensaje de la cola del suscriptor.
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
                catch (Exception ex)
                {
                    Console.WriteLine("Error al procesar cliente: " + ex.Message);
                }
            }
        }

        private void SendResponse(NetworkStream stream, string message)
        {
            byte[] responseBytes = Encoding.UTF8.GetBytes(message);
            stream.Write(responseBytes, 0, responseBytes.Length);
        }

        public void Subscribe(string appID, string topic)
        {
            Subscription newSub = new Subscription(appID, topic);

            // Si ya existe, no la duplicamos (la lista y colas se encargan de no duplicar).
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

        public void Unsubscribe(string appID, string topic)
        {
            Subscription sub = new Subscription(appID, topic);

            // Eliminamos de la lista de suscripciones.
            if (subscriptions.Remove(sub))
            {
                Console.WriteLine("Suscripción eliminada: " + sub.ToString());
            }
            else
            {
                Console.WriteLine("No se encontró la suscripción para eliminar: " + sub.ToString());
            }

            // Eliminamos la cola de mensajes de este suscriptor.
            subscriptionQueues.Remove(sub);
        }

        /// Publica un mensaje en el Topic. 
        /// Por cada suscriptor que tenga ese Topic, se encola el mensaje.
        /// Si no hay suscriptores para ese Topic, se ignora la publicación.
        public void Publish(string appID, string topic, string messageContent)
        {
            // 1. Verificamos si existe AL MENOS un suscriptor para ese topic.
            //    Usamos FindAllByTopic(...) en subscriptionQueues.
            var queues = subscriptionQueues.FindAllByTopic(topic);
            if (queues.Length == 0)
            {
                // No hay suscriptores para este Topic; ignoramos la publicación.
                Console.WriteLine($"Publish -> No hay suscriptores para el topic '{topic}'. Se ignora.");
                return;
            }

            // 2. Creamos el objeto Message.
            Message msg = new Message(appID, messageContent);

            // 3. Para cada suscriptor con ese topic, encolamos el mensaje.
            foreach (var sub in queues)
            {
                sub.Messages.Enqueue(msg);
            }

            Console.WriteLine($"Publish -> Se encoló mensaje '{messageContent}' para {queues.Length} suscriptores del topic '{topic}'.");
        }

        /// Retira el siguiente mensaje de la cola de (AppID, Topic), si existe.
        /// Retorna el contenido del mensaje o cadena vacía si no hay mensajes o no está suscrito.
        public string ReceiveMessage(string appID, string topic)
        {
            Subscription sub = new Subscription(appID, topic);

            // 1. Verificamos si (AppID, Topic) está suscrito.
            if (!subscriptions.Contains(sub))
            {
                Console.WriteLine($"Receive -> (AppID={appID}, Topic={topic}) NO está suscrito.");
                return string.Empty;
            }

            // 2. Buscamos la cola correspondiente.
            var queue = subscriptionQueues.Find(sub);
            if (queue == null)
            {
                Console.WriteLine($"Receive -> No existe cola para (AppID={appID}, Topic={topic}).");
                return string.Empty;
            }

            // 3. Desencolamos el siguiente mensaje.
            Message msg = queue.Messages.Dequeue();
            if (msg == null)
            {
                // No hay mensajes.
                Console.WriteLine($"Receive -> Cola vacía para (AppID={appID}, Topic={topic}).");
                return string.Empty;
            }

            Console.WriteLine($"Receive -> (AppID={appID}, Topic={topic}) recibió mensaje: '{msg.Content}'.");
            return msg.Content;
        }

        public void Stop()
        {
            isRunning = false;
            listener.Stop();
        }
    }
}
