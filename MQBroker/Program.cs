// Este archivo contiene la clase principal del proyecto, que incluye el método Main, el punto de entrada de la aplicación.
// Se encarga de iniciar el servidor MQBroker en un hilo independiente y esperar a que el usuario presione ENTER para detenerlo.

using System.Threading; // Importa el espacio de nombres System.Threading, que contiene clases para trabajar con hilos.

namespace MQBroker // Define un espacio de nombres llamado MQBroker.
{
    // Clase principal del proyecto.
    // Contiene el método Main, punto de entrada de la aplicación.
    class Program
    {
        static void Main(string[] args) // Método principal, punto de entrada de la aplicación.
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
            Console.ReadLine(); // Espera a que el usuario presione ENTER.

            // Al presionar ENTER, se detiene el servidor.
            broker.Stop();
        }
    }
}

