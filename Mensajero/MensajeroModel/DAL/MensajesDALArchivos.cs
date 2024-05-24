using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensajeroModel.DAL
{
    public class MensajesDALArchivos : IMensajesDAL
    {
        // Implementar Singleton
        // 1. El constructor debe ser private
        private MensajesDALArchivos()
        {
            
        }
        
        // 2. Debe poseer un atributo del mismo tipo de la clase y ser estático
        private static MensajesDALArchivos instancia;
        
        // 3. Debe tener un método GetInstance que retorne una referencia al atributo
        public static IMensajesDAL GetInstancia()
        {
            if (instancia == null)
            {
                instancia = new MensajesDALArchivos();
            }
            return instancia;
        }
        // TODO: Hacer thread-safe

        private static string url = Directory.GetCurrentDirectory();
        private static string archivo = url + "/mensajes.txt";
        public void AgregarMensaje(Mensaje mensaje)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(archivo, true))
                {
                    // TODO: Sanitizar caracteres relevantes al formato CSV
                    writer.WriteLine("{0};{1};{2}", mensaje.Remitente, mensaje.CuerpoMensaje, mensaje.Tipo);
                    writer.Flush();
                    // considera StreamWriter.AutoFlush()?
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public List<Mensaje> ObtenerMensajes()
        {
            List<Mensaje> lista = new List<Mensaje>();
            try
            {
                using (StreamReader reader = new StreamReader(archivo))
                {
                    string texto;
                    do
                    {
                        texto = reader.ReadLine();
                        if (texto != null)
                        {
                            string[] textoArchivo = texto.Trim().Split(';');
                            string remitente = textoArchivo[0];
                            string cuerpoMensaje = textoArchivo[1];
                            string tipo = textoArchivo[2];
                            Mensaje m = new Mensaje()
                            {
                                Remitente = remitente,
                                CuerpoMensaje = cuerpoMensaje,
                                Tipo = tipo
                            };

                            lista.Add(m);
                        }
                        

                    } while (texto != null);
                }
            } catch(Exception ex)
            {
                lista = null;
                Console.WriteLine(ex.ToString());
            }
            return lista;
        }
    }
}
