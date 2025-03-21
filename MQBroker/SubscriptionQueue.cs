using System;

namespace MQBroker
{
    /// Asocia una suscripción (AppID, Topic) con una cola de mensajes (MessageLinkedList).
    public class SubscriptionQueue
    {
        public Subscription Subscription { get; set; }
        public MessageLinkedList Messages { get; set; }

        public SubscriptionQueue(Subscription subscription)
        {
            Subscription = subscription;
            Messages = new MessageLinkedList();
        }

        // Representación de la cantidad de mensajes en cola para un un usuario, para debugging o logs.
        public override string ToString()
        {
            return $"{Subscription.ToString()}, Mensajes en cola: {Messages.Count}";
        }
    }
}
