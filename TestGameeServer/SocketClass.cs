using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;


namespace TestGameeServer
{
    class SocketClass
    {
        //필요 정보
        Socket _socket;
        int _uniqueIdx;
        [MarshalAs(UnmanagedType.U8, SizeConst = 8)]
        long _uuid;
        //---------

        bool _isJoin = false;
        bool _isLogin = false;

        //Socket 프로퍼티로 접근
        public Socket _Socket
        {
            get { return _socket; }
            set { _socket = value; }
        }

        public long _Uuid
        {
            get { return _uuid; }
            set { _uuid = value; }
        }

        public int _UniqueIdx
        {
            get { return _uniqueIdx; }
            set { _uniqueIdx = value; }
        }
        //생성자로 소켓 초기화(주소 포트 바인드, 대기상태로 변경)
        public SocketClass()
        {
            
        }

        

        //Send Function 
        public PacketClass1 SocketSendFun()
        {
            PacketClass1 packet = new PacketClass1();
            byte[] buffer = new byte[1024];
            packet = null;
            if (_socket.Connected && _socket.Poll(0, SelectMode.SelectWrite))
            {
                
            }

            return packet;
        }

        

        public void LoginRequest()
        {

        }

        //Close Function
        void SocketCloseFun()
        {
            //_socket.Close(); -> 일단 주석
        }

    }
}
