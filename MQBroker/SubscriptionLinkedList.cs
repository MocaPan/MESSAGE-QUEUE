using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQBroker
{
    internal class SubscriptionNode
    {
        public Subscription Data { get; set; }
        public SubscriptionNode? Next { get; set; }

        public SubscriptionNode(Subscription data)
        {
            this.Data = data;
            this.Next = null;
        }
    }

    public class SubscriptionLinkedList
    {
        private SubscriptionNode head;
        public int Count { get; private set; }

        public SubscriptionLinkedList()
        {
            head = null;
            Count = 0;
        }

        public void Add(Subscription subscription)
        {
            if (Contains(subscription))
                return;

            SubscriptionNode newNode = new SubscriptionNode(subscription);
            if (head == null)
            {
                head = newNode;
            }
            else
            {
                SubscriptionNode current = head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }
            Count++;
        }

        public SubscriptionNode Find(string appID, string topic)
        {
            SubscriptionNode current = head;
            while (current != null)
            {
                if (current.Data.AppID.Equals(appID, StringComparison.OrdinalIgnoreCase) &&
                    current.Data.Topic.Equals(topic, StringComparison.OrdinalIgnoreCase))
                {
                    return current;
                }
                current = current.Next;
            }
            return null;
        }


        public bool Contains(Subscription subscription)
        {
            return Find(subscription.AppID, subscription.Topic) != null;
        }

        public bool Remove(Subscription subscription)
        {
            if (head == null)
                return false;

            if (head.Data.Equals(subscription))
            {
                head = head.Next;
                Count--;
                return true;
            }

            SubscriptionNode current = head;
            while (current.Next != null)
            {
                if (current.Next.Data.Equals(subscription))
                {
                    current.Next = current.Next.Next;
                    Count--;
                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            SubscriptionNode current = head;
            while (current != null)
            {
                sb.AppendLine(current.Data.ToString());
                current = current.Next;
            }
            return sb.ToString();
        }
    }
}

