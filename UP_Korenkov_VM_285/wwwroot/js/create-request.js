let currentUserRole = getUserRole();
let currentUserId = parseInt(getUserId());


document.addEventListener('DOMContentLoaded', async function () {
    const canCreate = ['Оператор', 'Менеджер', 'Менеджер по качеству', 'Заказчик'].includes(currentUserRole);
    if (!canCreate) {
        window.location.href = '/pages/requests.html';
        return;
    }

    await loadEquipmentTypes();
    await setupClientFields();

    if (currentUserRole === 'Заказчик') {
        await loadClientProfile();
    }
});


async function loadEquipmentTypes() {
    try {
        const types = await dictionariesApi.getEquipmentTypes();
        const select = document.getElementById('equipmentTypeId');

        types.forEach(type => {
            const option = document.createElement('option');
            option.value = type.id;
            option.textContent = type.name;
            select.appendChild(option);
        });
    } catch (error) {
        console.error('Ошибка загрузки типов оборудования:', error);
        showError('Не удалось загрузить типы оборудования');
    }
}


async function setupClientFields() {
    const clientSelectGroup = document.getElementById('clientSelectGroup');
    const clientInfo = document.getElementById('clientInfo');

    if (currentUserRole === 'Оператор' || currentUserRole === 'Менеджер' || currentUserRole === 'Менеджер по качеству') {
       
        clientSelectGroup.style.display = 'block';
        await loadClients();
    } else if (currentUserRole === 'Заказчик') {

        clientSelectGroup.style.display = 'none';
        clientInfo.style.display = 'block';
        document.getElementById('clientId').required = false;
    }
}


async function loadClients() {
    try {
        const clients = await usersApi.getByRole(4); 
        const select = document.getElementById('clientId');
        select.innerHTML = '<option value="">Выберите заказчика</option>';

        clients.forEach(client => {
            const option = document.createElement('option');
            option.value = client.id;
            option.textContent = `${client.fullName} (${client.phone})`;
            select.appendChild(option);
        });
    } catch (error) {
        console.error('Ошибка загрузки заказчиков:', error);
        showError('Не удалось загрузить список заказчиков');
    }
}


async function loadClientProfile() {
    try {
        const profile = await usersApi.getProfile();
        document.getElementById('clientFullName').textContent = profile.fullName;
        document.getElementById('clientPhone').textContent = profile.phone;
    } catch (error) {
        console.error('Ошибка загрузки профиля:', error);
        showError('Не удалось загрузить данные профиля');
    }
}


function validateForm() {
    const equipmentTypeId = document.getElementById('equipmentTypeId').value;
    const equipmentModel = document.getElementById('equipmentModel').value.trim();
    const problemDescription = document.getElementById('problemDescription').value.trim();

    if (!equipmentTypeId) {
        showError('Выберите тип оборудования');
        return false;
    }

    if (!equipmentModel) {
        showError('Введите модель оборудования');
        return false;
    }

    if (equipmentModel.length > 200) {
        showError('Модель не может быть длиннее 200 символов');
        return false;
    }

    if (!problemDescription) {
        showError('Введите описание проблемы');
        return false;
    }

    if (problemDescription.length > 1000) {
        showError('Описание не может быть длиннее 1000 символов');
        return false;
    }

    if ((currentUserRole === 'Оператор' || currentUserRole === 'Менеджер' || currentUserRole === 'Менеджер по качеству')) {
        const clientId = document.getElementById('clientId').value;
        if (!clientId) {
            showError('Выберите заказчика');
            return false;
        }
    }

    return true;
}


async function handleCreateRequest(event) {
    event.preventDefault();

    if (!validateForm()) {
        return;
    }

    const submitBtn = event.target.querySelector('button[type="submit"]');
    submitBtn.disabled = true;
    submitBtn.textContent = 'Создание...';

    try {
 
        let clientId;
        if (currentUserRole === 'Заказчик') {
            clientId = currentUserId;
        } else {
            clientId = parseInt(document.getElementById('clientId').value);
        }


        const requestData = {
            startDate: new Date().toISOString(),
            equipmentTypeId: parseInt(document.getElementById('equipmentTypeId').value),
            equipmentModel: document.getElementById('equipmentModel').value.trim(),
            problemDescription: document.getElementById('problemDescription').value.trim(),
            requestStatusId: 3,
            clientId: clientId
        };


        const newRequest = await requestsApi.create(requestData);


        showSuccess('Заявка успешно создана!');

        setTimeout(() => {
            window.location.href = `/pages/request-detail.html?id=${newRequest.id}`;
        }, 1000);

    } catch (error) {
        console.error('Ошибка создания заявки:', error);
        showError(error.message || 'Не удалось создать заявку');
        submitBtn.disabled = false;
        submitBtn.textContent = 'Создать заявку';
    }
}

function goBack() {
    window.location.href = '/pages/requests.html';
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
    const alert = document.createElement('div');
    alert.className = 'alert alert-success';
    alert.textContent = message;

    const form = document.getElementById('createRequestForm');
    form.parentNode.insertBefore(alert, form);

    setTimeout(() => alert.remove(), 3000);
}


document.getElementById('equipmentModel').addEventListener('input', function () {
    const maxLength = 200;
    const currentLength = this.value.length;
    const small = this.nextElementSibling;
    if (currentLength > maxLength - 20) {
        small.textContent = `Осталось ${maxLength - currentLength} символов`;
        small.style.color = currentLength > maxLength ? 'red' : '#666';
    } else {
        small.textContent = 'Максимум 200 символов';
        small.style.color = '#666';
    }
});

document.getElementById('problemDescription').addEventListener('input', function () {
    const maxLength = 1000;
    const currentLength = this.value.length;
    const small = this.nextElementSibling;
    if (currentLength > maxLength - 50) {
        small.textContent = `Осталось ${maxLength - currentLength} символов`;
        small.style.color = currentLength > maxLength ? 'red' : '#666';
    } else {
        small.textContent = 'Максимум 1000 символов';
        small.style.color = '#666';
    }
});