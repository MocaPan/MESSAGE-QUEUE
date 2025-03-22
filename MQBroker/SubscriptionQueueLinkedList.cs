// Este archivo define una lista enlazada para administrar todas las colas de mensajes de cada suscriptor.
// La lista permite agregar, encontrar, verificar y eliminar colas de mensajes asociadas a suscripciones (AppID, Topic).

using System; // Importa el espacio de nombres System, que contiene clases fundamentales.

namespace MQBroker // Define un espacio de nombres llamado MQBroker.
{
    /// Nodo de la lista enlazada para SubscriptionQueue.
    public class SubscriptionQueueNode
    {
        public SubscriptionQueue Data { get; set; } // Propiedad para almacenar la cola de suscripción.
        public SubscriptionQueueNode Next { get; set; } // Propiedad para apuntar al siguiente nodo en la lista.

        // Constructor que recibe la cola de suscripción a almacenar en el nodo.
        public SubscriptionQueueNode(SubscriptionQueue data)
        {
            Data = data; // Asigna el valor del parámetro data a la propiedad Data.
            Next = null; // Inicialmente, no hay siguiente nodo.
        }
    }

    /// Lista enlazada para administrar todas las colas de mensajes de cada suscriptor.
    public class SubscriptionQueueLinkedList
    {
        private SubscriptionQueueNode head;  // Cabeza de la lista.
        public int Count { get; private set; } // Propiedad para contar el número de nodos en la lista.

        // Constructor que inicializa la lista vacía.
        public SubscriptionQueueLinkedList()
        {
            head = null; // Inicializa la cabeza como null.
            Count = 0; // Inicializa el contador de nodos como 0.
        }

        /// Agrega una nueva SubscriptionQueue si no existe para esa (AppID, Topic).
        public void Add(Subscription subscription)
        {
            // Verificar si ya existe la SubscriptionQueue para la suscripción (AppID, Topic)
            if (Find(subscription) != null)
                return;

            // Crear la nueva SubscriptionQueue y el nodo
            SubscriptionQueue newSQ = new SubscriptionQueue(subscription);
            SubscriptionQueueNode newNode = new SubscriptionQueueNode(newSQ);

            // Si la lista está vacía, hacemos que el nuevo nodo sea la cabeza
            if (head == null)
            {
                head = newNode; // Asigna el nuevo nodo a la cabeza.
            }
            else
            {
                // Si no está vacía, recorremos la lista hasta el final y agregamos el nuevo nodo
                SubscriptionQueueNode current = head;
                while (current.Next != null)
                {
                    current = current.Next; // Avanza al siguiente nodo.
                }
                current.Next = newNode; // Enlaza el nuevo nodo al final de la lista.
            }

            // Incrementamos el contador de nodos
            Count++;
        }

        /// Retorna la SubscriptionQueue correspondiente a la (AppID, Topic), o null si no existe.
        public SubscriptionQueue Find(Subscription subscription)
        {
            // Recorremos la lista buscando una SubscriptionQueue que coincida con la suscripción
            SubscriptionQueueNode current = head;
            while (current != null)
            {
                if (current.Data.Subscription.Equals(subscription))
                {
                    return current.Data; // Retorna la cola de suscripción si coincide.
                }
                current = current.Next; // Avanza al siguiente nodo.
            }

            // Si no se encuentra, retornamos null
            return null;
        }

        /// Elimina la SubscriptionQueue de la lista enlazada, si existe.
        public bool Remove(Subscription subscription)
        {
            if (head == null) // Si la lista está vacía, no hay nada que eliminar.
                return false;

            // Si la suscripción que queremos eliminar está en la cabeza, la cambiamos
            if (head.Data.Subscription.Equals(subscription))
            {
                head = head.Next; // Avanza la cabeza al siguiente nodo.
                Count--; // Decrementa el contador de nodos.
                return true;
            }

            // Recorremos la lista para buscar y eliminar la suscripción
            SubscriptionQueueNode current = head;
            while (current.Next != null)
            {
                if (current.Next.Data.Subscription.Equals(subscription))
                {
                    current.Next = current.Next.Next; // Elimina el nodo de la lista.
                    Count--; // Decrementa el contador de nodos.
                    return true;
                }
                current = current.Next; // Avanza al siguiente nodo.
            }

            return false; // No se encontró el nodo
        }

        /// Retorna una lista de SubscriptionQueue que coincidan con un Topic específico.
        /// Esto se usa al hacer Publish, para encolar en cada suscriptor de ese Topic.
        public SubscriptionQueue[] FindAllByTopic(string topic)
        {
            // 1. Contamos cuántos nodos tienen este 'topic'
            int count = 0;
            SubscriptionQueueNode current = head;
            while (current != null)
            {
                if (current.Data.Subscription.Topic.Equals(topic, StringComparison.OrdinalIgnoreCase))
                {
                    count++;
                }
                current = current.Next;
            }

            // 2. Creamos un array de tamaño 'count'
            SubscriptionQueue[] result = new SubscriptionQueue[count];

            // 3. Recorremos de nuevo para llenarlo
            current = head;
            int index = 0;
            while (current != null)
            {
                if (current.Data.Subscription.Topic.Equals(topic, StringComparison.OrdinalIgnoreCase))
                {
                    // Usamos LA MISMA COLA
                    result[index++] = current.Data;
                }
                current = current.Next;
            }

            return result;
        }

    }
}



