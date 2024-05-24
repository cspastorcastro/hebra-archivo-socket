using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SocketServidor
{
    public class ServerSocket
    {
        private int puerto;
        private Socket servidor;
        public ServerSocket(int puerto)
        {
            this.puerto = puerto;
        }

        // Se inicia la conexión con el servidor
        // Retorna true en caso de conexión exitosa, false en caso contrario

        public bool Iniciar()
        {
            try
            {
                this.servidor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                this.servidor.Bind(new IPEndPoint(IPAddress.Any, this.puerto));
                this.servidor.Listen(10);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Socket ObtenerCliente()
        {
            return this.servidor.Accept();
        }

        public bool Cerrar()
        {
            try
            {
                this.servidor.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}