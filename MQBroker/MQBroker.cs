// Este archivo define la clase MQBroker, que implementa un servidor de mensajería simple.
// El servidor permite suscribirse a temas, publicar mensajes en temas y recibir mensajes de temas.

using System; // Importa el espacio de nombres System, que contiene clases fundamentales.
using System.Net; // Importa el espacio de nombres System.Net, que contiene clases para trabajar con redes.
using System.Net.Sockets; // Importa el espacio de nombres System.Net.Sockets, que contiene clases para trabajar con sockets.
using System.Text; // Importa el espacio de nombres System.Text, que contiene clases para manipulación de texto.
using System.Threading; // Importa el espacio de nombres System.Threading, que contiene clases para trabajar con hilos.


namespace MQBroker // Define un espacio de nombres llamado MQBroker.
{
    public class MQBroker // Declara una clase pública llamada MQBroker.
    {
        private TcpListener listener; // Objeto para escuchar conexiones TCP.

        // Lista enlazada de suscripciones (AppID, Topic).
        private SubscriptionLinkedList subscriptions;

        // Lista enlazada de colas de mensajes para cada suscriptor (AppID, Topic).
        private SubscriptionQueueLinkedList subscriptionQueues;

        private bool isRunning; // Indica si el servidor está en ejecución.

        public MQBroker(int port) // Constructor que inicializa el servidor en el puerto especificado.
        {
            // Inicializamos ambas listas (suscripciones y colas).
            subscriptions = new SubscriptionLinkedList();
            subscriptionQueues = new SubscriptionQueueLinkedList();

            listener = new TcpListener(IPAddress.Any, port); // Inicializa el listener en el puerto especificado.
        }

        public void Start() // Método para iniciar el servidor.
        {
            listener.Start(); // Inicia el listener.
            isRunning = true; // Marca el servidor como en ejecución.
            Console.WriteLine("MQBroker: Servidor iniciado. Escuchando conexiones...");

            while (isRunning) // Bucle principal del servidor.
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient(); // Acepta una conexión de cliente.
                    Console.WriteLine("Cliente conectado.");

                    // Crea un nuevo hilo para procesar la conexión del cliente.
                    Thread clientThread = new Thread(() => ProcessClient(client));
                    clientThread.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al aceptar cliente: " + ex.Message);
                }
            }
        }

        private void ProcessClient(TcpClient client) // Método para procesar la conexión de un cliente.
        {
            using (client) // Asegura que el cliente se cierre correctamente.
            {
                NetworkStream stream = client.GetStream(); // Obtiene el stream de red del cliente.
                byte[] buffer = new byte[2048]; // Buffer para leer datos del cliente.

                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length); // Lee datos del cliente.
                    string request = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim(); // Convierte los datos a una cadena.
                    Console.WriteLine("Mensaje recibido: " + request);

                    // Formato esperado:
                    // SUBSCRIBE <AppID> <Topic>
                    // UNSUBSCRIBE <AppID> <Topic>
                    // PUBLISH <AppID> <Topic> <Mensaje...>
                    // RECEIVE <AppID> <Topic>
                    string[] parts = request.Split(' '); // Divide la solicitud en partes.

                    if (parts.Length >= 3) // Verifica que la solicitud tenga al menos 3 partes.
                    {
                        string command = parts[0].ToUpper(); // Comando (SUBSCRIBE, UNSUBSCRIBE, PUBLISH, RECEIVE).
                        string appID = parts[1]; // ID de la aplicación.
                        string topic = parts[2]; // Tema de la suscripción.

                        if (command == "SUBSCRIBE") // Maneja el comando SUBSCRIBE.
                        {
                            Subscribe(appID, topic); // Añade una suscripción.
                            SendResponse(stream, $"Suscripción añadida: AppID={appID}, Topic={topic}");
                        }
                        else if (command == "UNSUBSCRIBE") // Maneja el comando UNSUBSCRIBE.
                        {
                            Unsubscribe(appID, topic); // Elimina una suscripción.
                            SendResponse(stream, $"Suscripción eliminada: AppID={appID}, Topic={topic}");
                        }
                        else if (command == "PUBLISH") // Maneja el comando PUBLISH.
                        {
                            // Se requiere al menos 4 partes: PUBLISH <AppID> <Topic> <Mensaje...>
                            if (parts.Length >= 4)
                            {
                                // El mensaje puede tener espacios, lo reconstruimos.
                                string messageContent = string.Join(" ", parts, 3, parts.Length - 3);
                                Publish(appID, topic, messageContent); // Publica un mensaje.
                                SendResponse(stream, $"Mensaje publicado en Topic={topic}");
                            }
                            else
                            {
                                SendResponse(stream, "Formato incorrecto para PUBLISH. Se espera: PUBLISH <AppID> <Topic> <Mensaje>");
                            }
                        }
                        else if (command == "RECEIVE") // Maneja el comando RECEIVE.
                        {
                            // Retiramos el siguiente mensaje de la cola del suscriptor.
                            string msgContent = ReceiveMessage(appID, topic); // Recibe un mensaje.
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
                            SendResponse(stream, "Comando desconocido."); // Comando no reconocido.
                        }
                    }
                    else
                    {
                        SendResponse(stream, "Formato incorrecto. Se espera: COMMAND AppID Topic [Mensaje]"); // Formato incorrecto.
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al procesar cliente: " + ex.Message);
                }
            }
        }

        private void SendResponse(NetworkStream stream, string message) // Método para enviar una respuesta al cliente.
        {
            byte[] responseBytes = Encoding.UTF8.GetBytes(message); // Convierte el mensaje a bytes.
            stream.Write(responseBytes, 0, responseBytes.Length); // Envía la respuesta al cliente.
        }

        public void Subscribe(string appID, string topic) // Método para añadir una suscripción.
        {
            Subscription newSub = new Subscription(appID, topic); // Crea una nueva suscripción.

            // Si ya existe, no la duplicamos (la lista y colas se encargan de no duplicar).
            if (!subscriptions.Contains(newSub))
            {
                subscriptions.Add(newSub); // Añade la suscripción si no existe.
                Console.WriteLine("Nueva suscripción agregada: " + newSub.ToString());
            }
            else
            {
                Console.WriteLine("La suscripción ya existe: " + newSub.ToString());
            }

            // Creamos también la cola de mensajes para este suscriptor (si no existe).
            subscriptionQueues.Add(newSub);
        }

        public void Unsubscribe(string appID, string topic) // Método para eliminar una suscripción.
        {
            Subscription sub = new Subscription(appID, topic); // Crea una suscripción para eliminar.

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
        public void Publish(string appID, string topic, string messageContent) // Método para publicar un mensaje.
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
        public string ReceiveMessage(string appID, string topic) // Método para recibir un mensaje.
        {
            Subscription sub = new Subscription(appID, topic); // Crea una suscripción para buscar.

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
            return msg.Content; // Retorna el contenido del mensaje.
        }

        public void Stop() // Método para detener el servidor.
        {
            isRunning = false; // Marca el servidor como no en ejecución.
            listener.Stop(); // Detiene el listener.
        }
    }
}


