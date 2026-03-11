async function handleLogin(event) {
    event.preventDefault();

    const login = document.getElementById('login').value;
    const password = document.getElementById('password').value;
    const errorDiv = document.getElementById('error-message');
    const submitBtn = document.querySelector('button[type="submit"]');

    try {
        submitBtn.disabled = true;
        submitBtn.textContent = 'Вход...';

        const response = await authApi.login({ login, password });


        localStorage.setItem('token', response.token);
        localStorage.setItem('userId', response.userId);
        localStorage.setItem('userRole', response.role);
        localStorage.setItem('userName', response.fullName);


        window.location.href = '/pages/requests.html';
    } catch (error) {
        errorDiv.textContent = error.message;
        errorDiv.style.display = 'block';
    } finally {
        submitBtn.disabled = false;
        submitBtn.textContent = 'Войти';
    }
}


function checkLoggedIn() {
    if (localStorage.getItem('token')) {
        window.location.href = '/pages/requests.html';
    }
}