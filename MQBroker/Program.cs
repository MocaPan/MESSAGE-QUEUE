using System.Threading;

namespace MQBroker
{
    // Clase principal del proyecto.
    // Contiene el método Main, punto de entrada de la aplicación.
    class Program
    {
        static void Main(string[] args)
        {
            // Definimos el puerto en el cual el servidor MQBroker escuchará las conexiones.
            int port = 8080;

            // Instanciamos el servidor, pasándole el puerto.
            MQBroker broker = new MQBroker(port);

            // Iniciamos el servidor en un hilo independiente, para no bloquear la ejecución principal.
            Thread serverThread = new Thread(new ThreadStart(broker.Start));
            serverThread.Start();

            // Informamos al usuario que presione ENTER para detener el servidor.
            Console.WriteLine("Presione ENTER para detener el servidor...");
            Console.ReadLine();

            // Al presionar ENTER, se detiene el servidor.
            broker.Stop();
        }
    }
}