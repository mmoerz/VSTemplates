# TODO

# How this works
The test project utilizes XUnit for driving the tests.
The basic structure is derived from the different types of tests that have to be written.
## Unit Tests

## Integration Tests
 
## Tests that need a database
[Integrationtests]
XUnit provides a comfortable way to setup an environment for all tests contained in a class - the
so called fixtures. The project contains an abstract base fixture called DatabaseFixture. This
fixture reads the "Default" connection string from 'appsettings.json'. In a Microsoft OS based
development environment this should be a local database file.
```add local connection string example here```
This setting can be overriden with a 'secrets.json' containing the connecting string like:
```
{
  "ConnectionStrings": {
    "Default": "Persist Security Info=False;User ID=<youruser>;Password=<yourpassword>.;Initial Catalog=DAEMeldestelle;Server=localhost"
  }
}
```
Replace ```<youruser>``` and ```<yourpassword>``` accordingly. 
**NOTE: the user must be able to create databases**

After reading the default connection string the DatabaseFixture creates a new database with the
predefined basename appended by a guid value. That databasename is saved in a connection string and
can be accessed by the property ConnectionString.
This mechanism guarantees a new empty database for each test run.
Additionally the library Respawn is used for setting checkpoints in order to roll back changes after
each test.
A manual Rollback can be initiated by calling ResetDatabaseAsync().

### How to make my testcase database-aware
#### Setup your implementation of the abstract base class DatabaseFixture
[this step is already included]
```
```

#### Derive your TestClass from your DatabaseFixture implementation
```
IClassFixture<DAEMeldestelleDatabaseFixture>
```
Simply derive your test class from 

# Knowledge 
## Database Integration Testing
https://www.jvandertil.nl/posts/2020-04-02_sqlserverintegrationtesting/

## Database setup and teardown 
Includes "Best practice approach" with database project and migration scripts

https://www.kamilgrzybek.com/database/using-database-project-and-dbup-for-database-management/

## Information about desired state deployment vs. migrations
Desired state deployment is utilized by Visual Studio database projects.
Visual Studio database projects rely on state comparison (called diffs) to compare your source and target schemas and come up with a deployment script.
While these auto generated scripts are very good they aren�t perfect. The tooling can�t always create scripts for all the state transitions without
causing inconsistencies (data loss etc). This makes it impossible to integrate them into an automated deployment pipeline with confidence.

## examples
  * Example (intermediate)
  provides additional information for development vs. production deployment
  https://dasith.me/2020/06/08/database-project-conversion-to-migrations/

  * ASP.net core example
  https://medium.com/cheranga/database-migrations-using-dbup-in-an-asp-net-core-web-api-application-c24ccfe0cb43

  * expert level - code based migrations
  https://itnext.io/code-based-database-migrations-with-dbup-dd04dd2fe2ec

## Reasons for using DbUp
  * No lock in on EF - can use any DB Access Framework (EF, Dapper, Ado.Net, etc...)
  * Rollout of DB can be included in Web Application (arguable whether this is good or bad)
  * Rollout can be a simple console app (default that can be included in project build)


## IChangeToken
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/change-tokens?view=aspnetcore-5.0
https://developpaper.com/net-core-common-design-patterns-ichangetoken/

# Not important 
- How to detect OS platform
https://blog.magnusmontin.net/2018/11/05/platform-conditional-compilation-in-net-core/