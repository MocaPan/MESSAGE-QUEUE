// Este archivo define una lista enlazada simple para manejar suscripciones a temas por parte de aplicaciones o clientes.
// La lista permite agregar, encontrar, verificar y eliminar suscripciones, así como obtener una representación en cadena de todas las suscripciones.

using System; // Importa el espacio de nombres System, que contiene clases fundamentales.
using System.Text; // Importa el espacio de nombres System.Text, que contiene clases para manipulación de texto.

namespace NSMQBroker // Define un espacio de nombres llamado MQBroker.
{
    // Clase que representa un nodo en la lista enlazada de suscripciones.
    // Contiene:
    //  - Data: la suscripción actual.
    //  - Next: referencia al siguiente nodo en la lista.
    public class SubscriptionNode
    {
        public Subscription Data { get; set; } // Propiedad para almacenar la suscripción.
        public SubscriptionNode Next { get; set; } // Propiedad para apuntar al siguiente nodo en la lista.

        // Constructor que recibe la suscripción a almacenar en el nodo.
        public SubscriptionNode(Subscription data)
        {
            this.Data = data; // Asigna el valor del parámetro data a la propiedad Data.
            this.Next = null; // Inicialmente, no hay siguiente nodo.
        }
    }

    // Clase que implementa una lista enlazada simple para manejar las suscripciones
    // sin utilizar diccionarios u otras colecciones predefinidas.
    public class SubscriptionLinkedList
    {
        // Referencia al primer nodo (cabeza) de la lista.
        private SubscriptionNode head;

        // Contador de nodos en la lista, para llevar control de la cantidad de suscripciones.
        public int Count { get; private set; }

        // Constructor que inicializa la lista vacía.
        public SubscriptionLinkedList()
        {
            this.head = null; // Inicializa la cabeza como null.
            Count = 0; // Inicializa el contador de nodos como 0.
        }

        // Método para agregar una nueva suscripción al final de la lista,
        // siempre y cuando no exista ya en la lista.
        public void Add(Subscription subscription)
        {
            // Si la suscripción ya está presente, no se agrega.
            if (Contains(subscription))
                return;

            // Creamos un nuevo nodo para almacenar la suscripción.
            SubscriptionNode newNode = new SubscriptionNode(subscription);

            // Si la lista está vacía, el nuevo nodo pasa a ser la cabeza.
            if (this.head == null)
            {
                this.head = newNode; // Asigna el nuevo nodo a la cabeza.
            }
            else
            {
                // De lo contrario, recorremos hasta el último nodo y lo enlazamos.
                SubscriptionNode current = this.head;
                while (current.Next != null)
                {
                    current = current.Next; // Avanza al siguiente nodo.
                }
                current.Next = newNode; // Enlaza el nuevo nodo al final de la lista.
            }

            // Incrementamos el contador de nodos.
            Count++;
        }

        // Método para encontrar un nodo en la lista que coincida con el appID y topic.
        public SubscriptionNode Find(string appID, string topic)
        {
            SubscriptionNode current = this.head; // Comienza desde la cabeza.

            // Recorremos la lista y comparamos con los valores recibidos.
            while (current != null)
            {
                // Se compara ignorando mayúsculas, gracias a la implementación en Subscription.
                if (current.Data.AppID.Equals(appID, StringComparison.OrdinalIgnoreCase) &&
                    current.Data.Topic.Equals(topic, StringComparison.OrdinalIgnoreCase))
                {
                    return current; // Retorna el nodo si coincide.
                }
                current = current.Next; // Avanza al siguiente nodo.
            }

            // Si no se encuentra, retornamos null.
            return null;
        }

        // Verifica si la lista contiene una suscripción dada.
        public bool Contains(Subscription subscription)
        {
            // Utiliza el método Find para ver si existe un nodo que coincida.
            return Find(subscription.AppID, subscription.Topic) != null;
        }

        // Elimina una suscripción de la lista, si existe.
        public bool Remove(Subscription subscription)
        {
            // Si la lista está vacía, no hay nada que eliminar.
            if (this.head == null)
                return false;

            // Si la suscripción a eliminar está en la cabeza, reasignamos la cabeza.
            if (this.head.Data.Equals(subscription))
            {
                this.head = this.head.Next; // Avanza la cabeza al siguiente nodo.
                Count--; // Decrementa el contador de nodos.
                return true;
            }

            // De lo contrario, recorremos la lista en busca del nodo a eliminar.
            SubscriptionNode current = this.head;
            while (current.Next != null)
            {
                // Si el siguiente nodo coincide con la suscripción a eliminar,
                // "saltamos" ese nodo en la cadena enlazada.
                if (current.Next.Data.Equals(subscription))
                {
                    current.Next = current.Next.Next; // Elimina el nodo de la lista.
                    Count--; // Decrementa el contador de nodos.
                    return true;
                }
                current = current.Next; // Avanza al siguiente nodo.
            }

            // Si no se encontró, retornamos false.
            return false;
        }

        // Representación en cadena de todas las suscripciones de la lista.
        public override string ToString()
        {
            StringBuilder newString = new StringBuilder(); // Crea un StringBuilder para construir la cadena de salida.
            SubscriptionNode current = this.head; // Comienza desde la cabeza.

            while (current != null) // Recorre todos los nodos.
            {
                newString.AppendLine(current.Data.ToString()); // Añade la representación en texto de la suscripción al StringBuilder.
                current = current.Next; // Avanza al siguiente nodo.
            }

            return newString.ToString(); // Retorna la cadena construida.
        }
    }
}



