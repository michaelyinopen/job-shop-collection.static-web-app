# job-shop-collection.static-web-app api folder
Azure Functions for API of job-shop-collection website.

## How to run
Use Visual Studio installed with Azure development. Open solution, Run(F5).

## Database Migration
In Package Manager Console, use
```
$env:JobShopCollectionConnectionString="Server=(localdb)\mssqllocaldb;Database=JobShopCollectionDb;"
```
Before `Add-Migration` / `Update-Database`