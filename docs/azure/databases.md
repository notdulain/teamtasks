# Azure SQL Databases

## SQL Servers

| Server | FQDN | Location |
|---|---|---|
| `teamtasks-sql-qa` | `teamtasks-sql-qa.database.windows.net` | East Asia |
| `teamtasks-sql-staging` | `teamtasks-sql-staging.database.windows.net` | East Asia |
| `teamtasks-sql-prod` | `teamtasks-sql-prod.database.windows.net` | East Asia |

**Admin credentials:** `sqladmin` / `YourStrong@Passw0rd` (all servers)

## Firewall Rules

`AllowAzureServices` (0.0.0.0 → 0.0.0.0) configured on all 3 servers to allow Azure service connectivity.

## Databases

All databases are on the **Basic** tier and currently **Online**.

| QA | Staging | Prod |
|---|---|---|
| `users_qa` | `users_staging` | `users_prod` |
| `tasks_qa` | `tasks_staging` | `tasks_prod` |
| `notifications_qa` | `notifications_staging` | `notifications_prod` |

## Connection String Format

```
Server=tcp:teamtasks-sql-{env}.database.windows.net,1433;Initial Catalog={db_name};User ID=sqladmin;Password=YourStrong@Passw0rd;Encrypt=True;TrustServerCertificate=False;
```

### Examples

**QA - User Service:**
```
Server=tcp:teamtasks-sql-qa.database.windows.net,1433;Initial Catalog=users_qa;User ID=sqladmin;Password=YourStrong@Passw0rd;Encrypt=True;TrustServerCertificate=False;
```

**Staging - Task Service:**
```
Server=tcp:teamtasks-sql-staging.database.windows.net,1433;Initial Catalog=tasks_staging;User ID=sqladmin;Password=YourStrong@Passw0rd;Encrypt=True;TrustServerCertificate=False;
```

**Prod - Notification Service:**
```
Server=tcp:teamtasks-sql-prod.database.windows.net,1433;Initial Catalog=notifications_prod;User ID=sqladmin;Password=YourStrong@Passw0rd;Encrypt=True;TrustServerCertificate=False;
```
