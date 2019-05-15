namespace Celes
{
    public class ConnectionParameters
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public DatabaseDialect Dialect { get; set; }

        public string ToConnectionString()
        {
            switch (Dialect)
            {
                case DatabaseDialect.SqlServer:
                    return $"Server={Server};" +
                        $"Database={Database};" +
                        $"User ID={User};" +
                        $"Password={Password};";
                case DatabaseDialect.Mysql:
                    return $"Server={Server};" +
                        $"Database={Database};" +
                        $"Uid={User};" +
                        $"Pwd={Password};";
                case DatabaseDialect.Postgres:
                    return $"Host={Server};" +
                        $"Database={Database};" +
                        $"User ID={User};" +
                        $"Password={Password};";
                default:
                    return string.Empty;
            }
        }
    }
}
