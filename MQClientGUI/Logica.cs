using Logic;
using System;
using System.Net;


namespace Logic
{
    public class Logica
    {

        public bool ValidarPort(string numPort)
        {
            int num = Convert.ToInt32(numPort);
            
            if (num > 9999 || num < 1000)
            {
                return false;
            }
            else
            {
                return true;
            }


        }

        // Función que valida si una IP es válida usando la librería System.Net
        public bool ValidarIp(string ip)
        {
            // Intentar analizar la cadena 'ip' como una dirección IP
            return IPAddress.TryParse(ip, out _); // Si es válida, devuelve true; de lo contrario, false
        }
    }


}
