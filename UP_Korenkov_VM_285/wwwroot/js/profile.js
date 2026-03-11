document.addEventListener('DOMContentLoaded', async function () {
    await loadProfile();
});


async function loadProfile() {
    try {
        document.getElementById('loading').style.display = 'block';
        document.getElementById('profileContent').style.display = 'none';
        hideMessages();

        const profile = await usersApi.getProfile();

      
        document.getElementById('userId').textContent = profile.id;
        document.getElementById('fullName').value = profile.fullName || '';
        document.getElementById('phone').value = profile.phone || '';
        document.getElementById('login').value = profile.login || '';
        document.getElementById('userRole').textContent = profile.role || 'Не указана';

        document.getElementById('loading').style.display = 'none';
        document.getElementById('profileContent').style.display = 'block';
    } catch (error) {
        console.error('Ошибка загрузки профиля:', error);
        document.getElementById('loading').style.display = 'none';
        showError('Не удалось загрузить данные профиля: ' + (error.message || 'Неизвестная ошибка'));
    }
}

async function handleUpdateProfile(event) {
    event.preventDefault();

    const fullName = document.getElementById('fullName').value.trim();
    const phone = document.getElementById('phone').value.trim();
    const login = document.getElementById('login').value.trim();

    
    if (!fullName) {
        showError('Введите ФИО');
        return;
    }

    if (!phone) {
        showError('Введите номер телефона');
        return;
    }

    
    const phoneRegex = /^[0-9+\-\s()]{10,20}$/;
    if (!phoneRegex.test(phone)) {
        showError('Введите корректный номер телефона');
        return;
    }

    const submitBtn = event.target.querySelector('button[type="submit"]');
    const originalText = submitBtn.textContent;
    submitBtn.disabled = true;
    submitBtn.textContent = 'Сохранение...';
    hideMessages();

    try {
        await usersApi.updateProfile({
            fullName: fullName,
            phone: phone,
            login: login 
        });

        showSuccess('Профиль успешно обновлён');

        
        document.getElementById('fullName').value = fullName;
        document.getElementById('phone').value = phone;

    } catch (error) {
        console.error('Ошибка обновления профиля:', error);
        showError(error.message || 'Не удалось обновить профиль');
    } finally {
        submitBtn.disabled = false;
        submitBtn.textContent = originalText;
    }
}

async function handleChangePassword(event) {
    event.preventDefault();

    const oldPassword = document.getElementById('oldPassword').value;
    const newPassword = document.getElementById('newPassword').value;
    const confirmPassword = document.getElementById('confirmPassword').value;

    
    if (!oldPassword) {
        showError('Введите текущий пароль');
        return;
    }

    if (!newPassword) {
        showError('Введите новый пароль');
        return;
    }

    if (newPassword.length < 4) {
        showError('Новый пароль должен содержать минимум 4 символа');
        return;
    }

    if (newPassword !== confirmPassword) {
        showError('Новый пароль и подтверждение не совпадают');
        return;
    }

    const submitBtn = event.target.querySelector('button[type="submit"]');
    const originalText = submitBtn.textContent;
    submitBtn.disabled = true;
    submitBtn.textContent = 'Изменение...';
    hideMessages();

    try {
        await usersApi.changePassword({
            oldPassword: oldPassword,
            newPassword: newPassword
        });

        showSuccess('Пароль успешно изменён');

       
        document.getElementById('oldPassword').value = '';
        document.getElementById('newPassword').value = '';
        document.getElementById('confirmPassword').value = '';

    } catch (error) {
        console.error('Ошибка смены пароля:', error);

        
        if (error.message && error.message.includes('Неверный текущий пароль')) {
            showError('Неверный текущий пароль');
        } else {
            showError(error.message || 'Не удалось изменить пароль');
        }
    } finally {
        submitBtn.disabled = false;
        submitBtn.textContent = originalText;
    }
}


function goBack() {
    window.location.href = '/pages/requests.html';
}


function hideMessages() {
    document.getElementById('errorMessage').style.display = 'none';
    document.getElementById('successMessage').style.display = 'none';
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


document.getElementById('phone').addEventListener('input', function (e) {
    
    let value = this.value.replace(/\D/g, '');

    if (value.length === 0) {
        return;
    }

    
    if (value.length <= 11) {
        
    }
});


document.getElementById('newPassword').addEventListener('input', function () {
    const newPass = this.value;
    const confirmPass = document.getElementById('confirmPassword').value;
    const confirmField = document.getElementById('confirmPassword');

    if (confirmPass && newPass !== confirmPass) {
        confirmField.style.borderColor = '#dc3545';
    } else if (confirmPass) {
        confirmField.style.borderColor = '#28a745';
    } else {
        confirmField.style.borderColor = '#e0e0e0';
    }
});

document.getElementById('confirmPassword').addEventListener('input', function () {
    const newPass = document.getElementById('newPassword').value;
    const confirmPass = this.value;

    if (newPass !== confirmPass) {
        this.style.borderColor = '#dc3545';
    } else if (confirmPass) {
        this.style.borderColor = '#28a745';
    } else {
        this.style.borderColor = '#e0e0e0';
    }
});