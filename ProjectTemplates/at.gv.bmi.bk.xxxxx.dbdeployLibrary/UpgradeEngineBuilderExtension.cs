using System.Reflection;
using DbUp.Builder;
using DbUp.Engine.Output;
using DbUp.Helpers;

namespace $safeprojectname$
{
    public static class UpgradeEngineBuilderExtension
    {
        /// <summary>
        /// Either utilize a provided path or assemble one for this library.
        /// Default should be to use this library to add the migration scripts.
        /// </summary>
        /// <param name="builder">extension method for UpgradeEngineBuilder class</param>
        /// <param name="executingAssembly">Assembly containing the scripts to execute.</param>
        /// <param name="pathPrefixFilter">postfix for the path to search for scripts (directory).</param>
        /// <returns>return of the class UpgradeEngineBuilder for the extension method</returns>
        public static UpgradeEngineBuilder WithPrefixPostfixPathForEmbeddedScripts(
            this UpgradeEngineBuilder builder,
            Assembly executingAssembly,
            string pathPrefixFilter
            )
        {
            if (executingAssembly == null)
                throw new System.ArgumentNullException(nameof(executingAssembly));
            if (string.IsNullOrEmpty(pathPrefixFilter))
                throw new System.ArgumentNullException(nameof(pathPrefixFilter));

            //// String contains name, version, etc. (need to strip that away)
            var FullAssemblyPath = executingAssembly.FullName.Split(',')[0].ToLower();
            
            var FullPathFilter = pathPrefixFilter.StartsWith('.') ?
                FullAssemblyPath + pathPrefixFilter :
                FullAssemblyPath + "." + pathPrefixFilter;
            
            return builder
                .WithScriptsEmbeddedInAssembly(
                executingAssembly, file => {
                    var result = file.ToLower().StartsWith(FullPathFilter.ToLower());
                    return result;
                    })
                .LogTo(new ConsoleUpgradeLog());
        }

        /// <summary>
        /// Register a null journal if the script should always be run, otherwise use [app].[Migrations] table of the db.
        /// </summary>
        /// <param name="builder">extension method for UpgradeEngineBuilder class</param>
        /// <param name="alwaysRun">true when scripts should always run, disregarding prior executions</param>
        /// <returns>return of the class UpgradeEngineBuilder for the extension method</returns>
        public static UpgradeEngineBuilder WithPredefinedJournal(this UpgradeEngineBuilder builder, bool alwaysRun)
        {
            // use null journal if the scripts should be always run
            return alwaysRun ?
                builder.JournalTo(new NullJournal()) :
                builder.JournalToSqlTable("app", "Migrations");
        }
    }
}