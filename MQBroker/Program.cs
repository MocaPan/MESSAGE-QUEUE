using System.Threading;

namespace MQBroker
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 8080;
            MQBroker broker = new MQBroker(port);

            // Se inicia el servidor en un hilo independiente.
            Thread serverThread = new Thread(new ThreadStart(broker.Start));
            serverThread.Start();

            Console.WriteLine("Presione ENTER para detener el servidor...");
            Console.ReadLine();

            broker.Stop();
        }
    }
}