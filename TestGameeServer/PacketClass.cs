using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TestGameeServer
{
    struct stdPacket
    {
        long _UUID;
        int _USERIDX;
        int _DATASIZE;
        byte[] _DATA;
        PacketClass _paaa;
    }
    
    class PacketClass
    {
        //ppt에 있는 캐스트 식별자는 일단 생략

        //필요정보
        [MarshalAs(UnmanagedType.U4)]
        long _packetProtocolId; //패킷 프로토콜 아이디
        [MarshalAs(UnmanagedType.U4)]
        int _uniqueUserIdx;
        [MarshalAs(UnmanagedType.U4)]
        int _packetDataLength; //패킷 데이터 크기
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1004)]
        byte[] _packetData; //패킷 데이터
        //--------
        stdPacket _stdpacket;

        //구조체를 클래스화 하는 것임
        //페켓이 정보만 들고 있었는데 그것을 기능화 시키면됨
        //구조체로 해서 크기를 지정해줘 포인터로 배열로 바뀔것
        //제일 상위에 페켓 구조체가 있어야 함
        //그 페켓 구조체를 뽑아서 외부 클래스로 전달해주는 함수 필요
        //페켓 구조체를 생성하는 공장이라고 생각하면됨
       

        public PacketClass()
        {

        }

        //생성 기능
        public void SendPacketInit(object obj)
        {//페켓을 만들때 정보 생성
            int dataSize = Marshal.SizeOf(obj);
            IntPtr buff = Marshal.AllocHGlobal(dataSize);
            Marshal.StructureToPtr(obj, buff, false);
            byte[] data = new byte[dataSize];
            Marshal.Copy(buff, data, 0, dataSize);
            Marshal.FreeHGlobal(buff);
        }

        public void RecivePacketInit(Type type)
        {
            IntPtr buff = Marshal.AllocHGlobal(_packetDataLength);
            Marshal.Copy(_packetData, 0, buff, _packetDataLength);
            object obj = Marshal.PtrToStructure(buff, type);
            Marshal.FreeHGlobal(buff);
            if(Marshal.SizeOf(obj) != _packetDataLength)
            {
                //return null;
            }
            //return obj;
        }

        //수정 기능
        void PacketModifyFun()
        {

        }
    }
}
