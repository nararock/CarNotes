﻿//переменная для хранения значений CarSystem и CarSubsystem
var system;
//Cookie
document.addEventListener("DOMContentLoaded", ready);
//выполняется при открытии страницы
function ready() {
    updateVehicleSelector();

    //добавление пустой строки в начало выпадающего списка с АЗС при создании события
    elem = document.getElementById('newRefuelWindow');
    var elemStation = elem.getElementsByTagName("form")[0].Station;
    var option = document.createElement('option');
    option.innerHTML = "";
    option.selected = true;
    option.disabled = true;
    elemStation.prepend(option);

    /**добавление календаря при выборе даты во всплывающих окнах при помощи функции библиотеки jquery
     * используется в окнах создания и редактирования событий
     * */
    $('.MyDateRangePicker').daterangepicker({
        singleDatePicker: true,
        showDropdowns: true,
        locale: {
            format: 'DD.MM.YYYY',
            "cancelLabel": "Отмена",
            "applyLabel": "Применить",
             "daysOfWeek": [
                "Вс",
                "Пн",
                "Вт",
                "Ср",
                "Чт",
                "Пт",
                "Сб"
            ],
            "monthNames": [
                "Январь",
                "Февраль",
                "Март",
                "Апрель",
                "Май",
                "Июнь",
                "Июль",
                "Август",
                "Сентябрь",
                "Октябрь",
                "Ноябрь",
                "Декабрь"
            ],
        }
    })
    //получение всех значений CarSystem и CarSubsystem
    fetch("/Repair/GetSystem")
        .then(response => response.json())
        .then((data) => { system = data; });
}

//установка куки при загрузке страницы (если не определено)/выбор автотранспорта из имеющего списка (при определенной куки)
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
//CreateRepairEvent and RepairEdit
//CarParts
function addCarPart(id)
{
    createTable(id);
}
/**формирование полей таблицы с системами и подсистемами для создания (или правки) события ремонта
 * @param id строка, id элемента (table) 
**/
function createTable(id) {
    var elem = document.getElementById(id);
    var elementTBody = elem.getElementsByTagName('tbody')
    var tableRow = document.createElement('tr');
    var amount = elementTBody[0].rows.length;
    createCellInput(tableRow, "Parts[" + amount + "].Name");
    createCellsSelect(tableRow, "Parts[" + amount + "].SubSystemId");
    createCellInput(tableRow, "Parts[" + amount + "].CarManufacturer");
    createCellInput(tableRow, "Parts[" + amount + "].Article");
    createCellInput(tableRow, "Parts[" + amount + "].Price");
    var cell = document.createElement('td');
    cell.innerHTML = "&times";
    cell.addEventListener("click", function () {
        elemSelect[0].style.display = "none";
        cell.parentElement.remove();
    })
    tableRow.appendChild(cell);
    elementTBody[0].append(tableRow);
}
/**создание элемента input для ячейки таблицы с системами и подсистемами
 * @param {any} tableRow ссылка на элемент строки в таблице
 * @param {any} name строка, название поля таблицы, для которого создается input
 */
function createCellInput(tableRow, name) {
    var cell = document.createElement('td');
    var input = document.createElement('input');
    cell.appendChild(input);
    input.name = name;
    tableRow.appendChild(cell);
}
/** 
 *  создание выпадающего списка для системы и подсистемы для событий типа создания и редактирования 
 *  @param tableRow ссылка на элемент строки в таблице
 *  @param name строка, название поля таблицы, для которого создается select
 *  @param val id (номер) подсистемы (используется при создании таблицы на странице редактирования события ремонта)
 * */
function createCellsSelect(tableRow, name, val) {
    var cellSystem = document.createElement('td');
    var cellSubsystem = document.createElement('td');
    var selectSystem = document.createElement('select');
    var selectSubsystem = document.createElement('select');
    selectSystem.onchange = changeSubsystem;
    selectSystem.name = name.replace('SubSystemId', 'SystemId');
    selectSubsystem.name = name;
    for (var i = 0; i < system.length; i++) {
        var optionSystem = document.createElement('option');
        optionSystem.innerHTML = system[i].Name;
        if (val != undefined && val.SystemId - 1 == i) {
            optionSystem.selected = true;
        }
        optionSystem.value = system[i].Id;
        selectSystem.append(optionSystem);
    }
    var index = 0; //по умолчанию при создании события выбрана первая система в списке
    if (val != undefined) {
        index = val.SystemId - 1;
    }
    for (var j = 0; j < system[index].CarSubsystems.length; j++) {
        var optionSubsystem = document.createElement('option');
        optionSubsystem.innerHTML = system[index].CarSubsystems[j].Name;
        optionSubsystem.value = system[index].CarSubsystems[j].Id;
        selectSubsystem.append(optionSubsystem);   
    }
    cellSystem.appendChild(selectSystem);
    cellSubsystem.append(selectSubsystem);
    tableRow.appendChild(cellSystem);
    tableRow.appendChild(cellSubsystem);
    
}
/**удаление списка подсистем из таблицы с деталями автотранспорта при срабатывании события типа сменя системы
 * @param selectElement ссылка на список подсистем, которые нужно удалить
 */
function removeOptions(selectElement) {
    var i, L = selectElement.options.length - 1;
    for (i = L; i >= 0; i--) {
        selectElement.remove(i);
    }
}
/**заполнение новыми подсистемами в select в таблице при событии смены системы 
 * 
 * @param {any} event передача элемента при нажатии которого, произошло событие смены системы при создании (редактировании) таблицы 
 */
function changeSubsystem(event) {
    var selectSystem = event.target;
    var selectSubsystem = selectSystem.parentElement.nextElementSibling.childNodes[0];
    var selectValueSystem = selectSystem.value;
    removeOptions(selectSubsystem);
    for (var j = 0; j < system[selectValueSystem - 1].CarSubsystems.length; j++) {
        var optionSubsystem = document.createElement('option');
        optionSubsystem.innerHTML = system[selectValueSystem - 1].CarSubsystems[j].Name;
        optionSubsystem.value = system[selectValueSystem - 1].CarSubsystems[j].Id;
        selectSubsystem.append(optionSubsystem);
    }
}
//Vehicle _BasicTemplate
//change select vehicle
/**функция срабатывает на событие смены транспортного средства, смена параметра транпсортного средства (vehicleId) в адресной строке
 * */
function changeData()
{
    var elem = document.getElementById('vehicleSelect');
    var vehicle = elem.value;
    var location = window.location;
    location.search = "?vehicleId=" + vehicle;
}
//CreateRefuelEvent and RefuelEdit
/**срабатывает на событие смены название АЗС в списке при создании или редактировании события заправки автотранспорта
 * открывает поле для ввода заправки, которой нет в приведенном списке АЗС при выборе соотвествующего option
 * 
 * @param {any} e параметр, который был выбран при событии (option)
 */
function changeSelectList(e)
{
    var inputStation = e.target;
    if (inputStation.value == "1") {
        inputStation.parentElement.nextElementSibling.style.display = "inline-block";
    }
    else if (inputStation.value != "1") {
        inputStation.parentElement.nextElementSibling.style.display = "none";
    }
}
//SubMenu BasicTemplate
/**срабатывает на событие нажатие на кнопку выбора события (Новая заправка или Новый ремонт) в шапке мастер страницы
 * делает видимым выбранное частичное представление, которое было скрыто
 * @param {any} str имя выбранного частичного представления в подменю
 */
//create new event
function popup(str) {
    var elem;
    if (str == "Новый ремонт")
        elem = document.getElementById('newRepairWindow');
    else if (str == "Новая заправка") {
        elem = document.getElementById('newRefuelWindow');
    }
    elem.style.display = 'inline-block';
}
//delete events
//Repair (Index)
/**
 * срабатывает на событие нажатия на "крестик" в таблице с событиями ремонта
 * @param {any} id номер события, которое нужно удалить из таблицы с ремонтами, вызывается соотвествующий метод контроллера Repair  
 */
function deleteRepair(id)
{
    document.location = "/Repair/Delete?id=" + id;
}

//Refuel (Index)
/**
 * срабатывает на событие нажатия на "крестик" в таблице с событиями заправки
 * @param {any} id номер события, которое нужно удалить из таблицы с заправками, вызывается соотвествующий метод контроллера Refuel
 */
function deleteRefuel(id)
{
    document.location = "/Refuel/Delete?id=" + id;
}

//Home (Index)
/**
 * срабатывает на событие нажатия на "крестик" в общей таблице с событиями заправки и ремонта, 
 * вызывается метод DeleteEvent 
 * @param {any} record название удаляемого события
 * @param {any} id номер удаляемого события в соотвествующей событию таблице
 */
function deleteCommon(record, id)
{
   document.location = "/Home/DeleteEvent?record=" + record + "&id=" + id;
}

//edit events
//RefuelEdit
/**
 * срабатывает на событие нажатия на знак редактирования ("карандаш") в таблице для редактирования событий заправки
 * открыает окно для редактирования события, где заполняет форму редактирования данными выбранного события (id)
 * @param {any} id номер события заправки для редактирования
 */
function editRefuel(id)
{
    fetch("/Refuel/Get?id=" + id)
        .then(response => response.json())
        .then((data) => {
            //console.log(data);
            var elementsForm = document.getElementById('formEdit');
            elementsForm.Date.value = data.Date;
            elementsForm.Mileage.value = data.Mileage;
            elementsForm.Fuel.value = data.Fuel;
            elementsForm.Station.value = data.Station;
            if (data.Station == 1) {
                elementsForm.CustomStation.value = data.CustomStation;
                elementsForm.CustomStation.parentElement.style.display = "inline-block";
            }
            else if (data.Station != 1 && elementsForm.CustomStation.parentElement.style.display != "none") {
                elementsForm.CustomStation.parentElement.style.display = "none";
            }
            elementsForm.Volume.value = data.Volume;
            elementsForm.PricePerOneLiter.value = data.PricePerOneLiter;
            elementsForm.FullTankCheckbox.checked = data.FullTank;
            elementsForm.ForgotRecordPreviousGasStationCheckbox.checked = data.ForgotRecordPreviousGasStation;
            elementsForm.Id.value = data.Id;
            document.getElementById('EditRefuelData').style.display = 'inline-block';
        }, () => {
                alert("Произошла ошибка");
        });
}

/**
 * срабатывает на событие нажатия на кнопку "Сохранить изменения" в окне редактирования события
 * устанавливает значение полей типа checkbox в параметре value для отправки формы при сохранении изменений
 * */
function RefuelEditSubmit()
{
    var elementsForm = document.getElementById('formEdit');
    elementsForm.children.FullTank.value = elementsForm.children.FullTankCheckbox.checked;
    elementsForm.children.ForgotRecordPreviousGasStation.value = elementsForm.children.ForgotRecordPreviousGasStationCheckbox.checked;
    return true;
}

//RepairEdit
/**
 * заполняет таблицу с деталями автротранспорта в окне редактирования события ремонта
 * создает input с заданным параметром в ячейке таблицы
 * @param {any} tableRow ссылка на строку в таблице
 * @param {any} value значение параметра 
 * @param {any} name название параметра
 */
function createCellRepairInput(tableRow, value, name) {
    var cell = document.createElement('td');
    var input = document.createElement('input');
    cell.appendChild(input);
    input.value = value;
    input.name = name;
    tableRow.appendChild(cell);
}

/**
 * срабатывает на событие нажатия на знак редактирования ("карандаш") в таблице для редактирования событий ремонта
 * открывает окно для редактирования события, где заполняет форму редактирования данными выбранного события (id)
 * @param {any} id номер события заправки для редактирования
 */
function editRepair(id)
{
    fetch("/Repair/Get?id=" + id)
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
            var elementTbody = elem.getElementsByTagName("tbody");
            for (var i = 0; i < data.Parts.length; i++) {
                var tableRow = document.createElement('tr');
                createCellRepairInput(tableRow, data.Parts[i].Name, "Parts[" + i + "].Name");
                //createCellRepairSelect(tableRow, data.Parts[i].CarSubsystemModel, "Parts[" + i + "].CarSubsystemModel");
                createCellsSelect(tableRow, "Parts[" + i + "].SubSystemId", data.Parts[i]);
                createCellRepairInput(tableRow, data.Parts[i].CarManufacturer, "Parts["+i+"].CarManufacturer");
                createCellRepairInput(tableRow, data.Parts[i].Article, "Parts["+i+"].Article");
                createCellRepairInput(tableRow, data.Parts[i].Price, "Parts[" + i + "].Price");
                /*ячейка со скрытым значением Id*/
                var inputId = document.createElement('input');
                inputId.name = "Parts[" + i +"].Id";
                inputId.type = "hidden";
                inputId.value = data.Parts[i].Id;
                /*скрытая ячейка с булевым значением удаляется ли ячейка */
                var inputDelete = document.createElement('input');
                inputDelete.type = "hidden";
                inputDelete.className = "inputDelete";
                inputDelete.name = "Parts[" + i +"].IsDeleted";
                /*ячейка с событием скрытия поля по нажатию крестик*/
                var cell = document.createElement('td');
                cell.innerHTML = "&times";
                cell.append(inputId);
                cell.append(inputDelete);
                cell.addEventListener("click", function (e) {
                    e.target.parentElement.style.display = "none";
                    var IsDeletedInput = e.target.getElementsByClassName("inputDelete");
                    IsDeletedInput[0].value = "true";
                })
                tableRow.append(cell);
                elementTbody[0].append(tableRow);
            }
            document.getElementById('EditRepairData').style.display = 'inline-block';
            }, () => {
                alert("Произошла ошибка");
            });
}

//Home (Index)
/**
 * срабатывает на событие нажатия на знак редактирования ("карандаш") в общей таблице 
 * в зависимости от параметра record вызывает соотвествующую функцию для редавтирования события
 * @param {any} record название редактируемого события
 * @param {any} id номер события в соотвествующей таблице событий
 */
function editCommon(record, id)
{
    if (record == 'Refuel') {
        editRefuel(id);
    }
    else if (record == 'Repair')
    {
        editRepair(id);
    }
}

/*close windows ("cross")*/
/**
 * скрывает форму соотвествующего события при нажатии на крестик
 * @param {any} event элемент из соотвествующего окна, по которому произошло событие нажатия
 */
function closeWindow(event)
{
    var elem = event.target;
    var parent = elem.closest('.modal');
    parent.style.display = "none";
}
/*clear the form fields before closing*/
/**
 * срабатывает на события нажатия на крестик в окнах создания или редактирования событий
 * очищает форму перед удалением 
 * @param {any} event элемент из соотвествующего окна, по которому произошло событие нажатия 
 */
function clearEvent(event) {
    var element = event.target;
    var modalParent = element.closest('.modal');
    var formElements = modalParent.getElementsByTagName('form');
    formElements[0].reset();
    if (formElements[0].getElementsByTagName('tbody').length > 0) {
        formElements[0].getElementsByTagName('tbody')[0].innerHTML = "";
    }
    closeWindow(event);
}