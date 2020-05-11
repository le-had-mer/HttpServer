using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.IO;

namespace HTTPServerConsole
{
    class Client
    {
        public Client(TcpClient Client)
        {  
            string Request = "";
            byte[] Buffer = new byte[1024];
            int Count;
            while ((Count = Client.GetStream().Read(Buffer, 0, Buffer.Length)) > 0)
            {
                Request += Encoding.ASCII.GetString(Buffer, 0, Count);
                if (Request.LastIndexOf("\r\n\r\n") >= 0 || Request.Length>4096)
                {
                    HsLogger.Request(Client.Client.RemoteEndPoint.ToString(), Request);
                    break;
                }  
            }
            if (Request.Length == 0)
            {
                return;
            }

            Match ReqMatch = Regex.Match(Request, @"^\w+\s+([^\s\?]+)[^\s]*\s+HTTP/.*");
            if (ReqMatch.Equals((Match.Empty))&(Request.Length>0))
            {
                HsLogger.BadRequest(Request);
                SendError(Client, 400);
                return;
            }
            
            string RequestUri = ReqMatch.Groups[1].Value;
            RequestUri = Uri.UnescapeDataString(RequestUri);

            HsLogger.RequestFile(Client.Client.RemoteEndPoint.ToString(), RequestUri);

            if (RequestUri.IndexOf("..") >= 0)
            {
                HsLogger.BadRequest(RequestUri);
                SendError(Client, 400);
                return;
            }

            if (RequestUri.EndsWith("/"))
            {
                RequestUri += "index.html";
            }

            string FilePath = "storage/" + RequestUri; //хранилище
            

            if (!File.Exists(FilePath))
            {
                HsLogger.FileNotExists(Path.GetFullPath(FilePath));
                SendError(Client, 404);
                return;
            }

            string Extension = RequestUri.Substring(RequestUri.LastIndexOf('.'));
            string ContentType = "";

            switch (Extension)
            {
                case ".html":
                    ContentType = "text/html";
                    break;
                case ".jpg":
                    ContentType = "image/jpeg";
                    break;
                case ".jpeg":
                case ".png":
                case ".gif":
                    ContentType = "image/" + Extension.Substring(1);
                    break;
                default:
                    if (Extension.Length > 1)
                    {
                        ContentType = "application/" + Extension.Substring(1);
                    }
                    else
                    {
                        ContentType = "application/unknown";
                    }
                    break;
            }
            FileStream FS;
            
            try
            {
                FS = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            }

            catch (Exception)
            {
                HsLogger.FileOpenFailed(Path.GetFullPath(FilePath));
                SendError(Client, 500);
                return;
            }

            HsLogger.SendingFile(Client.Client.RemoteEndPoint.ToString(), Path.GetFullPath(FilePath));
          
            string Headers = "HTTP/1.1 200 OK\nContent-Type: " + ContentType + "\nContent-Length: " + FS.Length + "\n\n";
            byte[] HeadersBuffer = Encoding.ASCII.GetBytes(Headers);
            Client.GetStream().Write(HeadersBuffer, 0, HeadersBuffer.Length);
            HsLogger.SendingAnswer(Client.Client.RemoteEndPoint.ToString(), Headers);

            while (FS.Position < FS.Length)
            {
                Count = FS.Read(Buffer, 0, Buffer.Length);
                Client.GetStream().Write(Buffer, 0, Count);
            }

            FS.Close();
            Client.Close();

        }
        private void SendError(TcpClient Client, int Code)
        {
            string CodeStr = Code.ToString() + " " + ((HttpStatusCode)Code).ToString();
            string Html = "<html><body><h1>" + CodeStr + "</h1></body></html>";
            string Str = "HTTP/1.1 " + CodeStr + "\nContent-type: text/html\nContent-Length:" + Html.Length.ToString() + "\n\n" + Html;
            byte[] Buffer = Encoding.ASCII.GetBytes(Str);
            Client.GetStream().Write(Buffer, 0, Buffer.Length);
            Client.Close();
        }
    }
}
