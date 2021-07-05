//Cookie
document.addEventListener("DOMContentLoaded", ready);
function ready() {
    updateVehicleSelector();

    var elem = document.getElementById('addCarPart');
    elem.addEventListener("click", createTable);
}

function updateVehicleSelector() {
    var elem = document.getElementById('vehicleSelect');
    var vehicleIdCookie = getCookie('vehicleId');
    if (elem.children.length == 0) { return; }
    if (vehicleIdCookie == undefined) {
        elem.children[0].selected = true;
        setCookie('vehicleId', elem.value);
    }
    else if (vehicleIdCookie != undefined) {
        elem.value = vehicleIdCookie;
    }
    elem.addEventListener("change", function () {
        setCookie('vehicleId', elem.value);
    })
}

//CarParts
function createTable() {
    var elemTable = document.getElementById('carPartsTable');
    var tableRow = document.createElement('tr');
    var amountTh = elemTable.children[0].children[0].cells.length;
    for (var i = 0; i < amountTh; i++) {
        var cell = document.createElement('th');
        var input = document.createElement('input');
        cell.appendChild(input);
        tableRow.appendChild(cell);
    }
    var cell = document.createElement('th');
    cell.innerHTML = "&times";
    cell.addEventListener("click", function () {
        cell.parentElement.remove();
    })
    tableRow.appendChild(cell);
    elemTable.appendChild(tableRow);
}

