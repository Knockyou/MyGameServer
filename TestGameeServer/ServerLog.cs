using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGameeServer
{
    class ServerLog
    {
        int _logType; // 0:클라이언트 접속용,  1:클라이언트 플레이용
        string _logMsg;
        string _fileName = DateTime.Now.ToString("yyyy-MM-dd");


        public ServerLog(int type, string msg)
        {
            _logType = type;
            _logMsg = msg;
            if (_logType == 0)
                clientSocketWriteLog(_logMsg);



            ConsoleCmdWrite();
        }

        void clientSocketWriteLog(string msg)
        {
            const string folder = "ConnectLog/";
            string path = "../../Log/" + folder + _fileName + ".dat";

            if (File.Exists(path))
            {//있으면 덮어쓰기
                FileStream fs = new FileStream(path, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(msg);
                sw.Close();
                fs.Close();
            }
            else
            {//없으니까 생성
                FileStream fs = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(msg);
                sw.Close();
                fs.Close();
            }
        }


        void ConsoleCmdWrite()
        {
            Console.WriteLine(_logMsg);
        }

    }
}
