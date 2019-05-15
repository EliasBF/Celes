using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Npgsql;
using Dapper;

namespace Celes
{
    public class ConnectionParametersException : Exception
    {
        public ConnectionParametersException(string message) : base(message) {}
    }

    public class DatabaseProvider : IDisposable
    {
        private IDbConnection Connection;

        public DatabaseProvider(ConnectionParameters connectionParameters)
        {
            ConnectDatabase(connectionParameters);
        }

        private void ConnectDatabase(ConnectionParameters connectionParameters)
        {
            if (connectionParameters == null ||
                connectionParameters == default(ConnectionParameters))
            {
                throw new ConnectionParametersException(
                    "Invalid database connection params");
            }

            if (Connection != null && Connection?.State == ConnectionState.Open)
            {
                Connection.Dispose();
            }

            string ConnectionString = connectionParameters.ToConnectionString();

            switch (connectionParameters.Dialect)
            {
                case DatabaseDialect.SqlServer:
                    Connection = new SqlConnection(ConnectionString);
                    break;
                case DatabaseDialect.Mysql:
                    Connection = new MySqlConnection(ConnectionString);
                    break;
                case DatabaseDialect.Postgres:
                    Connection = new NpgsqlConnection(ConnectionString);
                    break;
            }
        }

        public T QueryOne<T>(
            string Query,
            object Parameters = null,
            ConnectionParameters connectionParameters = null)
        {
            if (connectionParameters != null)
            {
                ConnectDatabase(connectionParameters);
            }

            if (Parameters != null)
            {
                return Connection.QueryFirstOrDefault<T>(Query, Parameters);
            }

            return Connection.QueryFirstOrDefault<T>(Query);
        }

        public List<T> QueryAll<T>(string Query,
            object Parameters = null,
            ConnectionParameters connectionParameters = null)
        {
            if (connectionParameters != null)
            {
                ConnectDatabase(connectionParameters);
            }

            if (Parameters != null)
            {
                return Connection.Query<T>(Query, Parameters).ToList();
            }

            return Connection.Query<T>(Query).ToList();
        }

        public void ExecCommand(
            string Command,
            object Parameters = null,
            ConnectionParameters connectionParameters = null,
            bool IsProcedure = false)
        {
            if (connectionParameters != null)
            {
                ConnectDatabase(connectionParameters);
            }

            if (Parameters != null)
            {
                Connection.Execute(Command, Parameters,
                    commandType: IsProcedure
                        ? CommandType.StoredProcedure
                        : CommandType.Text);
                return;
            }

            Connection.Execute(Command,
                commandType: IsProcedure
                    ? CommandType.StoredProcedure
                    : CommandType.Text);
        }

        public void ExecCommands<T>(
            string Command,
            List<T> Parameters,
            ConnectionParameters connectionParameters = null,
            bool IsProcedure = false)
        {
            if (connectionParameters != null)
            {
                ConnectDatabase(connectionParameters);
            }

            Connection.Execute(Command, Parameters,
                commandType: IsProcedure
                    ? CommandType.StoredProcedure
                    : CommandType.Text);
            return;
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
