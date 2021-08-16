using System.Collections.Generic;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    /// <summary>
    /// represents loading and saving of data to and from a database.
    /// </summary>
    interface ISqlDataAccess
    {
        string ConnectionStringName { get; set; }

        Task<List<T>> LoadData<T, U>(string sql, U parameters);
        Task SaveData<T>(string sql, T parameters);
    }
}