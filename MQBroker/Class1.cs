using System;
using System.Threading;
using MQClientLibrary; // 🔹 Cambiado para que coincida con el nuevo namespace
using MQBroker;

class TestProgram
{
    static void Main(string[] args)
    {
        int port = 8080;
        MQBroker.MQBroker broker = new MQBroker.MQBroker(port);

        Thread serverThread = new Thread(new ThreadStart(broker.Start));
        serverThread.Start();

        Console.WriteLine("MQBroker iniciado en el puerto 8080.");
        Thread.Sleep(2000);

        Console.WriteLine("Iniciando MQClient...");
        MQClient client = new MQClient("127.0.0.1", port, Guid.NewGuid());

        client.Close();
        Console.WriteLine("Prueba de MQClient completada.");

        Console.WriteLine("Presione ENTER para detener el servidor...");
        Console.ReadLine();

        broker.Stop();
    }
}
