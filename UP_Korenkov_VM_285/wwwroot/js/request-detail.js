let currentRequest = null;
let requestStatuses = [];
let currentUserRole = getUserRole();
let currentUserId = parseInt(getUserId());

function getRequestIdFromUrl() {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get('id');
}

document.addEventListener('DOMContentLoaded', async function () {
    const requestId = getRequestIdFromUrl();
    if (!requestId) {
        showError('ID заявки не указан');
        return;
    }

    await loadRequestDetails(requestId);
    await loadStatuses();
});

async function loadRequestDetails(requestId) {
    try {
        document.getElementById('loading').style.display = 'block';
        document.getElementById('requestDetails').style.display = 'none';

        currentRequest = await requestsApi.getById(requestId);
        displayRequestDetails(currentRequest);

        document.getElementById('loading').style.display = 'none';
        document.getElementById('requestDetails').style.display = 'block';
    } catch (error) {
        console.error('Ошибка загрузки заявки:', error);
        document.getElementById('loading').style.display = 'none';
        showError('Не удалось загрузить информацию о заявке');
    }
}

async function loadStatuses() {
    try {
        requestStatuses = await dictionariesApi.getRequestStatuses();
    } catch (error) {
        console.error('Ошибка загрузки статусов:', error);
    }
}

function displayRequestDetails(request) {
    document.getElementById('requestId').textContent = request.id;
    document.getElementById('startDate').textContent = formatDate(request.startDate);
    document.getElementById('deadline').textContent = request.deadline ? formatDate(request.deadline) : 'Не установлен';
    document.getElementById('equipmentType').textContent = request.equipmentType;
    document.getElementById('equipmentModel').textContent = request.equipmentModel;
    document.getElementById('problemDescription').textContent = request.problemDescription;
    document.getElementById('clientName').textContent = request.clientName;
    document.getElementById('masterName').textContent = request.masterName || 'Не назначен';
    document.getElementById('repairParts').textContent = request.repairParts || 'Не указаны';
    document.getElementById('completionDate').textContent = request.completionDate ? formatDate(request.completionDate) : '—';

    const statusSpan = document.getElementById('requestStatus');
    statusSpan.textContent = request.requestStatus;
    statusSpan.className = `status-badge ${getStatusClass(request.requestStatusId)}`;

    displayComments(request.comments || []);
    setupActionButtons(request);

    if (request.requestStatusId === 2) {
        generateQRCode(request.id);
    }
}

function displayComments(comments) {
    const commentsList = document.getElementById('commentsList');
    commentsList.innerHTML = '';

    if (!comments || comments.length === 0) {
        commentsList.innerHTML = '<p class="alert alert-info">Комментариев пока нет</p>';
        return;
    }

    comments.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));

    comments.forEach(comment => {
        const commentDiv = document.createElement('div');
        commentDiv.className = 'comment';
        commentDiv.innerHTML = `
            <div class="comment-header">
                <span class="comment-author">${comment.masterName}</span>
                <span class="comment-date">${formatDate(comment.createdAt)}</span>
            </div>
            <div class="comment-text">${comment.message}</div>
        `;
        commentsList.appendChild(commentDiv);
    });
}

function setupActionButtons(request) {
    const actionButtons = document.getElementById('actionButtons');
    actionButtons.innerHTML = '';

    if (currentUserRole !== 'Заказчик') {
        const statusSelect = createStatusSelect(request);
        actionButtons.appendChild(statusSelect);
    }

    if (currentUserRole === 'Специалист' && request.masterId === currentUserId) {
        const partsDiv = createRepairPartsField(request);
        actionButtons.appendChild(partsDiv);
        document.getElementById('addCommentForm').style.display = 'block';
    }

    if (currentUserRole === 'Оператор' || currentUserRole === 'Менеджер' || currentUserRole === 'Менеджер по качеству') {
        if (!request.masterId) {
            const assignBtn = document.createElement('button');
            assignBtn.className = 'btn btn-primary';
            assignBtn.textContent = 'Назначить специалиста';
            assignBtn.onclick = () => showAssignMasterModal(request.id);
            actionButtons.appendChild(assignBtn);
        }
        document.getElementById('addCommentForm').style.display = 'block';
    }

    if (currentUserRole === 'Менеджер по качеству') {
        const extendBtn = document.createElement('button');
        extendBtn.className = 'btn btn-warning';
        extendBtn.textContent = 'Продлить срок';
        extendBtn.onclick = () => showExtendDeadlineModal(request.id);
        actionButtons.appendChild(extendBtn);
    }
}

function createStatusSelect(request) {
    const div = document.createElement('div');
    div.className = 'form-group';

    const label = document.createElement('label');
    label.textContent = 'Изменить статус:';

    const select = document.createElement('select');
    select.className = 'form-control';
    select.id = 'statusSelect';

    requestStatuses.forEach(status => {
        const option = document.createElement('option');
        option.value = status.id;
        option.textContent = status.name;
        option.selected = status.id === request.requestStatusId;
        select.appendChild(option);
    });

    select.onchange = () => handleStatusChange(request.id, select.value);

    div.appendChild(label);
    div.appendChild(select);
    return div;
}

function createRepairPartsField(request) {
    const div = document.createElement('div');
    div.className = 'form-group';

    const label = document.createElement('label');
    label.textContent = 'Использованные запчасти:';

    const textarea = document.createElement('textarea');
    textarea.className = 'form-control';
    textarea.id = 'repairPartsInput';
    textarea.rows = 3;
    textarea.value = request.repairParts || '';

    const saveBtn = document.createElement('button');
    saveBtn.className = 'btn btn-success btn-sm';
    saveBtn.textContent = 'Сохранить запчасти';
    saveBtn.style.marginTop = '10px';
    saveBtn.onclick = () => saveRepairParts(request.id);

    div.appendChild(label);
    div.appendChild(textarea);
    div.appendChild(saveBtn);
    return div;
}

async function handleStatusChange(requestId, newStatusId) {
    if (!confirm('Изменить статус заявки?')) return;

    try {
        await requestsApi.updateStatus(parseInt(requestId), parseInt(newStatusId));
        showSuccess('Статус обновлён');
        await loadRequestDetails(requestId);
    } catch (error) {
        console.error('Ошибка обновления статуса:', error);
        alert(error.message);
    }
}

async function saveRepairParts(requestId) {
    const repairParts = document.getElementById('repairPartsInput').value;

    try {
        await apiPut(`/requests/${requestId}`, { repairParts });
        showSuccess('Запчасти сохранены');
        await loadRequestDetails(requestId);
    } catch (error) {
        console.error('Ошибка сохранения запчастей:', error);
        alert(error.message);
    }
}

async function handleAddComment(event) {
    event.preventDefault();

    const requestId = getRequestIdFromUrl();
    const message = document.getElementById('commentText').value;

    if (!message.trim()) return;

    try {
        await requestsApi.addComment(requestId, message);
        document.getElementById('commentText').value = '';
        showSuccess('Комментарий добавлен');
        await loadRequestDetails(requestId);
    } catch (error) {
        console.error('Ошибка добавления комментария:', error);
        alert(error.message);
    }
}

async function showAssignMasterModal(requestId) {
    document.getElementById('assignMasterModal').classList.add('active');

    try {
        const specialists = await usersApi.getByRole(2);
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

document.getElementById('assignMasterForm').addEventListener('submit', async (event) => {
    event.preventDefault();

    const requestId = getRequestIdFromUrl();
    const masterId = document.getElementById('masterSelect').value;

    if (!masterId) {
        alert('Выберите специалиста');
        return;
    }

    try {
        await requestsApi.assignMaster(parseInt(requestId), parseInt(masterId));
        closeModal('assignMasterModal');
        showSuccess('Специалист назначен');
        await loadRequestDetails(requestId);
    } catch (error) {
        console.error('Ошибка назначения специалиста:', error);
        alert(error.message);
    }
});

function showExtendDeadlineModal(requestId) {
    document.getElementById('extendDeadlineModal').classList.add('active');
    const today = new Date().toISOString().slice(0, 16);
    document.getElementById('newDeadline').min = today;
}

document.getElementById('extendDeadlineForm').addEventListener('submit', async (event) => {
    event.preventDefault();

    const requestId = getRequestIdFromUrl();
    const newDeadline = document.getElementById('newDeadline').value;

    if (!newDeadline) {
        alert('Выберите новую дату');
        return;
    }

    try {
        await requestsApi.extendDeadline(parseInt(requestId), new Date(newDeadline).toISOString());
        closeModal('extendDeadlineModal');
        showSuccess('Срок выполнения продлён');
        await loadRequestDetails(requestId);
    } catch (error) {
        console.error('Ошибка продления срока:', error);
        alert(error.message);
    }
});

function generateQRCode(requestId) {
    const qrSection = document.getElementById('qrCodeSection');
    qrSection.style.display = 'block';
    document.getElementById('qrcode').innerHTML = '';
    const feedbackUrl = `https://docs.google.com/forms/d/e/1FAIpQLSeXUvx5LeVeJc1tKti_cnaCqvoyOzQosefat-xFomTD1mgCWg/viewform?usp=publish-editor{requestId}`;
    new QRCode(document.getElementById('qrcode'), {
        text: feedbackUrl,
        width: 200,
        height: 200
    });
}

function closeModal(modalId) {
    document.getElementById(modalId).classList.remove('active');
}

function goBack() {
    window.location.href = '/pages/requests.html';
}

function showError(message) {
    const errorDiv = document.getElementById('errorMessage');
    errorDiv.textContent = message;
    errorDiv.style.display = 'block';
    document.getElementById('loading').style.display = 'none';
}

function showSuccess(message) {
    const alert = document.createElement('div');
    alert.className = 'alert alert-success';
    alert.textContent = message;
    document.querySelector('.container').insertBefore(alert, document.querySelector('#requestDetails'));
    setTimeout(() => alert.remove(), 3000);
}