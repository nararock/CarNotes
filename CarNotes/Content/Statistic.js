//при открытии страницы Статистика выполняется:
document.addEventListener("DOMContentLoaded", goToCommonStatistic);

/**
 * переменная для хранения объекта для построения графика*/
var myChart;
/**
 * Метод для получения данных о расходах (заправки, ремонты, иные расходы) для построения круговой диаграммы "Общая статистика".
 * Вызывается при загрузке страницы "Статистика".*/
function goToCommonStatistic()
{
    var elementReference = document.getElementsByClassName("CommonStatistic");
    var anotherElementReference = document.getElementsByClassName("FuelFlowStatistic");
    if (!elementReference[0].classList.contains("active")) {
        elementReference[0].classList.add("active");
        anotherElementReference[0].classList.remove("active");
    }

    fetch("/Statistic/GetDataForCommonStatistic?vehicleId=" + vehicleId)
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
                var parentCanvas = document.getElementById('ChartWrapper');
                parentCanvas.classList.add('pieChart');//присваиваем класс для контроля размером в css стилях
                if (parentCanvas.classList.contains('barChart')) {
                    parentCanvas.classList.remove('barChart')
                }
                var canvas = document.getElementById('Chart');
                if (typeof myChart != 'undefined') {//при каждом переходе к новому графику нужно удалять данные переменной chart и чистить canvas
                    const context = canvas.getContext('2d');
                    context.clearRect(0, 0, canvas.width, canvas.height);
                    var w = canvas.width;
                    canvas.width = 1;
                    canvas.width = w;
                    myChart.destroy();
                }
                 myChart = new Chart(
                    canvas,
                    config
                );
            })
}

