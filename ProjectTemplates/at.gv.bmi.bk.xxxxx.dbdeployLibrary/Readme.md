# How this works
The test console application provides the command line tool to roll out the database (upgrade).
The library should contain the necessary scripts for the rollout in pre-, migration and -post
scripts direcotries.
Use numbers as a prefix for your scripts to provide the order of execution.


# Knowledge 
## DbUP Documentation
https://dbup.readthedocs.io/en/latest

## Reasons for using DbUp
  * No lock in on EF - can use any DB Access Framework (EF, Dapper, Ado.Net, etc...)
  * Rollout of DB can be included in Web Application (arguable whether this is good or bad)
  * Rollout can be a simple console app (default that can be included in project build)

