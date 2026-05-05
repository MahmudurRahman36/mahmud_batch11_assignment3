const API_BASE_URL = 'http://localhost:5000/api/info';

async function fetchIPInfo() {
    const clientIpEl = document.getElementById('client-ip');
    const appIpEl = document.getElementById('app-ip');
    const dbIpEl = document.getElementById('db-ip');

    // Show loading state
    clientIpEl.textContent = '...';
    appIpEl.textContent = '...';
    dbIpEl.textContent = '...';

    try {
        const response = await fetch(API_BASE_URL);
        if (!response.ok) throw new Error('Network response was not ok');
        
        const data = await response.json();
        
        clientIpEl.textContent = data.clientIP;
        appIpEl.textContent = data.appServerIP;
        dbIpEl.textContent = data.databaseIP;

        // Add success animation
        document.querySelectorAll('.ip-value').forEach(el => {
            el.style.animation = 'none';
            el.offsetHeight; // trigger reflow
            el.style.animation = 'fadeIn 0.5s ease-out';
        });

    } catch (error) {
        console.error('Error fetching IP info:', error);
        clientIpEl.textContent = 'Error';
        appIpEl.textContent = 'Error';
        dbIpEl.textContent = 'Check Console';
        
        clientIpEl.style.color = '#ef4444';
        appIpEl.style.color = '#ef4444';
        dbIpEl.style.color = '#ef4444';
    }
}

document.getElementById('refresh-btn').addEventListener('click', fetchIPInfo);

// Initial fetch
window.addEventListener('DOMContentLoaded', fetchIPInfo);
