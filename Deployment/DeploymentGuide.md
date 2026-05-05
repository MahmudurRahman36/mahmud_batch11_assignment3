# Deployment Guide: 3-Server Architecture

This guide explains how to deploy the IP Info Hub application across three separate servers for maximum separation of concerns and scalability.

## Infrastructure Overview
- **Server 1: Web & Proxy (Nginx)** - Serves the frontend and routes API calls.
- **Server 2: Application (Backend)** - Runs the .NET Web API.
- **Server 3: Database (PostgreSQL)** - Stores and retrieves data.

---

## Pre-deployment Checklist
Before starting, ensure you have the IP addresses of all three servers:
- **Server 1 (Nginx):** Public: `13.206.221.0`, Private: `10.0.9.114`
- **Server 2 (Application):** Public: `13.233.44.99`, Private: `10.0.10.112`
- **Server 3 (Database):** Public: `43.205.241.33`, Private: `10.0.10.187`

Ensure ports are open in your firewalls:
- **Server 1:** 80 (HTTP)
- **Server 2:** 5000 (API) - Allow access *only* from Server 1 IP.
- **Server 3:** 5432 (DB) - Allow access *only* from Server 2 IP.

---

sudo apt update && sudo apt upgrade -y
sudo apt install postgresql postgresql-contrib -y
sudo systemctl status postgresql

sudo vim /etc/postgresql/16/main/postgresql.conf
listen_addresses = '*'

sudo vim /etc/postgresql/16/main/pg_hba.conf
# Allow App Server to connect
host    all             all             10.0.10.112/32          md5

## 1. Server 3: Database Setup (PostgreSQL)
1. Install PostgreSQL on Server 3.
2. Copy `Database/setup.sql` to the server.
3. Execute the setup script:
   ```bash
   cat setup.sql | sudo -u postgres psql
   ```
4. **Important:** Edit `/etc/postgresql/16/main/pg_hba.conf` to allow Server 2 to connect:
   ```text
   host    ip_info_db    app_user    10.0.10.112/32    md5
   ```
5. Edit `/etc/postgresql/16/main/postgresql.conf` to listen on the server's IP:
   ```text
   listen_addresses = '*'
   ```
6. Restart PostgreSQL: `sudo systemctl restart postgresql`.

---

## 2. Server 2: Application Setup (.NET Backend)
1. Install .NET 9.0 Runtime on Server 2.
2. Update `appsettings.json` with the **Server 3 IP**:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=10.0.10.187;Username=app_user;Password=AppSecurePassword123;Database=ip_info_db"
   }
   ```
3. **Publish and transfer files:**
   - On your local machine, navigate to `Application/AppAPI` and run:
     ```bash
     dotnet publish -c Release -o ./publish
     ```
   - Transfer the contents of the `publish` folder to Server 2:
     ```bash
     # Example using SCP:
     scp -r ./publish/* ubuntu@13.233.44.99:/var/www/appapi
     ```
   - Ensure the directory exists on Server 2 first: `sudo mkdir -p /var/www/appapi && sudo chown -R ubuntu:ubuntu /var/www/appapi`
4. Copy `Deployment/appapi.service` to `/etc/systemd/system/`.
5. Start the service:
   ```bash
   sudo systemctl enable appapi.service
   sudo systemctl start appapi.service
   ```

---

## 3. Server 1: Web & Proxy Setup (Nginx)
1. Install Nginx on Server 1.
2. Update `Deployment/nginx.conf` with the **Server 2 IP**:
   ```nginx
   location /api/ {
       proxy_pass http://10.0.10.112:5000;
   }
   ```
3. Transfer `Presentation` files to `/var/www/presentation`.
4. Copy the updated `nginx.conf` to `/etc/nginx/sites-available/appapi`.
5. Enable and restart:
   ```bash
   sudo ln -s /etc/nginx/sites-available/appapi /etc/nginx/sites-enabled/
   sudo nginx -t
   sudo systemctl restart nginx
   ```

---

## 4. Final Verification
1. Access the application via `http://13.206.221.0`.
2. The UI should display:
   - **Client IP:** Your public IP.
   - **Application IP:** `10.0.10.112`.
   - **Database IP:** `10.0.10.187`.
