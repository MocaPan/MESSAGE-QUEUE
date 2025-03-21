using System;

namespace MQBroker
{

    public class Message
    {
        // Identificador de la aplicación que envía el mensaje (opcional para tracking).
        public string AppID { get; set; }

        // Contenido del mensaje (texto, en este ejemplo).
        public string Content { get; set; }

        public Message(string appID, string content)
        {
            AppID = appID;
            Content = content;
        }

        // Representación en texto para depuración.
        public override string ToString()
        {
            return $"[{AppID}] {Content}";
        }
    }
}
