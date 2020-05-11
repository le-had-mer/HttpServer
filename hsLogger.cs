using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace HTTPServerConsole
{
    static public class hsLogger
    {
        static public Logger log;
        static public void Start()
        {
            log.Info("Server started.\n");
            Console.WriteLine("Server started.\n");
        }

        static public void Close()
        {
            log.Info("Server closed.\n");
            Console.WriteLine("Closed.\n");
        }

        static public void Connected(string ip)
        {
            log.Info("{0} connected \n", ip);
            Console.WriteLine("{0} connected \n", ip);
        }

        static public void Request(string ip, string rq)
        {
            log.Info(
                "{0} requested:\n" +
                "{1}\n", ip, rq);
            /*Console.WriteLine(
                "{0} requested:\n" +
                "{1}\n", ip, rq);*/
        }
        static public void RequestFile(string ip, string file)
        {
            Console.WriteLine(
                "{0} requested file:\n" +
                "{1}\n", ip, file);
        }

        static public void SendingFile(string ip, string file)
        {
            log.Info(
                "Sending to {0}:\n" +
                "{1}\n", ip, file);
            Console.WriteLine(
                "Sending to {0}:\n" +
                "{1}\n", ip, file);
        }

        static public void SendingAnswer(string ip, string ans)
        {
            log.Info(
                "Sending to {0}:\n" +
                "{1}\n", ip, ans);
        }

        static public void FileOpenFailed(string file)
        {
            log.Error("500 Error opening file " + file + "\n");
            Console.WriteLine("500 Error opening file " + file + "\n");
        }

        static public void FileNotExists(string file)
        {
            log.Error("404 File " + file + " not exists\n");
            Console.WriteLine("404 File " + file + " not exists\n");
        }
        static public void BadRequest(string uri)
        {
            log.Error("400 Bad request \n" + uri + "\n");
            Console.WriteLine("400 Bad request\n" + uri + "\n");
        }
    }
}
