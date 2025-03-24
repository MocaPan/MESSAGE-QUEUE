// Este archivo define la clase Subscription, que encapsula la información de una suscripción a un tema por parte de una aplicación o cliente.

using System; // Importa el espacio de nombres System, que contiene clases fundamentales.
using System.Collections.Generic; // Importa el espacio de nombres System.Collections.Generic, que contiene clases para colecciones genéricas.
using System.Linq; // Importa el espacio de nombres System.Linq, que contiene clases para consultas LINQ.
using System.Text; // Importa el espacio de nombres System.Text, que contiene clases para manipulación de texto.
using System.Threading.Tasks; // Importa el espacio de nombres System.Threading.Tasks, que contiene clases para tareas asincrónicas.

namespace NSMQBroker // Define un espacio de nombres llamado MQBroker.
{
    // Clase que encapsula la información de una suscripción:
    // - AppID: Identificador de la aplicación o cliente.
    // - Topic: Tema al cual se suscribe.
    public class Subscription
    {
        // Propiedades de solo lectura (se asignan en el constructor).
        public string AppID { get; private set; } // Propiedad para almacenar el identificador de la aplicación o cliente.
        public string Topic { get; private set; } // Propiedad para almacenar el tema al cual se suscribe.

        // Constructor que recibe el identificador y el tema.
        public Subscription(string appID, string topic)
        {
            this.AppID = appID; // Asigna el valor del parámetro appID a la propiedad AppID.
            this.Topic = topic; // Asigna el valor del parámetro topic a la propiedad Topic.
        }

        // Sobrescribimos Equals para comparar dos Subscription
        // en función de su AppID y Topic, ignorando mayúsculas.
        public override bool Equals(object obj)
        {
            if (obj is Subscription sub2) // Verifica si el objeto es una instancia de Subscription.
            {
                return this.AppID.Equals(sub2.AppID, StringComparison.OrdinalIgnoreCase) && // Compara AppID ignorando mayúsculas.
                       this.Topic.Equals(sub2.Topic, StringComparison.OrdinalIgnoreCase); // Compara Topic ignorando mayúsculas.
            }
            return false; // Retorna false si el objeto no es una instancia de Subscription.
        }

        // Sobrescribimos GetHashCode para que coincida con Equals.
        // Combinamos AppID y Topic en una cadena y usamos su hash.
        public override int GetHashCode()
        {
            return (AppID + Topic).GetHashCode(); // Combina AppID y Topic y retorna su hash.
        }

        // Representación en cadena de la suscripción, para debugging o logs.
        public override string ToString()
        {
            return $"AppID: {AppID}, Topic: {Topic}"; // Retorna una cadena que contiene AppID y Topic.
        }
    }
}


