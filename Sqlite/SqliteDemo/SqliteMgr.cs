using UnityEngine;
using System.Data;
using System.Text;
using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Data.Sqlite;
using System.Runtime.InteropServices;

namespace SqliteMgrOld
{
    public class TypeName
    {
        public const string typeString = "System.String";
        public const string typeInt = "System.Int32";
        public const string typeFloat = "System.Single";
        public const string typeByte = "System.Byte";
        public const string typeBool = "System.Boolean";
        //public const string typeLong = "System.Int64";
    }

    public class SqliteMgr : MonoBehaviour
    {
        [DllImport("Sqlite3.dll", EntryPoint = "MySquare", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 MySquare(Int32 a, Int32 b);
        static GameObject m_go;
        static SqliteMgr m_instance = null;

        
        static private IDbConnection _connection = null;
        static private IDbCommand _command = null;
        static private IDataReader _reader = null;
        static string m_sqlName;
        private string v;

        public SqliteMgr(string v)
        {
            this.v = v;
        }
        
        public static SqliteMgr Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_go = new GameObject();
                    DontDestroyOnLoad(m_go);
                    m_go.name = "SqliteMgr";
                    m_instance = m_go.AddComponent<SqliteMgr>();
                }
                return m_instance;
            }
        }

        private void Awake()
        {
            //if (m_instance != null && m_instance != this)
            //{
            //    Destroy(gameObject);
            //}
        }


        public static void SqlInit(string sqlName)
        {
            try
            {
               // m_sqlName = sqlName;
                //Application.streamingAssetsPath
                string pathName = Application.streamingAssetsPath + "/" + sqlName +".db";
                _connection = new SqliteConnection(@"Data Source = " + pathName);

                _connection.Open();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
            // WAL = write ahead logging, very huge speed increase
            //_command.CommandText = "PRAGMA journal_mode = WAL;";
            //_command.ExecuteNonQuery();

            //_command.CommandText = "PRAGMA synchronous = OFF";
            //_command.ExecuteNonQuery();
        }

        /// <summary>
        /// 查找全部
        /// </summary>
        /// <param name="tabName">表名</param>
        /// <param name="func"></param>
        public static void SelectAll(string tableName, Action<IDataReader> func)
        {
            //_connection.Open();
            _command = _connection.CreateCommand();
            // if you have a bunch of stuff, this is going to be inefficient and a pain.  it's just for testing/show
            _command.CommandText = "SELECT * FROM " + tableName;
            //_reader.Close();
            _reader = _command.ExecuteReader();

            while (_reader.Read())
            {
                func(_reader);

            }
        }
        /// <summary>
        /// 查找全部
        /// </summary>
        /// <param name="tabName">表名</param>
        /// <param name="func"></param>
        public static void SelectAndSort(string tableName, string columnName, string SortType, Action<IDataReader> func)
        {
            _command = _connection.CreateCommand();
            // if you have a bunch of stuff, this is going to be inefficient and a pain.  it's just for testing/show
            _command.CommandText = "SELECT * FROM " + tableName + " ORDER BY " + columnName +" "+ SortType;
            //_reader.Close();
            _reader = _command.ExecuteReader();

            while (_reader.Read())
            {
                func(_reader);

            }

        }
       
        /// <summary>
        /// Basic execute command - open, create command, execute, close
        /// </summary>
        /// <param name="commandText"></param>
        public static IDataReader ExecuteQuery(string commandText)
        {
            IDataReader read = null;
            try
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                _command = _connection.CreateCommand();
                _command.CommandText = commandText;
                //IDataReader read = null;
                read = _command.ExecuteReader();
                //_command.ExecuteNonQuery();
                // _connection.Close();

            }
            catch (Exception ex)
            {
                Debug.Log("ex:" + ex);
                SQLiteClose();
            }
            return read;
        }

        /// <summary>
        /// Clean up everything for SQLite
        /// </summary>
        public static void SQLiteClose()
        {
            if (_reader != null && !_reader.IsClosed)
            {
                _reader.Close();
                _reader = null;
            }
            if (_command != null)
            {
                _command.Dispose();
                _command = null;
            }
            if (_connection != null && _connection.State != ConnectionState.Closed)
            {
                _connection.Close();
                _connection = null;
            }
        }

        /// <summary>
        /// 删除指定数据表内的数据（条件为“且（or）”）
        /// </summary>
        /// <param name="tableName">数据表名称</param>
        /// <param name="colNames">判定条件字段名</param>
        /// <param name="operations">条件符号</param>
        /// <param name="colValues">字段名对应的数据</param>
        static public void DeleteValues(string tableName, string[] colNames, string[] operations, string[] colValues, bool isAnd = true)
        {
            //当字段名称和字段数值不对应时引发异常
            if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length)
            {
                Debug.Log("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
            }

            StringBuilder queryString = new StringBuilder();

            queryString.AppendFormat("DELETE FROM {0} WHERE {1}{2}'{3}'", tableName, colNames[0], operations[0], colValues[0]);
            string orAnd = "";
            if (isAnd == true)
            {
                orAnd = "AND";
            }
            else
            {
                orAnd = "OR";
            }
            for (int i = 1; i < colValues.Length; i++)
            {
                queryString.AppendFormat(" {0} {1}{2}'{3}'", orAnd, colNames[i], operations[i], colValues[i]);
            }
            //Debug.Log(queryString.ToString());
            ExecuteQuery(queryString.ToString());
        }

        /// <summary>
        /// 向指定数据表中插入数据
        /// </summary>
        /// <param name="tableName">数据表名称</param>
        /// <param name="values">插入的数组</param>
        /// <returns></returns>
        static public void InsertValues(string tableName, string[] keys, string[] values)
        {
            //获取数据表中字段数目
            int fieldCount = ReadFullTable(tableName).FieldCount;
            //当插入的数据长度不等于字段数目时引发异常
            if (values.Length != fieldCount)
            {
                Debug.LogError("values.Length != fieldCount");
                return;
            }

            string commond = "INSERT OR REPLACE INTO " + tableName + " (";
            for (int i = 0; i < keys.Length; i++)
            {
                commond += keys[i];
                if (i != keys.Length - 1)
                    commond += ",";
            }
            commond += ") VALUES (";
            for (int i = 0; i < values.Length; i++)
            {
                commond += "'" + values[i] + "'";
                if (i != values.Length - 1)
                    commond += ",";
            }
            commond += ");";
            ExecuteQuery(commond);
        }

        static public IDataReader ReadFullTable(string tableName)
        {
            string queryString = "SELECT * FROM " + tableName;
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 执行SQL命令
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        static public IDataReader ExecuteQuery(string commandText, IDataParameter para)
        {
            //_connection.Open();
            _command.CommandText = commandText;
            _command.Parameters.Add(para);
            IDataReader read = _command.ExecuteReader();
            //_connection.Close();
            return read;
        }
   
        static public List<T> GetAllValuesOld<T>(string tableName) where T : new()
        {
            List<T> listTmp = new List<T>();

            Type type = typeof(T);
            PropertyInfo[] pInfos = type.GetProperties();
            //for (int i = 0; i < pInfos.Length; i++)
            //{
            //    Debug.Log("属性名：" + pInfos[i].Name);
            //}

            //_connection.Open();

            // if you have a bunch of stuff, this is going to be inefficient and a pain.  it's just for testing/show
            _command = _connection.CreateCommand();
            _command.CommandText = "SELECT * FROM " + tableName;
            _reader = _command.ExecuteReader();
            while (_reader.Read())
            {
                //Debug.Log("_reader[ID]"+_reader["ID"]);
                T newTmp = new T();
                for (int i = 0; i < pInfos.Length; i++)
                {
                    PropertyInfo propertyInfo = newTmp.GetType().GetProperty(pInfos[i].Name.ToString());
                    string sType = propertyInfo.PropertyType.ToString();

                    switch (sType)
                    {
                        case TypeName.typeFloat:
                            float f = (float)GetTypeData(sType, _reader, i);
                            propertyInfo.SetValue(newTmp, f, null);
                            break;
                        case TypeName.typeBool:
                            bool b = (bool)GetTypeData(sType, _reader, i);
                            propertyInfo.SetValue(newTmp, b, null);
                            break;
                        case TypeName.typeInt:
                            int iValue = (int)GetTypeData(sType, _reader, i);
                            propertyInfo.SetValue(newTmp, iValue, null);
                            break;
                        //case TypeName.typeLong:
                        //    Int64 int64Value = (Int64)GetTypeData(sType, _reader, i);
                        //    propertyInfo.SetValue(newTmp,int64Value, null);
                        //    break;
                        case TypeName.typeByte:
                            byte byteValue = (byte)GetTypeData(sType, _reader, i);
                            propertyInfo.SetValue(newTmp, byteValue, null);
                            break;
                        case TypeName.typeString:
                            string sValue = (string)GetTypeData(sType, _reader, i);
                            propertyInfo.SetValue(newTmp, sValue, null);
                            break;
                    }


                }
                listTmp.Add(newTmp);

            }
            _reader.Close();
            //_connection.Close();
            return listTmp;
        }

        static public List<T> GetAllValues<T>(string tableName) where T : new()
        {
            List<T> listTmp = new List<T>();

            Type type = typeof(T);
            PropertyInfo[] pInfos = type.GetProperties();

            //_connection.Open();
            _command = _connection.CreateCommand();
            // if you have a bunch of stuff, this is going to be inefficient and a pain.  it's just for testing/show
            _command.CommandText = "SELECT * FROM " + tableName;
            //_reader.Close();
            _reader = _command.ExecuteReader();

            while (_reader.Read())
            {
                T newTmp = new T();
                for (int i = 0; i < pInfos.Length; i++)
                {
                    PropertyInfo propertyInfo = newTmp.GetType().GetProperty(pInfos[i].Name.ToString());
                    object value = _reader[pInfos[i].Name];
                    propertyInfo.SetValue(newTmp, value, null);
                }
                listTmp.Add(newTmp);

            }
            _reader.Close();
            //_connection.Close();
            return listTmp;
        }

        static object GetTypeData(string typeName, IDataReader reader, int idx)
        {
            object obj = null;
            switch (typeName)
            {
                case TypeName.typeFloat:
                    obj = reader.GetDouble(idx);
                    break;
                case TypeName.typeBool:
                    obj = reader.GetBoolean(idx);
                    break;
                case TypeName.typeInt:
                    obj = reader.GetInt32(idx);
                    break;
                case TypeName.typeByte:
                    obj = reader.GetByte(idx);
                    break;
                case TypeName.typeString:
                    obj = reader.GetString(idx);
                    break;
            }
            return obj;
        }

        static public IDataReader UpdateValues(string tableName, string[] colNames, string[] colValues, string key, string operation, string value)
        {
            //当字段名称和字段数值不对应时引发异常
            if (colNames.Length != colValues.Length)
            {
                Debug.Log("colNames.Length != colValues.Length");
                return null;
            }

            StringBuilder queryString = new StringBuilder();

            queryString.AppendFormat("UPDATE {0} SET {1}='{2}'", tableName, colNames[0], colValues[0]);
            for (int i = 1; i < colValues.Length; i++)
            {
                queryString.AppendFormat(" , {0}='{1}'", colNames[i], colValues[i]);
            }
            queryString.AppendFormat(" WHERE {0}{1}'{2}'", key, operation, value);
            Debug.Log(queryString.ToString());
            return ExecuteQuery(queryString.ToString());
        }
        static public IDataReader UpdateOneData(string tableName, string colName, string colValue, string key, string operation, string value)
        {
            StringBuilder queryString = new StringBuilder();
            queryString.AppendFormat("UPDATE {0} SET {1}='{2}' WHERE {3} {4} '{5}'", tableName, colName, colValue, key, operation, value);
            return ExecuteQuery(queryString.ToString());
        }
        /// <summary>
        /// 导出数据表为固定格式文件
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="filePath">指定一个文件路径</param>
        /// <param name="func"></param>
        static public IDataReader ExportFile(string tableName, string filePath)
        {
            StringBuilder queryString = new StringBuilder();
            // if you have a bunch of stuff, this is going to be inefficient and a pain.  it's just for testing/show
            //导出为文本.txt文件用如下语句
            queryString.AppendFormat("header -csv E:/CQLGunivercity/CQ_UI/Assets/StreamingAssets/TeamSource" +  " SELECT * FROM" +  tableName) ;
            //导出为.csv文件用如下句式
            //queryString.AppendFormat( "SELECT * FROM " + tableName + " INTO OUTFILE  " + filePath + "FIELDS TERMINATED BY ',' ENCLOSED BY '''' LINES TERMINATED BY '\r\n'");
            return ExecuteQuery(queryString.ToString());


        }
        /// <summary>
        /// 导入固定格式文件
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="filePath">指定一个文件路径</param>
        /// <param name="func"></param>
        static public IDataReader ImportFile(string tableName, string filePath)
        {
            StringBuilder queryString = new StringBuilder();
            //本地导入用关键词LOCAL
            //导入文本文件如下
            // queryString.AppendFormat( "LOAD DATA LOCAL INFILE" + filePath + "INTO TABLE" + tableName );
            //导入.csv文件如下
            queryString.AppendFormat("LOAD DATA LOCAL INFILE" + filePath + "INTO TABLE" + tableName + "FIELDS TERMINATED BY ',' ENCLOSED BY '''' LINES TERMINATED BY '\r\n'");
            return ExecuteQuery(queryString.ToString());
        }
        private void OnApplicationQuit()
        {
            SQLiteClose();
        }

        static public IDataReader CreateTable(string tableName, string[] colNames, string[] colTypes)
        {
            StringBuilder queryString = new StringBuilder();

            queryString.AppendFormat("CREATE TABLE IF NOT EXISTS {0}( {1} {2}", tableName, colNames[0], colTypes[0]);
            for (int i = 1; i < colNames.Length; i++)
            {
                queryString.AppendFormat(", {0} {1}", colNames[i], colTypes[i]);
            }

            queryString.Append(" )");
            Debug.Log(queryString.ToString());
            return ExecuteQuery(queryString.ToString());
        }

        static public IDataReader DropTable(string tableName)
        {
            StringBuilder queryString = new StringBuilder();
            queryString.AppendFormat("DROP TABLE IF EXISTS {0}", tableName);
            return ExecuteQuery(queryString.ToString());
        }


        //这里只查询具有唯一key，只是一条信息
        public static bool SelectData<T>(string tableName, string key, string operation, string value, ref T newTmp) where T : new()
        {
            bool isExist = false;
            Type type = typeof(T);
            PropertyInfo[] pInfos = type.GetProperties();
            StringBuilder queryString = new StringBuilder();

            queryString.AppendFormat("SELECT * FROM {0} WHERE {1}{2} '{3}'", tableName, key, operation, value);

            //_connection.Open();

            _command.CommandText = queryString.ToString();

            _reader = _command.ExecuteReader();

            while (_reader.Read())
            {
                if (_reader.GetString(0).ToString() == value)
                {
                    isExist = true;
                    for (int i = 0; i < pInfos.Length; i++)
                    {
                        PropertyInfo propertyInfo = newTmp.GetType().GetProperty(pInfos[i].Name.ToString());
                        string sType = propertyInfo.PropertyType.ToString();

                        switch (sType)
                        {
                            case TypeName.typeFloat:
                                float f = (float)GetTypeData(sType, _reader, i);
                                propertyInfo.SetValue(newTmp, f, null);
                                break;
                            case TypeName.typeBool:
                                bool b = (bool)GetTypeData(sType, _reader, i);
                                propertyInfo.SetValue(newTmp, b, null);
                                break;
                            case TypeName.typeInt:
                                int iValue = (int)GetTypeData(sType, _reader, i);
                                propertyInfo.SetValue(newTmp, iValue, null);
                                break;
                            case TypeName.typeByte:
                                byte byteValue = (byte)GetTypeData(sType, _reader, i);
                                propertyInfo.SetValue(newTmp, byteValue, null);
                                break;
                            case TypeName.typeString:
                                string sValue = (string)GetTypeData(sType, _reader, i);
                                propertyInfo.SetValue(newTmp, sValue, null);
                                break;
                        }
                    }
                }
                if (isExist == true)
                {
                    break;
                }
            }
            return isExist;
        }

        public static bool IsExistTable(string tableName)
        {
            bool isExist = false;
            StringBuilder queryString = new StringBuilder();

            queryString.AppendFormat("SELECT * FROM sqlite_master where type='table' and name='{0}'", tableName);
            //_connection.Open();

            _command.CommandText = queryString.ToString();

            _reader = _command.ExecuteReader();

            while (_reader.Read())
            {
                if (_reader.GetString(0) != null)
                {
                    isExist = true;
                }
                if (isExist == true)
                {
                    break;
                }
            }
            return isExist;
        }

        internal void CreateTable(string v1, string[] v2)
        {
            throw new NotImplementedException();
        }
    }
}