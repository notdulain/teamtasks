# Azure Resource Groups & Infrastructure

## Resource Groups

| Resource Group | Location |
|---|---|
| `teamtasks-qa` | East Asia |
| `teamtasks-staging` | East Asia |
| `teamtasks-prod` | East Asia |

## App Service Plans

| Plan | SKU | Environment |
|---|---|---|
| `teamtasks-plan-qa` | B1 (Basic) | QA |
| `teamtasks-plan-staging` | B1 (Basic) | Staging |
| `teamtasks-plan-prod` | B2 (Basic) | Prod |

## Web Apps

| Web App | URL |
|---|---|
| `teamtasks-user-service-qa` | teamtasks-user-service-qa.azurewebsites.net |
| `teamtasks-task-service-qa` | teamtasks-task-service-qa.azurewebsites.net |
| `teamtasks-notification-service-qa` | teamtasks-notification-service-qa.azurewebsites.net |
| `teamtasks-user-service-staging` | teamtasks-user-service-staging.azurewebsites.net |
| `teamtasks-task-service-staging` | teamtasks-task-service-staging.azurewebsites.net |
| `teamtasks-notification-service-staging` | teamtasks-notification-service-staging.azurewebsites.net |
| `teamtasks-user-service-prod` | teamtasks-user-service-prod.azurewebsites.net |
| `teamtasks-task-service-prod` | teamtasks-task-service-prod.azurewebsites.net |
| `teamtasks-notification-service-prod` | teamtasks-notification-service-prod.azurewebsites.net |

> **Note:** All web apps are currently running the `nginx` placeholder image. CI/CD will deploy the actual service containers.
