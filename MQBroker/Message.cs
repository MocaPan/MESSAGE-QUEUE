// Este archivo define la clase Message, que representa un mensaje con un identificador de aplicación y contenido de texto.

using System; // Importa el espacio de nombres System, que contiene clases fundamentales.

namespace NSMQBroker // Define un espacio de nombres llamado MQBroker.
{
    public class Message // Declara una clase pública llamada Message.
    {
        // Propiedad para almacenar el identificador de la aplicación que envía el mensaje (opcional para tracking).
        public string AppID { get; set; }

        // Propiedad para almacenar el contenido del mensaje (texto, en este ejemplo).
        public string Content { get; set; }

        // Constructor que inicializa las propiedades AppID y Content con los valores proporcionados.
        public Message(string appID, string content)
        {
            AppID = appID; // Asigna el valor del parámetro appID a la propiedad AppID.
            Content = content; // Asigna el valor del parámetro content a la propiedad Content.
        }

        // Método que devuelve una representación en texto del mensaje para depuración.
        public override string ToString()
        {
            // Devuelve una cadena que contiene el AppID y el contenido del mensaje.
            return $"[{AppID}] {Content}";
        }
    }
}