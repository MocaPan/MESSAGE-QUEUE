using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQBroker
{
    // Clase que encapsula la información de una suscripción:
    // - AppID: Identificador de la aplicación o cliente.
    // - Topic: Tema al cual se suscribe.
    public class Subscription
    {
        // Propiedades de solo lectura (se asignan en el constructor).
        public string AppID { get; private set; }
        public string Topic { get; private set; }

        // Constructor que recibe el identificador y el tema.
        public Subscription(string appID, string topic)
        {
            this.AppID = appID;
            this.Topic = topic;
        }

        // Sobrescribimos Equals para comparar dos Subscription
        // en función de su AppID y Topic, ignorando mayúsculas.
        public override bool Equals(object obj)
        {
            if (obj is Subscription other)
            {
                return this.AppID.Equals(other.AppID, StringComparison.OrdinalIgnoreCase) &&
                       this.Topic.Equals(other.Topic, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        // Sobrescribimos GetHashCode para que coincida con Equals.
        // Combinamos AppID y Topic en una cadena y usamos su hash.
        public override int GetHashCode()
        {
            return (AppID + Topic).GetHashCode();
        }

        // Representación en cadena de la suscripción, para debugging o logs.
        public override string ToString()
        {
            return $"AppID: {AppID}, Topic: {Topic}";
        }
    }
}
