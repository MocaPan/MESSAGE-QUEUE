using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQBroker
{
    public class Subscription
    {
        public string AppID { get; private set; }
        public string Topic { get; private set; }

        public Subscription(string appID, string topic)
        {
            this.AppID = appID;
            this.Topic = topic;
        }

        public override bool Equals(object obj)
        {
            if (obj is Subscription other)
            {
                return this.AppID.Equals(other.AppID, StringComparison.OrdinalIgnoreCase) &&
                       this.Topic.Equals(other.Topic, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (AppID + Topic).GetHashCode();
        }

        public override string ToString()
        {
            return $"AppID: {AppID}, Topic: {Topic}";
        }
    }
}

