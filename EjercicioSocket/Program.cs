using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServidorSocketUtil
{
    class Program
    {
        private int puerto;
        private Socket servidor;

        static void GenerarComunicacion(ClienteCom clienteCom)
        {
            bool terminar = false;
            while (!terminar)
            {
                string mensaje = clienteCom.Leer();
                if (mensaje != null)
                {
                    Console.WriteLine("C: {0}", mensaje);
                    if (mensaje.ToLower() == "chao")
                    {
                        terminar = true;
                    }
                    else
                    {
                        Console.Write("Ingrese Respuesta: ");
                        mensaje = Console.ReadLine().Trim();
                        clienteCom.Escribir(mensaje);
                        if (mensaje.ToLower() == "chao")
                        {
                            terminar = true;
                        }
                    }

                }else
                {
                    terminar = false;
                }
                if (terminar)
                {
                    clienteCom.Desconectar();
                }
            }
        }
        static void Main(string[] args)
        {
            int puerto = Convert.ToInt32(ConfigurationManager.AppSettings["puerto"]);
            Console.WriteLine("Iniciando Servidor en puerto {0}", puerto);
            ServerSocket servidor = new ServerSocket(puerto);

            if (servidor.Iniciar())
            {
                Console.WriteLine("Servidor Iniciado");
                while (true)
                {
                    Console.WriteLine("Esperando Cliente");
                    Socket socketCliente = servidor.ObtenerCliente();

                    ClienteCom clienteCom = new ClienteCom(socketCliente);
                    GenerarComunicacion(clienteCom);

                }
            }
            else
            {
                Console.WriteLine("Error, el puerto {0} esta en uso", puerto);
            }
        }
    }
}
