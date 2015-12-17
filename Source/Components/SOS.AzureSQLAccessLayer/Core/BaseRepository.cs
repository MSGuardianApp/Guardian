using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SOS.ConfigManager;

namespace SOS.AzureSQLAccessLayer
{
    public class BaseRepository
    {
        protected readonly int maxRetries;
        protected readonly int retryDuration;
        protected readonly int sqlCommandTimeout;
        public BaseRepository()
        {
            this.maxRetries = 3;
            this.retryDuration = 100; //TO DO : pull it from config
            this.sqlCommandTimeout = 600;
        }

        #region GetCommand
        public SqlCommand GetCommand(string commandText,SqlConnection connection,CommandType cmdType = CommandType.StoredProcedure)
        {
            SqlCommand cmd = new SqlCommand(commandText,connection);
            cmd.CommandType = cmdType;
            cmd.CommandTimeout = sqlCommandTimeout;
            return cmd;
        }
        #endregion

        #region Connection
        public SqlConnection GetConnection()
        {
            return (new SqlConnection(Config.AzureSQLConnectionString));
        }

        /// <summary>
        /// Get SQL connection
        /// </summary>
        /// <returns>SQL Connection</returns>
        private SqlConnection GetConnection(string connectionString)
        {
            SqlConnection sqlcon = new SqlConnection();
            sqlcon.ConnectionString = connectionString;
            return sqlcon;
        }

        #endregion

        #region Data Access
        /// <summary>
        /// Get data set for Stored Procedure
        /// </summary>
        /// <param name="SPName">Stored procedure name</param>
        /// <param name="parameters">Collection of parameters</param>
        /// <returns></returns>
        protected DataSet GetDataset(string SPName, List<Parameter> parameters, string connectionString = null)
        {
            using (SqlConnection sqlcon = (connectionString == null) ? GetConnection() : GetConnection(connectionString))
            {
                try
                {
                    using (SqlCommand sqlcmd = new SqlCommand(SPName, sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandTimeout = sqlCommandTimeout;
                        SqlParameter parameter;
                        if (parameters != null)
                        {
                            foreach (Parameter param in parameters)
                            {
                                parameter = new SqlParameter();
                                parameter.ParameterName = param.ParamName.ToString();
                                if (param.ParamValue != null)
                                    parameter.Value = param.ParamValue.ToString();
                                else
                                    parameter.Value = DBNull.Value;
                                parameter.SqlDbType = param.DbType;
                                sqlcmd.Parameters.Add(parameter);
                            }
                        }
                        using (SqlDataAdapter da = new SqlDataAdapter(sqlcmd))
                        {
                            sqlcon.Open();
                            DataSet dataset = new DataSet();
                            da.Fill(dataset);
                            return dataset;
                        }
                    }
                }
                finally
                {
                    if (sqlcon.State == ConnectionState.Open)
                        sqlcon.Close();
                }
            }
        }

        /// <summary>
        /// Save the DB entity based on SP name and parameters
        /// </summary>
        /// <param name="SPName">Storedprocedure name</param>
        /// <param name="parameters">Collection of parameters</param>
        /// <returns></returns>
        protected int SaveEntity(string SPName, List<Parameter> parameters)
        {
            using (SqlConnection sqlcon = GetConnection())
            {
                try
                {
                    using (SqlCommand sqlcmd = new SqlCommand(SPName, sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandTimeout = sqlCommandTimeout;
                        SqlParameter parameter;
                        if (parameters != null)
                        {
                            foreach (Parameter param in parameters)
                            {
                                parameter = new SqlParameter();
                                parameter.ParameterName = param.ParamName.ToString();
                                if (param.ParamValue != null)
                                    parameter.Value = param.ParamValue.ToString();
                                else
                                    parameter.Value = DBNull.Value;
                                parameter.SqlDbType = param.DbType;
                                sqlcmd.Parameters.Add(parameter);
                            }
                        }
                        sqlcon.Open();
                        return Convert.ToInt32(sqlcmd.ExecuteScalar());
                    }
                }
                finally
                {
                    if (sqlcon.State == ConnectionState.Open)
                        sqlcon.Close();
                }
            }
        }

        #endregion

    }
}
