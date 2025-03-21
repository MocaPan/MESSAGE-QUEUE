using System;
using System.Text;

namespace MQBroker
{
   
    internal class MessageNode
    {
        public Message Data { get; set; }
        public MessageNode Next { get; set; }

        public MessageNode(Message data)
        {
            Data = data;
            Next = null;
        }
    }

    public class MessageLinkedList
    {
        private MessageNode head; // Apunta al primer nodo (frente de la cola).
        private MessageNode tail; // Apunta al último nodo (final de la cola).
        public int Count { get; private set; }

        public MessageLinkedList()
        {
            head = null;
            tail = null;
            Count = 0;
        }

        /// Encola (agrega al final) un mensaje.
        public void Enqueue(Message message)
        {
            MessageNode newNode = new MessageNode(message);

            if (head == null)
            {
                // La cola está vacía; head y tail apuntan al nuevo nodo.
                head = newNode;
                tail = newNode;
            }
            else
            {
                // Agregamos al final.
                tail.Next = newNode;
                tail = newNode;
            }
            Count++;
        }

        /// Desencola (retira del frente) y retorna el mensaje.
        public Message Dequeue()
        {
            if (head == null)
            {
                // Cola vacía.
                return null;
            }

            // Guardamos el nodo frontal.
            MessageNode temp = head;
            // Avanzamos la cabeza.
            head = head.Next;

            // Si la cabeza es null, significa que la cola quedó vacía; tail = null.
            if (head == null)
            {
                tail = null;
            }

            Count--;
            return temp.Data;
        }

        /// Permite ver el primer mensaje sin eliminarlo.
        public Message Peek()
        {
            return head?.Data;
        }

        /// Verifica si la cola está vacía.
        public bool IsEmpty()
        {
            return Count == 0;
        }

        /// Devuelve una representación en texto de todos los mensajes.
        public override string ToString()
        {
            StringBuilder newMessage = new StringBuilder();
            MessageNode current = head;
            while (current != null)
            {
                newMessage.AppendLine(current.Data.ToString());
                current = current.Next;
            }
            return newMessage.ToString();
        }
    }
}
