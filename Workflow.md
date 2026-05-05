# Project Workflow

## Phase 1: Initialization
- Created directory structure: `Presentation`, `Application`, `Database`.
- Created `.gitignore` for .NET and web development.
- Initialized `Workflow.md` to track progress.

## Phase 2: Data Layer
## Phase 3: Deployment Configuration
- Configured EC2 instance IPs across all relevant files:
  - **Nginx:** 13.206.221.0 (Public)
  - **Application:** 10.0.10.112 (Private)
  - **Database:** 10.0.10.187 (Private)
- Updated `appsettings.json`, `nginx.conf`, and `DeploymentGuide.md` with the new networking details.
- Switched frontend to use relative API paths for better proxy compatibility.
- **Database Layer Setup:** Successfully executed `setup.sql` on Server 3 (10.0.10.187). Verified that `ip_info_db` and `app_user` are created.
