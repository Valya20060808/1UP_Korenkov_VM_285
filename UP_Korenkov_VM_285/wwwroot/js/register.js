const roles = [
    { id: 1, name: 'Менеджер' },
    { id: 2, name: 'Специалист' },
    { id: 3, name: 'Оператор' },
    { id: 4, name: 'Заказчик' },
    { id: 5, name: 'Менеджер по качеству' }
];

document.addEventListener('DOMContentLoaded', function () {
    const userRole = getUserRole();

    if (userRole !== 'Менеджер') {
        window.location.href = '/pages/requests.html';
        return;
    }

    loadRoles();
    setupValidation();
});


function loadRoles() {
    const roleSelect = document.getElementById('roleId');

    roles.forEach(role => {
        const option = document.createElement('option');
        option.value = role.id;
        option.textContent = role.name;
        roleSelect.appendChild(option);
    });
}


function setupValidation() {
    const password = document.getElementById('password');
    const confirmPassword = document.getElementById('confirmPassword');

    password.addEventListener('input', validatePasswords);
    confirmPassword.addEventListener('input', validatePasswords);
}


function validatePasswords() {
    const password = document.getElementById('password').value;
    const confirmPassword = document.getElementById('confirmPassword').value;
    const confirmField = document.getElementById('confirmPassword');

    if (confirmPassword && password !== confirmPassword) {
        confirmField.style.borderColor = '#dc3545';
        return false;
    } else if (confirmPassword) {
        confirmField.style.borderColor = '#28a745';
        return true;
    } else {
        confirmField.style.borderColor = '#e0e0e0';
        return false;
    }
}


async function handleRegister(event) {
    event.preventDefault();

    const fullName = document.getElementById('fullName').value.trim();
    const phone = document.getElementById('phone').value.trim();
    const login = document.getElementById('login').value.trim();
    const password = document.getElementById('password').value;
    const confirmPassword = document.getElementById('confirmPassword').value;
    const roleId = parseInt(document.getElementById('roleId').value);

    
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

    if (!password) {
        showError('Введите пароль');
        return;
    }

    if (password.length < 4) {
        showError('Пароль должен содержать минимум 4 символа');
        return;
    }

    if (password !== confirmPassword) {
        showError('Пароли не совпадают');
        return;
    }

    if (!roleId) {
        showError('Выберите роль');
        return;
    }

    const submitBtn = event.target.querySelector('button[type="submit"]');
    const originalText = submitBtn.textContent;
    submitBtn.disabled = true;
    submitBtn.textContent = 'Создание...';
    hideError();

    try {
        const userData = {
            fullName: fullName,
            phone: phone,
            login: login,
            password: password,
            roleId: roleId
        };

        await authApi.register(userData);

        showSuccess('Пользователь успешно создан!');

        
        document.getElementById('registerForm').reset();

       
        setTimeout(() => {
            window.location.href = '/pages/users.html';
        }, 2000);

    } catch (error) {
        console.error('Ошибка регистрации:', error);

        
        if (error.message && error.message.includes('уже существует')) {
            showError('Пользователь с таким логином уже существует');
        } else {
            showError(error.message || 'Не удалось создать пользователя');
        }

        submitBtn.disabled = false;
        submitBtn.textContent = originalText;
    }
}

function goBack() {
    window.location.href = '/pages/users.html';
}

function showError(message) {
    const errorDiv = document.getElementById('errorMessage');
    errorDiv.textContent = message;
    errorDiv.style.display = 'block';

    setTimeout(() => {
        errorDiv.style.display = 'none';
    }, 5000);
}

function hideError() {
    document.getElementById('errorMessage').style.display = 'none';
}


function showSuccess(message) {
    const alert = document.createElement('div');
    alert.className = 'alert alert-success';
    alert.textContent = message;
    alert.style.position = 'fixed';
    alert.style.top = '20px';
    alert.style.right = '20px';
    alert.style.zIndex = '1000';
    alert.style.minWidth = '200px';

    document.body.appendChild(alert);

    setTimeout(() => {
        alert.remove();
    }, 3000);
}