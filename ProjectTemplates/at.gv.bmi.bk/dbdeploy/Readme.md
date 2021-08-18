# Database Information
## local db
In order to create a local .mdf file (and log) right click on the project and select ```add new item``` and then 
```service based Database```. This creates an empty local database file (initial size 8KB).

## connection string
The syntax follows the standard connection string rules and it must be supplied in the configuration file.

*Hint: don't fumble around construting a string, instead register the database in VS and use the connection string from
the property pane.*

Examples for connection strings:
### local db (.mdf)
This is at least safe without password.
```
"ConnectionStrings": {
    "Default": "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\micha\\source\\repos\\DAEMeldestelle\\at.gv.bmi.bk.DAEMeldestelle.dbdeploy\\Compact.mdf;Integrated Security=True"
  }
```

### remote sql server db
```
"ConnectionStrings": {
    "Default": "Persist Security Info=False;User ID=sa;Password=testme;Initial Catalog=DAEMeldestelle;Server=localhost"
  }
```
### secrets.json
Connection strings for development should be stored here and the file must be excluded in .gitignore.

### using dotnet user-secrets
If you already initialized your project at the console level with something like:
```
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:development" "... your connection string"
```
Then you will have to stick with that way of setting the secrets, or clear all secrets.
Otherwise secrets.json won't work.

e.g:
```
*/secrets.json
```
# SQL migration scripts
Must be built into the library as an "embedded ressource".
The executeable could also be used, but then it would be necessary to supply the correct Assembly as 
a start for searching the scripts like:
```
	baseAssembly = Assembly.GetExecutingAssembly(Program);
	AdditionalAssembly = Assembly.GetExecutingAssembly(Program);
	SqlServerMigrationRunner migrationRunner = 
	    new(connectionString, baseAssembly, AdditionalAssembly, variables);
```
Keep in mind that you need to create the directories "PreDeployment", "Migrations", "PostDeployment" and place all sql scripts there.
!!! It is also necessary to right click and select the build action ```embedded ressource``` !!!

## variable substitution in the embedded SQL scripts
See [[https://dbup.readthedocs.io/en/latest/more-info/variable-substitution/]] for an example.

# Configuration Manager
https://www.itnota.com/use-system-configuration-configurationmanager-dot-net-core/
https://docs.microsoft.com/en-us/dotnet/api/system.configuration.configurationmanager.connectionstrings?view=net-5.0#System_Configuration_ConfigurationManager_ConnectionStrings