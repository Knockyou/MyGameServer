using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TestGameeServer
{
    class SocketManager
    {
        //필요정보
        static List<SocketClass> _socketList = new List<SocketClass>();
        //-------
        Thread _reciveThread;

        BackgroundWorker worker;
        public SocketManager()
        {
            Task reciveTask = SocketManagerReciveFun();
        }

        //Accept기능
        public void SocketManagerAcceptFun(Socket waitSocket)
        {//TCPServer에서 연결이 된 SocketClass를 매개변수로 전달

            SocketClass temp = new SocketClass();
            
            temp._Socket = waitSocket.Accept();

            _socketList.Add(temp); //전달받은 SocketClass를 List<SocketClass> 맴버변수에 Add
            string msg = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]Accept(";

            temp._Uuid = TCPServer._nowUUID; //TCPServer에 있는 _nowUUID를 Accept된 SocketClass의 uuid에넣어준다

            msg += TCPServer._nowUUID + ")...";
            ServerLog log = new ServerLog(0, msg);
            

            //Accept후 해당 클라이언트에게 uuid 보냄
            if (temp._Socket.Poll(0, SelectMode.SelectWrite))
            {
                byte[] buffer;
                PacketClass1 connPacketClass = new PacketClass1();
                ConnectPacket connPacket;
                connPacket._UUID = temp._Uuid;
                buffer = connPacketClass.SendPacketInit(connPacket);
                temp._Socket.Send(buffer);
                msg = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]Send(uuid)";
                log = new ServerLog(0, msg);
            }
            else
            {
                msg = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]Send(fail)";
                log = new ServerLog(0, msg);
            }

            //로그 남기는 코드
            TCPServer._nowUUID++; //넣어준후에 +1을 해준다
        }

        //Send Function
        /*public void SocketManagerSendFun()//클라이언트가 send하는 것을 Recive하여 처리후 다시 클라이언트에게 Send해야함
        {
            while (true)
            {
                for (int i = 0; i < _socketList.Count; i++)
                {
                    PacketClass1 packet = new PacketClass1();

                    if (_socketList[i]._Socket.Poll(0, SelectMode.SelectWrite))
                    {
                        packet = _socketList[i].SocketSendFun();
                        if (packet != null)
                            TCPServer._sendQueue.Enqueue(packet);

                    }

                }
            }
        }*/

        private static async Task SocketManagerReciveFun()//클라이언트가 send하는 것을 Recive하여 처리후 다시 클라이언트에게 Send해야함
        {
            await Task.Run(() => { 
                while (true)
                {
                    for(int i = 0; i < _socketList.Count; i++)
                    {

                        PacketClass1 packet;

                        if (/*_socketList[i]._Socket.Connected && */_socketList[i]._Socket.Poll(0, SelectMode.SelectRead))
                        {
                            byte[] buffer = new byte[1024];
                            int recvLength = _socketList[i]._Socket.Receive(buffer);

                            //가입 요청일때
                            if(recvLength == 912)
                            {
                                packet = new PacketClass1(buffer, typeof(JoinPacket), recvLength);
                                TCPServer._reciveQueue.Enqueue(packet);
                            }
                            else if(recvLength == 608) //로그인시
                            {
                                packet = new PacketClass1(buffer, typeof(LoginPacket), recvLength);

                                TCPServer._reciveQueue.Enqueue(packet);
                            }
                            else if (recvLength == 312) //가입시
                            {
                                packet = new PacketClass1(buffer, typeof(JoinCheckPacket), recvLength);

                                TCPServer._reciveQueue.Enqueue(packet);
                            }
                            else if(recvLength == 320)//골드
                            {
                                packet = new PacketClass1(buffer, typeof(GoldPacket), recvLength);

                                TCPServer._reciveQueue.Enqueue(packet);
                            }
                            else if (recvLength == 40)//경험치
                            {
                                packet = new PacketClass1(buffer, typeof(ExpPacket), recvLength);

                                TCPServer._reciveQueue.Enqueue(packet);
                            }
                            else if (recvLength == 48)//퀘스트
                            {
                                packet = new PacketClass1(buffer, typeof(QuestProgressPacket), recvLength);

                                TCPServer._reciveQueue.Enqueue(packet);
                            }


                        }
                    }
                }
            
            });
        }

       
        

        //Find Function
        public static SocketClass SocketManagerFind(long uuid)
        {
            int idx = 0;
            foreach (var item in _socketList)
            {
                if(item._Uuid == uuid)
                {
                    idx = _socketList.IndexOf(item);
                }
            }
            return _socketList[idx];
        }

        //Close Function
        void  SocketManagerClose()
        {
            int idx = 0;
            foreach (var item in _socketList)
            {
                if (!item._Socket.Connected)
                {
                    idx = _socketList.IndexOf(item);
                    string msg = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]Close(" + item._Uuid + "])";
                    ServerLog log = new ServerLog(0, msg);
                    _socketList.RemoveAt(idx);
                }
            }
        }
    }
}
