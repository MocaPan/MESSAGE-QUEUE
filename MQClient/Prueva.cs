using System;

namespace NSMQClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Guid appId = Guid.NewGuid();
                MQClient client = new MQClient("127.0.0.1", 9090, appId);

                Console.WriteLine("Conectado al servidor MQBroker.");

                // Suscribirse a un topic
                Console.WriteLine("Intentando suscribirse al topic 'TestTopic'...");
                if (client.Subscribe("TestTopic"))
                    Console.WriteLine("Suscripción exitosa.");
                else
                    Console.WriteLine("Error en la suscripción.");

                // Publicar un mensaje en el topic
                Console.WriteLine("Publicando mensaje en 'TestTopic'...");
                if (client.Publish("TestTopic", "¡Hola desde el cliente MQ!"))
                    Console.WriteLine("Mensaje publicado correctamente.");
                else
                    Console.WriteLine("Error al publicar mensaje.");

                // Intentar recibir un mensaje del topic
                Console.WriteLine("Intentando recibir mensaje de 'TestTopic'...");
                if (client.Receive("TestTopic"))
                    Console.WriteLine("Mensaje recibido correctamente o comando enviado con éxito.");
                else
                    Console.WriteLine("No se recibió ningún mensaje o hubo un error.");

                // Cancelar la suscripción
                Console.WriteLine("Cancelando suscripción a 'TestTopic'...");
                if (client.Unsubscribe("TestTopic"))
                    Console.WriteLine("Desuscripción exitosa.");
                else
                    Console.WriteLine("Error en la desuscripción.");

                client.Close();
                Console.WriteLine("Conexión cerrada.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error durante la prueba: {ex.Message}");
            }
        }
    }
}
