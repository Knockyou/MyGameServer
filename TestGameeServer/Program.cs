using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TestGameeServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TCPServer server = new TCPServer(80);
            while(true)
            {
                if(!server.MainProcess())
                {
                    break;
                }
            }


            Console.Read();
        }
    }
}
