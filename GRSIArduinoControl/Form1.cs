using System;
using System.IO.Ports;
using System.Windows.Forms;
using System.Drawing;

namespace GRSIArduinoControl
{
    public partial class Form1 : Form
    {
        private string[] availableCOMs;
        private string selectedCOM;
        private SerialPort port = new SerialPort();
        private int inData;
        private String flagLED;


        public Form1()
        {
            InitializeComponent();
        }


        private void btnObterCOMs_Click(object sender, EventArgs e)
        {
            bool flag = false;
            do
            {
                try
                {
                    availableCOMs = SerialPort.GetPortNames();
                    if (availableCOMs.Length == 0)
                    {
                        throw new FormException();
                    }
                    else
                    {
                        cbCOM.Items.Clear();
                        cbCOM.Items.AddRange(availableCOMs);
                        flag = true;
                    }
                    
                }
                catch (FormException)
                {
                    if(!showException("Não há COMs disponíveis! \n Tentar de novo?\n"))
                    {
                        Application.Exit();
                    }
                    else
                    {
                        flag = true;
                    }
                }
            } while (flag == false);
        }

        public Boolean showException(string message)
        {
            string title = "";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, 
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            if(result == DialogResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void cbCOM_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cbCOM.SelectedIndex;
            Object selectedIndex = cbCOM.SelectedItem;
            selectedCOM = selectedIndex.ToString();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            bool flag = false;
            do
            {
                try
                {
                    if(selectedCOM == null)
                    {
                        throw new FormException();
                    }
                    else
                    {
                        connect();
                        //subscrever os eventos de chegada de dados à porta COM
                        port.DataReceived += port_SerialDataReceivedEvent;
                        flag = true;
                    }

                }
                catch (FormException)
                {
                    if (!showException("Tem de selecionar uma COM! \n Tentar de novo?\n"))
                    {
                        Application.Exit();
                    }
                    else
                    {
                        flag = true;
                    }
                }
            } while (flag == false);
        }

        public void connect()
        {
            port.PortName = selectedCOM;
            port.BaudRate = 9600;
            port.Parity = Parity.None;
            port.DataBits = 8; 
            port.Open();
            btnTerminar.Enabled = true;
            btnIniciar.Enabled = false;
            port.Write("I");

        }

        public void port_SerialDataReceivedEvent (object sender, SerialDataReceivedEventArgs e)
        {
            bool flag = false;
            do
            {
                try
                {
                    //converte para int o caracter que chega (0 ou 1)
                    inData = Int16.Parse(port.ReadLine());
                    this.Invoke(new EventHandler(displayData));
                }
                catch (OperationCanceledException)
                {
                    flag = true;
                }
            } while (flag == false);
            
        }

        public void displayData(object sender, EventArgs e)
        {
            tbLEDRed.Clear();

            if(inData == 0)
            {
                switch(flagLED)
                {
                    case "red" :
                        tbLEDRed.BackColor = Color.Red;
                        tbLEDRed.Text = "DESLIGADO";
                        break;
                    case "yellow":
                        tbLEDYellow.BackColor = Color.Red;
                        tbLEDYellow.Text = "DESLIGADO";
                        break;
                    case "green":
                        tbLEDGreen.BackColor = Color.Red;
                        tbLEDGreen.Text = "LIGADO";
                        break;
                }

            }
            if(inData == 1)
            {
                switch (flagLED)
                {
                    case "red":
                        tbLEDRed.BackColor = Color.Green;
                        tbLEDRed.Text = "LIGADO";
                        break;
                    case "yellow":
                        tbLEDYellow.BackColor = Color.Green;
                        tbLEDYellow.Text = "LIGADO";
                        break;
                    case "green":
                        tbLEDGreen.BackColor = Color.Red;
                        tbLEDGreen.Text = "LIGADO";
                        break;
                }

            }
                    
        }

        private void btnTerminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (port.IsOpen)
                {
                    //anular a subscrição do evento DataReceived
                    port.DataReceived -= port_SerialDataReceivedEvent;
                    port.Close();
                    btnTerminar.Enabled = false;
                    btnIniciar.Enabled = true;
                }  
            }
            catch (FormException)
            {

            }
            
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            try
            {
                if (port.IsOpen)
                {
                    port.Close();
                    //anular a subscrição do evento DataReceived
                    port.DataReceived -= port_SerialDataReceivedEvent;
                    Application.Exit();
                }
                else
                {
                    Application.Exit();
                }
            }
            catch (FormException)
            {

            }
        }

        private void btnONRed_Click(object sender, EventArgs e)
        {
            flagLED = "red";
            port.Write("L");
        }

        private void btnOFFRed_Click(object sender, EventArgs e)
        {
            flagLED = "red";
            port.Write("D");
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnONVerde_Click(object sender, EventArgs e)
        {
            flagLED = "green"
;           port.Write("G");
        }

        private void btnOFFVerde_Click(object sender, EventArgs e)
        {
            flagLED = "green";
           port.Write("H");
        }

        private void btnONYellow_Click(object sender, EventArgs e)
        {
            flagLED = "yellow";
            port.Write("O");
        }

        private void btnOFFYellow_Click(object sender, EventArgs e)
        {
            flagLED = "yellow";
            port.Write("P");
        }
    }



    //classe de exceções personalizadas
    public class FormException : Exception
    {
        public FormException()
        {
            //construtor
        }
    }
}
