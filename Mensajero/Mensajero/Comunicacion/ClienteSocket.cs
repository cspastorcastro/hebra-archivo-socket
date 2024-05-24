using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mensajero
{
    public class ClienteSocket
    {
        private int puerto;
        private string servidor;
        private Socket cliente;
        private StreamReader reader;
        private StreamWriter writer;

        public ClienteSocket(string servidor, int puerto)
        {
            this.servidor = servidor;
            this.puerto = puerto;
        }

        public bool Conectar()
        {
            try
            {
                this.cliente = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(this.servidor), this.puerto);
                this.cliente.Connect(endPoint);
                Stream stream = new NetworkStream(this.cliente);
                this.reader = new StreamReader(stream);
                this.writer = new StreamWriter(stream);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public string Leer()
        {
            try
            {
                return this.reader.ReadLine().Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }


        public bool Escribir(string mensaje)
        {
            try
            {
                this.writer.WriteLine(mensaje);
                this.writer.Flush();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public void Desconectar()
        {
            try
            {
                this.cliente.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}