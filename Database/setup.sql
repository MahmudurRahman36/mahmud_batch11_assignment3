-- ============================================================
-- POSTGRESQL FULL SETUP GUIDE (A-Z)
-- ============================================================

/*
PHASE 1: OS LEVEL COMMANDS (Linux/Ubuntu)
Run these in your terminal to install and start PostgreSQL
------------------------------------------------------------
1. Install PostgreSQL:
   sudo apt update
   sudo apt install -y postgresql postgresql-contrib

2. Start and Enable PostgreSQL:
   sudo systemctl start postgresql
   sudo systemctl enable postgresql

3. Access the PostgreSQL prompt:
   sudo -u postgres psql
------------------------------------------------------------
*/

/* 
PHASE 2: SQL COMMANDS 
Execute these within the 'psql' prompt
------------------------------------------------------------
*/

-- 1. Create a dedicated user for the application
CREATE USER app_user WITH PASSWORD 'AppSecurePassword123';

-- 2. Create the database
CREATE DATABASE ip_info_db OWNER app_user;

-- 3. Connect to the new database
\c ip_info_db

-- 4. Create a function to get the server's IP address
-- This is used by the Application Layer to verify connectivity.
CREATE OR REPLACE FUNCTION get_db_ip() RETURNS inet AS $$
BEGIN
    RETURN inet_server_addr();
END;
$$ LANGUAGE plpgsql;

-- 5. Grant permissions to the application user
GRANT ALL PRIVILEGES ON DATABASE ip_info_db TO app_user;
GRANT EXECUTE ON FUNCTION get_db_ip() TO app_user;

-- 6. Configure Remote Access (Optional - if App Server is on a different machine)
-- You must also modify /etc/postgresql/XX/main/pg_hba.conf 
-- and /etc/postgresql/XX/main/postgresql.conf (listen_addresses = '*')
-- to allow remote connections.
