using System;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
using System.Collections;

namespace Wells.Tools
{
    /// <summary>
    /// SqlServer数据访问帮助类
    /// </summary>
    public sealed class clsSql
    {
        #region 私有构造函数和方法
        private clsSql() {}
        /// <summary>
        /// 将SqlParameter参数数组(参数值)分配给SqlCommand命令.
        /// 这个方法将给任何一个参数分配DBNull.Value;
        /// 该操作将阻止默认值的使用.
        /// </summary>clsSqlclsSql
        /// <param>命令名</param>
        /// <param>SqlParameters数组</param>
        private static void attachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if( command == null ) throw new ArgumentNullException( "command" );
            if( commandParameters != null )
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if( p != null )
                    {
                        // 检查未分配值的输出参数,将其分配以DBNull.Value.
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
        /// 将DataRow类型的列值分配到SqlParameter参数数组.
        /// </summary>
        /// <param>要分配值的SqlParameter参数数组</param>
        /// <param>将要分配给存储过程参数的DataRow</param>
        private static void assignParameterValues(SqlParameter[] commandParameters, DataRow dataRow)
        {
            if ((commandParameters == null) || (dataRow == null)) 
            {
                return;
            }
            int i = 0;
            // 设置参数值
            foreach(SqlParameter commandParameter in commandParameters)
            {
                // 创建参数名称,如果不存在,只抛出一个异常.
                if( commandParameter.ParameterName == null || 
                    commandParameter.ParameterName.Length <= 1 )
                    throw new Exception( 
                        string.Format("请提供参数{0}一个有效的名称{1}.", i, commandParameter.ParameterName ) );
                // 从dataRow的表中获取为参数数组中数组名称的列的索引.
                // 如果存在和参数名称相同的列,则将列值赋给当前名称的参数.
                if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
                i++;
            }
        }
        
        /// <summary>
        /// 将一个对象数组分配给SqlParameter参数数组.
        /// </summary>
        /// <param>要分配值的SqlParameter参数数组</param>
        /// <param>将要分配给存储过程参数的对象数组</param>
        private static void assignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null)) 
            {
                return;
            }
            // 确保对象数组个数与参数个数匹配,如果不匹配,抛出一个异常.
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("参数值个数与参数不匹配.");
            }
            // 给参数赋值
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
        /// 预处理用户提供的命令,数据库连接/事务/命令类型/参数
        /// </summary>
        /// <param>要处理的SqlCommand</param>
        /// <param>数据库连接</param>
        /// <param>一个有效的事务或者是null值</param>
        /// <param>命令类型 (存储过程,命令文本, 其它.)</param>
        /// <param>存储过程名或都T-SQL命令文本</param>
        /// <param>和命令相关联的SqlParameter参数数组,如果没有参数为'null'</param>
        /// <param><c>true</c> 如果连接是打开的,则为true,其它情况下为false.</param>
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
            // 给命令分配一个数据库连接.
            command.Connection = connection;
            // 设置命令文本(存储过程名或SQL语句)
            command.CommandText = commandText;
            // 分配事务
            if (transaction != null)
            {
                if( transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
                command.Transaction = transaction;
            }
            // 设置命令类型.
            command.CommandType = commandType;
            // 分配命令参数
            if (commandParameters != null)
            {
                attachParameters(command, commandParameters);
            }
            return;
        }
        #endregion 私有构造函数和方法结束
            
        #region executeNonQuery命令
        /// <summary>
        /// 执行指定连接字符串,类型的SqlCommand.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  int result = executeNonQuery(connString, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param>一个有效的数据库连接字符串</param>
        /// <param>命令类型 (存储过程,命令文本, 其它.)</param>
        /// <param>存储过程名称或SQL语句</param>
        /// <returns>返回命令影响的行数</returns>
        public static int executeNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            return executeNonQuery(connectionString, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// 执行指定连接字符串,类型的SqlCommand.如果没有提供参数,不返回结果.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  int result = executeNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>一个有效的数据库连接字符串</param>
        /// <param>命令类型 (存储过程,命令文本, 其它.)</param>
        /// <param>存储过程名称或SQL语句</param>
        /// <param>SqlParameter参数数组</param>
        /// <returns>返回命令影响的行数</returns>
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
        /// 执行指定连接字符串的存储过程,将对象数组的值赋给存储过程参数,
        /// 此方法需要在参数缓存方法中探索参数并生成参数.
        /// </summary>
        /// <remarks>
        /// 这个方法没有提供访问输出参数和返回值.
        /// 示例:  
        ///  int result = executeNonQuery(connString, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param>一个有效的数据库连接字符串/param>
        /// <param>存储过程名称</param>
        /// <param>分配到存储过程输入参数的对象数组</param>
        /// <returns>返回受影响的行数</returns>
        public static int executeNonQuery(string connectionString, string spName, params object[] parameterValues)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // 如果存在参数值
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // 从探索存储过程参数(加载到缓存)并分配给存储过程参数数组.
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connectionString, spName);
                // 给存储过程参数赋值
                assignParameterValues(commandParameters, parameterValues);
                return executeNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                // 没有参数情况下
                return executeNonQuery(connectionString, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// 执行指定数据库连接对象的命令 
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  int result = executeNonQuery(conn, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>命令类型(存储过程,命令文本或其它.)</param>
        /// <param>存储过程名称或T-SQL语句</param>
        /// <returns>返回影响的行数</returns>
        public static int executeNonQuery(SqlConnection connection, CommandType commandType, string commandText)
        {
            return executeNonQuery(connection, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// 执行指定数据库连接对象的命令
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  int result = executeNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>命令类型(存储过程,命令文本或其它.)</param>
        /// <param>T存储过程名称或T-SQL语句</param>
        /// <param>SqlParamter参数数组</param>
        /// <returns>返回影响的行数</returns>
        public static int executeNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {    
            if( connection == null ) throw new ArgumentNullException( "connection" );
            // 创建SqlCommand命令,并进行预处理
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            prepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );
            
            // Finally, execute the command
            int retval = cmd.ExecuteNonQuery();
            
            // 清除参数,以便再次使用.
            cmd.Parameters.Clear();
            if( mustCloseConnection )
                connection.Close();
            return retval;
        }
        
        /// <summary>
        /// 执行指定数据库连接对象的命令,将对象数组的值赋给存储过程参数.
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输出参数和返回值
        /// 示例:  
        ///  int result = executeNonQuery(conn, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>存储过程名</param>
        /// <param>分配给存储过程输入参数的对象数组</param>
        /// <returns>返回影响的行数</returns>
        public static int executeNonQuery(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // 如果有参数值
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // 从缓存中加载存储过程参数
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                // 给存储过程分配参数值
                assignParameterValues(commandParameters, parameterValues);
                return executeNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                return executeNonQuery(connection, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// 执行带事务的SqlCommand.
        /// </summary>
        /// <remarks>
        /// 示例.:  
        ///  int result = executeNonQuery(trans, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>命令类型(存储过程,命令文本或其它.)</param>
        /// <param>存储过程名称或T-SQL语句</param>
        /// <returns>返回影响的行数/returns>
        public static int executeNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return executeNonQuery(transaction, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// 执行带事务的SqlCommand(指定参数).
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  int result = executeNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>命令类型(存储过程,命令文本或其它.)</param>
        /// <param>存储过程名称或T-SQL语句</param>
        /// <param>SqlParamter参数数组</param>
        /// <returns>返回影响的行数</returns>
        public static int executeNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            // 预处理
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            prepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );
                
            // 执行
            int retval = cmd.ExecuteNonQuery();
                
            // 清除参数集,以便再次使用.
            cmd.Parameters.Clear();
            return retval;
        }
        
        /// <summary>
        /// 执行带事务的SqlCommand(指定参数值).
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输出参数和返回值
        /// 示例:  
        ///  int result = executeNonQuery(conn, trans, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>存储过程名</param>
        /// <param>分配给存储过程输入参数的对象数组</param>
        /// <returns>返回受影响的行数</returns>
        public static int executeNonQuery(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // 如果有参数值
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                // 给存储过程参数赋值
                assignParameterValues(commandParameters, parameterValues);
                // 调用重载方法
                return executeNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                // 没有参数值
                return executeNonQuery(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion executeNonQuery方法结束
            
        #region executeDataset方法
        /// <summary>
        /// 执行指定数据库连接字符串的命令,返回DataSet.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  DataSet ds = executeDataset(connString, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param>一个有效的数据库连接字符串</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名称或T-SQL语句</param>
        /// <returns>返回一个包含结果集的DataSet</returns>
        public static DataSet executeDataset(string connectionString, CommandType commandType, string commandText)
        {
            return executeDataset(connectionString, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// 执行指定数据库连接字符串的命令,返回DataSet.
        /// </summary>
        /// <remarks>
        /// 示例: 
        ///  DataSet ds = executeDataset(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>一个有效的数据库连接字符串</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名称或T-SQL语句</param>
        /// <param>SqlParamters参数数组</param>
        /// <returns>返回一个包含结果集的DataSet</returns>
        public static DataSet executeDataset(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            // 创建并打开数据库连接对象,操作完成释放对象.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // 调用指定数据库连接字符串重载方法.
                return executeDataset(connection, commandType, commandText, commandParameters);
            }
        }
        
        /// <summary>
        /// 执行指定数据库连接字符串的命令,直接提供参数值,返回DataSet.
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输出参数和返回值.
        /// 示例: 
        ///  DataSet ds = executeDataset(connString, "GetOrders", 24, 36);
        /// </remarks>
        /// <param>一个有效的数据库连接字符串</param>
        /// <param>存储过程名</param>
        /// <param>分配给存储过程输入参数的对象数组</param>
        /// <returns>返回一个包含结果集的DataSet</returns>
        public static DataSet executeDataset(string connectionString, string spName, params object[] parameterValues)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // 从缓存中检索存储过程参数
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connectionString, spName);
                // 给存储过程参数分配值
                assignParameterValues(commandParameters, parameterValues);
                return executeDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                return executeDataset(connectionString, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// 执行指定数据库连接对象的命令,返回DataSet.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  DataSet ds = executeDataset(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名或T-SQL语句</param>
        /// <returns>返回一个包含结果集的DataSet</returns>
        public static DataSet executeDataset(SqlConnection connection, CommandType commandType, string commandText)
        {
            return executeDataset(connection, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// 执行指定数据库连接对象的命令,指定存储过程参数,返回DataSet.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  DataSet ds = executeDataset(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名或T-SQL语句</param>
        /// <param>SqlParamter参数数组</param>
        /// <returns>返回一个包含结果集的DataSet</returns>
        public static DataSet executeDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            // 预处理
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            prepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );
                
            // 创建SqlDataAdapter和DataSet.
            using( SqlDataAdapter da = new SqlDataAdapter(cmd) )
            {
                DataSet ds = new DataSet();
                // 填充DataSet.
                da.Fill(ds);
                
                cmd.Parameters.Clear();
                if( mustCloseConnection )
                    connection.Close();
                return ds;
            }    
        }
        
        /// <summary>
        /// 执行指定数据库连接对象的命令,指定参数值,返回DataSet.
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输入参数和返回值.
        /// 示例.:  
        ///  DataSet ds = executeDataset(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>存储过程名</param>
        /// <param>分配给存储过程输入参数的对象数组</param>
        /// <returns>返回一个包含结果集的DataSet</returns>
        public static DataSet executeDataset(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // 比缓存中加载存储过程参数
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                // 给存储过程参数分配值
                assignParameterValues(commandParameters, parameterValues);
                return executeDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                return executeDataset(connection, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// 执行指定事务的命令,返回DataSet.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  DataSet ds = executeDataset(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param>事务</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名或T-SQL语句</param>
        /// <returns>返回一个包含结果集的DataSet</returns>
        public static DataSet executeDataset(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return executeDataset(transaction, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// 执行指定事务的命令,指定参数,返回DataSet.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  DataSet ds = executeDataset(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>事务</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名或T-SQL语句</param>
        /// <param>SqlParamter参数数组</param>
        /// <returns>返回一个包含结果集的DataSet</returns>
        public static DataSet executeDataset(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            // 预处理
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            prepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );
                
            // 创建 DataAdapter & DataSet
            using( SqlDataAdapter da = new SqlDataAdapter(cmd) )
            {
                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.Parameters.Clear();
                return ds;
            }    
        }
        
        /// <summary>
        /// 执行指定事务的命令,指定参数值,返回DataSet.
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输入参数和返回值.
        /// 示例.:  
        ///  DataSet ds = executeDataset(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param>事务</param>
        /// <param>存储过程名</param>
        /// <param>分配给存储过程输入参数的对象数组</param>
        /// <returns>返回一个包含结果集的DataSet</returns>
        public static DataSet executeDataset(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // 从缓存中加载存储过程参数
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                // 给存储过程参数分配值
                assignParameterValues(commandParameters, parameterValues);
                return executeDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                return executeDataset(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion executeDataset数据集命令结束
        
        #region executeReader 数据阅读器
        /// <summary>
        /// 枚举,标识数据库连接是由clsSql提供还是由调用者提供
        /// </summary>
        private enum SqlConnectionOwnership    
        {
            /// <summary>由clsSql提供连接</summary>
            Internal, 
            /// <summary>由调用者提供连接</summary>
            External
        }
        
        /// <summary>
        /// 执行指定数据库连接对象的数据阅读器.
        /// </summary>
        /// <remarks>
        /// 如果是clsSql打开连接,当连接关闭DataReader也将关闭.
        /// 如果是调用都打开连接,DataReader由调用都管理.
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>一个有效的事务,或者为 'null'</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名或T-SQL语句</param>
        /// <param>SqlParameters参数数组,如果没有参数则为'null'</param>
        /// <param>标识数据库连接对象是由调用者提供还是由clsSql提供</param>
        /// <returns>返回包含结果集的SqlDataReader</returns>
        private static SqlDataReader executeReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
        {    
            if( connection == null ) throw new ArgumentNullException( "connection" );
            bool mustCloseConnection = false;
            // 创建命令
            SqlCommand cmd = new SqlCommand();
            try
            {
                prepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );
            
                // 创建数据阅读器
                SqlDataReader dataReader;
                if (connectionOwnership == SqlConnectionOwnership.External)
                {
                    dataReader = cmd.ExecuteReader();
                }
                else
                {
                    dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            
                // 清除参数,以便再次使用..
                // HACK: There is a problem here, the output parameter values are fletched 
                // when the reader is closed, so if the parameters are detached from the command
                // then the SqlReader can磘 set its values. 
                // When this happen, the parameters can磘 be used again in other command.
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
        /// 执行指定数据库连接字符串的数据阅读器.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  SqlDataReader dr = executeReader(connString, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param>一个有效的数据库连接字符串</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名或T-SQL语句</param>
        /// <returns>返回包含结果集的SqlDataReader</returns>
        public static SqlDataReader executeReader(string connectionString, CommandType commandType, string commandText)
        {
            return executeReader(connectionString, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// 执行指定数据库连接字符串的数据阅读器,指定参数.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  SqlDataReader dr = executeReader(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>一个有效的数据库连接字符串</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名或T-SQL语句</param>
        /// <param>SqlParamter参数数组(new SqlParameter("@prodid", 24))</param>
        /// <returns>返回包含结果集的SqlDataReader</returns>
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
        /// 执行指定数据库连接字符串的数据阅读器,指定参数值.
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输出参数和返回值参数.
        /// 示例:  
        ///  SqlDataReader dr = executeReader(connString, "GetOrders", 24, 36);
        /// </remarks>
        /// <param>一个有效的数据库连接字符串</param>
        /// <param>存储过程名</param>
        /// <param>分配给存储过程输入参数的对象数组</param>
        /// <returns>返回包含结果集的SqlDataReader</returns>
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
        /// 执行指定数据库连接对象的数据阅读器.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  SqlDataReader dr = executeReader(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名或T-SQL语句</param>
        /// <returns>返回包含结果集的SqlDataReader</returns>
        public static SqlDataReader executeReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            return executeReader(connection, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// [调用者方式]执行指定数据库连接对象的数据阅读器,指定参数.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  SqlDataReader dr = executeReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>SqlParamter参数数组</param>
        /// <returns>返回包含结果集的SqlDataReader</returns>
        public static SqlDataReader executeReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            return executeReader(connection, (SqlTransaction)null, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }
        
        /// <summary>
        /// [调用者方式]执行指定数据库连接对象的数据阅读器,指定参数值.
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输出参数和返回值参数.
        /// 示例:  
        ///  SqlDataReader dr = executeReader(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>T存储过程名</param>
        /// <param>分配给存储过程输入参数的对象数组</param>
        /// <returns>返回包含结果集的SqlDataReader</returns>
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
        /// [调用者方式]执行指定数据库事务的数据阅读器,指定参数值.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  SqlDataReader dr = executeReader(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param>一个有效的连接事务</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名称或T-SQL语句</param>
        /// <returns>返回包含结果集的SqlDataReader</returns>
        public static SqlDataReader executeReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return executeReader(transaction, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// [调用者方式]执行指定数据库事务的数据阅读器,指定参数.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///   SqlDataReader dr = executeReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>一个有效的连接事务</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名称或T-SQL语句</param>
        /// <param>分配给命令的SqlParamter参数数组</param>
        /// <returns>返回包含结果集的SqlDataReader</returns>
        public static SqlDataReader executeReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            return executeReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }
        
        /// <summary>
        /// [调用者方式]执行指定数据库事务的数据阅读器,指定参数值.
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输出参数和返回值参数.
        /// 
        /// 示例:  
        ///  SqlDataReader dr = executeReader(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param>一个有效的连接事务</param>
        /// <param>存储过程名称</param>
        /// <param>分配给存储过程输入参数的对象数组</param>
        /// <returns>返回包含结果集的SqlDataReader</returns>
        public static SqlDataReader executeReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // 如果有参数值
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                assignParameterValues(commandParameters, parameterValues);
                return executeReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                // 没有参数值
                return executeReader(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion executeReader数据阅读器
            
        #region executeScalar 返回结果集中的第一行第一列        
        /// <summary>
        /// 执行指定数据库连接字符串的命令,返回结果集中的第一行第一列.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  int orderCount = (int)executeScalar(connString, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param>一个有效的数据库连接字符串</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名称或T-SQL语句</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object executeScalar(string connectionString, CommandType commandType, string commandText)
        {
            // 执行参数为空的方法
            return executeScalar(connectionString, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// 执行指定数据库连接字符串的命令,指定参数,返回结果集中的第一行第一列.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  int orderCount = (int)executeScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>一个有效的数据库连接字符串</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名称或T-SQL语句</param>
        /// <param>分配给命令的SqlParamter参数数组</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object executeScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            // 创建并打开数据库连接对象,操作完成释放对象.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // 调用指定数据库连接字符串重载方法.
                return executeScalar(connection, commandType, commandText, commandParameters);
            }
        }
        
        /// <summary>
        /// 执行指定数据库连接字符串的命令,指定参数值,返回结果集中的第一行第一列.
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输出参数和返回值参数.
        /// 
        /// 示例:  
        ///  int orderCount = (int)executeScalar(connString, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param>一个有效的数据库连接字符串</param>
        /// <param>存储过程名称</param>
        /// <param>分配给存储过程输入参数的对象数组</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object executeScalar(string connectionString, string spName, params object[] parameterValues)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            
            // 如果有参数值
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connectionString, spName);
                // 给存储过程参数赋值
                assignParameterValues(commandParameters, parameterValues);
                // 调用重载方法
                return executeScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                // 没有参数值
                return executeScalar(connectionString, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// 执行指定数据库连接对象的命令,返回结果集中的第一行第一列.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  int orderCount = (int)executeScalar(conn, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名称或T-SQL语句</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object executeScalar(SqlConnection connection, CommandType commandType, string commandText)
        {
            // 执行参数为空的方法
            return executeScalar(connection, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// 执行指定数据库连接对象的命令,指定参数,返回结果集中的第一行第一列.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  int orderCount = (int)executeScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名称或T-SQL语句</param>
        /// <param>分配给命令的SqlParamter参数数组</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object executeScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            // 创建SqlCommand命令,并进行预处理
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            prepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );
                
            // 执行SqlCommand命令,并返回结果.
            object retval = cmd.ExecuteScalar();
                
            // 清除参数,以便再次使用.
            cmd.Parameters.Clear();
            if( mustCloseConnection )
                connection.Close();
            return retval;
        }
        
        /// <summary>
        /// 执行指定数据库连接对象的命令,指定参数值,返回结果集中的第一行第一列.
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输出参数和返回值参数.
        /// 
        /// 示例:  
        ///  int orderCount = (int)executeScalar(conn, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>存储过程名称</param>
        /// <param>分配给存储过程输入参数的对象数组</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object executeScalar(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // 如果有参数值
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                // 给存储过程参数赋值
                assignParameterValues(commandParameters, parameterValues);
                // 调用重载方法
                return executeScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                // 没有参数值
                return executeScalar(connection, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// 执行指定数据库事务的命令,返回结果集中的第一行第一列.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  int orderCount = (int)executeScalar(trans, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param>一个有效的连接事务</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名称或T-SQL语句</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object executeScalar(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // 执行参数为空的方法
            return executeScalar(transaction, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// 执行指定数据库事务的命令,指定参数,返回结果集中的第一行第一列.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  int orderCount = (int)executeScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>一个有效的连接事务</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名称或T-SQL语句</param>
        /// <param>分配给命令的SqlParamter参数数组</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object executeScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            // 创建SqlCommand命令,并进行预处理
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            prepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );
                
            // 执行SqlCommand命令,并返回结果.
            object retval = cmd.ExecuteScalar();
                
            // 清除参数,以便再次使用.
            cmd.Parameters.Clear();
            return retval;
        }
        
        /// <summary>
        /// 执行指定数据库事务的命令,指定参数值,返回结果集中的第一行第一列.
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输出参数和返回值参数.
        /// 
        /// 示例:  
        ///  int orderCount = (int)executeScalar(trans, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param>一个有效的连接事务</param>
        /// <param>存储过程名称</param>
        /// <param>分配给存储过程输入参数的对象数组</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object executeScalar(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // 如果有参数值
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // PPull the parameters for this stored procedure from the parameter cache ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                // 给存储过程参数赋值
                assignParameterValues(commandParameters, parameterValues);
                // 调用重载方法
                return executeScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                // 没有参数值
                return executeScalar(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion executeScalar 
               
        #region executeXmlReader XML阅读器
        /// <summary>
        /// 执行指定数据库连接对象的SqlCommand命令,并产生一个XmlReader对象做为结果集返回.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  XmlReader r = executeXmlReader(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名称或T-SQL语句 using "FOR XML AUTO"</param>
        /// <returns>返回XmlReader结果集对象.</returns>
        public static XmlReader executeXmlReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            // 执行参数为空的方法
            return executeXmlReader(connection, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// 执行指定数据库连接对象的SqlCommand命令,并产生一个XmlReader对象做为结果集返回,指定参数.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  XmlReader r = executeXmlReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名称或T-SQL语句 using "FOR XML AUTO"</param>
        /// <param>分配给命令的SqlParamter参数数组</param>
        /// <returns>返回XmlReader结果集对象.</returns>
        public static XmlReader executeXmlReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            bool mustCloseConnection = false;
            // 创建SqlCommand命令,并进行预处理
            SqlCommand cmd = new SqlCommand();
            try
            {
                prepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );
            
                // 执行命令
                XmlReader retval = cmd.ExecuteXmlReader();
            
                // 清除参数,以便再次使用.
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
        /// 执行指定数据库连接对象的SqlCommand命令,并产生一个XmlReader对象做为结果集返回,指定参数值.
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输出参数和返回值参数.
        /// 
        /// 示例:  
        ///  XmlReader r = executeXmlReader(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>存储过程名称 using "FOR XML AUTO"</param>
        /// <param>分配给存储过程输入参数的对象数组</param>
        /// <returns>返回XmlReader结果集对象.</returns>
        public static XmlReader executeXmlReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // 如果有参数值
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                // 给存储过程参数赋值
                assignParameterValues(commandParameters, parameterValues);
                // 调用重载方法
                return executeXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                // 没有参数值
                return executeXmlReader(connection, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// 执行指定数据库事务的SqlCommand命令,并产生一个XmlReader对象做为结果集返回.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  XmlReader r = executeXmlReader(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param>一个有效的连接事务</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名称或T-SQL语句 using "FOR XML AUTO"</param>
        /// <returns>返回XmlReader结果集对象.</returns>
        public static XmlReader executeXmlReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // 执行参数为空的方法
            return executeXmlReader(transaction, commandType, commandText, (SqlParameter[])null);
        }
        
        /// <summary>
        /// 执行指定数据库事务的SqlCommand命令,并产生一个XmlReader对象做为结果集返回,指定参数.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  XmlReader r = executeXmlReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>一个有效的连接事务</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名称或T-SQL语句 using "FOR XML AUTO"</param>
        /// <param>分配给命令的SqlParamter参数数组</param>
        /// <returns>返回XmlReader结果集对象.</returns>
        public static XmlReader executeXmlReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            // 创建SqlCommand命令,并进行预处理
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            prepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );
            
            // 执行命令
            XmlReader retval = cmd.ExecuteXmlReader();
            
            // 清除参数,以便再次使用.
            cmd.Parameters.Clear();
            return retval;            
        }
        
        /// <summary>
        /// 执行指定数据库事务的SqlCommand命令,并产生一个XmlReader对象做为结果集返回,指定参数值.
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输出参数和返回值参数.
        /// 
        /// 示例:  
        ///  XmlReader r = executeXmlReader(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param>一个有效的连接事务</param>
        /// <param>存储过程名称</param>
        /// <param>分配给存储过程输入参数的对象数组</param>
        /// <returns>返回一个包含结果集的DataSet.</returns>
        public static XmlReader executeXmlReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // 如果有参数值
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                // 给存储过程参数赋值
                assignParameterValues(commandParameters, parameterValues);
                // 调用重载方法
                return executeXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else 
            {
                // 没有参数值
                return executeXmlReader(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion executeXmlReader 阅读器结束
            
        #region FillDataset 填充数据集
        /// <summary>
        /// 执行指定数据库连接字符串的命令,映射数据表并填充数据集.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param>一个有效的数据库连接字符串</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名称或T-SQL语句</param>
        /// <param>要填充结果集的DataSet实例</param>
        /// <param>表映射的数据表数组
        /// 用户定义的表名 (可有是实际的表名.)</param>
        public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( dataSet == null ) throw new ArgumentNullException( "dataSet" );
            
            // 创建并打开数据库连接对象,操作完成释放对象.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // 调用指定数据库连接字符串重载方法.
                FillDataset(connection, commandType, commandText, dataSet, tableNames);
            }
        }
        
        /// <summary>
        /// 执行指定数据库连接字符串的命令,映射数据表并填充数据集.指定命令参数.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>一个有效的数据库连接字符串</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名称或T-SQL语句</param>
        /// <param>分配给命令的SqlParamter参数数组</param>
        /// <param>要填充结果集的DataSet实例</param>
        /// <param>表映射的数据表数组
        /// 用户定义的表名 (可有是实际的表名.)
        /// </param>
        public static void FillDataset(string connectionString, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( dataSet == null ) throw new ArgumentNullException( "dataSet" );
            // 创建并打开数据库连接对象,操作完成释放对象.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // 调用指定数据库连接字符串重载方法.
                FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
            }
        }
        
        /// <summary>
        /// 执行指定数据库连接字符串的命令,映射数据表并填充数据集,指定存储过程参数值.
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输出参数和返回值参数.
        /// 
        /// 示例:  
        ///  FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, 24);
        /// </remarks>
        /// <param>一个有效的数据库连接字符串</param>
        /// <param>存储过程名称</param>
        /// <param>要填充结果集的DataSet实例</param>
        /// <param>表映射的数据表数组
        /// 用户定义的表名 (可有是实际的表名.)
        /// </param>    
        /// <param>分配给存储过程输入参数的对象数组</param>
        public static void FillDataset(string connectionString, string spName,
            DataSet dataSet, string[] tableNames,
            params object[] parameterValues)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( dataSet == null ) throw new ArgumentNullException( "dataSet" );
            // 创建并打开数据库连接对象,操作完成释放对象.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // 调用指定数据库连接字符串重载方法.
                FillDataset (connection, spName, dataSet, tableNames, parameterValues);
            }
        }
        
        /// <summary>
        /// 执行指定数据库连接对象的命令,映射数据表并填充数据集.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名称或T-SQL语句</param>
        /// <param>要填充结果集的DataSet实例</param>
        /// <param>表映射的数据表数组
        /// 用户定义的表名 (可有是实际的表名.)
        /// </param>    
        public static void FillDataset(SqlConnection connection, CommandType commandType, 
            string commandText, DataSet dataSet, string[] tableNames)
        {
            FillDataset(connection, commandType, commandText, dataSet, tableNames, null);
        }
        
        /// <summary>
        /// 执行指定数据库连接对象的命令,映射数据表并填充数据集,指定参数.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名称或T-SQL语句</param>
        /// <param>要填充结果集的DataSet实例</param>
        /// <param>表映射的数据表数组
        /// 用户定义的表名 (可有是实际的表名.)
        /// </param>
        /// <param>分配给命令的SqlParamter参数数组</param>
        public static void FillDataset(SqlConnection connection, CommandType commandType, 
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            FillDataset(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
        }
        
        /// <summary>
        /// 执行指定数据库连接对象的命令,映射数据表并填充数据集,指定存储过程参数值.
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输出参数和返回值参数.
        /// 
        /// 示例:  
        ///  FillDataset(conn, "GetOrders", ds, new string[] {"orders"}, 24, 36);
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>存储过程名称</param>
        /// <param>要填充结果集的DataSet实例</param>
        /// <param>表映射的数据表数组
        /// 用户定义的表名 (可有是实际的表名.)
        /// </param>
        /// <param>分配给存储过程输入参数的对象数组</param>
        public static void FillDataset(SqlConnection connection, string spName, 
            DataSet dataSet, string[] tableNames,
            params object[] parameterValues)
        {
            if ( connection == null ) throw new ArgumentNullException( "connection" );
            if (dataSet == null ) throw new ArgumentNullException( "dataSet" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // 如果有参数值
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                // 给存储过程参数赋值
                assignParameterValues(commandParameters, parameterValues);
                // 调用重载方法
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
            }
            else 
            {
                // 没有参数值
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }    
        }
        
        /// <summary>
        /// 执行指定数据库事务的命令,映射数据表并填充数据集.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param>一个有效的连接事务</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名称或T-SQL语句</param>
        /// <param>要填充结果集的DataSet实例</param>
        /// <param>表映射的数据表数组
        /// 用户定义的表名 (可有是实际的表名.)
        /// </param>
        public static void FillDataset(SqlTransaction transaction, CommandType commandType, 
            string commandText,
            DataSet dataSet, string[] tableNames)
        {
            FillDataset (transaction, commandType, commandText, dataSet, tableNames, null);    
        }
        
        /// <summary>
        /// 执行指定数据库事务的命令,映射数据表并填充数据集,指定参数.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>一个有效的连接事务</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名称或T-SQL语句</param>
        /// <param>要填充结果集的DataSet实例</param>
        /// <param>表映射的数据表数组
        /// 用户定义的表名 (可有是实际的表名.)
        /// </param>
        /// <param>分配给命令的SqlParamter参数数组</param>
        public static void FillDataset(SqlTransaction transaction, CommandType commandType, 
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);
        }
        
        /// <summary>
        /// 执行指定数据库事务的命令,映射数据表并填充数据集,指定存储过程参数值.
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输出参数和返回值参数.
        /// 
        /// 示例:  
        ///  FillDataset(trans, "GetOrders", ds, new string[]{"orders"}, 24, 36);
        /// </remarks>
        /// <param>一个有效的连接事务</param>
        /// <param>存储过程名称</param>
        /// <param>要填充结果集的DataSet实例</param>
        /// <param>表映射的数据表数组
        /// 用户定义的表名 (可有是实际的表名.)
        /// </param>
        /// <param>分配给存储过程输入参数的对象数组</param>
        public static void FillDataset(SqlTransaction transaction, string spName,
            DataSet dataSet, string[] tableNames,
            params object[] parameterValues) 
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( dataSet == null ) throw new ArgumentNullException( "dataSet" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // 如果有参数值
            if ((parameterValues != null) && (parameterValues.Length > 0)) 
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                // 给存储过程参数赋值
                assignParameterValues(commandParameters, parameterValues);
                // 调用重载方法
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
            }
            else 
            {
                // 没有参数值
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }    
        }
        
        /// <summary>
        /// [私有方法][内部调用]执行指定数据库连接对象/事务的命令,映射数据表并填充数据集,DataSet/TableNames/SqlParameters.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  FillDataset(conn, trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>一个有效的连接事务</param>
        /// <param>命令类型 (存储过程,命令文本或其它)</param>
        /// <param>存储过程名称或T-SQL语句</param>
        /// <param>要填充结果集的DataSet实例</param>
        /// <param>表映射的数据表数组
        /// 用户定义的表名 (可有是实际的表名.)
        /// </param>
        /// <param>分配给命令的SqlParamter参数数组</param>
        private static void FillDataset(SqlConnection connection, SqlTransaction transaction, CommandType commandType, 
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( dataSet == null ) throw new ArgumentNullException( "dataSet" );
            // 创建SqlCommand命令,并进行预处理
            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            prepareCommand(command, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );
                
            // 执行命令
            using( SqlDataAdapter dataAdapter = new SqlDataAdapter(command) )
            {
                
                // 追加表映射
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
                
                // 填充数据集使用默认表名称
                dataAdapter.Fill(dataSet);
                // 清除参数,以便再次使用.
                command.Parameters.Clear();
            }
            if( mustCloseConnection )
                connection.Close();
        }
        #endregion
        
        #region UpdateDataset 更新数据集
        /// <summary>
        /// 执行数据集更新到数据库,指定inserted, updated, or deleted命令.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  UpdateDataset(conn, insertCommand, deleteCommand, updateCommand, dataSet, "Order");
        /// </remarks>
        /// <param>[追加记录]一个有效的T-SQL语句或存储过程</param>
        /// <param>[删除记录]一个有效的T-SQL语句或存储过程</param>
        /// <param>[更新记录]一个有效的T-SQL语句或存储过程</param>
        /// <param>要更新到数据库的DataSet</param>
        /// <param>要更新到数据库的DataTable</param>
        public static void UpdateDataset(SqlCommand insertCommand, SqlCommand deleteCommand, SqlCommand updateCommand, DataSet dataSet, string tableName)
        {
            if( insertCommand == null ) throw new ArgumentNullException( "insertCommand" );
            if( deleteCommand == null ) throw new ArgumentNullException( "deleteCommand" );
            if( updateCommand == null ) throw new ArgumentNullException( "updateCommand" );
            if( tableName == null || tableName.Length == 0 ) throw new ArgumentNullException( "tableName" ); 
            // 创建SqlDataAdapter,当操作完成后释放.
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter())
            {
                // 设置数据适配器命令
                dataAdapter.UpdateCommand = updateCommand;
                dataAdapter.InsertCommand = insertCommand;
                dataAdapter.DeleteCommand = deleteCommand;
                // 更新数据集改变到数据库
                dataAdapter.Update (dataSet, tableName); 
                // 提交所有改变到数据集.
                dataSet.AcceptChanges();
            }
        }
        #endregion
            
        #region CreateCommand 创建一条SqlCommand命令
        /// <summary>
        /// 创建SqlCommand命令,指定数据库连接对象,存储过程名和参数.
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  SqlCommand command = CreateCommand(conn, "AddCustomer", "CustomerID", "CustomerName");
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>存储过程名称</param>
        /// <param>源表的列名称数组</param>
        /// <returns>返回SqlCommand命令</returns>
        public static SqlCommand CreateCommand(SqlConnection connection, string spName, params string[] sourceColumns) 
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // 创建命令
            SqlCommand cmd = new SqlCommand( spName, connection );
            cmd.CommandType = CommandType.StoredProcedure;
            // 如果有参数值
            if ((sourceColumns != null) && (sourceColumns.Length > 0)) 
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                // 将源表的列到映射到DataSet命令中.
                for (int index=0; index < sourceColumns.Length; index++)
                    commandParameters[index].SourceColumn = sourceColumns[index];
                // Attach the discovered parameters to the SqlCommand object
                attachParameters (cmd, commandParameters);
            }
            return cmd;
        }
        #endregion
            
        #region executeNonQueryTypedParams 类型化参数(DataRow)
        /// <summary>
        /// 执行指定连接数据库连接字符串的存储过程,使用DataRow做为参数值,返回受影响的行数.
        /// </summary>
        /// <param>一个有效的数据库连接字符串</param>
        /// <param>存储过程名称</param>
        /// <param>使用DataRow作为参数值</param>
        /// <returns>返回影响的行数</returns>
        public static int executeNonQueryTypedParams(String connectionString, String spName, DataRow dataRow)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            
            // 如果row有值,存储过程必须初始化.
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connectionString, spName);
                
                // 分配参数值
                assignParameterValues(commandParameters, dataRow);
                                
                return clsSql.executeNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeNonQuery(connectionString, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// 执行指定连接数据库连接对象的存储过程,使用DataRow做为参数值,返回受影响的行数.
        /// </summary>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>存储过程名称</param>
        /// <param>使用DataRow作为参数值</param>
        /// <returns>返回影响的行数</returns>
        public static int executeNonQueryTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // 如果row有值,存储过程必须初始化.
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                
                // 分配参数值
                assignParameterValues(commandParameters, dataRow);
                                
                return clsSql.executeNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeNonQuery(connection, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// 执行指定连接数据库事物的存储过程,使用DataRow做为参数值,返回受影响的行数.
        /// </summary>
        /// <param>一个有效的连接事务 object</param>
        /// <param>存储过程名称</param>
        /// <param>使用DataRow作为参数值</param>
        /// <returns>返回影响的行数</returns>
        public static int executeNonQueryTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // Sf the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                
                // 分配参数值
                assignParameterValues(commandParameters, dataRow);
                                
                return clsSql.executeNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeNonQuery(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion
            
        #region executeDatasetTypedParams 类型化参数(DataRow)
        /// <summary>
        /// 执行指定连接数据库连接字符串的存储过程,使用DataRow做为参数值,返回DataSet.
        /// </summary>
        /// <param>一个有效的数据库连接字符串</param>
        /// <param>存储过程名称</param>
        /// <param>使用DataRow作为参数值</param>
        /// <returns>返回一个包含结果集的DataSet.</returns>
        public static DataSet executeDatasetTypedParams(string connectionString, String spName, DataRow dataRow)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            //如果row有值,存储过程必须初始化.
            if ( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connectionString, spName);
                
                // 分配参数值
                assignParameterValues(commandParameters, dataRow);
                
                return clsSql.executeDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeDataset(connectionString, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// 执行指定连接数据库连接对象的存储过程,使用DataRow做为参数值,返回DataSet.
        /// </summary>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>存储过程名称</param>
        /// <param>使用DataRow作为参数值</param>
        /// <returns>返回一个包含结果集的DataSet.</returns>
        /// 
        public static DataSet executeDatasetTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // 如果row有值,存储过程必须初始化.
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                
                // 分配参数值
                assignParameterValues(commandParameters, dataRow);
                
                return clsSql.executeDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeDataset(connection, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// 执行指定连接数据库事务的存储过程,使用DataRow做为参数值,返回DataSet.
        /// </summary>
        /// <param>一个有效的连接事务 object</param>
        /// <param>存储过程名称</param>
        /// <param>使用DataRow作为参数值</param>
        /// <returns>返回一个包含结果集的DataSet.</returns>
        public static DataSet executeDatasetTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // 如果row有值,存储过程必须初始化.
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                
                // 分配参数值
                assignParameterValues(commandParameters, dataRow);
                
                return clsSql.executeDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeDataset(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion
            
        #region executeReaderTypedParams 类型化参数(DataRow)
        /// <summary>
        /// 执行指定连接数据库连接字符串的存储过程,使用DataRow做为参数值,返回DataReader.
        /// </summary>
        /// <param>一个有效的数据库连接字符串</param>
        /// <param>存储过程名称</param>
        /// <param>使用DataRow作为参数值</param>
        /// <returns>返回包含结果集的SqlDataReader</returns>
        public static SqlDataReader executeReaderTypedParams(String connectionString, String spName, DataRow dataRow)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            
            // 如果row有值,存储过程必须初始化.
            if ( dataRow != null && dataRow.ItemArray.Length > 0 )
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connectionString, spName);
                
                // 分配参数值
                assignParameterValues(commandParameters, dataRow);
                
                return clsSql.executeReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeReader(connectionString, CommandType.StoredProcedure, spName);
            }
        }
                
        /// <summary>
        /// 执行指定连接数据库连接对象的存储过程,使用DataRow做为参数值,返回DataReader.
        /// </summary>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>存储过程名称</param>
        /// <param>使用DataRow作为参数值</param>
        /// <returns>返回包含结果集的SqlDataReader</returns>
        public static SqlDataReader executeReaderTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // 如果row有值,存储过程必须初始化.
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                
                // 分配参数值
                assignParameterValues(commandParameters, dataRow);
                
                return clsSql.executeReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeReader(connection, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// 执行指定连接数据库事物的存储过程,使用DataRow做为参数值,返回DataReader.
        /// </summary>
        /// <param>一个有效的连接事务 object</param>
        /// <param>存储过程名称</param>
        /// <param>使用DataRow作为参数值</param>
        /// <returns>返回包含结果集的SqlDataReader</returns>
        public static SqlDataReader executeReaderTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // 如果row有值,存储过程必须初始化.
            if( dataRow != null && dataRow.ItemArray.Length > 0 )
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                
                // 分配参数值
                assignParameterValues(commandParameters, dataRow);
                
                return clsSql.executeReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeReader(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion
            
        #region executeScalarTypedParams 类型化参数(DataRow)
        /// <summary>
        /// 执行指定连接数据库连接字符串的存储过程,使用DataRow做为参数值,返回结果集中的第一行第一列.
        /// </summary>
        /// <param>一个有效的数据库连接字符串</param>
        /// <param>存储过程名称</param>
        /// <param>使用DataRow作为参数值</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object executeScalarTypedParams(String connectionString, String spName, DataRow dataRow)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            
            // 如果row有值,存储过程必须初始化.
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connectionString, spName);
                
                // 分配参数值
                assignParameterValues(commandParameters, dataRow);
                
                return clsSql.executeScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeScalar(connectionString, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// 执行指定连接数据库连接对象的存储过程,使用DataRow做为参数值,返回结果集中的第一行第一列.
        /// </summary>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>存储过程名称</param>
        /// <param>使用DataRow作为参数值</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object executeScalarTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // 如果row有值,存储过程必须初始化.
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                
                // 分配参数值
                assignParameterValues(commandParameters, dataRow);
                
                return clsSql.executeScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeScalar(connection, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// 执行指定连接数据库事务的存储过程,使用DataRow做为参数值,返回结果集中的第一行第一列.
        /// </summary>
        /// <param>一个有效的连接事务 object</param>
        /// <param>存储过程名称</param>
        /// <param>使用DataRow作为参数值</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object executeScalarTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // 如果row有值,存储过程必须初始化.
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                
                // 分配参数值
                assignParameterValues(commandParameters, dataRow);
                
                return clsSql.executeScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeScalar(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion
            
        #region executeXmlReaderTypedParams 类型化参数(DataRow)
        /// <summary>
        /// 执行指定连接数据库连接对象的存储过程,使用DataRow做为参数值,返回XmlReader类型的结果集.
        /// </summary>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>存储过程名称</param>
        /// <param>使用DataRow作为参数值</param>
        /// <returns>返回XmlReader结果集对象.</returns>
        public static XmlReader executeXmlReaderTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // 如果row有值,存储过程必须初始化.
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(connection, spName);
                
                // 分配参数值
                assignParameterValues(commandParameters, dataRow);
                
                return clsSql.executeXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return clsSql.executeXmlReader(connection, CommandType.StoredProcedure, spName);
            }
        }
        
        /// <summary>
        /// 执行指定连接数据库事务的存储过程,使用DataRow做为参数值,返回XmlReader类型的结果集.
        /// </summary>
        /// <param>一个有效的连接事务 object</param>
        /// <param>存储过程名称</param>
        /// <param>使用DataRow作为参数值</param>
        /// <returns>返回XmlReader结果集对象.</returns>
        public static XmlReader executeXmlReaderTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            // 如果row有值,存储过程必须初始化.
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. ()
                SqlParameter[] commandParameters = clsSqlParameterCache.getSpParameterSet(transaction.Connection, spName);
                
                // 分配参数值
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
    /// clsSqlParameterCache提供缓存存储过程参数,并能够在运行时从存储过程中探索参数.
    /// </summary>
    public sealed class clsSqlParameterCache
    {
        #region 私有方法,字段,构造函数
        // 私有构造函数,妨止类被实例化.
        private clsSqlParameterCache() {}
        // 这个方法要注意
        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());
        /// <summary>
        /// 探索运行时的存储过程,返回SqlParameter参数数组.
        /// 初始化参数值为 DBNull.Value.
        /// </summary>
        /// <param>一个有效的数据库连接</param>
        /// <param>存储过程名称</param>
        /// <param>是否包含返回值参数</param>
        /// <returns>返回SqlParameter参数数组</returns>
        private static SqlParameter[] discoverSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );
            SqlCommand cmd = new SqlCommand(spName, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();
            // 检索cmd指定的存储过程的参数信息,并填充到cmd的Parameters参数集中.
            SqlCommandBuilder.DeriveParameters(cmd);
            connection.Close();
            // 如果不包含返回值参数,将参数集中的每一个参数删除.
            if (!includeReturnValueParameter) 
            {
                cmd.Parameters.RemoveAt(0);
            }
                
            // 创建参数数组
            SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count];
            // 将cmd的Parameters参数集复制到discoveredParameters数组.
            cmd.Parameters.CopyTo(discoveredParameters, 0);
            // 初始化参数值为 DBNull.Value.
            foreach (SqlParameter discoveredParameter in discoveredParameters)
            {
                discoveredParameter.Value = DBNull.Value;
            }
            return discoveredParameters;
        }
        
        /// <summary>
        /// SqlParameter参数数组的深层拷贝.
        /// </summary>
        /// <param>原始参数数组</param>
        /// <returns>返回一个同样的参数数组</returns>
        private static SqlParameter[] cloneParameters(SqlParameter[] originalParameters)
        {
            SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];
            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
            }
            return clonedParameters;
        }
        #endregion 私有方法,字段,构造函数结束
            
        #region 缓存方法
        /// <summary>
        /// 追加参数数组到缓存.
        /// </summary>
        /// <param>一个有效的数据库连接字符串</param>
        /// <param>存储过程名或SQL语句</param>
        /// <param>要缓存的参数数组</param>
        public static void cacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( commandText == null || commandText.Length == 0 ) throw new ArgumentNullException( "commandText" );
            string hashKey = connectionString + ":" + commandText;
            paramCache[hashKey] = commandParameters;
        }
        
        /// <summary>
        /// 从缓存中获取参数数组.
        /// </summary>
        /// <param>一个有效的数据库连接字符</param>
        /// <param>存储过程名或SQL语句</param>
        /// <returns>参数数组</returns>
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
        #endregion 缓存方法结束
            
        #region 检索指定的存储过程的参数集
        /// <summary>
        /// 返回指定的存储过程的参数集
        /// </summary>
        /// <remarks>
        /// 这个方法将查询数据库,并将信息存储到缓存.
        /// </remarks>
        /// <param>一个有效的数据库连接字符</param>
        /// <param>存储过程名</param>
        /// <returns>返回SqlParameter参数数组</returns>
        public static SqlParameter[] getSpParameterSet(string connectionString, string spName)
        {
            return getSpParameterSet(connectionString, spName, false);
        }
        
        /// <summary>
        /// 返回指定的存储过程的参数集
        /// </summary>
        /// <remarks>
        /// 这个方法将查询数据库,并将信息存储到缓存.
        /// </remarks>
        /// <param>一个有效的数据库连接字符.</param>
        /// <param>存储过程名</param>
        /// <param>是否包含返回值参数</param>
        /// <returns>返回SqlParameter参数数组</returns>
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
        /// [内部]返回指定的存储过程的参数集(使用连接对象).
        /// </summary>
        /// <remarks>
        /// 这个方法将查询数据库,并将信息存储到缓存.
        /// </remarks>
        /// <param>一个有效的数据库连接字符</param>
        /// <param>存储过程名</param>
        /// <returns>返回SqlParameter参数数组</returns>
        internal static SqlParameter[] getSpParameterSet(SqlConnection connection, string spName)
        {
            return getSpParameterSet(connection, spName, false);
        }
        
        /// <summary>
        /// [内部]返回指定的存储过程的参数集(使用连接对象)
        /// </summary>
        /// <remarks>
        /// 这个方法将查询数据库,并将信息存储到缓存.
        /// </remarks>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>存储过程名</param>
        /// <param>
        /// 是否包含返回值参数
        /// </param>
        /// <returns>返回SqlParameter参数数组</returns>
        internal static SqlParameter[] getSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            using (SqlConnection clonedConnection = (SqlConnection)((ICloneable)connection).Clone())
            {
                return getSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
            }
        }
        
        /// <summary>
        /// [私有]返回指定的存储过程的参数集(使用连接对象)
        /// </summary>
        /// <param>一个有效的数据库连接对象</param>
        /// <param>存储过程名</param>
        /// <param>是否包含返回值参数</param>
        /// <returns>返回SqlParameter参数数组</returns>
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
        
        #endregion 参数集检索结束
    }
}