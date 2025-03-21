using System;

namespace MQBroker
{
    /// Nodo de la lista enlazada para SubscriptionQueue.
    public class SubscriptionQueueNode
    {
        public SubscriptionQueue Data { get; set; }
        public SubscriptionQueueNode Next { get; set; }

        public SubscriptionQueueNode(SubscriptionQueue data)
        {
            Data = data;
            Next = null;
        }
    }

    /// Lista enlazada para administrar todas las colas de mensajes de cada suscriptor.
    public class SubscriptionQueueLinkedList
    {
        private SubscriptionQueueNode head;  // Cabeza de la lista.
        public int Count { get; private set; }

        public SubscriptionQueueLinkedList()
        {
            head = null;
            Count = 0;
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
                head = newNode;
            }
            else
            {
                // Si no está vacía, recorremos la lista hasta el final y agregamos el nuevo nodo
                SubscriptionQueueNode current = head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
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
                    return current.Data;
                }
                current = current.Next;
            }

            // Si no se encuentra, retornamos null
            return null;
        }

        /// Elimina la SubscriptionQueue de la lista enlazada, si existe.
        public bool Remove(Subscription subscription)
        {
            if (head == null)
                return false;

            // Si la suscripción que queremos eliminar está en la cabeza, la cambiamos
            if (head.Data.Subscription.Equals(subscription))
            {
                head = head.Next;
                Count--;
                return true;
            }

            // Recorremos la lista para buscar y eliminar la suscripción
            SubscriptionQueueNode current = head;
            while (current.Next != null)
            {
                if (current.Next.Data.Subscription.Equals(subscription))
                {
                    current.Next = current.Next.Next;
                    Count--;
                    return true;
                }
                current = current.Next;
            }

            return false; // No se encontró el nodo
        }

        /// Retorna una lista de SubscriptionQueue que coincidan con un Topic específico.
        /// Esto se usa al hacer Publish, para encolar en cada suscriptor de ese Topic.
        public SubscriptionQueue[] FindAllByTopic(string topic)
        {
            // Lista auxiliar para almacenar los nodos que coincidan con el Topic.
            SubscriptionQueueNode current = head;
            SubscriptionQueueLinkedList resultList = new SubscriptionQueueLinkedList(); // Usamos nuestra propia lista enlazada

            while (current != null)
            {
                if (current.Data.Subscription.Topic.Equals(topic, StringComparison.OrdinalIgnoreCase))
                {
                    resultList.Add(current.Data.Subscription);  // Agregar la suscripción al resultado
                }
                current = current.Next;
            }

            // Devolvemos la lista resultante como un array
            return ConvertListToArray(resultList);
        }

        /// Convierte la lista enlazada a un array de SubscriptionQueue.
        private SubscriptionQueue[] ConvertListToArray(SubscriptionQueueLinkedList list)
        {
            SubscriptionQueue[] result = new SubscriptionQueue[list.Count];
            SubscriptionQueueNode current = list.head;
            int index = 0;

            while (current != null)
            {
                result[index++] = current.Data;
                current = current.Next;
            }

            return result;
        }
    }
}
