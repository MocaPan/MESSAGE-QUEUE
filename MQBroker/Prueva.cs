using System;
using System.Threading;
using MQClientLibrary; // Importa MQClient
using MQBroker; // Importa MQBroker
using EstructurasParaMQBroker; // Importa Topic

class TestProgram
{
    static void Main(string[] args)
    {
        int port = 8080;

        // Iniciar el servidor MQBroker en un hilo separado
        MQBroker.MQBroker broker = new MQBroker.MQBroker(port);
        Thread serverThread = new Thread(new ThreadStart(broker.Start));
        serverThread.Start();

        Console.WriteLine("🟢 MQBroker iniciado en el puerto 8080.");
        Thread.Sleep(2000); // Espera para asegurar que el servidor esté activo

        try
        {
            Console.WriteLine("🔵 Iniciando MQClient...");
            MQClient client = new MQClient("127.0.0.1", port, Guid.NewGuid());

            // Prueba de Subscribe()
            Topic topic = new Topic("Noticias");
            Console.WriteLine($"📢 Intentando suscribirse a: {topic.Name}");

            bool suscrito = client.Subscribe(topic);

            if (suscrito)
                Console.WriteLine($"✅ Prueba exitosa: Suscripción a '{topic.Name}' realizada.");
            else
                Console.WriteLine($"❌ Prueba fallida: No se pudo suscribir a '{topic.Name}'.");

            client.Close();
            Console.WriteLine("🛑 Prueba de MQClient completada.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error en la prueba: {ex.Message}");
        }

        Console.WriteLine("⏳ Presione ENTER para detener el servidor...");
        Console.ReadLine();

        broker.Stop();
    }
}
