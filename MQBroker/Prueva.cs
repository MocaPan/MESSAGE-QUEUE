using System;
using System.Threading;
using NSMQBroker;
using NSMQClient;

namespace NSMQBroker
{
    using BrokerMessage = NSMQBroker.Message;
    using ClientMessage = NSMQClient.Message;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Iniciar el broker en un hilo separado
                Thread brokerThread = new Thread(() =>
                {
                    MQBroker broker = new MQBroker(5000);
                    broker.Start();
                });
                brokerThread.Start();

                Thread.Sleep(1000); // Espera para asegurar que el broker está corriendo

                // Crear un cliente y probar los métodos
                Guid appId = Guid.NewGuid();
                MQClient client = new MQClient("127.0.0.1", 5000, appId);
                Topic topic = new Topic("TestTopic");

                Console.WriteLine("Probando suscripción...");
                bool suscripcionExitosa = client.Subscribe(topic);
                Console.WriteLine(suscripcionExitosa ? "Suscripción exitosa" : "Error en suscripción");

                Console.WriteLine("Probando publicación...");
                ClientMessage message = new ClientMessage("Hola, este es un mensaje de prueba.");
                bool publicacionExitosa = client.Publish(message, topic);
                Console.WriteLine(publicacionExitosa ? "Publicación exitosa" : "Error en publicación");

                Console.WriteLine("Probando recepción...");
                ClientMessage receivedMessage = client.Receive(topic);
                Console.WriteLine(receivedMessage != null
                    ? $"Mensaje recibido: {receivedMessage}"
                    : "No hay mensajes disponibles");

                Console.WriteLine("Probando desuscripción...");
                bool desuscripcionExitosa = client.Unsubscribe(topic);
                Console.WriteLine(desuscripcionExitosa ? "Desuscripción exitosa" : "Error en desuscripción");

                // Cerrar cliente
                client.Close();
                Console.WriteLine("Cliente cerrado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error: {ex.Message}");
            }
        }
    }
}
