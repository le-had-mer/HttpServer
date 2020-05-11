using System;
using NLog;
using System.IO;

namespace HTTPServerConsole
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            Console.WriteLine("Files to search:\n" +
                "1.jpg\n" +
                "2.jpg\n" +
                "3.jpg\n" +
                "4.jpg\n" +
                "5.html\n" +
                "6.gif");

            hsLogger.log = logger;
            Server server = new Server(80, logger);
        }
    }
}
