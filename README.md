# Simple 3-Tier IP Info Hub

This project demonstrates a professional 3-tier architecture (Presentation, Application, and Data layers) deployed across three separate AWS EC2 instances. It displays the public/private IP addresses of the Client, Application Server, and Database to verify connectivity and architecture.

## 🏗️ Architecture Overview
The application is deployed using the following EC2 instances:

![EC2 Infrastructure](./screenshot/EC2_Screenshot.png)

- **Server 1 (Nginx):** Public: `13.206.221.0` | Private: `10.0.9.114`
- **Server 2 (Application):** Public: `13.233.44.99` | Private: `10.0.10.112`
- **Server 3 (Database):** Public: `43.205.241.33` | Private: `10.0.10.187`

### 🔒 Security Group Configuration
Ports 80 (HTTP), 5000 (API), and 5432 (PostgreSQL) were configured to allow secure communication between layers.

![Security Groups](./screenshot/Security%20group.png)

---

## 🚀 Setup & Commands

### 1. Database Layer (PostgreSQL)
![Database Server](./screenshot/Mahmud_Ostad_Batch11_database.png)

**Commands executed on Server 3:**
```bash
# Install PostgreSQL
sudo apt update && sudo apt install postgresql postgresql-contrib -y

# Configure remote access in postgresql.conf
sudo sed -i "s/#listen_addresses = 'localhost'/listen_addresses = '*'/g" /etc/postgresql/16/main/postgresql.conf

# Add permission in pg_hba.conf for Application Server
echo "host ip_info_db app_user 10.0.10.112/32 md5" | sudo tee -a /etc/postgresql/16/main/pg_hba.conf

# Initialize database
cat setup.sql | sudo -u postgres psql

# Restart service
sudo systemctl restart postgresql
```
![Database Verification](./screenshot/DatabasSetup.png)

### 2. Application Layer (.NET 9.0 Web API)
![Application Server](./screenshot/Mahmud_Ostad_Batch11_web.png)

**Commands executed on Server 2:**
```bash
# Install .NET Runtime
sudo add-apt-repository ppa:dotnet/backports -y
sudo apt update && sudo apt install -y aspnetcore-runtime-9.0 git

# Deploy published files
sudo mkdir -p /var/www/appapi && sudo chown $USER:$USER /var/www/appapi
git clone https://github.com/MahmudurRahman36/mahmud_batch11_assignment3.git
cp -r mahmud_batch11_assignment3/Application/AppAPI/publish/* /var/www/appapi/

# Start Service
sudo cp mahmud_batch11_assignment3/Deployment/appapi.service /etc/systemd/system/
sudo systemctl daemon-reload && sudo systemctl enable --now appapi.service
```
![API Service Status](./screenshot/WebHostInService.png)

### 3. Presentation Layer (Nginx)
![Nginx Server](./screenshot/Mahmud_Ostad_Batch11_nginx.png)

**Commands executed on Server 1:**
```bash
# Install Nginx
sudo apt update && sudo apt install nginx git -y

# Deploy Frontend files
sudo mkdir -p /var/www/presentation && sudo chown $USER:$USER /var/www/presentation
git clone https://github.com/MahmudurRahman36/mahmud_batch11_assignment3.git
cp -r mahmud_batch11_assignment3/Presentation/* /var/www/presentation/

# Configure Reverse Proxy
sudo cp mahmud_batch11_assignment3/Deployment/nginx.conf /etc/nginx/sites-available/appapi
sudo ln -sf /etc/nginx/sites-available/appapi /etc/nginx/sites-enabled/
sudo nginx -t && sudo systemctl restart nginx
```
![Nginx Config](./screenshot/NginxSetup.png)

---

## ⚙️ Configuration Details

### Database Connection (`appsettings.json`)
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=10.0.10.187;Username=app_user;Password=AppSecurePassword123;Database=ip_info_db"
}
```

### Nginx Proxy (`nginx.conf`)
```nginx
location /api/ {
    proxy_pass http://10.0.10.112:5000;
}
```

---

## 🌐 Application Access Result

The application is publicly accessible at: **[http://13.206.221.0](http://13.206.221.0)**

![Final Result](./screenshot/WebPageAccessFromInternet.png)

---
*Project developed as part of Ostad DevOps Batch 11 Assignment.*
