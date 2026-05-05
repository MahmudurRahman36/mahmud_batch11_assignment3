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
- **Server 3: (Database):** Public: `43.205.241.33`, Private: `10.0.10.187`

**Firewall Ports:**
- **Server 1:** 80 (HTTP)
- **Server 2:** 5000 (API) - Allow access *only* from Server 1 Private IP.
- **Server 3:** 5432 (DB) - Allow access *only* from Server 2 Private IP.

---

## 1. Server 3: Database Setup (PostgreSQL)

1. **Install PostgreSQL:**
   ```bash
   sudo apt update && sudo apt upgrade -y
   sudo apt install postgresql postgresql-contrib -y
   ```

2. **Configure Remote Access:**
   Edit `/etc/postgresql/16/main/postgresql.conf`:
   ```bash
   sudo sed -i "s/#listen_addresses = 'localhost'/listen_addresses = '*'/g" /etc/postgresql/16/main/postgresql.conf
   ```
   Edit `/etc/postgresql/16/main/pg_hba.conf` and add the following line to allow Server 2:
   ```text
   host    ip_info_db    app_user    10.0.10.112/32    md5
   ```

3. **Initialize Database:**
   Copy `Database/setup.sql` to the server and execute:
   ```bash
   cat setup.sql | sudo -u postgres psql
   ```

4. **Restart Service:**
   ```bash
   sudo systemctl restart postgresql
   ```

---

## 2. Server 2: Application Setup (.NET Backend)

1. **Install .NET 9.0 Runtime:**
   ```bash
   sudo add-apt-repository ppa:dotnet/backports -y
   sudo apt update
   sudo apt install -y aspnetcore-runtime-9.0 git
   ```

2. **Deploy Application:**
   ```bash
   # Create directory
   sudo mkdir -p /var/www/appapi
   sudo chown $USER:$USER /var/www/appapi

   # Clone and copy files (or use SCP from local)
   git clone https://github.com/MahmudurRahman36/mahmud_batch11_assignment3.git
   cp -r mahmud_batch11_assignment3/Application/AppAPI/publish/* /var/www/appapi/
   ```

3. **Configure Service:**
   Copy the service file and start it:
   ```bash
   sudo cp mahmud_batch11_assignment3/Deployment/appapi.service /etc/systemd/system/
   sudo systemctl daemon-reload
   sudo systemctl enable appapi.service
   sudo systemctl start appapi.service
   ```

---

## 3. Server 1: Web & Proxy Setup (Nginx)

1. **Install Nginx & Git:**
   ```bash
   sudo apt update
   sudo apt install nginx git -y
   ```

2. **Deploy Frontend:**
   ```bash
   sudo mkdir -p /var/www/presentation
   sudo chown $USER:$USER /var/www/presentation
   
   git clone https://github.com/MahmudurRahman36/mahmud_batch11_assignment3.git
   cp -r mahmud_batch11_assignment3/Presentation/* /var/www/presentation/
   ```

3. **Configure Nginx:**
   Copy the configuration and enable the site:
   ```bash
   sudo cp mahmud_batch11_assignment3/Deployment/nginx.conf /etc/nginx/sites-available/appapi
   sudo ln -sf /etc/nginx/sites-available/appapi /etc/nginx/sites-enabled/
   sudo nginx -t
   sudo systemctl restart nginx
   ```

---

## 4. Final Verification

1. **Access the application:** `http://13.206.221.0`
2. **Expected Output:**
   - **Client IP:** Your public IP.
   - **Application Server IP:** `10.0.10.112`
   - **Database Server IP:** `10.0.10.187`
