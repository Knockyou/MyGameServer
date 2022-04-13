using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGameeServer
{
    class GameUserInfo
    {
        DBCONN _dbconn;
        List<List<string>> _selectList;

        public GameUserInfo() { _dbconn = new DBCONN(); _selectList = new List<List<string>>(); }

        /*public void CloseDBDConn()
        {
            _dbconn.CloseConn();
        }*/

        public bool AddUser(string id, string pwd, string nName)
        {
            string joindate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            EncryptionPWD EnPwd = new EncryptionPWD();
            pwd = EnPwd.SHA256Hash(pwd);
            string query = "INSERT INTO knockknockdungeon.userinfo(USER_ID, USER_PWD, NICKNAME, JOIN_DATE) VALUES('" + id + "','" + pwd + "','" + nName + "', '" + joindate + "')";
            if (!_dbconn.ExecuteQuery(query))
                return false;
            else
            {

                //ingameuserinfo 삽입
                string query1 = "SELECT U_INDEX FROM knockknockdungeon.userinfo WHERE USER_ID = '" + id +"'";
                string result = "";
                _selectList = _dbconn.selectValue(query1);
                result = _selectList[0][0];

                string query2 = "INSERT INTO knockknockdungeon.ingameuserinfo(G_INDEX, GOLD, PLAYCHARACTER_IDX, COMPLETE_QUEST, CURRENT_QUEST, QUEST_DATA, QUEST_ING)";
                query2 += "VALUES(" + result + ", " + 0 + "," + 0 + ", '"+ "-1" +"' ,'"+"-1"+"','0', 0)";
                if (!_dbconn.ExecuteQuery(query2))
                    return false;
                else
                {

                    string query3 = "INSERT INTO knockknockdungeon.personalcharacterinfo(USER_IDX, CHARACTER_IDX, LEVEL, EXP, CURRENT_EXP)";
                    query3 += "VALUES(" + result + ",'" + 0 + "'," + 1 + ", " + 100 + " , " + 0 + ")";
                    if (!_dbconn.ExecuteQuery(query3))
                        return false;
                    else
                        return true;
                }
            }
            
        }

        public bool Login(string id, string pwd)
        {
            EncryptionPWD EnPwd = new EncryptionPWD();
            pwd = EnPwd.SHA256Hash(pwd);
            string query = "SELECT U_INDEX FROM knockknockdungeon.userinfo  WHERE USER_ID = '"+id+"'";
            query += " AND USER_PWD = '" +pwd+ "'";
            _selectList = _dbconn.selectValue(query);

            try
            {
                if (_selectList[0][0] != null)
                    return true;
                else
                    return false;
            }
            catch(Exception E)
            {
                return false;
            }
        }

        public List<string> LoginUserInfo(string id)
        {
            string query = "SELECT U.NICKNAME, U.USER_ID, C.LEVEL, C.EXP, C.CURRENT_EXP, G.GOLD, G.PLAYCHARACTER_IDX,";
            query += " G.COMPLETE_QUEST, G.CURRENT_QUEST, G.QUEST_DATA, G.QUEST_ING FROM knockknockdungeon.userinfo AS U";
            query += " LEFT JOIN knockknockdungeon.ingameuserinfo AS G ON U.U_INDEX = G.G_INDEX";
            query += " LEFT JOIN knockknockdungeon.personalcharacterinfo AS C ON G.G_INDEX = C.USER_IDX";
            query += " WHERE U.USER_ID = '"+id+"'";


            _selectList = _dbconn.selectValue(query);

            if (_selectList == null)
                Console.WriteLine("login info DB load fail");

            return _selectList[0];
        }


        public int joinNameOrIdCheck(string Vstr, string wStr)
        {
            int result = _dbconn.selectCount("knockknockdungeon.userinfo", wStr, Vstr);
            return result;
        }

        public bool UserGetGold(string id, int gold)
        {
            string query1 = "select U_INDEX from knockknockdungeon.userinfo Where USER_ID = '" + id + "'";

            _selectList = _dbconn.selectValue(query1);
            if (_selectList == null)
                Console.WriteLine("login info DB load fail");

            string query2 = "UPDATE knockknockdungeon.ingameuserinfo SET GOLD = '" + gold + "' WHERE G_INDEX = " + _selectList[0][0].ToString();
            if (!_dbconn.ExecuteQuery(query2))
                return false;
            else
                return true;
        }

        public string UserGetExp(string id, int targetCharacterIdx, int Exp)
        {

            string query = "SELECT P.P_INDEX, P.CURRENT_EXP FROM knockknockdungeon.userinfo AS U";
            query += " LEFT JOIN knockknockdungeon.personalcharacterinfo AS P ON U.U_INDEX = P.USER_IDX";
            query += " WHERE U.USER_ID = '" + id + "' AND P.CHARACTER_IDX = '" + targetCharacterIdx + "'";

            _selectList = _dbconn.selectValue(query);
            if (_selectList == null)
            {
                Console.WriteLine("login info DB load fail");
                return null;
            }

            int totalExp = int.Parse(_selectList[0][1]) + Exp; //
            string query1 = "UPDATE knockknockdungeon.personalcharacterinfo SET CURRENT_EXP = " + totalExp + " WHERE P_INDEX = '" + _selectList[0][0].ToString() + "'";

            if (!_dbconn.ExecuteQuery(query1))
                return null;


            string query2 = "UPDATE knockknockdungeon.personalcharacterinfo SET LEVEL = if(EXP <= CURRENT_EXP, LEVEL + 1, LEVEL)";
            query2 += " ,EXP = if(EXP <= CURRENT_EXP, (LEVEL)*100, EXP) ,CURRENT_EXP = if(EXP <= CURRENT_EXP, 0, CURRENT_EXP)  WHERE P_INDEX = '" + _selectList[0][0].ToString() + "'";

            if (!_dbconn.ExecuteQuery(query2))
            {
                return null;
            }
            else
                return _selectList[0][0].ToString();
        }

        public List<string> SendExpUpdate(string pid)
        {
            string query = "SELECT CHARACTER_IDX, LEVEL, CURRENT_EXP FROM knockknockdungeon.personalcharacterinfo";
            query += " WHERE P_INDEX = '" + pid + "'";
            _selectList = _dbconn.selectValue(query);
            if (_selectList == null)
            {
                Console.WriteLine("login info DB load fail");
                return null;
            }
            else
                return _selectList[0];
        }

        public void QuestdataUpdata(string id, string data)
        {
            string query = "SELECT U_INDEX FROM knockknockdungeon.userinfo WHERE USER_ID = '" +id+"'";
            _selectList = _dbconn.selectValue(query);
            if (_selectList == null)
            {
                Console.WriteLine("login info DB load fail");
                return;
            }

            string query1 = "";
            if (data == "ComplteM1")
            {
                query1 = "UPDATE knockknockdungeon.ingameuserinfo SET QUEST_DATA = 0,";
                query1 += " CURRENT_QUEST = 1, QUEST_ING = 0";
                query1 += " WHERE G_INDEX = " + _selectList[0][0].ToString();
            }
            else if(data == "Complte1")
            {
                query1 = "UPDATE knockknockdungeon.ingameuserinfo SET QUEST_DATA = 0,";
                query1 += " COMPLETE_QUEST = CONCAT(COMPLETE_QUEST,',1'),";
                query1 += " CURRENT_QUEST = 2, QUEST_ING = 0";
                query1 += " WHERE G_INDEX =" + _selectList[0][0].ToString();
            }
            else if (data == "Complte2")
            {
                query1 = "UPDATE knockknockdungeon.ingameuserinfo SET QUEST_DATA = 0,";
                query1 += " COMPLETE_QUEST = CONCAT(COMPLETE_QUEST,',2'),";
                query1 += " CURRENT_QUEST = 3, QUEST_ING = 0";
                query1 += " WHERE G_INDEX =" + _selectList[0][0].ToString();
            }
            else if (data == "Complte3")
            {
                query1 = "UPDATE knockknockdungeon.ingameuserinfo SET QUEST_DATA = 0,";
                query1 += " COMPLETE_QUEST = CONCAT(COMPLETE_QUEST,',3'),";
                query1 += " CURRENT_QUEST = 999, QUEST_ING = 0";
                query1 += " WHERE G_INDEX =" + _selectList[0][0].ToString();
            }
            else if (data == "Questccept")
            {
                query1 = "UPDATE knockknockdungeon.ingameuserinfo SET QUEST_DATA = 0,";
                query1 += " QUEST_ING = 1";
                query1 += " WHERE G_INDEX =" + _selectList[0][0].ToString();
            }
            else
            {
                query1 = "UPDATE knockknockdungeon.ingameuserinfo SET QUEST_DATA = '"+ data +"' WHERE G_INDEX = " + _selectList[0][0].ToString();
            }

            if (!_dbconn.ExecuteQuery(query1))
                return;

            /*string query2 = "select CURRENT_QUEST, QUEST_DATA, QUEST_ING from knockknockdungeon.ingameuserinfo WHERE G_INDEX = " + _selectList[0][0].ToString();

            _selectList = _dbconn.selectValue(query2);
            if (_selectList == null)
            {
                Console.WriteLine("login info DB load fail");
                return null;
            }
            else
            {
                return _selectList[0];
            }*/

        }
    }
}
