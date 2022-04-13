using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace TestGameeServer
{
    //[StructLayout(LayoutKind.Sequential)]
    public struct ConnectPacket
    {
        //[MarshalAs(UnmanagedType.U4)]
        public long _UUID;
    }
    public struct UserInfocheck
    {//0:login, 1:joinid, 2:joinNName
        [MarshalAs(UnmanagedType.Bool, SizeConst = 4)]
        public bool _isUserInfo;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public int _chkType;
    }
    public struct LoginPacket// -> 로그인용 구조체
    {
        [MarshalAs(UnmanagedType.U8, SizeConst = 12)]
        public long _UUID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
        public string _ID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
        public string _PWD;
    }

    public struct JoinPacket// -> 가입용 구조체
    {
        [MarshalAs(UnmanagedType.U8, SizeConst = 8)]
        public long _UUID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
        public string _ID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
        public string _PWD;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
        public string _NICKNAME;
    }

    public struct JoinCheckPacket// -> 가입 아이디 확인용 구조체
    {
        [MarshalAs(UnmanagedType.U8, SizeConst = 8)]
        public long _UUID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
        public string _TargetString;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public int _chkType;
    }

    public struct LoginInfoPacket// -> 로그인 요청후에 성공시 정보 
    {
        [MarshalAs(UnmanagedType.U8, SizeConst = 8)]
        public long _UUID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
        public string _ID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
        public string _NICKNAME;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public int _AVTIDX;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public int _LEVEL;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public int _GOLD;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public int _EXP;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public int _CURRENT_EXP;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _COMPLE_QUEST;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _CURRENT_QUEST;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _QUESTDATA;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public int _QUESTING;
    }

    public struct GoldPacket
    {//보낼때와 받을때 둘다 사용
     //Type골드를 얻을시에는 true, 사용 핼때는 false
        [MarshalAs(UnmanagedType.U8, SizeConst = 8)]
        public long _UUID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
        public string _ID;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public int _GOLD;
        [MarshalAs(UnmanagedType.Bool, SizeConst = 4)]
        public bool _GTYPE;
    }

    public struct ExpPacket// -> 경험치 구조체
    {
        [MarshalAs(UnmanagedType.U8, SizeConst = 8)]
        public long _UUID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] _ID;
        [MarshalAs(UnmanagedType.I2, SizeConst = 2)]
        public short _AVTIDX;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public int _Exp;
    }

    public struct SendExpPacket// -> 경험치 구조체
    {
        [MarshalAs(UnmanagedType.U8, SizeConst = 8)]
        public long _UUID;
        [MarshalAs(UnmanagedType.I2, SizeConst = 2)]
        public short _AVTIDX;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public int _Level;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public int _CurrentExp;
    }

    public struct QuestProgressPacket// -> 퀘스트 저장 받음 구조체
    {
        [MarshalAs(UnmanagedType.U8, SizeConst = 8)]
        public long _UUID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] _ID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] _QUDATE;
    }

    public struct QuestInfoSendPacket// -> 퀘스트 업뎃 보내는 구조체
    {
        [MarshalAs(UnmanagedType.U8, SizeConst = 8)]
        public long _UUID;
        /*[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] _CURRQUEST;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] _QUDATE;*/
        /*[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
        public string _CURRQUEST;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public int _QUDATE;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public int _QUING;*/
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
        public string _QUESTDATA;



        /*public QuestInfoSendPacket(long uuid, string data)
        {
            _UUID = uuid;
            _QUESTDATA = new byte[10];

            byte[] _curr = new UTF8Encoding(true, true).GetBytes(data);
            Array.Copy(_curr, 0, _QUESTDATA, 0, _curr.Length);
        }*/
    }


    class PacketClass1 // -> 구조체 그대로 사용 하려고 하는 임시 클래스
    {
        //[MarshalAs(UnmanagedType.U4)]
        long _packetProtocolId; //패킷 프로토콜 아이디
        //[MarshalAs(UnmanagedType.U4)]
        int _uniqueUserIdx;
        //[MarshalAs(UnmanagedType.U4)]
        int _packetDataLength; //패킷 데이터 크기
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1004)]
        byte[] _packetData; //패킷 데이터

        object _obj;


        bool _isLoginCheck = false;


        public Type _packetType;

        public PacketClass1() { }

        public PacketClass1(byte[] data, Type type, int size) //Recive용 생성자
        {
            if (type == typeof(ConnectPacket))
                this._packetType = typeof(ConnectPacket);
            else if (type == typeof(JoinPacket))
                this._packetType = typeof(JoinPacket);
            else if (type == typeof(LoginPacket))
                this._packetType = typeof(LoginPacket);
            else if (type == typeof(UserInfocheck))
                this._packetType = typeof(UserInfocheck);
            else if (type == typeof(JoinCheckPacket))
                this._packetType = typeof(JoinCheckPacket);
            else if (type == typeof(LoginInfoPacket))
                this._packetType = typeof(LoginInfoPacket);
            else if (type == typeof(GoldPacket))
                this._packetType = typeof(GoldPacket);
            else if (type == typeof(ExpPacket))
                this._packetType = typeof(ExpPacket);
            else if (type == typeof(SendExpPacket))
                this._packetType = typeof(SendExpPacket);
            else if (type == typeof(QuestProgressPacket))
                this._packetType = typeof(QuestProgressPacket);
            else if (type == typeof(QuestInfoSendPacket))
                this._packetType = typeof(QuestInfoSendPacket);

            _packetData = data;
            _packetDataLength = size;
        }


        //생성 기능
        public byte[] SendPacketInit(object obj)
        {//페켓을 만들때 정보 생성
            int dataSize = Marshal.SizeOf(obj);
            IntPtr buff = Marshal.AllocHGlobal(dataSize);
            Marshal.StructureToPtr(obj, buff, false);
            byte[] data = new byte[dataSize];
            Marshal.Copy(buff, data, 0, dataSize);
            Marshal.FreeHGlobal(buff);
            return data;
        }

        public object RecivePacketInit(byte[] data, Type type, int size)
        {
            IntPtr buff = Marshal.AllocHGlobal(_packetDataLength);
            Marshal.Copy(_packetData, 0, buff, _packetDataLength);
            object obj = Marshal.PtrToStructure(buff, type);
            Marshal.FreeHGlobal(buff);
            if (Marshal.SizeOf(obj) != _packetDataLength)
            {
                return null;
            }
            return obj;
        }

        //수정 기능
        void PacketModifyFun()
        {

        }


        //Server script에서 Dequeu로 실행 하는 함수
        public void ReciveProcess()
        {
            if (_packetType == typeof(ConnectPacket))
            {//연결 페켓 일때

            }
            else if (_packetType == typeof(JoinPacket))
            {//가입 페켓일때

                _obj = RecivePacketInit(_packetData, _packetType, _packetDataLength);

                JoinPacket packet = (JoinPacket)_obj;

                /*Console.WriteLine(packet._UUID.ToString());
                Console.WriteLine(packet._ID.ToString());
                Console.WriteLine(packet._PWD.ToString());
                Console.WriteLine(packet._NICKNAME.ToString());
                Console.WriteLine(packet._AVTIDX.ToString());*/
                _packetProtocolId = packet._UUID;
                GameUserInfo userSQL = new GameUserInfo();

                string msg = "";
                if (userSQL.AddUser(packet._ID, packet._PWD, packet._NICKNAME))
                    msg = "Join[" + packet._ID.ToString() + "]Sucsess";
                else
                    msg = "Join[" + packet._ID.ToString() + "]Fail";

                ServerLog log = new ServerLog(0, msg);

            }
            else if (_packetType == typeof(LoginPacket))
            {//로그인 페켓일떄
                _obj = RecivePacketInit(_packetData, _packetType, _packetDataLength);
                LoginPacket packet = (LoginPacket)_obj;

                //디비조회후 
                //UUID, ID, 닉네임, 플레이중인 캐릭터 인덱스
                //로그인 했다는 페켓 보냄
                //다음 유저정보 보냄
                GameUserInfo userSQL = new GameUserInfo();

                if (userSQL.Login(packet._ID, packet._PWD))
                {
                    _isLoginCheck = true;

                    byte[] buffer;
                    UserInfocheck chkPacket;

                    chkPacket._isUserInfo = _isLoginCheck;
                    chkPacket._chkType = 0;
                    //로그인 성공 했다고 알림
                    buffer = SendPacketInit(chkPacket);
                    PacketClass1 LoginChk = new PacketClass1(buffer, typeof(UserInfocheck), buffer.Length);
                    LoginChk._packetProtocolId = packet._UUID;
                    TCPServer._sendQueue.Enqueue(LoginChk);

                    //로그인 성공 했으니 정보 보냄
                    //조회후 보내야함

                    GameUserInfo userinfo = new GameUserInfo();
                    List<string> loginLoadInfo = new List<string>();
                    loginLoadInfo = userinfo.LoginUserInfo(packet._ID);

                    byte[] buffer1;
                    LoginInfoPacket loginInfoPacket;

                    loginInfoPacket._UUID = packet._UUID;
                    loginInfoPacket._NICKNAME = loginLoadInfo[0].ToString();
                    loginInfoPacket._ID = packet._ID.ToString();
                    loginInfoPacket._LEVEL = int.Parse(loginLoadInfo[2]);
                    loginInfoPacket._EXP = int.Parse(loginLoadInfo[3]);
                    loginInfoPacket._CURRENT_EXP = int.Parse(loginLoadInfo[4]);
                    loginInfoPacket._GOLD = int.Parse(loginLoadInfo[5]);
                    loginInfoPacket._AVTIDX = int.Parse(loginLoadInfo[6]);
                    loginInfoPacket._COMPLE_QUEST = loginLoadInfo[7].ToString();
                    loginInfoPacket._CURRENT_QUEST = loginLoadInfo[8].ToString();
                    loginInfoPacket._QUESTDATA = loginLoadInfo[9].ToString();
                    loginInfoPacket._QUESTING = int.Parse(loginLoadInfo[10]);

                    //packet size => 648
                    buffer1 = SendPacketInit(loginInfoPacket);

                    PacketClass1 loginInfo = new PacketClass1(buffer1, typeof(LoginInfoPacket), buffer1.Length);
                    loginInfo._packetProtocolId = packet._UUID;
                    TCPServer._sendQueue.Enqueue(loginInfo);
                }
                else
                {
                    _isLoginCheck = false;

                    byte[] buffer;
                    UserInfocheck chkPacket;
                    chkPacket._isUserInfo = _isLoginCheck;
                    chkPacket._chkType = 0;
                    buffer = SendPacketInit(chkPacket);
                    PacketClass1 LoginChk1 = new PacketClass1(buffer, typeof(UserInfocheck), buffer.Length);
                    LoginChk1._packetProtocolId = _packetProtocolId;
                    TCPServer._sendQueue.Enqueue(LoginChk1);
                }



                Console.WriteLine("Login Result : {0}", _isLoginCheck); 

            }
            else if (_packetType == typeof(JoinCheckPacket))
            {
                _obj = RecivePacketInit(_packetData, _packetType, _packetDataLength);
                JoinCheckPacket packet = (JoinCheckPacket)_obj;

                int cnt = 0;

                string SearchStr = "";
                int type = 0;
                //로그인 체크 패켓 한개로 id 닉네임 검사함
                if (packet._chkType == 0)
                {
                    SearchStr = "USER_ID";
                    type = 1;
                }
                else
                {
                    SearchStr = "NICKNAME";
                    type = 2;
                }

                //가입시 아이디랑 닉네임 체크 

                GameUserInfo joinCheckinfo = new GameUserInfo();
                cnt = joinCheckinfo.joinNameOrIdCheck(packet._TargetString, SearchStr);


                byte[] buffer;
                UserInfocheck chkPacket;

                if (cnt != 0) //정보가 없으니 true
                {
                    chkPacket._isUserInfo = true;
                    chkPacket._chkType = type;
                    buffer = SendPacketInit(chkPacket);
                    PacketClass1 LoginChk = new PacketClass1(buffer, typeof(UserInfocheck), buffer.Length);
                    LoginChk._packetProtocolId = packet._UUID;
                    TCPServer._sendQueue.Enqueue(LoginChk);
                }

                if (cnt == 0) //
                {

                    chkPacket._isUserInfo = false;
                    chkPacket._chkType = type;
                    buffer = SendPacketInit(chkPacket);
                    PacketClass1 LoginChk1 = new PacketClass1(buffer, typeof(UserInfocheck), buffer.Length);
                    LoginChk1._packetProtocolId = packet._UUID;
                    TCPServer._sendQueue.Enqueue(LoginChk1);
                }


            }
            else if (_packetType == typeof(GoldPacket))
            {
                _obj = RecivePacketInit(_packetData, _packetType, _packetDataLength);
                GoldPacket Goldpack = (GoldPacket)_obj;

                if (Goldpack._GTYPE == true)
                {//골드 획득시
                    GameUserInfo goldget = new GameUserInfo();
                    goldget.UserGetGold(Goldpack._ID, Goldpack._GOLD);
                }
                else//골드 사용시 
                {

                }

            }
            else if (_packetType == typeof(ExpPacket))
            {
                _obj = RecivePacketInit(_packetData, _packetType, _packetDataLength);
                ExpPacket expPack = (ExpPacket)_obj;
                GameUserInfo expUpdate = new GameUserInfo();
                string id = UTF8Encoding.Default.GetString(expPack._ID, 0, expPack._ID.Length).TrimEnd('\0');
                string pId = expUpdate.UserGetExp(id, expPack._AVTIDX, expPack._Exp);


                byte[] buffer;
                SendExpPacket expSendPack;
                GameUserInfo sendExp = new GameUserInfo();
                List<string> expInfo = sendExp.SendExpUpdate(pId);

                expSendPack._AVTIDX = short.Parse(expInfo[0]);
                expSendPack._Level = int.Parse(expInfo[1]);
                expSendPack._CurrentExp = int.Parse(expInfo[2]);
                expSendPack._UUID = expPack._UUID;
                buffer = SendPacketInit(expSendPack);
                PacketClass1 SendExpForClient = new PacketClass1(buffer, typeof(SendExpPacket), buffer.Length);
                SendExpForClient._packetProtocolId = expPack._UUID;
                TCPServer._sendQueue.Enqueue(SendExpForClient);

            }
            else if (_packetType == typeof(QuestProgressPacket))
            {
                _obj = RecivePacketInit(_packetData, _packetType, _packetDataLength);
                QuestProgressPacket QuestPack = (QuestProgressPacket)_obj;

                string id = UTF8Encoding.Default.GetString(QuestPack._ID, 0, QuestPack._ID.Length).TrimEnd('\0');
                string data = UTF8Encoding.Default.GetString(QuestPack._QUDATE, 0, QuestPack._QUDATE.Length).TrimEnd('\0');
                GameUserInfo questUpdate = new GameUserInfo();
                questUpdate.QuestdataUpdata(id, data);

                //questUpdate.CloseDBDConn();

                GameUserInfo userinfo = new GameUserInfo();
                List<string> loginLoadInfo = new List<string>();
                loginLoadInfo = userinfo.LoginUserInfo(id);

                byte[] buffer1;
                LoginInfoPacket loginInfoPacket;

                loginInfoPacket._UUID = QuestPack._UUID;
                loginInfoPacket._NICKNAME = loginLoadInfo[0].ToString();
                loginInfoPacket._ID = id;
                loginInfoPacket._LEVEL = int.Parse(loginLoadInfo[2]);
                loginInfoPacket._EXP = int.Parse(loginLoadInfo[3]);
                loginInfoPacket._CURRENT_EXP = int.Parse(loginLoadInfo[4]);
                loginInfoPacket._GOLD = int.Parse(loginLoadInfo[5]);
                loginInfoPacket._AVTIDX = int.Parse(loginLoadInfo[6]);
                loginInfoPacket._COMPLE_QUEST = loginLoadInfo[7].ToString();
                loginInfoPacket._CURRENT_QUEST = loginLoadInfo[8].ToString();
                loginInfoPacket._QUESTDATA = loginLoadInfo[9].ToString();
                loginInfoPacket._QUESTING = int.Parse(loginLoadInfo[10]);

                //packet size => 648
                buffer1 = SendPacketInit(loginInfoPacket);

                PacketClass1 loginInfo = new PacketClass1(buffer1, typeof(LoginInfoPacket), buffer1.Length);
                loginInfo._packetProtocolId = QuestPack._UUID;
                TCPServer._sendQueue.Enqueue(loginInfo);

                //userinfo.CloseDBDConn();
            }
        }

        //Server script에서 Dequeu로 실행 하는 함수
        public void SendProcess()
        {
            if (_packetType == typeof(ConnectPacket))
            {//연결 페켓 일때

            }
            else if (_packetType == typeof(JoinPacket))
            {//가입 페켓일때

            }
            else if (_packetType == typeof(UserInfocheck))
            {//로그인 페켓일떄
                SocketClass socket = SocketManager.SocketManagerFind(_packetProtocolId);
                if (socket._Socket.Poll(0, SelectMode.SelectWrite))
                {
                    socket._Socket.Send(_packetData);
                }
            }
            else if (_packetType == typeof(LoginInfoPacket))
            {//로그인 정보를 보내야 할때
                SocketClass socket = SocketManager.SocketManagerFind(_packetProtocolId);
                
                Console.WriteLine(_packetProtocolId);
                if (socket._Socket.Poll(0, SelectMode.SelectWrite))
                {
                    socket._Socket.Send(_packetData);
                }
            }
            else if (_packetType == typeof(SendExpPacket))
            {//레벨관련
                SocketClass socket = SocketManager.SocketManagerFind(_packetProtocolId);
                if (socket._Socket.Poll(0, SelectMode.SelectWrite))
                {
                    socket._Socket.Send(_packetData);
                }
            }

            else if (_packetType == typeof(QuestInfoSendPacket))
            {//퀘슽 관련
                SocketClass socket = SocketManager.SocketManagerFind(_packetProtocolId);
                if (socket._Socket.Poll(0, SelectMode.SelectWrite))
                {
                    //Console.WriteLine("Send 확인용 length : " + _packetData.Length);
                    socket._Socket.Send(_packetData);
                }
            }
        }












    }
}