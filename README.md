# Running Episerver as a console application #

This project shows that Episerver (CMS and commerce) can be run as console application. I have personally used this to run long-running jobs, which otherwise would be interrupted by IIS. 

## Install ##

Follow these steps exactly.

1. Create CMS and commerce databases in a SQL server instance
2. Set connection strings accordingly in Episerver.Console.Web and Episerver.Console.Client
3. Run Initialize-EPiDatabase with default project Episerver.Console.Web
4. Run Update-EPiDatabase with default project Episerver.Console.Web
5. Start web site Episerver.Console.Web
6. [Login](http://localhost:51517/Util/login.aspx) using your Windows account
7. Execute migration steps
8. Stop debugging web site
9. Run Episerver.Console.Client
10. Start web site

## Result ##
You should now see the start page, which was programmatically created in the console application. The console application also created a catalog, category, product and a variant. Enter the [catalog UI](http://localhost:51517/EPiServer/Commerce/Catalog) to verify this.