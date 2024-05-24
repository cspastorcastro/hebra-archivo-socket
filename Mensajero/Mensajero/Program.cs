using MensajeroModel;
using MensajeroModel.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Mensajero
{
    public class Program
    {
        private static IMensajesDAL mensajesDAL = MensajesDALArchivos.GetInstancia();
        static void Main(string[] args)
        {
            while (Menu());

        }

        static void EnviarMensajes()
        {
            int puerto = Convert.ToInt32(ConfigurationManager.AppSettings["puerto"]);
            //string servidor = ConfigurationManager.AppSettings["servidor"];


            Console.WriteLine("Ingrese dirección IP del servidor (deje en blanco para loopback):");
            string servidor = Console.ReadLine().Trim();
            if (servidor == "")
            {
                servidor = ConfigurationManager.AppSettings["servidor"];
            }
            /*
            Console.WriteLine("Ingrese puerto a conectarse:");
            puerto = Convert.ToInt32(Console.ReadLine().Trim());
            */
            Console.WriteLine("Conectando a Servidor {0} en puerto {1}", servidor, puerto);
            ClienteSocket clienteSocket = new ClienteSocket(servidor, puerto);


            if (clienteSocket.Conectar())
            {
                Console.WriteLine("¡Conectado correctamente!");
                Console.Write("Ingrese su nombre: ");
                string nombre = Console.ReadLine().Trim();
                string recibido = "";
                string enviado = "";

                do
                {
                    Console.Write("==> ");
                    enviado = Console.ReadLine().Trim();

                    clienteSocket.Escribir(enviado);

                    if (enviado.ToLower() == "/quit")
                    {
                        break;
                    }

                    recibido = clienteSocket.Leer();
                    Console.WriteLine("[Servidor]: " + recibido);

                    // Crear mensaje para la DAL
                    Mensaje mensajeAplicacion = new Mensaje()
                    {
                        Remitente = nombre,
                        CuerpoMensaje = enviado,
                        Tipo = "Aplicación"
                    };
                    
                    Mensaje mensajeServidor = new Mensaje()
                    {
                        Remitente = "Servidor",
                        CuerpoMensaje = recibido,
                        Tipo = "TCP"
                    };
                    mensajesDAL.AgregarMensaje(mensajeAplicacion);
                    if (recibido.ToLower() != "/quit")
                    {
                        mensajesDAL.AgregarMensaje(mensajeServidor);
                    }
                } while (enviado.ToLower() != "/quit" && recibido.ToLower() != "/quit");
                clienteSocket.Desconectar();
                Console.WriteLine("Desconectado");
            }
        }

        static void MostrarMensajes()
        {
            Console.Clear();
            List<Mensaje> mensajes = mensajesDAL.ObtenerMensajes();
            foreach(Mensaje mensaje in mensajes)
            {
                Console.WriteLine(mensaje);
            }
            Console.WriteLine("Presione cualquier tecla para salir...");
            Console.ReadKey();
        }

        static bool Menu()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Clear();
            bool continuar = true;
            Console.WriteLine("1. Enviar Mensaje");
            Console.WriteLine("2. Ver Mensajes Enviados");
            Console.WriteLine("q. Salir");

            switch (Console.ReadLine().Trim())
            {
                case "1":
                    EnviarMensajes();
                    break;
                case "2":
                    MostrarMensajes();
                    break;
                case "q":
                    continuar = false;
                    break;
                default:
                    Console.WriteLine("Opción inválida");
                    break;
            }

            return continuar;
        }
    }
}
