using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    /// <summary>
    /// realizes loading and saving data to a db using dapper
    /// </summary>
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public string ConnectionStringName { get; set; } = "Default";
        
        /// <summary>
        /// Create an object for SQL db access using Dapper.
        /// </summary>
        /// <param name="config">a IConfiguration implementation providing a "Default" connection string for db access.</param>
        /// <param name="logger">a named ILogger for dependency injection</param>
        public SqlDataAccess(IConfiguration config, ILogger<SqlDataAccess> logger)
        {
            _config = config;
        }

        /// <summary>
        /// Execute a query asynchronously in a Task and map the resultset to <typeparamref name="T"/> type data.
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <typeparam name="U">parameters for the query</typeparam>
        /// <param name="sql">sql statement</param>
        /// <param name="parameters">parameters for the query</param>
        /// <returns>A list of data of <typeparamref name="T"/>; </returns>
        public async Task<List<T>> LoadData<T, U>(string sql, U parameters)
        {
            string connectionString = _config.GetConnectionString(ConnectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var data = await connection.QueryAsync<T>(sql, parameters);

                return data.ToList();
            }
        }

        /// <summary>
        /// Execute a query to store the data <typeparamref name="T"/> in the db.
        /// </summary>
        /// <typeparam name="T">data type to store</typeparam>
        /// <param name="sql">sql statement for storing</param>
        /// <param name="parameters">parameters for the sql statememnt</param>
        /// <returns></returns>
        public async Task SaveData<T>(string sql, T parameters)
        {
            string connectionString = _config.GetConnectionString(ConnectionStringName);

            using IDbConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(sql, parameters);
        }

    }
}
