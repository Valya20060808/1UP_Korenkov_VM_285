const API_URL = 'https://localhost:7036/api';


async function apiRequest(endpoint, method = 'GET', data = null) {
    const token = localStorage.getItem('token');
    
    const options = {
        method,
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    };

    if (data) {
        options.body = JSON.stringify(data);
    }

    try {
        console.log(`API Request: ${method} ${API_URL}${endpoint}`, data); 
        
        const response = await fetch(`${API_URL}${endpoint}`, options);
        

        if (response.status === 401) {
            console.log('Unauthorized, redirecting to login');
            localStorage.clear();
            window.location.href = '/pages/login.html';
            throw new Error('Сессия истекла');
        }

        if (response.status === 403) {
            throw new Error('У вас нет прав для этого действия');
        }


        if (response.status === 204) {
            return null;
        }

        const contentType = response.headers.get('content-type');
        const responseText = await response.text();
        
        console.log(`Response status: ${response.status}, Content-Type: ${contentType}`);
        console.log('Response text:', responseText); 


        if (!responseText) {
            if (!response.ok) {
                throw new Error(`HTTP error ${response.status}`);
            }
            return null;
        }

        let result;
        if (contentType && contentType.includes('application/json')) {
            try {
                result = JSON.parse(responseText);
            } catch (e) {
                console.error('Failed to parse JSON:', responseText);
                throw new Error('Неверный формат ответа от сервера');
            }
        } else {

            result = responseText;
        }
        
        if (!response.ok) {
            throw new Error(result.message || `HTTP error ${response.status}`);
        }

        return result;
    } catch (error) {
        console.error('API Error:', error);
        throw error;
    }
}


async function apiGet(endpoint) {
    return apiRequest(endpoint, 'GET');
}

async function apiPost(endpoint, data) {
    return apiRequest(endpoint, 'POST', data);
}

async function apiPut(endpoint, data) {
    return apiRequest(endpoint, 'PUT', data);
}

async function apiDelete(endpoint) {
    return apiRequest(endpoint, 'DELETE');
}


const requestsApi = {
    getAll: (statusId = null) => {
        const url = statusId ? `/requests?statusId=${statusId}` : '/requests';
        return apiGet(url);
    },
    getMy: () => apiGet('/requests/my'),
    getById: (id) => apiGet(`/requests/${id}`),
    create: (data) => apiPost('/requests', data),
    updateStatus: (id, statusId) => apiPut(`/requests/${id}/status`, statusId),
    assignMaster: (id, masterId) => apiPut(`/requests/${id}/assign-master`, { masterId }),
    addComment: (id, message) => apiPost(`/requests/${id}/comments`, { message }),
    extendDeadline: (id, newDeadline) => apiPut(`/requests/${id}/extend-deadline`, { newDeadline }),
    getStatuses: () => apiGet('/requests/statuses'),
    getStatistics: () => apiGet('/requests/statistics')
};


const usersApi = {
    getAll: () => apiGet('/users'),
    getById: (id) => apiGet(`/users/${id}`),
    getByRole: (roleId) => apiGet(`/users/role/${roleId}`),
    getProfile: () => apiGet('/users/profile'),
    updateProfile: (data) => apiPut('/users/profile', data),
    updateUser: (id, data) => apiPut(`/users/${id}`, data),
    deleteUser: (id) => apiDelete(`/users/${id}`),
    changePassword: (data) => apiPost('/users/change-password', data)
};

const dictionariesApi = {
    getEquipmentTypes: () => apiGet('/equipmenttypes'),
    getRequestStatuses: () => apiGet('/requests/statuses')
};

const authApi = {
    login: (credentials) => apiPost('/auth/login', credentials),
    register: (userData) => apiPost('/auth/register', userData)
};

function formatDate(dateString) {
    if (!dateString) return '—';
    const date = new Date(dateString);
    return date.toLocaleDateString('ru-RU', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
}

function getStatusClass(statusId) {
    const classes = {
        1: 'status-process',      
        2: 'status-ready',       
        3: 'status-new',          
        4: 'status-waiting'       
    };
    return classes[statusId] || 'status-new';
}