
using MQBroker;
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
        private SubscriptionLinkedList subscriptions;
        private bool isRunning;

        public MQBroker(int port)
        {
            subscriptions = new SubscriptionLinkedList();
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
                byte[] buffer = new byte[1024];

                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string request = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                    Console.WriteLine("Mensaje recibido: " + request);

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
                        else
                        {
                            SendResponse(stream, "Comando desconocido.");
                        }
                    }
                    else
                    {
                        SendResponse(stream, "Formato incorrecto. Se espera: COMMAND AppID Topic");
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

        public void Stop()
        {
            isRunning = false;
            listener.Stop();
        }
    }
}
