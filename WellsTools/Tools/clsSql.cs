using System;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
using System.Collections;

namespace Wells.Tools
{
    /// <summary>
    /// SqlServer���ݷ��ʰ�����
    /// </summary>
    public sealed class clsSql
    {
        #region ˽�й��캯���ͷ���
        private clsSql() {}
        /// <summary>
        /// ��SqlParameter��������(����ֵ)�����SqlCommand����.
        /// ������������κ�һ����������DBNull.Value;
        /// �ò�������ֹĬ��ֵ��ʹ��.
        /// </summary>clsSqlclsSql
        /// <param>������</param>
        /// <param>SqlParameters����</param>
        private static void attachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if( command == null ) throw new ArgumentNullException( "command" );
            if( commandParameters != null )
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if( p != null )
                    {
                        // ���δ����ֵ���������,���������DBNull.Value.
                        if ( ( p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input ) && 
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }
        
        /// <summary>
        /// ��DataRow���͵���ֵ���䵽SqlParameter��������.
        /// </summary>
        /// <param>Ҫ����ֵ��SqlParameter��������</param>
        /// <param>��Ҫ������洢���̲�����DataRow</param>
        private static void assignParameterValues(SqlParameter[] commandParameters, DataRow dataRow)
        {
            if ((commandParameters == null) || (dataRow == null)) 
            {
                return;
            }
            int i = 0;
            // ���ò���ֵ
            foreach(SqlParameter commandParameter in commandParameters)
            {
                // ������������,���������,ֻ�׳�һ���쳣.
                if( commandParameter.ParameterName == null || 
                    commandParameter.ParameterName.Length <= 1 )
                    throw new Exception( 
                        string.Format("���ṩ����{0}һ����Ч������{1}.", i, commandParameter.ParameterName ) );
                // ��dataRow�ı��л�ȡΪ�����������������Ƶ��е�����.
                // ������ںͲ���������ͬ����,����ֵ������ǰ���ƵĲ���.
                if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
                i++;
            }
        }
        
        /// <summary>
        /// ��һ��������������SqlParameter��������.
        /// </summary>
        /// <param>Ҫ����ֵ��SqlParameter��������</param>
        /// <param>��Ҫ������洢���̲����Ķ�������</param>
        private static void assignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null)) 
            {
                return;
            }
            // ȷ����������������������ƥ��,�����ƥ��,�׳�һ���쳣.
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("����ֵ�����������ƥ��.");
            }
            // ��������ֵ
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                // If the current array value derives from IDbDataParameter, then assign its Value property
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if( paramInstance.Value == null )
                    {
                        commandParameters[i].Value = DBNull.Value; 
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if (parameterValues[i] == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
        }
        
        /// <summary>
        /// Ԥ�����û��ṩ������,���ݿ�����/����/��������/����
        /// </summary>
        /// <param>Ҫ�����SqlCommand</param>
        /// <param>���ݿ�����</param>
        /// <param>һ����Ч�����������nullֵ</param>
        /// <param>�������� (�洢����,�����ı�, ����.)</param>
        /// <param>�洢��������T-SQL�����ı�</param>
        /// <param>�������������SqlParameter��������,���û�в���Ϊ'null'</param>
        /// <param><c>true</c> ��������Ǵ򿪵�,��Ϊtrue,���������Ϊfalse.</param>
        private static void prepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection )
        {
            if( command == null ) throw new ArgumentNullException( "command" );
            if( commandText == null || commandText.Length == 0 ) throw new ArgumentNullException( "commandText" );
            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }
            // ���������һ�����ݿ�����.
            command.Connection = connection;
            // ���������ı�(�洢��������SQL���)
            command.CommandText = commandText;
            // ��������
            if (transaction != null)
            {
                if( transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
                command.Transaction = transaction;
            }
            // ������������.
            command.CommandType = commandType;
            // �����������
            if (commandParameters != null)
            {
                attachParameters(command, commandParameters);
            }
            return;
        }
        #endregion ˽�й��캯���ͷ�������
            
        #region executeNonQuery����
        /// <summary>
        /// ִ��ָ�������ַ���,���͵�SqlCommand.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = executeNonQuery(connString, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param>һ����Ч�����ݿ������ַ���</param>
        /// <param>�������� (�洢����,�����ı�, ����.)</param>
        /// <param>�洢�������ƻ�SQL���</param>
        /// <returns>��������Ӱ�������</returns>
        public static int executeNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            return executeNonQuery(connectionString, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// ִ��ָ�������ַ���,���͵�SqlCommand.���û���ṩ����,�����ؽ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = executeNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>һ����Ч�����ݿ������ַ���</param>
        /// <param>�������� (�洢����,�����ı�, ����.)</param>
        /// <param>�洢�������ƻ�SQL���</param>
        /// <param>SqlParameter��������</param>
        /// <returns>��������Ӱ�������</returns>
        public static int executeNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return executeNonQuery(connection, commandType, commandText, commandParameters);
            }
        }
        
        /// <summary>
        /// ִ��ָ�������ַ����Ĵ洢����,�����������ֵ�����洢���̲���,
        /// �˷�����Ҫ�ڲ������淽����̽�����������ɲ���.
        /// </summary>
        /// <remarks>
        /// �������û���ṩ������������ͷ���ֵ.
        /// ʾ��:  
        ///  int result = executeNonQuery(connString, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param>һ����Ч�����ݿ������ַ���/param>
        /// <param>�洢��������</param>
        /// <param>���䵽�洢������������Ķ�������</param>
        /// <returns>������Ӱ�������</returns>
        public static int executeNonQuery(string connectionString, string spName, params object[] parameterValues)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // ������ڲ���ֵ
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // ��̽���洢���̲���(���ص�����)��������洢���̲�������.
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connectionString, spName);
                // ���洢���̲�����ֵ
                assignParameterValues(commandParameters, parameterValues);
                return executeNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                // û�в��������
                return executeNonQuery(connectionString, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ�������� 
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = executeNonQuery(conn, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>��������(�洢����,�����ı�������.)</param>
        /// <param>�洢�������ƻ�T-SQL���</param>
        /// <returns>����Ӱ�������</returns>
        public static int executeNonQuery(SqlConnection connection, CommandType commandType, string commandText)
        {
            return executeNonQuery(connection, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = executeNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>��������(�洢����,�����ı�������.)</param>
        /// <param>T�洢�������ƻ�T-SQL���</param>
        /// <param>SqlParamter��������</param>
        /// <returns>����Ӱ�������</returns>
        public static int executeNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {    
            if( connection == null ) throw new ArgumentNullException( "connection" );
            // ����SqlCommand����,������Ԥ����
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            prepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );
            
            // Finally, execute the command
            int retval = cmd.ExecuteNonQuery();
            
            // �������,�Ա��ٴ�ʹ��.
            cmd.Parameters.Clear();
            if( mustCloseConnection )
                connection.Close();
            return retval;
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,�����������ֵ�����洢���̲���.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ
        /// ʾ��:  
        ///  int result = executeNonQuery(conn, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�洢������</param>
        /// <param>������洢������������Ķ�������</param>
        /// <returns>����Ӱ�������</returns>
        public static int executeNonQuery(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // ����в���ֵ
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // �ӻ����м��ش洢���̲���
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                // ���洢���̷������ֵ
                assignParameterValues(commandParameters, parameterValues);
                return executeNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                return executeNonQuery(connection, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// ִ�д������SqlCommand.
        /// </summary>
        /// <remarks>
        /// ʾ��.:  
        ///  int result = executeNonQuery(trans, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>��������(�洢����,�����ı�������.)</param>
        /// <param>�洢�������ƻ�T-SQL���</param>
        /// <returns>����Ӱ�������/returns>
        public static int executeNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return executeNonQuery(transaction, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// ִ�д������SqlCommand(ָ������).
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = executeNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>��������(�洢����,�����ı�������.)</param>
        /// <param>�洢�������ƻ�T-SQL���</param>
        /// <param>SqlParamter��������</param>
        /// <returns>����Ӱ�������</returns>
        public static int executeNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            // Ԥ����
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            prepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );
                
            // ִ��
            int retval = cmd.ExecuteNonQuery();
                
            // ���������,�Ա��ٴ�ʹ��.
            cmd.Parameters.Clear();
            return retval;
        }
        
        /// <summary>
        /// ִ�д������SqlCommand(ָ������ֵ).
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ
        /// ʾ��:  
        ///  int result = executeNonQuery(conn, trans, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�洢������</param>
        /// <param>������洢������������Ķ�������</param>
        /// <returns>������Ӱ�������</returns>
        public static int executeNonQuery(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // ����в���ֵ
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                // ���洢���̲�����ֵ
                assignParameterValues(commandParameters, parameterValues);
                // �������ط���
                return executeNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                // û�в���ֵ
                return executeNonQuery(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion executeNonQuery��������
            
        #region executeDataset����
        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataSet ds = executeDataset(connString, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param>һ����Ч�����ݿ������ַ���</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢�������ƻ�T-SQL���</param>
        /// <returns>����һ�������������DataSet</returns>
        public static DataSet executeDataset(string connectionString, CommandType commandType, string commandText)
        {
            return executeDataset(connectionString, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��: 
        ///  DataSet ds = executeDataset(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>һ����Ч�����ݿ������ַ���</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢�������ƻ�T-SQL���</param>
        /// <param>SqlParamters��������</param>
        /// <returns>����һ�������������DataSet</returns>
        public static DataSet executeDataset(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            // �����������ݿ����Ӷ���,��������ͷŶ���.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // ����ָ�����ݿ������ַ������ط���.
                return executeDataset(connection, commandType, commandText, commandParameters);
            }
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,ֱ���ṩ����ֵ,����DataSet.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ.
        /// ʾ��: 
        ///  DataSet ds = executeDataset(connString, "GetOrders", 24, 36);
        /// </remarks>
        /// <param>һ����Ч�����ݿ������ַ���</param>
        /// <param>�洢������</param>
        /// <param>������洢������������Ķ�������</param>
        /// <returns>����һ�������������DataSet</returns>
        public static DataSet executeDataset(string connectionString, string spName, params object[] parameterValues)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // �ӻ����м����洢���̲���
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connectionString, spName);
                // ���洢���̲�������ֵ
                assignParameterValues(commandParameters, parameterValues);
                return executeDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                return executeDataset(connectionString, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataSet ds = executeDataset(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢��������T-SQL���</param>
        /// <returns>����һ�������������DataSet</returns>
        public static DataSet executeDataset(SqlConnection connection, CommandType commandType, string commandText)
        {
            return executeDataset(connection, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ָ���洢���̲���,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataSet ds = executeDataset(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢��������T-SQL���</param>
        /// <param>SqlParamter��������</param>
        /// <returns>����һ�������������DataSet</returns>
        public static DataSet executeDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            // Ԥ����
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            prepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );
                
            // ����SqlDataAdapter��DataSet.
            using( SqlDataAdapter da = new SqlDataAdapter(cmd) )
            {
                DataSet ds = new DataSet();
                // ���DataSet.
                da.Fill(ds);
                
                cmd.Parameters.Clear();
                if( mustCloseConnection )
                    connection.Close();
                return ds;
            }    
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ָ������ֵ,����DataSet.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ.
        /// ʾ��.:  
        ///  DataSet ds = executeDataset(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�洢������</param>
        /// <param>������洢������������Ķ�������</param>
        /// <returns>����һ�������������DataSet</returns>
        public static DataSet executeDataset(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // �Ȼ����м��ش洢���̲���
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                // ���洢���̲�������ֵ
                assignParameterValues(commandParameters, parameterValues);
                return executeDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                return executeDataset(connection, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// ִ��ָ�����������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataSet ds = executeDataset(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param>����</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢��������T-SQL���</param>
        /// <returns>����һ�������������DataSet</returns>
        public static DataSet executeDataset(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return executeDataset(transaction, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// ִ��ָ�����������,ָ������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataSet ds = executeDataset(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>����</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢��������T-SQL���</param>
        /// <param>SqlParamter��������</param>
        /// <returns>����һ�������������DataSet</returns>
        public static DataSet executeDataset(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            // Ԥ����
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            prepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );
                
            // ���� DataAdapter & DataSet
            using( SqlDataAdapter da = new SqlDataAdapter(cmd) )
            {
                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.Parameters.Clear();
                return ds;
            }    
        }
        
        /// <summary>
        /// ִ��ָ�����������,ָ������ֵ,����DataSet.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ.
        /// ʾ��.:  
        ///  DataSet ds = executeDataset(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param>����</param>
        /// <param>�洢������</param>
        /// <param>������洢������������Ķ�������</param>
        /// <returns>����һ�������������DataSet</returns>
        public static DataSet executeDataset(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // �ӻ����м��ش洢���̲���
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                // ���洢���̲�������ֵ
                assignParameterValues(commandParameters, parameterValues);
                return executeDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                return executeDataset(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion executeDataset���ݼ��������
        
        #region executeReader �����Ķ���
        /// <summary>
        /// ö��,��ʶ���ݿ���������clsSql�ṩ�����ɵ������ṩ
        /// </summary>
        private enum SqlConnectionOwnership    
        {
            /// <summary>��clsSql�ṩ����</summary>
            Internal, 
            /// <summary>�ɵ������ṩ����</summary>
            External
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ���������Ķ���.
        /// </summary>
        /// <remarks>
        /// �����clsSql������,�����ӹر�DataReaderҲ���ر�.
        /// ����ǵ��ö�������,DataReader�ɵ��ö�����.
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>һ����Ч������,����Ϊ 'null'</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢��������T-SQL���</param>
        /// <param>SqlParameters��������,���û�в�����Ϊ'null'</param>
        /// <param>��ʶ���ݿ����Ӷ������ɵ������ṩ������clsSql�ṩ</param>
        /// <returns>���ذ����������SqlDataReader</returns>
        private static SqlDataReader executeReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
        {    
            if( connection == null ) throw new ArgumentNullException( "connection" );
            bool mustCloseConnection = false;
            // ��������
            SqlCommand cmd = new SqlCommand();
            try
            {
                prepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );
            
                // ���������Ķ���
                SqlDataReader dataReader;
                if (connectionOwnership == SqlConnectionOwnership.External)
                {
                    dataReader = cmd.ExecuteReader();
                }
                else
                {
                    dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            
                // �������,�Ա��ٴ�ʹ��..
                // HACK: There is a problem here, the output parameter values are fletched 
                // when the reader is closed, so if the parameters are detached from the command
                // then the SqlReader can�t set its values. 
                // When this happen, the parameters can�t be used again in other command.
                bool canClear = true;
                foreach(SqlParameter commandParameter in cmd.Parameters)
                {
                    if (commandParameter.Direction != ParameterDirection.Input)
                        canClear = false;
                }
            
                if (canClear)
                {
                    cmd.Parameters.Clear();
                }
                return dataReader;
            }
            catch
            {
                if( mustCloseConnection )
                    connection.Close();
                throw;
            }
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ������ַ����������Ķ���.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  SqlDataReader dr = executeReader(connString, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param>һ����Ч�����ݿ������ַ���</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢��������T-SQL���</param>
        /// <returns>���ذ����������SqlDataReader</returns>
        public static SqlDataReader executeReader(string connectionString, CommandType commandType, string commandText)
        {
            return executeReader(connectionString, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ������ַ����������Ķ���,ָ������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  SqlDataReader dr = executeReader(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>һ����Ч�����ݿ������ַ���</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢��������T-SQL���</param>
        /// <param>SqlParamter��������(new SqlParameter("@prodid", 24))</param>
        /// <returns>���ذ����������SqlDataReader</returns>
        public static SqlDataReader executeReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                return executeReader(connection, null, commandType, commandText, commandParameters,SqlConnectionOwnership.Internal);
            }
            catch
            {
                // If we fail to return the SqlDatReader, we need to close the connection ourselves
                if( connection != null ) connection.Close();
                throw;
            }
            
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ������ַ����������Ķ���,ָ������ֵ.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// ʾ��:  
        ///  SqlDataReader dr = executeReader(connString, "GetOrders", 24, 36);
        /// </remarks>
        /// <param>һ����Ч�����ݿ������ַ���</param>
        /// <param>�洢������</param>
        /// <param>������洢������������Ķ�������</param>
        /// <returns>���ذ����������SqlDataReader</returns>
        public static SqlDataReader executeReader(string connectionString, string spName, params object[] parameterValues)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connectionString, spName);
                assignParameterValues(commandParameters, parameterValues);
                return executeReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                return executeReader(connectionString, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ���������Ķ���.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  SqlDataReader dr = executeReader(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢��������T-SQL���</param>
        /// <returns>���ذ����������SqlDataReader</returns>
        public static SqlDataReader executeReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            return executeReader(connection, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// [�����߷�ʽ]ִ��ָ�����ݿ����Ӷ���������Ķ���,ָ������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  SqlDataReader dr = executeReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>SqlParamter��������</param>
        /// <returns>���ذ����������SqlDataReader</returns>
        public static SqlDataReader executeReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            return executeReader(connection, (SqlTransaction)null, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }
        
        /// <summary>
        /// [�����߷�ʽ]ִ��ָ�����ݿ����Ӷ���������Ķ���,ָ������ֵ.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// ʾ��:  
        ///  SqlDataReader dr = executeReader(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>T�洢������</param>
        /// <param>������洢������������Ķ�������</param>
        /// <returns>���ذ����������SqlDataReader</returns>
        public static SqlDataReader executeReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                assignParameterValues(commandParameters, parameterValues);
                return executeReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                return executeReader(connection, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// [�����߷�ʽ]ִ��ָ�����ݿ�����������Ķ���,ָ������ֵ.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  SqlDataReader dr = executeReader(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param>һ����Ч����������</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢�������ƻ�T-SQL���</param>
        /// <returns>���ذ����������SqlDataReader</returns>
        public static SqlDataReader executeReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return executeReader(transaction, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// [�����߷�ʽ]ִ��ָ�����ݿ�����������Ķ���,ָ������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///   SqlDataReader dr = executeReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>һ����Ч����������</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢�������ƻ�T-SQL���</param>
        /// <param>����������SqlParamter��������</param>
        /// <returns>���ذ����������SqlDataReader</returns>
        public static SqlDataReader executeReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            return executeReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }
        
        /// <summary>
        /// [�����߷�ʽ]ִ��ָ�����ݿ�����������Ķ���,ָ������ֵ.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  SqlDataReader dr = executeReader(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param>һ����Ч����������</param>
        /// <param>�洢��������</param>
        /// <param>������洢������������Ķ�������</param>
        /// <returns>���ذ����������SqlDataReader</returns>
        public static SqlDataReader executeReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // ����в���ֵ
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                assignParameterValues(commandParameters, parameterValues);
                return executeReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                // û�в���ֵ
                return executeReader(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion executeReader�����Ķ���
            
        #region executeScalar ���ؽ�����еĵ�һ�е�һ��        
        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)executeScalar(connString, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param>һ����Ч�����ݿ������ַ���</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢�������ƻ�T-SQL���</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public static object executeScalar(string connectionString, CommandType commandType, string commandText)
        {
            // ִ�в���Ϊ�յķ���
            return executeScalar(connectionString, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,ָ������,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)executeScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>һ����Ч�����ݿ������ַ���</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢�������ƻ�T-SQL���</param>
        /// <param>����������SqlParamter��������</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public static object executeScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            // �����������ݿ����Ӷ���,��������ͷŶ���.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // ����ָ�����ݿ������ַ������ط���.
                return executeScalar(connection, commandType, commandText, commandParameters);
            }
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,ָ������ֵ,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  int orderCount = (int)executeScalar(connString, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param>һ����Ч�����ݿ������ַ���</param>
        /// <param>�洢��������</param>
        /// <param>������洢������������Ķ�������</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public static object executeScalar(string connectionString, string spName, params object[] parameterValues)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            
            // ����в���ֵ
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connectionString, spName);
                // ���洢���̲�����ֵ
                assignParameterValues(commandParameters, parameterValues);
                // �������ط���
                return executeScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                // û�в���ֵ
                return executeScalar(connectionString, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)executeScalar(conn, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢�������ƻ�T-SQL���</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public static object executeScalar(SqlConnection connection, CommandType commandType, string commandText)
        {
            // ִ�в���Ϊ�յķ���
            return executeScalar(connection, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ָ������,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)executeScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢�������ƻ�T-SQL���</param>
        /// <param>����������SqlParamter��������</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public static object executeScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            // ����SqlCommand����,������Ԥ����
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            prepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );
                
            // ִ��SqlCommand����,�����ؽ��.
            object retval = cmd.ExecuteScalar();
                
            // �������,�Ա��ٴ�ʹ��.
            cmd.Parameters.Clear();
            if( mustCloseConnection )
                connection.Close();
            return retval;
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ָ������ֵ,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  int orderCount = (int)executeScalar(conn, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�洢��������</param>
        /// <param>������洢������������Ķ�������</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public static object executeScalar(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // ����в���ֵ
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                // ���洢���̲�����ֵ
                assignParameterValues(commandParameters, parameterValues);
                // �������ط���
                return executeScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                // û�в���ֵ
                return executeScalar(connection, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����������,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)executeScalar(trans, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param>һ����Ч����������</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢�������ƻ�T-SQL���</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public static object executeScalar(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // ִ�в���Ϊ�յķ���
            return executeScalar(transaction, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����������,ָ������,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)executeScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>һ����Ч����������</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢�������ƻ�T-SQL���</param>
        /// <param>����������SqlParamter��������</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public static object executeScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            // ����SqlCommand����,������Ԥ����
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            prepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );
                
            // ִ��SqlCommand����,�����ؽ��.
            object retval = cmd.ExecuteScalar();
                
            // �������,�Ա��ٴ�ʹ��.
            cmd.Parameters.Clear();
            return retval;
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����������,ָ������ֵ,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  int orderCount = (int)executeScalar(trans, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param>һ����Ч����������</param>
        /// <param>�洢��������</param>
        /// <param>������洢������������Ķ�������</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public static object executeScalar(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // ����в���ֵ
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // PPull the parameters for this stored procedure from the parameter cache ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                // ���洢���̲�����ֵ
                assignParameterValues(commandParameters, parameterValues);
                // �������ط���
                return executeScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                // û�в���ֵ
                return executeScalar(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion executeScalar 
               
        #region executeXmlReader XML�Ķ���
        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ����SqlCommand����,������һ��XmlReader������Ϊ���������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  XmlReader r = executeXmlReader(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢�������ƻ�T-SQL��� using "FOR XML AUTO"</param>
        /// <returns>����XmlReader���������.</returns>
        public static XmlReader executeXmlReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            // ִ�в���Ϊ�յķ���
            return executeXmlReader(connection, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ����SqlCommand����,������һ��XmlReader������Ϊ���������,ָ������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  XmlReader r = executeXmlReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢�������ƻ�T-SQL��� using "FOR XML AUTO"</param>
        /// <param>����������SqlParamter��������</param>
        /// <returns>����XmlReader���������.</returns>
        public static XmlReader executeXmlReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            bool mustCloseConnection = false;
            // ����SqlCommand����,������Ԥ����
            SqlCommand cmd = new SqlCommand();
            try
            {
                prepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );
            
                // ִ������
                XmlReader retval = cmd.ExecuteXmlReader();
            
                // �������,�Ա��ٴ�ʹ��.
                cmd.Parameters.Clear();
                return retval;
            }
            catch
            {    
                if( mustCloseConnection )
                    connection.Close();
                throw;
            }
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ����SqlCommand����,������һ��XmlReader������Ϊ���������,ָ������ֵ.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  XmlReader r = executeXmlReader(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�洢�������� using "FOR XML AUTO"</param>
        /// <param>������洢������������Ķ�������</param>
        /// <returns>����XmlReader���������.</returns>
        public static XmlReader executeXmlReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // ����в���ֵ
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                // ���洢���̲�����ֵ
                assignParameterValues(commandParameters, parameterValues);
                // �������ط���
                return executeXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                // û�в���ֵ
                return executeXmlReader(connection, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ������SqlCommand����,������һ��XmlReader������Ϊ���������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  XmlReader r = executeXmlReader(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param>һ����Ч����������</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢�������ƻ�T-SQL��� using "FOR XML AUTO"</param>
        /// <returns>����XmlReader���������.</returns>
        public static XmlReader executeXmlReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // ִ�в���Ϊ�յķ���
            return executeXmlReader(transaction, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ������SqlCommand����,������һ��XmlReader������Ϊ���������,ָ������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  XmlReader r = executeXmlReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>һ����Ч����������</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢�������ƻ�T-SQL��� using "FOR XML AUTO"</param>
        /// <param>����������SqlParamter��������</param>
        /// <returns>����XmlReader���������.</returns>
        public static XmlReader executeXmlReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            // ����SqlCommand����,������Ԥ����
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            prepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );
            
            // ִ������
            XmlReader retval = cmd.ExecuteXmlReader();
            
            // �������,�Ա��ٴ�ʹ��.
            cmd.Parameters.Clear();
            return retval;            
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ������SqlCommand����,������һ��XmlReader������Ϊ���������,ָ������ֵ.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  XmlReader r = executeXmlReader(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param>һ����Ч����������</param>
        /// <param>�洢��������</param>
        /// <param>������洢������������Ķ�������</param>
        /// <returns>����һ�������������DataSet.</returns>
        public static XmlReader executeXmlReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // ����в���ֵ
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                // ���洢���̲�����ֵ
                assignParameterValues(commandParameters, parameterValues);
                // �������ط���
                return executeXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                // û�в���ֵ
                return executeXmlReader(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion executeXmlReader �Ķ�������
            
        #region FillDataset ������ݼ�
        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,ӳ�����ݱ�������ݼ�.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param>һ����Ч�����ݿ������ַ���</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢�������ƻ�T-SQL���</param>
        /// <param>Ҫ���������DataSetʵ��</param>
        /// <param>��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)</param>
        public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( dataSet == null ) throw new ArgumentNullException( "dataSet" );
            
            // �����������ݿ����Ӷ���,��������ͷŶ���.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // ����ָ�����ݿ������ַ������ط���.
                FillDataset(connection, commandType, commandText, dataSet, tableNames);
            }
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,ӳ�����ݱ�������ݼ�.ָ���������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>һ����Ч�����ݿ������ַ���</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢�������ƻ�T-SQL���</param>
        /// <param>����������SqlParamter��������</param>
        /// <param>Ҫ���������DataSetʵ��</param>
        /// <param>��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>
        public static void FillDataset(string connectionString, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( dataSet == null ) throw new ArgumentNullException( "dataSet" );
            // �����������ݿ����Ӷ���,��������ͷŶ���.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // ����ָ�����ݿ������ַ������ط���.
                FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
            }
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,ӳ�����ݱ�������ݼ�,ָ���洢���̲���ֵ.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, 24);
        /// </remarks>
        /// <param>һ����Ч�����ݿ������ַ���</param>
        /// <param>�洢��������</param>
        /// <param>Ҫ���������DataSetʵ��</param>
        /// <param>��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>    
        /// <param>������洢������������Ķ�������</param>
        public static void FillDataset(string connectionString, string spName,
            DataSet dataSet, string[] tableNames,
            params object[] parameterValues)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( dataSet == null ) throw new ArgumentNullException( "dataSet" );
            // �����������ݿ����Ӷ���,��������ͷŶ���.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // ����ָ�����ݿ������ַ������ط���.
                FillDataset (connection, spName, dataSet, tableNames, parameterValues);
            }
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ӳ�����ݱ�������ݼ�.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢�������ƻ�T-SQL���</param>
        /// <param>Ҫ���������DataSetʵ��</param>
        /// <param>��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>    
        public static void FillDataset(SqlConnection connection, CommandType commandType, 
            string commandText, DataSet dataSet, string[] tableNames)
        {
            FillDataset(connection, commandType, commandText, dataSet, tableNames, null);
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ӳ�����ݱ�������ݼ�,ָ������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢�������ƻ�T-SQL���</param>
        /// <param>Ҫ���������DataSetʵ��</param>
        /// <param>��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>
        /// <param>����������SqlParamter��������</param>
        public static void FillDataset(SqlConnection connection, CommandType commandType, 
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            FillDataset(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ӳ�����ݱ�������ݼ�,ָ���洢���̲���ֵ.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  FillDataset(conn, "GetOrders", ds, new string[] {"orders"}, 24, 36);
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�洢��������</param>
        /// <param>Ҫ���������DataSetʵ��</param>
        /// <param>��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>
        /// <param>������洢������������Ķ�������</param>
        public static void FillDataset(SqlConnection connection, string spName, 
            DataSet dataSet, string[] tableNames,
            params object[] parameterValues)
        {
            if ( connection == null ) throw new ArgumentNullException( "connection" );
            if (dataSet == null ) throw new ArgumentNullException( "dataSet" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // ����в���ֵ
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                // ���洢���̲�����ֵ
                assignParameterValues(commandParameters, parameterValues);
                // �������ط���
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
            }
            else 
            {
                // û�в���ֵ
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }    
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����������,ӳ�����ݱ�������ݼ�.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param>һ����Ч����������</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢�������ƻ�T-SQL���</param>
        /// <param>Ҫ���������DataSetʵ��</param>
        /// <param>��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>
        public static void FillDataset(SqlTransaction transaction, CommandType commandType, 
            string commandText,
            DataSet dataSet, string[] tableNames)
        {
            FillDataset (transaction, commandType, commandText, dataSet, tableNames, null);    
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����������,ӳ�����ݱ�������ݼ�,ָ������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>һ����Ч����������</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢�������ƻ�T-SQL���</param>
        /// <param>Ҫ���������DataSetʵ��</param>
        /// <param>��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>
        /// <param>����������SqlParamter��������</param>
        public static void FillDataset(SqlTransaction transaction, CommandType commandType, 
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);
        }
        
        /// <summary>
        /// ִ��ָ�����ݿ����������,ӳ�����ݱ�������ݼ�,ָ���洢���̲���ֵ.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  FillDataset(trans, "GetOrders", ds, new string[]{"orders"}, 24, 36);
        /// </remarks>
        /// <param>һ����Ч����������</param>
        /// <param>�洢��������</param>
        /// <param>Ҫ���������DataSetʵ��</param>
        /// <param>��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>
        /// <param>������洢������������Ķ�������</param>
        public static void FillDataset(SqlTransaction transaction, string spName,
            DataSet dataSet, string[] tableNames,
            params object[] parameterValues) 
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( dataSet == null ) throw new ArgumentNullException( "dataSet" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // ����в���ֵ
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                // ���洢���̲�����ֵ
                assignParameterValues(commandParameters, parameterValues);
                // �������ط���
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
            }
            else 
            {
                // û�в���ֵ
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }    
        }
        
        /// <summary>
        /// [˽�з���][�ڲ�����]ִ��ָ�����ݿ����Ӷ���/���������,ӳ�����ݱ�������ݼ�,DataSet/TableNames/SqlParameters.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  FillDataset(conn, trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>һ����Ч����������</param>
        /// <param>�������� (�洢����,�����ı�������)</param>
        /// <param>�洢�������ƻ�T-SQL���</param>
        /// <param>Ҫ���������DataSetʵ��</param>
        /// <param>��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>
        /// <param>����������SqlParamter��������</param>
        private static void FillDataset(SqlConnection connection, SqlTransaction transaction, CommandType commandType, 
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( dataSet == null ) throw new ArgumentNullException( "dataSet" );
            // ����SqlCommand����,������Ԥ����
            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            prepareCommand(command, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );
                
            // ִ������
            using( SqlDataAdapter dataAdapter = new SqlDataAdapter(command) )
            {
                
                // ׷�ӱ�ӳ��
                if (tableNames != null && tableNames.Length > 0)
                {
                    string tableName = "Table";
                    for (int index=0; index < tableNames.Length; index++)
                    {
                        if( tableNames[index] == null || tableNames[index].Length == 0 ) throw new ArgumentException( "The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames" );
                        dataAdapter.TableMappings.Add(tableName, tableNames[index]);
                        tableName += (index + 1).ToString();
                    }
                }
                
                // ������ݼ�ʹ��Ĭ�ϱ�����
                dataAdapter.Fill(dataSet);
                // �������,�Ա��ٴ�ʹ��.
                command.Parameters.Clear();
            }
            if( mustCloseConnection )
                connection.Close();
        }
        #endregion
        
        #region UpdateDataset �������ݼ�
        /// <summary>
        /// ִ�����ݼ����µ����ݿ�,ָ��inserted, updated, or deleted����.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  UpdateDataset(conn, insertCommand, deleteCommand, updateCommand, dataSet, "Order");
        /// </remarks>
        /// <param>[׷�Ӽ�¼]һ����Ч��T-SQL����洢����</param>
        /// <param>[ɾ����¼]һ����Ч��T-SQL����洢����</param>
        /// <param>[���¼�¼]һ����Ч��T-SQL����洢����</param>
        /// <param>Ҫ���µ����ݿ��DataSet</param>
        /// <param>Ҫ���µ����ݿ��DataTable</param>
        public static void UpdateDataset(SqlCommand insertCommand, SqlCommand deleteCommand, SqlCommand updateCommand, DataSet dataSet, string tableName)
        {
            if( insertCommand == null ) throw new ArgumentNullException( "insertCommand" );
            if( deleteCommand == null ) throw new ArgumentNullException( "deleteCommand" );
            if( updateCommand == null ) throw new ArgumentNullException( "updateCommand" );
            if( tableName == null || tableName.Length == 0 ) throw new ArgumentNullException( "tableName" ); 
            // ����SqlDataAdapter,��������ɺ��ͷ�.
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter())
            {
                // ������������������
                dataAdapter.UpdateCommand = updateCommand;
                dataAdapter.InsertCommand = insertCommand;
                dataAdapter.DeleteCommand = deleteCommand;
                // �������ݼ��ı䵽���ݿ�
                dataAdapter.Update (dataSet, tableName); 
                // �ύ���иı䵽���ݼ�.
                dataSet.AcceptChanges();
            }
        }
        #endregion
            
        #region CreateCommand ����һ��SqlCommand����
        /// <summary>
        /// ����SqlCommand����,ָ�����ݿ����Ӷ���,�洢�������Ͳ���.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  SqlCommand command = CreateCommand(conn, "AddCustomer", "CustomerID", "CustomerName");
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�洢��������</param>
        /// <param>Դ�������������</param>
        /// <returns>����SqlCommand����</returns>
        public static SqlCommand CreateCommand(SqlConnection connection, string spName, params string[] sourceColumns) 
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // ��������
            SqlCommand cmd = new SqlCommand( spName, connection );
            cmd.CommandType = CommandType.StoredProcedure;
            // ����в���ֵ
            if ((sourceColumns != null) && (sourceColumns.Length > 0)) 
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                // ��Դ����е�ӳ�䵽DataSet������.
                for (int index=0; index < sourceColumns.Length; index++)
                    commandParameters[index].SourceColumn = sourceColumns[index];
                // Attach the discovered parameters to the SqlCommand object
                attachParameters (cmd, commandParameters);
            }
            return cmd;
        }
        #endregion
            
        #region executeNonQueryTypedParams ���ͻ�����(DataRow)
        /// <summary>
        /// ִ��ָ���������ݿ������ַ����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,������Ӱ�������.
        /// </summary>
        /// <param>һ����Ч�����ݿ������ַ���</param>
        /// <param>�洢��������</param>
        /// <param>ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>����Ӱ�������</returns>
        public static int executeNonQueryTypedParams(String connectionString, String spName, DataRow dataRow)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            
            // ���row��ֵ,�洢���̱����ʼ��.
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connectionString, spName);
                
                // �������ֵ
                assignParameterValues(commandParameters, dataRow);
                                
                return clsSql.executeNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeNonQuery(connectionString, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// ִ��ָ���������ݿ����Ӷ���Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,������Ӱ�������.
        /// </summary>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�洢��������</param>
        /// <param>ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>����Ӱ�������</returns>
        public static int executeNonQueryTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // ���row��ֵ,�洢���̱����ʼ��.
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                
                // �������ֵ
                assignParameterValues(commandParameters, dataRow);
                                
                return clsSql.executeNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeNonQuery(connection, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// ִ��ָ���������ݿ�����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,������Ӱ�������.
        /// </summary>
        /// <param>һ����Ч���������� object</param>
        /// <param>�洢��������</param>
        /// <param>ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>����Ӱ�������</returns>
        public static int executeNonQueryTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // Sf the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                
                // �������ֵ
                assignParameterValues(commandParameters, dataRow);
                                
                return clsSql.executeNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeNonQuery(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion
            
        #region executeDatasetTypedParams ���ͻ�����(DataRow)
        /// <summary>
        /// ִ��ָ���������ݿ������ַ����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����DataSet.
        /// </summary>
        /// <param>һ����Ч�����ݿ������ַ���</param>
        /// <param>�洢��������</param>
        /// <param>ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>����һ�������������DataSet.</returns>
        public static DataSet executeDatasetTypedParams(string connectionString, String spName, DataRow dataRow)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            //���row��ֵ,�洢���̱����ʼ��.
            if ( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connectionString, spName);
                
                // �������ֵ
                assignParameterValues(commandParameters, dataRow);
                
                return clsSql.executeDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeDataset(connectionString, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// ִ��ָ���������ݿ����Ӷ���Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����DataSet.
        /// </summary>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�洢��������</param>
        /// <param>ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>����һ�������������DataSet.</returns>
        /// 
        public static DataSet executeDatasetTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // ���row��ֵ,�洢���̱����ʼ��.
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                
                // �������ֵ
                assignParameterValues(commandParameters, dataRow);
                
                return clsSql.executeDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeDataset(connection, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// ִ��ָ���������ݿ�����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����DataSet.
        /// </summary>
        /// <param>һ����Ч���������� object</param>
        /// <param>�洢��������</param>
        /// <param>ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>����һ�������������DataSet.</returns>
        public static DataSet executeDatasetTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // ���row��ֵ,�洢���̱����ʼ��.
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                
                // �������ֵ
                assignParameterValues(commandParameters, dataRow);
                
                return clsSql.executeDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeDataset(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion
            
        #region executeReaderTypedParams ���ͻ�����(DataRow)
        /// <summary>
        /// ִ��ָ���������ݿ������ַ����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����DataReader.
        /// </summary>
        /// <param>һ����Ч�����ݿ������ַ���</param>
        /// <param>�洢��������</param>
        /// <param>ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>���ذ����������SqlDataReader</returns>
        public static SqlDataReader executeReaderTypedParams(String connectionString, String spName, DataRow dataRow)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            
            // ���row��ֵ,�洢���̱����ʼ��.
            if ( dataRow != null && dataRow.ItemArray.Length > 0 )
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connectionString, spName);
                
                // �������ֵ
                assignParameterValues(commandParameters, dataRow);
                
                return clsSql.executeReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeReader(connectionString, CommandType.StoredProcedure, spName);
            }
        }
                
        /// <summary>
        /// ִ��ָ���������ݿ����Ӷ���Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����DataReader.
        /// </summary>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�洢��������</param>
        /// <param>ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>���ذ����������SqlDataReader</returns>
        public static SqlDataReader executeReaderTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // ���row��ֵ,�洢���̱����ʼ��.
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                
                // �������ֵ
                assignParameterValues(commandParameters, dataRow);
                
                return clsSql.executeReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeReader(connection, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// ִ��ָ���������ݿ�����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����DataReader.
        /// </summary>
        /// <param>һ����Ч���������� object</param>
        /// <param>�洢��������</param>
        /// <param>ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>���ذ����������SqlDataReader</returns>
        public static SqlDataReader executeReaderTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // ���row��ֵ,�洢���̱����ʼ��.
            if( dataRow != null && dataRow.ItemArray.Length > 0 )
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                
                // �������ֵ
                assignParameterValues(commandParameters, dataRow);
                
                return clsSql.executeReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeReader(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion
            
        #region executeScalarTypedParams ���ͻ�����(DataRow)
        /// <summary>
        /// ִ��ָ���������ݿ������ַ����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <param>һ����Ч�����ݿ������ַ���</param>
        /// <param>�洢��������</param>
        /// <param>ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public static object executeScalarTypedParams(String connectionString, String spName, DataRow dataRow)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            
            // ���row��ֵ,�洢���̱����ʼ��.
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connectionString, spName);
                
                // �������ֵ
                assignParameterValues(commandParameters, dataRow);
                
                return clsSql.executeScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeScalar(connectionString, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// ִ��ָ���������ݿ����Ӷ���Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�洢��������</param>
        /// <param>ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public static object executeScalarTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // ���row��ֵ,�洢���̱����ʼ��.
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                
                // �������ֵ
                assignParameterValues(commandParameters, dataRow);
                
                return clsSql.executeScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeScalar(connection, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// ִ��ָ���������ݿ�����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <param>һ����Ч���������� object</param>
        /// <param>�洢��������</param>
        /// <param>ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public static object executeScalarTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // ���row��ֵ,�洢���̱����ʼ��.
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                
                // �������ֵ
                assignParameterValues(commandParameters, dataRow);
                
                return clsSql.executeScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeScalar(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion
            
        #region executeXmlReaderTypedParams ���ͻ�����(DataRow)
        /// <summary>
        /// ִ��ָ���������ݿ����Ӷ���Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����XmlReader���͵Ľ����.
        /// </summary>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�洢��������</param>
        /// <param>ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>����XmlReader���������.</returns>
        public static XmlReader executeXmlReaderTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // ���row��ֵ,�洢���̱����ʼ��.
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                
                // �������ֵ
                assignParameterValues(commandParameters, dataRow);
                
                return clsSql.executeXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeXmlReader(connection, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// ִ��ָ���������ݿ�����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����XmlReader���͵Ľ����.
        /// </summary>
        /// <param>һ����Ч���������� object</param>
        /// <param>�洢��������</param>
        /// <param>ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>����XmlReader���������.</returns>
        public static XmlReader executeXmlReaderTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // ���row��ֵ,�洢���̱����ʼ��.
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                
                // �������ֵ
                assignParameterValues(commandParameters, dataRow);
                
                return clsSql.executeXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeXmlReader(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion
    }
    
    /// <summary>
    /// clsSqlParameterCache�ṩ����洢���̲���,���ܹ�������ʱ�Ӵ洢������̽������.
    /// </summary>
    public sealed class clsSqlParameterCache
    {
        #region ˽�з���,�ֶ�,���캯��
        // ˽�й��캯��,��ֹ�౻ʵ����.
        private clsSqlParameterCache() {}
        // �������Ҫע��
        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());
        /// <summary>
        /// ̽������ʱ�Ĵ洢����,����SqlParameter��������.
        /// ��ʼ������ֵΪ DBNull.Value.
        /// </summary>
        /// <param>һ����Ч�����ݿ�����</param>
        /// <param>�洢��������</param>
        /// <param>�Ƿ��������ֵ����</param>
        /// <returns>����SqlParameter��������</returns>
        private static SqlParameter[] discoverSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            SqlCommand cmd = new SqlCommand(spName, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();
            // ����cmdָ���Ĵ洢���̵Ĳ�����Ϣ,����䵽cmd��Parameters��������.
            SqlCommandBuilder.DeriveParameters(cmd);
            connection.Close();
            // �������������ֵ����,���������е�ÿһ������ɾ��.
            if (!includeReturnValueParameter) 
            {
                cmd.Parameters.RemoveAt(0);
            }
                
            // ������������
            SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count];
            // ��cmd��Parameters���������Ƶ�discoveredParameters����.
            cmd.Parameters.CopyTo(discoveredParameters, 0);
            // ��ʼ������ֵΪ DBNull.Value.
            foreach (SqlParameter discoveredParameter in discoveredParameters)
            {
                discoveredParameter.Value = DBNull.Value;
            }
            return discoveredParameters;
        }
        
        /// <summary>
        /// SqlParameter�����������㿽��.
        /// </summary>
        /// <param>ԭʼ��������</param>
        /// <returns>����һ��ͬ���Ĳ�������</returns>
        private static SqlParameter[] cloneParameters(SqlParameter[] originalParameters)
        {
            SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];
            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
            }
            return clonedParameters;
        }
        #endregion ˽�з���,�ֶ�,���캯������
            
        #region ���淽��
        /// <summary>
        /// ׷�Ӳ������鵽����.
        /// </summary>
        /// <param>һ����Ч�����ݿ������ַ���</param>
        /// <param>�洢��������SQL���</param>
        /// <param>Ҫ����Ĳ�������</param>
        public static void cacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( commandText == null || commandText.Length == 0 ) throw new ArgumentNullException( "commandText" );
            string hashKey = connectionString + ":" + commandText;
            paramCache[hashKey] = commandParameters;
        }
        
        /// <summary>
        /// �ӻ����л�ȡ��������.
        /// </summary>
        /// <param>һ����Ч�����ݿ������ַ�</param>
        /// <param>�洢��������SQL���</param>
        /// <returns>��������</returns>
        public static SqlParameter[] getCachedParameterSet(string connectionString, string commandText)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( commandText == null || commandText.Length == 0 ) throw new ArgumentNullException( "commandText" );
            string hashKey = connectionString + ":" + commandText;
            SqlParameter[] cachedParameters = paramCache[hashKey] as SqlParameter[];
            if (cachedParameters == null)
            {            
                return null;
            }
            else
            {
                return cloneParameters(cachedParameters);
            }
        }
        #endregion ���淽������
            
        #region ����ָ���Ĵ洢���̵Ĳ�����
        /// <summary>
        /// ����ָ���Ĵ洢���̵Ĳ�����
        /// </summary>
        /// <remarks>
        /// �����������ѯ���ݿ�,������Ϣ�洢������.
        /// </remarks>
        /// <param>һ����Ч�����ݿ������ַ�</param>
        /// <param>�洢������</param>
        /// <returns>����SqlParameter��������</returns>
        public static SqlParameter[] getSpParameterSet(string connectionString, string spName)
        {
            return getSpParameterSet(connectionString, spName, false);
        }
        
        /// <summary>
        /// ����ָ���Ĵ洢���̵Ĳ�����
        /// </summary>
        /// <remarks>
        /// �����������ѯ���ݿ�,������Ϣ�洢������.
        /// </remarks>
        /// <param>һ����Ч�����ݿ������ַ�.</param>
        /// <param>�洢������</param>
        /// <param>�Ƿ��������ֵ����</param>
        /// <returns>����SqlParameter��������</returns>
        public static SqlParameter[] getSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                return getSpParameterSetInternal(connection, spName, includeReturnValueParameter);
            }
        }
        
        /// <summary>
        /// [�ڲ�]����ָ���Ĵ洢���̵Ĳ�����(ʹ�����Ӷ���).
        /// </summary>
        /// <remarks>
        /// �����������ѯ���ݿ�,������Ϣ�洢������.
        /// </remarks>
        /// <param>һ����Ч�����ݿ������ַ�</param>
        /// <param>�洢������</param>
        /// <returns>����SqlParameter��������</returns>
        internal static SqlParameter[] getSpParameterSet(SqlConnection connection, string spName)
        {
            return getSpParameterSet(connection, spName, false);
        }
        
        /// <summary>
        /// [�ڲ�]����ָ���Ĵ洢���̵Ĳ�����(ʹ�����Ӷ���)
        /// </summary>
        /// <remarks>
        /// �����������ѯ���ݿ�,������Ϣ�洢������.
        /// </remarks>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�洢������</param>
        /// <param>
        /// �Ƿ��������ֵ����
        /// </param>
        /// <returns>����SqlParameter��������</returns>
        internal static SqlParameter[] getSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            using (SqlConnection clonedConnection = (SqlConnection)((ICloneable)connection).Clone())
            {
                return getSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
            }
        }
        
        /// <summary>
        /// [˽��]����ָ���Ĵ洢���̵Ĳ�����(ʹ�����Ӷ���)
        /// </summary>
        /// <param>һ����Ч�����ݿ����Ӷ���</param>
        /// <param>�洢������</param>
        /// <param>�Ƿ��������ֵ����</param>
        /// <returns>����SqlParameter��������</returns>
        private static SqlParameter[] getSpParameterSetInternal(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            string hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter":"");
            SqlParameter[] cachedParameters;
            
            cachedParameters = paramCache[hashKey] as SqlParameter[];
            if (cachedParameters == null)
            {    
                SqlParameter[] spParameters = discoverSpParameterSet(connection, spName, includeReturnValueParameter);
                paramCache[hashKey] = spParameters;
                cachedParameters = spParameters;
            }
            
            return cloneParameters(cachedParameters);
        }
        
        #endregion ��������������
    }
}