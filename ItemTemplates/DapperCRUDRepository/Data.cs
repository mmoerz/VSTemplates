using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    public class $safeitemname$Repository
    {
        private readonly ISqlDataAccess _db;
		private const string _tablename = "[dbo].[]";

        public $safeitemname$Repository(ISqlDataAccess db)
        {
            _db = db;
        }

		private const string _sqlDelete =
		    @$"DELETE FROM {_tablename}
               WHERE ";
						   
        public void Delete(Model data)
        {
            _db.Execute(_sqlDelete, data);
        }
		public Task DeleteAsync(Model data)
        {
            return _db.ExecuteAsync(_sqlDelete, data);
        }

		private const string _sqlGet =
			@"SELECT * FROM {_tablename}";
        public List<Model> Get()
        {
            return _db.LoadData<Model, dynamic>(_sqlGet, new { });
        }
		
		public Task<List<Model>> GetAsync()
        {
            return _db.LoadDataAsync<Model, dynamic>(_sqlGet, new { });
        }

		private const string _sqlInsert = 
		    @$"INSERT INTO {_tablename} ()
               VALUES ()";
        public int Insert(Model data)
        {
            return _db.SaveDataWithIdentity(_sqlInsert, data);
        }
		public Task<int> InsertAsync(Model data)
        {
            return _db.SaveDataWithIdentityAsync(_sqlInsert, data);
        }

		private const string _sqlUpdate =
			@$"UPDATE {_tablename}
			   SET 
               WHERE ";
        public void Update(Model data)
        {
            _db.Execute(_sqlUpdate, data);
        }
		public Task UpdateAsync(Model data)
        {
            return _db.ExecuteAsync(_sqlUpdate, data);
        }
    }
}
