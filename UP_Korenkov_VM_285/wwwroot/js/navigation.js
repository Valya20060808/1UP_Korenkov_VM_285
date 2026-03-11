function checkAuth() {
    const token = localStorage.getItem('token');
    if (!token) {
        window.location.href = '/pages/login.html';
        return false;
    }
    return true;
}


function getUserRole() {
    return localStorage.getItem('userRole');
}

function getUserId() {
    return localStorage.getItem('userId');
}


function logout() {
    localStorage.clear();
    window.location.href = '/pages/login.html';
}


function createNavigation() {
    const role = getUserRole();
    const nav = document.querySelector('.nav-menu');

    if (!nav) return;

    const menuItems = [
        { text: 'Главная', url: '/pages/requests.html', roles: ['all'] },
        { text: 'Статистика', url: '/pages/statistics.html', roles: ['Менеджер', 'Менеджер по качеству'] }, 
        { text: 'Пользователи', url: '/pages/users.html', roles: ['Менеджер'] },
        { text: 'Профиль', url: '/pages/profile.html', roles: ['all'] }
    ];

    nav.innerHTML = '';

    menuItems.forEach(item => {
        if (item.roles.includes('all') || item.roles.includes(role)) {
            const li = document.createElement('li');
            const a = document.createElement('a');
            a.href = item.url;
            a.textContent = item.text;


            if (window.location.pathname.endsWith(item.url)) {
                a.classList.add('active');
            }

            li.appendChild(a);
            nav.appendChild(li);
        }
    });


    const logoutLi = document.createElement('li');
    const logoutBtn = document.createElement('button');
    logoutBtn.textContent = 'Выход';
    logoutBtn.onclick = logout;
    logoutLi.appendChild(logoutBtn);
    nav.appendChild(logoutLi);
}


function initPage() {
    if (!checkAuth()) return;
    createNavigation();
}


function checkRole(allowedRoles) {
    const role = getUserRole();
    if (!allowedRoles.includes(role) && !allowedRoles.includes('all')) {
        window.location.href = '/pages/requests.html';
        return false;
    }
    return true;
}

document.addEventListener('DOMContentLoaded', initPage);