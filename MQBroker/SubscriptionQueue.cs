// Este archivo define la clase SubscriptionQueue, que asocia una suscripción (AppID, Topic) con una cola de mensajes (MessageLinkedList).
// La clase permite gestionar los mensajes en cola para cada suscripción.

using System; // Importa el espacio de nombres System, que contiene clases fundamentales.

namespace MQBroker // Define un espacio de nombres llamado MQBroker.
{
    /// Asocia una suscripción (AppID, Topic) con una cola de mensajes (MessageLinkedList).
    public class SubscriptionQueue
    {
        public Subscription Subscription { get; set; } // Propiedad para almacenar la suscripción.
        public MessageLinkedList Messages { get; set; } // Propiedad para almacenar la cola de mensajes.

        // Constructor que recibe una suscripción y crea una nueva cola de mensajes.
        public SubscriptionQueue(Subscription subscription)
        {
            Subscription = subscription; // Asigna la suscripción a la propiedad Subscription.
            Messages = new MessageLinkedList(); // Inicializa la cola de mensajes.
        }

        // Representación de la cantidad de mensajes en cola para un usuario, para debugging o logs.
        public override string ToString()
        {
            // Retorna una cadena que contiene la representación de la suscripción y la cantidad de mensajes en cola.
            return $"{Subscription.ToString()}, Mensajes en cola: {Messages.Count}";
        }
    }
}


