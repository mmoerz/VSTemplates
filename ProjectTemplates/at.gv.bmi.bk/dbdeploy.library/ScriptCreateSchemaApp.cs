using DbUp.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    /// <summary>
    /// Script checks for schema app and creates it when it does not exist.
    /// This is necessary because dbup version >= 4.0.0 doesn't create
    /// the schema for the Version table anymore.
    /// </summary>
    class ScriptCheckAndCreateSchemaApp : IScript
    {
        public bool SchemaFound { get; set; } = false;

        public string ProvideScript(Func<IDbCommand> dbCommandFactory)
        {
            var cmd = dbCommandFactory();
            cmd.CommandText = "SELECT count(*) FROM sys.schemas WHERE name = 'app'";

            using (var reader = cmd.ExecuteReader())
            {
                reader.Read();
                if (reader.GetInt32(0) == 1)
                {
                    SchemaFound = true;
                    return "";
                }
            }
                            
            return "CREATE SCHEMA [app] AUTHORIZATION [dbo]";
        }
    }
}
