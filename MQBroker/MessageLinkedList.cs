// Este archivo define una lista enlazada para manejar mensajes, con operaciones para encolar, desencolar y visualizar mensajes.

using System; // Importa el espacio de nombres System, que contiene clases fundamentales.
using System.Text; // Importa el espacio de nombres System.Text, que contiene clases para manipulación de texto.

namespace MQBroker // Define un espacio de nombres llamado MQBroker.
{
    internal class MessageNode // Declara una clase interna llamada MessageNode.
    {
        public Message Data { get; set; } // Propiedad para almacenar el mensaje.
        public MessageNode Next { get; set; } // Propiedad para apuntar al siguiente nodo en la lista.

        public MessageNode(Message data) // Constructor que inicializa las propiedades Data y Next.
        {
            Data = data; // Asigna el valor del parámetro data a la propiedad Data.
            Next = null; // Inicializa la propiedad Next como null.
        }
    }

    public class MessageLinkedList // Declara una clase pública llamada MessageLinkedList.
    {
        private MessageNode head; // Apunta al primer nodo (frente de la cola).
        private MessageNode tail; // Apunta al último nodo (final de la cola).
        public int Count { get; private set; } // Propiedad para contar el número de elementos en la lista.

        public MessageLinkedList() // Constructor que inicializa la lista vacía.
        {
            head = null; // Inicializa head como null.
            tail = null; // Inicializa tail como null.
            Count = 0; // Inicializa Count como 0.
        }

        /// Encola (agrega al final) un mensaje.
        public void Enqueue(Message message)
        {
            MessageNode newNode = new MessageNode(message); // Crea un nuevo nodo con el mensaje.

            if (head == null) // Si la cola está vacía.
            {
                // La cola está vacía; head y tail apuntan al nuevo nodo.
                head = newNode; // Asigna el nuevo nodo a head.
                tail = newNode; // Asigna el nuevo nodo a tail.
            }
            else
            {
                // Agregamos al final.
                tail.Next = newNode; // Enlaza el nuevo nodo al final de la cola.
                tail = newNode; // Actualiza tail para que apunte al nuevo nodo.
            }
            Count++; // Incrementa el contador de elementos.
        }

        /// Desencola (retira del frente) y retorna el mensaje.
        public Message Dequeue()
        {
            if (head == null) // Si la cola está vacía.
            {
                // Cola vacía.
                return null; // Retorna null.
            }

            // Guardamos el nodo frontal.
            MessageNode temp = head; // Guarda el nodo frontal.
            // Avanzamos la cabeza.
            head = head.Next; // Avanza head al siguiente nodo.

            // Si la cabeza es null, significa que la cola quedó vacía; tail = null.
            if (head == null) // Si la cola quedó vacía.
            {
                tail = null; // Asigna null a tail.
            }

            Count--; // Decrementa el contador de elementos.
            return temp.Data; // Retorna el mensaje del nodo frontal.
        }

        /// Permite ver el primer mensaje sin eliminarlo.
        public Message Peek()
        {
            return head?.Data; // Retorna el mensaje del nodo frontal sin eliminarlo.
        }

        /// Verifica si la cola está vacía.
        public bool IsEmpty()
        {
            return Count == 0; // Retorna true si la cola está vacía, de lo contrario false.
        }

        /// Devuelve una representación en texto de todos los mensajes.
        public override string ToString()
        {
            StringBuilder newMessage = new StringBuilder(); // Crea un StringBuilder para construir la cadena de salida.
            MessageNode current = head; // Comienza desde el nodo frontal.
            while (current != null) // Recorre todos los nodos.
            {
                newMessage.AppendLine(current.Data.ToString()); // Añade la representación en texto del mensaje al StringBuilder.
                current = current.Next; // Avanza al siguiente nodo.
            }
            return newMessage.ToString(); // Retorna la cadena construida.
        }
    }
}