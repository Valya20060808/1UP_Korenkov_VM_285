
let allUsers = [];
let currentUserRole = getUserRole();


document.addEventListener('DOMContentLoaded', function () {

    if (currentUserRole !== 'Менеджер') {
        window.location.href = '/pages/requests.html';
        return;
    }

    loadRoles();
    loadUsers();
    setupEventListeners();
});

function setupEventListeners() {
    document.getElementById('searchInput').addEventListener('input', filterUsers);
    document.getElementById('roleFilter').addEventListener('change', filterUsers);
}


async function loadRoles() {
    try {

        const roles = [
            { id: 1, name: 'Менеджер' },
            { id: 2, name: 'Специалист' },
            { id: 3, name: 'Оператор' },
            { id: 4, name: 'Заказчик' },
            { id: 5, name: 'Менеджер по качеству' }
        ];

        const roleFilter = document.getElementById('roleFilter');
        const roleSelect = document.getElementById('editRoleId');

        roles.forEach(role => {
            const option1 = document.createElement('option');
            option1.value = role.id;
            option1.textContent = role.name;
            roleFilter.appendChild(option1);

            const option2 = document.createElement('option');
            option2.value = role.id;
            option2.textContent = role.name;
            roleSelect.appendChild(option2);
        });
    } catch (error) {
        console.error('Ошибка загрузки ролей:', error);
    }
}

async function loadUsers() {
    try {
        document.getElementById('loading').style.display = 'block';
        document.getElementById('usersTable').style.display = 'none';
        document.getElementById('noUsers').style.display = 'none';
        hideMessages();

        console.log('Загрузка пользователей...');
        allUsers = await usersApi.getAll();
        console.log('Получены пользователи:', allUsers);

        displayUsers(allUsers);

        document.getElementById('loading').style.display = 'none';
        document.getElementById('usersTable').style.display = 'block';
    } catch (error) {
        console.error('Ошибка загрузки пользователей:', error);
        document.getElementById('loading').style.display = 'none';
        showError('Не удалось загрузить пользователей: ' + (error.message || 'Неизвестная ошибка'));
    }
}

function displayUsers(users) {
    const tbody = document.getElementById('usersTableBody');
    tbody.innerHTML = '';

    if (users.length === 0) {
        document.getElementById('usersTable').style.display = 'none';
        document.getElementById('noUsers').style.display = 'block';
        return;
    }

    users.forEach(user => {
        const row = tbody.insertRow();

        row.innerHTML = `
            <td>${user.id}</td>
            <td>${user.fullName || '—'}</td>
            <td>${user.login || '—'}</td>
            <td>${user.phone || '—'}</td>
            <td>${user.role || '—'}</td>
            <td class="action-buttons">
                <button class="btn btn-sm btn-primary" onclick="editUser(${user.id})">✏️</button>
                <button class="btn btn-sm btn-danger" onclick="deleteUser(${user.id}, '${user.fullName}')">🗑️</button>
            </td>
        `;
    });
}

function filterUsers() {
    const searchTerm = document.getElementById('searchInput').value.toLowerCase();
    const roleId = document.getElementById('roleFilter').value;

    const filtered = allUsers.filter(user => {
        const matchesSearch =
            (user.fullName && user.fullName.toLowerCase().includes(searchTerm)) ||
            (user.login && user.login.toLowerCase().includes(searchTerm)) ||
            (user.phone && user.phone.toLowerCase().includes(searchTerm));

        const matchesRole = !roleId || user.roleId === parseInt(roleId);

        return matchesSearch && matchesRole;
    });

    displayUsers(filtered);
}

async function editUser(userId) {
    try {
        const user = allUsers.find(u => u.id === userId);
        if (!user) return;

        document.getElementById('editUserId').value = user.id;
        document.getElementById('editFullName').value = user.fullName || '';
        document.getElementById('editPhone').value = user.phone || '';
        document.getElementById('editLogin').value = user.login || '';
        document.getElementById('editPassword').value = '';

        const roleSelect = document.getElementById('editRoleId');
        const role = user.role || '';
        const roleId = getRoleIdByName(role);
        if (roleId) {
            roleSelect.value = roleId;
        }

        document.getElementById('editUserModal').classList.add('active');
    } catch (error) {
        console.error('Ошибка при открытии редактирования:', error);
        showError('Не удалось загрузить данные пользователя');
    }
}

function getRoleIdByName(roleName) {
    const roles = {
        'Менеджер': 1,
        'Специалист': 2,
        'Оператор': 3,
        'Заказчик': 4,
        'Менеджер по качеству': 5
    };
    return roles[roleName] || '';
}

async function handleEditUser(event) {
    event.preventDefault();

    const userId = document.getElementById('editUserId').value;
    const fullName = document.getElementById('editFullName').value.trim();
    const phone = document.getElementById('editPhone').value.trim();
    const login = document.getElementById('editLogin').value.trim();
    const password = document.getElementById('editPassword').value;
    const roleId = document.getElementById('editRoleId').value;

 
    if (!fullName) {
        showError('Введите ФИО');
        return;
    }

    if (!phone) {
        showError('Введите телефон');
        return;
    }

    const phoneRegex = /^[0-9+\-\s()]{10,20}$/;
    if (!phoneRegex.test(phone)) {
        showError('Введите корректный номер телефона');
        return;
    }

    if (!login) {
        showError('Введите логин');
        return;
    }

    if (login.length < 3) {
        showError('Логин должен содержать минимум 3 символа');
        return;
    }

    if (password && password.length < 4) {
        showError('Пароль должен содержать минимум 4 символа');
        return;
    }

    if (!roleId) {
        showError('Выберите роль');
        return;
    }

    const submitBtn = event.target.querySelector('button[type="submit"]');
    const originalText = submitBtn.textContent;
    submitBtn.disabled = true;
    submitBtn.textContent = 'Сохранение...';
    hideMessages();

    try {
        const userData = {
            fullName: fullName,
            phone: phone,
            login: login,
            roleId: parseInt(roleId)
        };

        if (password) {
            userData.password = password;
        }

        await usersApi.updateUser(parseInt(userId), userData);

        closeModal('editUserModal');
        await loadUsers();
        showSuccess('Пользователь успешно обновлён');
    } catch (error) {
        console.error('Ошибка обновления пользователя:', error);

        if (error.message && error.message.includes('уже существует')) {
            showError('Пользователь с таким логином уже существует');
        } else {
            showError(error.message || 'Не удалось обновить пользователя');
        }
    } finally {
        submitBtn.disabled = false;
        submitBtn.textContent = originalText;
    }
}


let deleteUserId = null;

function deleteUser(userId, userName) {
    deleteUserId = userId;
    document.getElementById('deleteUserName').textContent = userName;
    document.getElementById('deleteUserModal').classList.add('active');
}

async function confirmDelete() {
    if (!deleteUserId) return;

    const deleteBtn = document.querySelector('#deleteUserModal .btn-danger');
    const originalText = deleteBtn.textContent;
    deleteBtn.disabled = true;
    deleteBtn.textContent = 'Удаление...';

    try {
        await usersApi.deleteUser(deleteUserId);

        closeModal('deleteUserModal');
        await loadUsers();
        showSuccess('Пользователь успешно удалён');
    } catch (error) {
        console.error('Ошибка удаления пользователя:', error);

        if (error.message && error.message.includes('заявки')) {
            showError('Нельзя удалить пользователя, у которого есть заявки');
        } else {
            showError(error.message || 'Не удалось удалить пользователя');
        }
    } finally {
        deleteBtn.disabled = false;
        deleteBtn.textContent = originalText;
        deleteUserId = null;
    }
}


function closeModal(modalId) {
    document.getElementById(modalId).classList.remove('active');
}


function goBack() {
    window.location.href = '/pages/requests.html';
}


function hideMessages() {
    const errorDiv = document.getElementById('errorMessage');
    const successDiv = document.getElementById('successMessage');
    if (errorDiv) errorDiv.style.display = 'none';
    if (successDiv) successDiv.style.display = 'none';
}


function showError(message) {
    const errorDiv = document.getElementById('errorMessage');
    errorDiv.textContent = message;
    errorDiv.style.display = 'block';

    setTimeout(() => {
        errorDiv.style.display = 'none';
    }, 5000);
}


function showSuccess(message) {
    const successDiv = document.getElementById('successMessage');
    successDiv.textContent = message;
    successDiv.style.display = 'block';

    setTimeout(() => {
        successDiv.style.display = 'none';
    }, 3000);
}