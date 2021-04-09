# job-shop-collection.static-web-app
Source code of front-end react app, and back-end Azure Functions of job-shop-collection website

## Findings
- Still have slow start (7s)
- Queries after first request are slower (Azure-web-app 30-72ms) vs (Axure-static-web-app.AzureFunctions 269ms-434ms)
- Difficult to configure secrets, for the connection string
- Don't know how to publish with database(connected service?)
- Abandond this project
