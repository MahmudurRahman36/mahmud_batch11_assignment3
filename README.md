# Simple 3-Tier IP Info Hub

This project is a demonstration of a 3-tier architecture (Presentation, Application, and Data layers) designed to display the IP addresses of the Client, Application Server, and Database.

## Architecture
- **Presentation Layer**: HTML5, Vanilla CSS3 (Premium Design), and JavaScript. Hosted on **Server 1** with Nginx.
- **Application Layer**: .NET 9.0 Web API using C#. Hosted on **Server 2**.
- **Data Layer**: PostgreSQL Database. Hosted on **Server 3**.

## Prerequisites
- .NET 9.0 SDK/Runtime
- PostgreSQL Server
- Nginx

## Setup Instructions

For a detailed walkthrough of deploying this project across three separate servers, please refer to the:

👉 **[Deployment Guide (3-Server Setup)](./Deployment/DeploymentGuide.md)**

### Quick Summary:
1. **Database (Server 3 - 10.0.13.128):** Run `Database/setup.sql` and allow Server 2 IP (`10.0.7.96`).
2. **Backend (Server 2 - 10.0.7.96):** Configure `appsettings.json` with Server 3 IP (`10.0.13.128`).
3. **Nginx (Server 1 - 13.232.177.25):** Configure `nginx.conf` with Server 2 IP (`10.0.7.96`).

## Development Workflow
The development process was documented in [Workflow.md](./Workflow.md).

## Verification
The application was verified by:
1. Testing the API JSON response directly.
2. Validating the frontend UI layout and responsiveness.
3. Checking the SQL syntax for PostgreSQL compatibility.
