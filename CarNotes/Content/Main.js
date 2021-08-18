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
    var amount = elemTable.children.length  - 1;
    createCell(tableRow, "Parts[" + amount + "].Name");
    createCell(tableRow, "Parts[" + amount + "].CarManufacturer");
    createCell(tableRow, "Parts[" + amount + "].Article");
    createCell(tableRow, "Parts[" + amount + "].Price");
    var cell = document.createElement('td');
    cell.innerHTML = "&times";
    cell.addEventListener("click", function () {
        cell.parentElement.remove();
    })
    tableRow.appendChild(cell);
    elemTable.appendChild(tableRow);
}

function createCell(tableRow, name) {
    var cell = document.createElement('td');
    var input = document.createElement('input');
    cell.appendChild(input);
    input.name = name;
    tableRow.appendChild(cell);
}

//change select
function changeData()
{
    var elem = document.getElementById('vehicleSelect');
    var vehicle = elem.value;
    var location = window.location;
    location.search = "?vehicleId=" + vehicle;
}

//delete events
function deleteRepair(id)
{
    document.location = "/Repair/Delete?id=" + id;
}

function deleteRefuel(id)
{
    document.location = "/Refuel/Delete?id=" + id;
}

function deleteCommon(record, id)
{
   document.location = "/Home/DeleteEvent?record=" + record + "&id=" + id;
}

//edit events
function editRefuel(id)
{
    fetch("/Refuel/RefuelEdit?id=" + id)
        .then(response => response.json())
        .then((data) => {
            console.log(data);
            var elementsForm = document.getElementById('formEdit');
            elementsForm.children.Date.value = data.Date;
            elementsForm.children.Mileage.value = data.Mileage;
            elementsForm.children.Fuel.value = data.Fuel;
            elementsForm.children.Station.value = data.Station;
            elementsForm.children.Volume.value = data.Volume;
            elementsForm.children.PricePerOneLiter.value = data.PricePerOneLiter;
            elementsForm.children.FullTankCheckbox.checked = data.FullTank;
            elementsForm.children.ForgotRecordPreviousGasStationCheckbox.checked = data.ForgotRecordPreviousGasStation;
            elementsForm.children.Id.value = data.Id;
            document.getElementById('EditRefuelData')[0].style.display = 'inline-block';
        }, () => {
                alert("Произошла ошибка");
        });
}

function RefuelEditSubmit()
{
    var elementsForm = document.getElementById('formEdit');
    elementsForm.children.FullTank.value = elementsForm.children.FullTankCheckbox.checked;
    elementsForm.children.ForgotRecordPreviousGasStation.value = elementsForm.children.ForgotRecordPreviousGasStationCheckbox.checked;
    return true;
}

function createCellRepairTable(tableRow, value) {
    var cell = document.createElement('td');
    var input = document.createElement('input');
    cell.appendChild(input);
    input.value = value;
    tableRow.appendChild(cell);
}

function editRepair(id)
{
    fetch("/Repair/RepairEdit?id=" + id)
        .then(response => response.json())
        .then((data) => {
            var elementsForm = document.getElementById('RepairFormEdit');
            elementsForm.children.Date.value = data.Date;
            elementsForm.children.Mileage.value = data.Mileage;
            elementsForm.children.Repair.value = data.Repair;
            elementsForm.children.CarService.value = data.CarService;
            elementsForm.children.RepairCost.value = data.RepairCost;
            elementsForm.children.Comments.value = data.Comments;
            elementsForm.children.Id.value = data.Id;
            var elem = document.getElementById("EditRepairData");
            var elementTable = elem.getElementsByTagName("table");
            for (var i = 0; i < data.Parts.length; i++) {
                var tableRow = document.createElement('tr');
                createCellRepairTable(tableRow, data.Parts[i].Name);
                createCellRepairTable(tableRow, data.Parts[i].CarManufacturer);
                createCellRepairTable(tableRow, data.Parts[i].Article);
                createCellRepairTable(tableRow, data.Parts[i].Price);
                var cell = document.createElement('td');
                cell.innerHTML = "&times";
                cell.addEventListener("click", function () {
                    cell.parentElement.remove();
                })
                tableRow.append(cell);
                elementTable[0].append(tableRow);
            }
            document.getElementById('EditRepairData').style.display = 'inline-block';
            }, () => {
                alert("Произошла ошибка");
            });
}