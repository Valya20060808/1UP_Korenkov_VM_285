let allRequests = [];
let requestStatuses = [];
let specialists = [];
let currentUserRole = getUserRole();

document.addEventListener('DOMContentLoaded', async function () {
    const canCreate = ['Оператор', 'Менеджер', 'Менеджер по качеству'].includes(currentUserRole);
    document.getElementById('createRequestBtn').style.display = canCreate ? 'inline-block' : 'none';

    await loadStatuses();
    await loadRequests();

    document.getElementById('searchInput').addEventListener('input', filterRequests);
    document.getElementById('statusFilter').addEventListener('change', filterRequests);
    document.getElementById('createRequestBtn').addEventListener('click', () => {
        window.location.href = '/pages/create-request.html';
    });
    document.getElementById('assignMasterForm').addEventListener('submit', handleAssignMaster);
});

async function loadStatuses() {
    try {
        requestStatuses = await dictionariesApi.getRequestStatuses();
        const statusFilter = document.getElementById('statusFilter');

        requestStatuses.forEach(status => {
            const option = document.createElement('option');
            option.value = status.id;
            option.textContent = status.name;
            statusFilter.appendChild(option);
        });
    } catch (error) {
        console.error('Ошибка загрузки статусов:', error);
    }
}

async function loadRequests() {
    try {
        document.getElementById('loading').style.display = 'block';
        document.getElementById('desktopTable').style.display = 'none';
        document.getElementById('mobileCards').style.display = 'none';
        document.getElementById('noRequests').style.display = 'none';

        if (currentUserRole === 'Заказчик' || currentUserRole === 'Специалист') {
            allRequests = await requestsApi.getMy();
        } else {
            allRequests = await requestsApi.getAll();
        }

        displayRequests(allRequests);
    } catch (error) {
        console.error('Ошибка загрузки заявок:', error);
        showError('Не удалось загрузить заявки');
    } finally {
        document.getElementById('loading').style.display = 'none';
    }
}

function displayRequests(requests) {
    const tbody = document.getElementById('requestsTableBody');
    const cardsContainer = document.getElementById('mobileCards');

    tbody.innerHTML = '';
    cardsContainer.innerHTML = '';

    if (requests.length === 0) {
        document.getElementById('noRequests').style.display = 'block';
        document.getElementById('desktopTable').style.display = 'none';
        document.getElementById('mobileCards').style.display = 'none';
        return;
    }

    requests.forEach(request => {
        const row = tbody.insertRow();
        row.onclick = () => window.location.href = `/pages/request-detail.html?id=${request.id}`;

        row.innerHTML = `
            <td>#${request.id}</td>
            <td>${formatDate(request.startDate)}</td>
            <td>${request.equipmentType}</td>
            <td>${request.equipmentModel}</td>
            <td><span class="status-badge ${getStatusClass(request.requestStatusId)}">${request.requestStatus}</span></td>
            <td>${request.clientName}</td>
            <td>${request.masterName || 'Не назначен'}</td>
            <td>
                ${canAssignMaster(request) ? `<button class="btn btn-sm btn-primary" onclick="showAssignMasterModal(${request.id}, event)">Назначить</button>` : ''}
            </td>
        `;

        const card = document.createElement('div');
        card.className = 'request-card';
        card.onclick = () => window.location.href = `/pages/request-detail.html?id=${request.id}`;

        card.innerHTML = `
            <div class="request-card-header">
                <span class="request-number">#${request.id}</span>
                <span class="request-date">${formatDate(request.startDate)}</span>
            </div>
            <div class="request-info">
                <p><strong>Тип:</strong> ${request.equipmentType}</p>
                <p><strong>Модель:</strong> ${request.equipmentModel}</p>
                <p><strong>Статус:</strong> <span class="status-badge ${getStatusClass(request.requestStatusId)}">${request.requestStatus}</span></p>
                <p><strong>Заказчик:</strong> ${request.clientName}</p>
                <p><strong>Специалист:</strong> ${request.masterName || 'Не назначен'}</p>
            </div>
        `;

        cardsContainer.appendChild(card);
    });

    if (window.innerWidth > 768) {
        document.getElementById('desktopTable').style.display = 'block';
        document.getElementById('mobileCards').style.display = 'none';
    } else {
        document.getElementById('desktopTable').style.display = 'none';
        document.getElementById('mobileCards').style.display = 'grid';
    }

    window.addEventListener('resize', () => {
        if (window.innerWidth > 768) {
            document.getElementById('desktopTable').style.display = 'block';
            document.getElementById('mobileCards').style.display = 'none';
        } else {
            document.getElementById('desktopTable').style.display = 'none';
            document.getElementById('mobileCards').style.display = 'grid';
        }
    });
}

function filterRequests() {
    const searchTerm = document.getElementById('searchInput').value.toLowerCase();
    const statusId = document.getElementById('statusFilter').value;

    const filtered = allRequests.filter(request => {
        const matchesSearch =
            request.id.toString().includes(searchTerm) ||
            request.equipmentModel.toLowerCase().includes(searchTerm) ||
            request.problemDescription.toLowerCase().includes(searchTerm);

        const matchesStatus = !statusId || request.requestStatusId === parseInt(statusId);

        return matchesSearch && matchesStatus;
    });

    displayRequests(filtered);
}

function canAssignMaster(request) {
    return (currentUserRole === 'Оператор' || currentUserRole === 'Менеджер' || currentUserRole === 'Менеджер по качеству')
        && !request.masterId;
}

async function showAssignMasterModal(requestId, event) {
    event.stopPropagation();

    document.getElementById('assignRequestId').value = requestId;
    document.getElementById('assignMasterModal').classList.add('active');

    try {
        specialists = await usersApi.getByRole(2);
        const select = document.getElementById('masterSelect');
        select.innerHTML = '<option value="">Выберите специалиста</option>';

        specialists.forEach(specialist => {
            const option = document.createElement('option');
            option.value = specialist.id;
            option.textContent = specialist.fullName;
            select.appendChild(option);
        });
    } catch (error) {
        console.error('Ошибка загрузки специалистов:', error);
        alert('Не удалось загрузить список специалистов');
    }
}

function closeModal(modalId) {
    document.getElementById(modalId).classList.remove('active');
}

async function handleAssignMaster(event) {
    event.preventDefault();

    const requestId = document.getElementById('assignRequestId').value;
    const masterId = document.getElementById('masterSelect').value;

    if (!masterId) {
        alert('Выберите специалиста');
        return;
    }

    try {
        await requestsApi.assignMaster(parseInt(requestId), parseInt(masterId));
        closeModal('assignMasterModal');
        await loadRequests();
        showSuccess('Специалист назначен');
    } catch (error) {
        console.error('Ошибка назначения специалиста:', error);
        alert(error.message);
    }
}

function showError(message) {
    const alert = document.createElement('div');
    alert.className = 'alert alert-danger';
    alert.textContent = message;
    document.querySelector('.container').insertBefore(alert, document.querySelector('.card'));
    setTimeout(() => alert.remove(), 3000);
}

function showSuccess(message) {
    const alert = document.createElement('div');
    alert.className = 'alert alert-success';
    alert.textContent = message;
    document.querySelector('.container').insertBefore(alert, document.querySelector('.card'));
    setTimeout(() => alert.remove(), 3000);
}