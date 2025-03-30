using System;
using System.Windows.Forms;
using EstructurasPersonalizadas;
using Logic;
using NSMQClient;
using EstructurasPersonalizadas;
using System.Xml.Serialization;
using System.ComponentModel.Design;





namespace MQClientGUI



{
    public partial class Form1 : Form
    {
        private MQClient client;
        private ListaSimple<string> Topics;
        private ListaSimple<string> Mensajes;
        private ListaSimple<string> Historial;
        public Form1()
        {
            InitializeComponent();
            // Inicializa el cliente MQClient a null
            Frame1.Visible = false;
            HistorialBox.ReadOnly = false;
            TemasSubs.ReadOnly = false;
            GeneraIP.Visible = false;
            groupBox1.Visible = false;
            this.Topics = new ListaSimple<string>();
            this.Mensajes = new ListaSimple<string>();
            this.Historial = new ListaSimple<string>();
            



        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            TrySub();
        }

        private void button2_Click(object sender, EventArgs e)
        {
          Descuscribirse();  
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Publicar();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }



        private void button5_Click(object sender, EventArgs e)
        {
            ConectarCliente();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string ip = GenerarIp();
            AppID.Text = ip;
        }


        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void TemasSubs_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Topics.RecorrerEscribe(TemasSubs);
        }





        /// <summary>  
        /// Logica para obtenr datos de los campos de texto y enviarlos al servidor
        /// </summary>

        public void ConectarCliente()
        {
            string MQPort = BrokerIPBox.Text;
            String MQIP = AppID.Text;
            Logica validacion = new Logica();

            if (validacion.ValidarPort(MQPort))
            {
                
                int MQPortInt = Convert.ToInt32(MQPort);
                if (validacion.ValidarIp(MQIP))
                {
                    
                    Guid appIdG = Guid.NewGuid();
                    MQClient client = new MQClient(MQIP, MQPortInt, appIdG, Frame1, ConectarB);
                    this.client = client;
                }
                else
                {
                    MessageBox.Show("IP Invalida");
                }

            }
            else
            {
                MessageBox.Show("Puerto Invalida");
            }

        }


        public string GenerarIp()
        {
            Random random = new Random();

            int firstOctet;

            // Genera un primer octeto que no esté en el rango reservado.
            do
            {
                firstOctet = random.Next(1, 256); // El primer octeto debe ser entre 1 y 255.
            } while (firstOctet == 127 || // Evitar la dirección localhost 127.x.x.x
                     (firstOctet >= 169 && firstOctet <= 170) || // Evitar rango de direcciones de red privada (169.x.x.x)
                     (firstOctet >= 172 && firstOctet <= 191) || // Evitar clase B privada (172.x.x.x)
                     firstOctet == 192); // Evitar clase C privada (192.x.x.x)

            // Genera los otros tres octetos (que pueden ser cualquier número entre 0 y 255).
            int secondOctet = random.Next(0, 256);
            int thirdOctet = random.Next(0, 256);
            int fourthOctet = random.Next(0, 256);

            // Devuelve la IP generada en el formato estándar "xxx.xxx.xxx.xxx"
            return $"{firstOctet}.{secondOctet}.{thirdOctet}.{fourthOctet}";
        }


        public void TrySub()
        {
            if (TextTopic.Text != "")
            {
                if (!this.Topics.Contiene(TextTopic.Text))
                {
                    client.Subscribe(TextTopic.Text);
                    MessageBox.Show("Suscripcion exitosa");
                    Topics.Agregar(TextTopic.Text);
                    TemasSubs.Text += TextTopic.Text + "\n";

                }
                else
                {
                    MessageBox.Show($"Ya esta suscrito al tema {TextTopic}");
                }
            }
            else
            {
                MessageBox.Show("Ingrese un topic");
            }
        }


        public void Publicar()
        {
            if (TextTopic.Text == "")
            {
                MessageBox.Show("Ingrese un Tema");
                return;
            }

            if (MensajeBox.Text == "")
            {
                MessageBox.Show("Ingrese un mensaje");
                return;
            }

           


            client.Publish(TextTopic.Text, MensajeBox.Text);
            MessageBox.Show("Mensaje publicado");
            MensajeBox.Clear();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (TextTopic.Text != "")
            {
                client.Receive(TextTopic.Text);
                HistorialBox.Text += $"{TextTopic.Text}:  {client.response} " + "\n";
            }
            else
            {
                MessageBox.Show("Ingrese un tema");
            }

        }


        public void Descuscribirse()
        {
            if (TextTopic.Text != "")
            {
                
                client.Unsubscribe(TextTopic.Text);
                MessageBox.Show("Desuscripcion exitosa");
                Topics.Eliminar(TextTopic.Text);
                TemasSubs.Text = "";
                
            
               
            }
            else
            {
                MessageBox.Show("Ingrese un tema");
            }
        }
    }
}
    