using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TestGameeServer
{
    class DBCONN
    {
        string IPADDR = "localhost";
        int PORT = 3306;
        string DBNAME = "knockknockdungeon";
        string ID = "root";
        string PWD = "knockq";
        MySqlConnection conn;

        //생성자, DB 연결
        public DBCONN()
        {
            conn = DBConnection();

            if (conn == null)
            {
                throw new Exception();
            }
            else
            {
                //PrintDBTableInfo(DBNAME);
            }
        }
        //DB 연결, DB연결 확인 함수
        MySqlConnection DBConnection()
        {
            MySqlConnection conTemp = null;

            //"Server=아이피;Port=번호;Database=스키마이름(모델);Uid=root;Pwd=비번" 
            using (conn = new MySqlConnection("Server=" + IPADDR + ";Port=" + PORT + ";Database=" + DBNAME + ";Uid=" + ID + ";Pwd=" + PWD)) ;

            try
            {
                conn.Open();

                if (conn.Ping())
                {
                    conTemp = conn;
                    //Console.WriteLine("Connected To DB Sucess");
                }
                else
                {
                    conTemp = null;
                    Console.WriteLine("Connected To DB Fail");
                }

                conn.Close();
            }
            catch (Exception e)
            {
                conTemp = null;
                Console.WriteLine("Fail : " + e.ToString());
            }

            return conTemp;
        }

        /*public void CloseConn()
        {
            conn.Close();
        }*/

        //쿼리 실행 함수
        public bool ExecuteQuery(string query)
        {
            try
            {
                conn.Open();

                MySqlCommand command = new MySqlCommand(query, conn);

                if (command.ExecuteNonQuery() == 1)
                {
                    //Console.WriteLine("Operation Complete");
                }
                else
                    Console.WriteLine("Operation Fail");

                conn.Close();
                return true;
            }
            catch (Exception ep)
            {
                Console.WriteLine("실패 : " + ep.ToString());
                return false;
            }
        }
        //중복 값 확인용 함수
        public int selectCount(string tableName, string wStr, string wVal)
        {
            string query = "SELECT * FROM " + tableName + " WHERE " + wStr + " = '" + wVal + "'";

            int cnt = 0;

            try
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader selectData = command.ExecuteReader();

                while (selectData.Read())
                {
                    cnt++;
                }

                selectData.Close();
                conn.Close();
            }
            catch (Exception ep)
            {
                Console.WriteLine("실패 : " + ep.ToString());
            }

            return cnt;
        }
        //유저 정보 조회 함수
        public List<List<string>> selectValue(string query)
        {
            List<List<string>> rowVal = new List<List<string>>();
            try
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader selectData = command.ExecuteReader();

                while (selectData.Read())
                {
                    
                    List<string> selectVal = new List<string>();
                    int i = 0;
                    while (true)
                    {
                        try
                        {
                            //Console.WriteLine(selectData.GetString(i));
                            selectVal.Add(selectData.GetString(i));
                            i++;
                        }
                        catch(Exception e)
                        {
                            break;
                        }
                    }
                    rowVal.Add(selectVal);
                    
                }

                selectData.Close();
                conn.Close();
            }
            catch (Exception ep)
            {
                rowVal = null;
                Console.WriteLine("실패 : " + ep.ToString());
            }


            return rowVal;
        }

        //데이터 베이스 의 테이블 정보 출력함수
        void PrintDBTableInfo(string dbname)
        {
            string query = "SHOW TABLES FROM " + dbname;
            try
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader selectData = command.ExecuteReader();

                Console.WriteLine("[테이블 목록]");

                while (selectData.Read())
                {
                    Console.WriteLine(selectData.GetString(0));
                }

                selectData.Close();
                conn.Close();
            }
            catch (Exception ep)
            {
                Console.WriteLine("실패 : " + ep.ToString());
            }
        }

        
        
    }
}
