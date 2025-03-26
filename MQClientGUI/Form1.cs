using System;
using System.Windows.Forms;
using Logic;
using NSMQClient;





namespace MQClientGUI



{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

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
                MessageBox.Show("Puerto valido");
                int MQPortInt = Convert.ToInt32(MQPort);
                if (validacion.ValidarIp(MQIP))
                    {
                    MessageBox.Show("IP valida");
                    Guid appIdG = Guid.NewGuid();
                    MQClient client = new MQClient(MQIP, MQPortInt, appIdG);
                }
                else
                {
                    MessageBox.Show("IP Invalida");
                }

            }
            else
            {
                MessageBox.Show("IP Invalida");
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



    private void button5_Click(object sender, EventArgs e)
        {
            ConectarCliente();

        }

    private void button6_Click(object sender, EventArgs e)
        {
            string ip = GenerarIp();
            AppID.Text = ip;
        }
    }
}
