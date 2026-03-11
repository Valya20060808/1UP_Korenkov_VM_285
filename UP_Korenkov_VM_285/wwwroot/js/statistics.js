
let statusChart = null;
let equipmentChart = null;
let statisticsData = null;


document.addEventListener('DOMContentLoaded', function () {
    const userRole = getUserRole();
    const allowedRoles = ['Менеджер', 'Менеджер по качеству'];

    if (!allowedRoles.includes(userRole)) {
        window.location.href = '/pages/requests.html';
        return;
    }

    loadStatistics();
});


async function loadStatistics() {
    try {
        document.getElementById('loading').style.display = 'block';
        document.getElementById('statisticsContent').style.display = 'none';
        hideError();

        statisticsData = await requestsApi.getStatistics();
        displayStatistics(statisticsData);

        document.getElementById('loading').style.display = 'none';
        document.getElementById('statisticsContent').style.display = 'block';
    } catch (error) {
        console.error('Ошибка загрузки статистики:', error);
        document.getElementById('loading').style.display = 'none';
        showError('Не удалось загрузить статистику: ' + (error.message || 'Неизвестная ошибка'));
    }
}


function displayStatistics(data) {

    document.getElementById('totalRequests').textContent = data.totalRequests;
    document.getElementById('completedRequests').textContent = data.completedRequests;
    document.getElementById('avgCompletionDays').textContent = data.averageCompletionDays.toFixed(1);


    document.getElementById('detailTotal').textContent = data.totalRequests;
    document.getElementById('detailCompleted').textContent = data.completedRequests;
    document.getElementById('detailAvgDays').textContent = data.averageCompletionDays.toFixed(1) + ' дней';

    let inProgress = 0, newRequests = 0, waiting = 0;

    if (data.requestsByStatus) {
        inProgress = data.requestsByStatus['В процессе ремонта'] || 0;
        newRequests = data.requestsByStatus['Новая заявка'] || 0;
        waiting = data.requestsByStatus['Ожидание комплектующих'] || 0;

        document.getElementById('detailInProgress').textContent = inProgress;
        document.getElementById('detailNew').textContent = newRequests;
        document.getElementById('detailWaiting').textContent = waiting;
    }

    if (data.requestsByEquipmentType) {
        const equipmentEntries = Object.entries(data.requestsByEquipmentType);
        if (equipmentEntries.length > 0) {
            const mostPopular = equipmentEntries.reduce((max, item) =>
                item[1] > max[1] ? item : max
            );
            document.getElementById('mostPopularEquipment').textContent =
                `${mostPopular[0]} (${mostPopular[1]} шт.)`;
        }
    }

    if (data.requestsByStatus) {
        const statusEntries = Object.entries(data.requestsByStatus);
        if (statusEntries.length > 0) {
            const mostActive = statusEntries.reduce((max, item) =>
                item[1] > max[1] ? item : max
            );
            document.getElementById('mostActiveStatus').textContent =
                `${mostActive[0]} (${mostActive[1]} шт.)`;
        }
    }

    createCharts(data);
}

function createCharts(data) {
    if (statusChart) statusChart.destroy();
    if (equipmentChart) equipmentChart.destroy();

    const colors = [
        '#667eea', '#28a745', '#ffc107', '#dc3545', '#17a2b8',
        '#6f42c1', '#fd7e14', '#20c997', '#e83e8c', '#6c757d'
    ];

    if (data.requestsByStatus) {
        const statusCtx = document.getElementById('statusChart').getContext('2d');
        statusChart = new Chart(statusCtx, {
            type: 'pie',
            data: {
                labels: Object.keys(data.requestsByStatus),
                datasets: [{
                    data: Object.values(data.requestsByStatus),
                    backgroundColor: colors.slice(0, Object.keys(data.requestsByStatus).length),
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: true,
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            font: {
                                size: 12
                            }
                        }
                    }
                }
            }
        });
    }

    if (data.requestsByEquipmentType) {
        const equipmentCtx = document.getElementById('equipmentChart').getContext('2d');
        equipmentChart = new Chart(equipmentCtx, {
            type: 'bar',
            data: {
                labels: Object.keys(data.requestsByEquipmentType),
                datasets: [{
                    label: 'Количество заявок',
                    data: Object.values(data.requestsByEquipmentType),
                    backgroundColor: colors.slice(0, Object.keys(data.requestsByEquipmentType).length),
                    borderColor: colors.slice(0, Object.keys(data.requestsByEquipmentType).length),
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: true,
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: {
                            stepSize: 1
                        }
                    }
                },
                plugins: {
                    legend: {
                        display: false
                    }
                }
            }
        });
    }
}


function exportToCSV() {
    if (!statisticsData) return;

    let csvContent = "Показатель,Значение\n";

    csvContent += `Всего заявок,${statisticsData.totalRequests}\n`;
    csvContent += `Выполнено заявок,${statisticsData.completedRequests}\n`;
    csvContent += `Среднее время ремонта (дней),${statisticsData.averageCompletionDays.toFixed(2)}\n\n`;

    csvContent += "Статус,Количество\n";
    if (statisticsData.requestsByStatus) {
        Object.entries(statisticsData.requestsByStatus).forEach(([status, count]) => {
            csvContent += `${status},${count}\n`;
        });
    }

    csvContent += "\nТип оборудования,Количество\n";
    if (statisticsData.requestsByEquipmentType) {
        Object.entries(statisticsData.requestsByEquipmentType).forEach(([type, count]) => {
            csvContent += `${type},${count}\n`;
        });
    }

   
    const blob = new Blob(["\uFEFF" + csvContent], { type: 'text/csv;charset=utf-8;' }); 
    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);

    link.setAttribute('href', url);
    link.setAttribute('download', `statistics_${new Date().toISOString().split('T')[0]}.csv`);
    link.style.display = 'none';

    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    URL.revokeObjectURL(url);

    showSuccess('Отчёт скачан');
}

async function refreshStatistics() {
    await loadStatistics();
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

setInterval(refreshStatistics, 300000);