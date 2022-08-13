//при открытии страницы Статистика выполняется:
document.addEventListener("DOMContentLoaded", getStatisticData);

function getStatisticData()
{
    fetch("/Statistic/GetAllExpenseData?vehicleId=" + vehicleId)
        .then(response => response.json())
            .then((allExpenseData) => {
                const labels = [];
                const dataPie = [];
                const color = [];
                for (var i = 0; i < allExpenseData.length; i++) {
                    labels.push(allExpenseData[i].Name);
                    dataPie.push(allExpenseData[i].Cost);
                    color.push(allExpenseData[i].Color);
                }
                const data = {
                    labels: labels,
                    datasets: [{
                        label: 'Все расходы',
                        backgroundColor: color,
                        data: dataPie,
                    }]
                };
                const config = {
                    type: 'pie',
                    data: data,
                    options: {
                        plugins: {
                            title: {
                                display: true,
                                text: 'Общая информация',
                                font: {
                                    size: 16
                                }
                            },
                            legend: {
                                labels: {
                                    font: {
                                        size: 14
                                    }
                                }
                            }
                        }
                    }
                };
                const myChart = new Chart(
                    document.getElementById('Chart'),
                    config
                );
            })

}