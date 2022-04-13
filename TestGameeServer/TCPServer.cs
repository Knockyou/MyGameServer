using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;

namespace TestGameeServer
{
    class TCPServer
    {
        //필요정보
        short _port;

        //[MarshalAs(UnmanagedType.U4, SizeConst = 8)]
        static public long _nowUUID = 1000000000000000000;


        Socket _waitSocket;

        static public Queue<PacketClass1> _sendQueue = new Queue<PacketClass1>();
        static public Queue<PacketClass1> _reciveQueue = new Queue<PacketClass1>();

        Thread _sendThread; //SendQueue를 실행할 쓰레드
        Thread _reciveThread; //Recive를 실행할 쓰레드
        //-----------


        //서버에서는 소켓메니저를 사용한다
        SocketManager _socketManager = new SocketManager();
        
        public TCPServer(short port)
        {
            _port = port;

            //정보 파일을 읽어 온다.
            //-> 일단 나중에 추가


            //소켓 클래스 만들고
            //소켓에 주소와 포트 바인드
            //대기상태로 변경
            try
            {
                _waitSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _waitSocket.Bind(new IPEndPoint(IPAddress.Any, port)); //바인드
                _waitSocket.Listen(30); //대기 상태
                string msg = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]Socket Create...";
                ServerLog log = new ServerLog(0, msg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }


            //쓰레드 시작
            _sendThread = new Thread(SendProcess);
            _reciveThread = new Thread(() => ReciveProcess()); //위와 같은 방법
            _sendThread.Start(); //SendQ 실행
            _reciveThread.Start(); //Recive 실행

        }

        ~TCPServer()
        {
            //서버 종료시 해지에 필요한 내용.
        }

        //메인 함수에서 실행 하는 함수
        public bool MainProcess()
        {
            //대기 소켓에 클라이언트가 붙는가
            if (_waitSocket.Poll(0, SelectMode.SelectRead))
            {
                //붙으면 SocketManager에서 매개변수로 SocketClass를 보낸후 Accept함수 실행
                _socketManager.SocketManagerAcceptFun(_waitSocket);
            }

            //버퍼를 주고 받는 함수 from SocketManager
            //_socketManager.SocketManagerSendFun();  -> 일단 주석


            //서버를 종료 할것인가
            //-> 서버를 종료할시 false를 반환해야함


            return true;
        }

        #region [SendQ]
        void SendProcess()
        {
            while (true)
            {
                if (_sendQueue.Count > 0)
                {
                    _sendQueue.Dequeue().SendProcess();
                }
            }

        }
        #endregion


        #region [ReciveQ]
        void ReciveProcess()
        {
            while (true)
            {
                if (_reciveQueue.Count > 0)
                {
                    _reciveQueue.Dequeue().ReciveProcess();
                }
            }
        }
        #endregion
    }
}
