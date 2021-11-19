This project come from my need to repeatedly creating new .NET 6 MVC project with asp.net identity (for STUDY or TESTING, NOT SUITABLE FOR PRODUCTION) while studying asp.net core.  The standard project template created a project that require you to :

1. Run migration to create a new database
2. Add user that meet strong password
3. randomized the identity db name
4. randomized the port of the project
5. if you have many projects in a solution, you often make mistake by openning the wrong program.cs or startup.cs or HomeController.cs file

The project elimiate all these steps.
1. No need for migration, identity db is already created
2. You can add user with only a single character password
3. identity db name is always asp.net.identity.db
4. port is always 5000 and 44300 (ssl)
5. program.cs and HomeController.cs files are appended with project name so that you know which project's program.cs you are editing when you have many projects.


In addition, this project include 

1. OpenAPISupport (Swashbuckle)
2. Runtime razor compilation
3. Commented out content in razor view that support PonyAuth
4. Blazor Server Side support
5. (.NET 6) Both w3c loging and HTTP logging middleware are included. It include a controller to view w3c log
6. (C# 10) Global using file GlobalUsings.cs
7. Code to create sample user data




WHO might find this project useful ?

Those who need to create asp.net core app with individual account often (for STUDY or TESTING), and may need to swtich between SQLite and SQL Server Localdb


The only explanation that is needed is that in order to allow unrestricted password , these files are copied from identity scalfolding and updated with project namespace

Register.cshtml
Register.cshtml.cs


You probably want to open this project, and modify according your need and export this project as a project template






