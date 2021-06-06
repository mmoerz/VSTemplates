using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class $safeitemname$Data
    {
        private readonly ISqlDataAccess _db;

        public $safeitemname$Data(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task Delete(Model data)
        {
            string sql = @"DELETE FROM [dbo].[]
                           WHERE ";

            return _db.Execute(sql, data);
        }

        public Task<List<Model>> Get()
        {
            string sql = @"SELECT * FROM [dbo].[]";

            return _db.LoadData<Model, dynamic>(sql, new { });
        }

        public Task Insert(Model data)
        {
            string sql = @"INSERT INTO [dbo].[] ()
                           VALUES ()";

            return _db.SaveData(sql, data);
        }

        public Task Update(Model data)
        {
            string sql = @"UPDATE [dbo].[] SET 
                           WHERE ";

            return _db.Execute(sql, data);
        }
    }
}
