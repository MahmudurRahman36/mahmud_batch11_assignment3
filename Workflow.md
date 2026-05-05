# Project Workflow

## Phase 1: Initialization
- Created directory structure: `Presentation`, `Application`, `Database`.
- Created `.gitignore` for .NET and web development.
- Initialized `Workflow.md` to track progress.

## Phase 2: Data Layer
## Phase 3: Deployment Configuration
- Configured EC2 instance IPs across all relevant files:
  - **Nginx:** 13.232.177.25 (Public)
  - **Application:** 10.0.7.96 (Private)
  - **Database:** 10.0.13.128 (Private)
- Updated `appsettings.json`, `nginx.conf`, and `DeploymentGuide.md` with the new networking details.
- Switched frontend to use relative API paths for better proxy compatibility.
- **Database Layer Setup:** Successfully executed `setup.sql` on Server 3 (10.0.13.128). Verified that `ip_info_db` and `app_user` are created.
