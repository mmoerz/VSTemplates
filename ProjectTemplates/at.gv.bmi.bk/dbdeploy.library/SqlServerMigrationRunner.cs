using DbUp;
using DbUp.Builder;
using DbUp.Engine;
using DbUp.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace $safeprojectname$
{
    /// <summary>
    /// A helper class for executing sql scripts embedded in an assembly having
    /// the purpose to migrate (create/upgrade) a database.
    /// </summary>
    public class SqlServerMigrationRunner
    {
        public enum ResultType
        {
            PREDEPLOY,
            ADDITIONALPREDEPLOY,
            MIGRATION,
            POSTDEPLOY,
            ADDITIONALPOSTDEPLOY
        }

        /// <summary>
        /// stores the database upgrade results of the different deployment phases.
        /// </summary>
        public Dictionary<ResultType, DatabaseUpgradeResult> UpgradeResults { get; private set; }

        public string ConnectionString { get; set; }
        public Assembly BaseAssembly { get; set; }
        public Assembly AdditionalAssembly { get; set; }     
        public Dictionary<string,string> Variables { get; set; }

        // See how to use variables in your scripts: 
        // https://dbup.readthedocs.io/en/latest/more-info/variable-substitution/

        /// <summary>
        /// class that runs the pre, migrations and post scripts.
        /// </summary>
        /// <param name="connectionString">connectionstring for the database to migrate</param>
        /// <param name="baseNameSpace">if null, the pre, migration, and postscripts of the library are used</param>
        /// <param name="additionalBaseNameSpace">path for additional scripts for pre and post migration</param>
        /// <param name="variables">variable substitutions for the sripts</param>
        public SqlServerMigrationRunner(string connectionString,
            Assembly baseAssembly,
            Assembly additionalAssembly,
            Dictionary<string, string> variables)
        {
            BaseAssembly = CheckOrSetToThisLibrary(baseAssembly);
            AdditionalAssembly = CheckOrSetToThisLibrary(additionalAssembly);
            ConnectionString = connectionString;
            Variables = variables;
            UpgradeResults = new Dictionary<ResultType, DatabaseUpgradeResult>();

            PrepareAppSchema();
        }

        /// <summary>
        /// Checks that assembly is not null and if it is sets this library as the assembly.
        /// </summary>
        /// <param name="assembly">assembly to check for null</param>
        /// <returns>returns assembly if not null otherwise assembly of this library.</returns>
        private static Assembly CheckOrSetToThisLibrary(Assembly assembly)
        {
            if (assembly != null)
                return assembly;
            return Assembly.GetAssembly(typeof(UpgradeEngineBuilderExtension));
        }

        /// <summary>
        /// DbUP doesn't create the schema anymore, so this creates [app] schema, since
        /// [app].[Migrations] table is used to store the journal for the execution state of scripts.
        /// </summary>
        /// <returns>Success when app schema was properly created.</returns>
        private DatabaseUpgradeResult PrepareAppSchema()
        {
             UpgradeEngineBuilder builder = DeployChanges.To
                .SqlDatabase(ConnectionString)
                .JournalTo(new NullJournal())
                .WithScripts(new ScriptCheckAndCreateSchemaApp());
            return builder.Build().PerformUpgrade();
        }

        /// <summary>
        /// executes the scripts found in the given assembly and subdirectory.
        /// </summary>
        /// <param name="assembly">assembly that contains the scripts</param>
        /// <param name="pathPostfix">subdirectory in VS containing the scripts</param>
        /// <param name="alwaysRun">specify whether the operation should be tracked in the journal or not.</param>
        /// <returns>result of the upgrade.</returns>
        private DatabaseUpgradeResult RunIt(Assembly assembly, string pathPostfix, bool alwaysRun)
        {
            UpgradeEngineBuilder builder;
            builder = DeployChanges.To
                .SqlDatabase(ConnectionString)
                .WithVariables(Variables)
                .WithPrefixPostfixPathForEmbeddedScripts(assembly, pathPostfix)
                .WithPredefinedJournal(alwaysRun);
            return builder.Build().PerformUpgrade();
        }

        /// <summary>
        /// run predeployment scripts normally found in "PreDeployment" folder.
        /// That folder can have subdirectories containing scripts as well. 
        /// Those will be executed as well.
        /// </summary>
        /// <param name="preDeploymentPostfix">name of the folder to filter for.</param>
        /// <returns>result of the upgrade.</returns>
        public DatabaseUpgradeResult RunPredeployment(string preDeploymentPostfix)
        {
            UpgradeResults.Add(
                ResultType.PREDEPLOY, 
                RunIt(BaseAssembly, preDeploymentPostfix, true)
                );
            return UpgradeResults[ResultType.PREDEPLOY];
        }

        /// <summary>
        /// run additional predeployment scripts.
        /// </summary>
        /// <param name="additionalPreDeploymentPostfix">name of the folder to filter for.</param>
        /// <returns>result of the upgrade.</returns>
        public DatabaseUpgradeResult RunAdditionalPredeployment(string additionalPreDeploymentPostfix)
        {
            if (string.IsNullOrWhiteSpace(additionalPreDeploymentPostfix)) 
                return new DatabaseUpgradeResult(new List<SqlScript>(), false, null, null);
            UpgradeResults.Add(
                ResultType.ADDITIONALPREDEPLOY,
                RunIt(AdditionalAssembly, additionalPreDeploymentPostfix, true)
            );
            return UpgradeResults[ResultType.ADDITIONALPREDEPLOY];
        }

        /// <summary>
        /// Run the (core) migration scripts.
        /// That folder can have subdirectories containing scripts as well. 
        /// Those will be executed as well.
        /// </summary>
        /// <param name="migrationsNamespace">name of the folder to filter for.</param>
        /// <returns>result of the upgrade.</returns>
        public DatabaseUpgradeResult RunMigration(string migrationsNamespace)
        {
            if (string.IsNullOrWhiteSpace(migrationsNamespace))
                throw new System.ArgumentNullException(nameof(migrationsNamespace));

            UpgradeResults.Add(
                ResultType.MIGRATION,
                RunIt(BaseAssembly, migrationsNamespace, false)
                );
            return UpgradeResults[ResultType.MIGRATION];
        }

        /// <summary>
        /// Run the post deployment scripts, normally found in "PostDeployment" folder.
        /// That folder can have subdirectories containing scripts as well. 
        /// Those will be executed as well.
        /// </summary>
        /// <param name="postDeploymentPostfix">name of the folder to filter for.</param>
        /// <returns>result of the upgrade.</returns>
        public DatabaseUpgradeResult RunPostdeployment(string postDeploymentPostfix)
        {
            UpgradeResults.Add(
                ResultType.POSTDEPLOY, 
                RunIt(BaseAssembly, postDeploymentPostfix, true)
            );
            return UpgradeResults[ResultType.POSTDEPLOY];
        }

        /// <summary>
        /// Run the additional post deployment scripts.
        /// That folder can have subdirectories containing scripts as well. 
        /// Those will be executed as well.
        /// </summary>
        /// <param name="additionalPostDeploymentPostfix">name of the folder to filter for.</param>
        /// <returns>result of the upgrade.</returns>
        public DatabaseUpgradeResult RunAdditionalPostdeployment(string additionalPostDeploymentPostfix)
        {
            if (string.IsNullOrWhiteSpace(additionalPostDeploymentPostfix)) 
                return new DatabaseUpgradeResult(new List<SqlScript>(), false, null, null);
            UpgradeResults.Add(
                ResultType.ADDITIONALPREDEPLOY,
                RunIt(AdditionalAssembly, additionalPostDeploymentPostfix, true)
            );
            return UpgradeResults[ResultType.ADDITIONALPREDEPLOY];
        }

        /// <summary>
        /// Summarizes results of all executed migration steps.
        /// </summary>
        /// <returns>merged result of all upgrade. True on full successe, otherwise false.</returns>
        public bool GetMergedResult()
        {
            bool result = true;
            foreach (var item in UpgradeResults)
            {
                result &= item.Value.Successful;
            }

            return result;
        }
    }
}
