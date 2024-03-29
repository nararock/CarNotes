﻿//переменная для хранения значений CarSystem и CarSubsystem
var system;
//Cookie
document.addEventListener("DOMContentLoaded", ready);
//выполняется при открытии страницы
function ready() {
    updateVehicleSelector();

    $('.vehicleSelect').dropdown();
    $('.myPopupClass').popup({
    });

    $('#mobile-header .dropdown').dropdown({
        displayType: 'block'
    });

    //добавление пустой строки в начало выпадающего списка с АЗС при создании события
    elem = document.getElementById('newRefuelWindow');
    var elemStation = elem.getElementsByTagName("form")[0].Station;
    elemStation.selectedIndex = -1;
    
    /**
     * получение всех значений CarSystem и CarSubsystem
     * */
    fetch("/Repair/GetSystem")
        .then(response => response.json())
        .then((data) => { system = data; });
}

/**
 * установка куки при загрузке страницы (если не определено)/выбор автотранспорта из имеющего списка (при определенной куки)
 * */
function updateVehicleSelector() {
    var vehicleIdCookie =  getCookie('vehicleId');
    var select = document.getElementsByClassName('vehicleSelect');
    if (select.length == 0) return;
    if (vehicleIdCookie == undefined) { return;}
    $('.vehicleSelect').dropdown('set selected', vehicleIdCookie);
}

/**
 * активирует календарь
 * @param {any} calendarDate дата начала календаря
 */
function activeCalendar(calendarDate)
{
   $('.MyDateRangePicker').daterangepicker({
        singleDatePicker: true,
        showDropdowns: true,
        minYear: 1999,
        maxYear: (new Date().getFullYear()) + 1,
        autoApply: true,
        startDate: calendarDate,
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
                "Сб",                
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
            "firstDay": 1
        }
    })
}

//CreateRepairEvent and RepairEdit
//CarParts
/**формирование полей таблицы с системами и подсистемами для создания (или правки) события ремонта
 * @param id строка, id элемента (table) 
**/
function addCarPart() {
    var mainTable = document.querySelector("#repairPartsTable");
    mainTable.style.display = '';
    var amount = mainTable.childElementCount;
    var cloneTable = document.querySelector("#clone-repairPartsTable tr");
    var clone = cloneTable.cloneNode(true);
    mainTable.append(clone);    
    createCellInput(clone, "Name", amount);
    createCellsSelect(clone, "Parts[" + amount + "]");
    createCellInput(clone, "CarManufacturer", amount);
    createCellInput(clone, "Article", amount);
    createCellInput(clone, "Price", amount);
    var button = clone.querySelector('.deleteCarPartButton');
    button.addEventListener("click", function () {
        if (button.closest('table').children.length == 1) {
            button.closest('table').style.display = "none";
        }
        button.closest('tr').remove();
    });
    //опустить скролл при добавлении строчки таблицы
    var elementContent = document.getElementsByClassName("scrolling content repair");
    elementContent[0].scrollTop = elementContent[0].scrollHeight;
}
/**создание элемента input для ячейки таблицы с системами и подсистемами
 * @param {any} tableRow ссылка на элемент строки в таблице
 * @param {any} name строка, название поля таблицы, для которого создается input
 */
function createCellInput(element, fieldName, index) {
    var input = element.querySelector("[name=" + fieldName + "]");
    input.name = "Parts[" + index + "]." + fieldName;
}
/** 
 *  заполнение выпадающего списка значениями для систем и подсистем ТС при создании и редактировании события
 *  @param element ссылка на таблицу
 *  @param name строка, название поля таблицы, для которого создается select
 *  @param value id (номер) подсистемы (используется при создании таблицы на странице редактирования события ремонта)
 * */
function createCellsSelect(element, name, value) {
    var selectSystem = element.querySelector("[name=" + "System" + "]");
    var selectSubsystem = element.querySelector("[name=" + "Subsystem" + "]");
    selectSystem.name = name + '.SystemId';
    selectSubsystem.name = name + '.SubSystemId';
    for (var i = 0; i < system.length; i++) {
        var optionSystem = document.createElement('option');
        optionSystem.innerHTML = system[i].Name;
        if (value != undefined && value.SystemId - 1 == i) {
            optionSystem.selected = true;
        }
        optionSystem.value = system[i].Id;
        selectSystem.append(optionSystem);
    }
    var index = 0; //по умолчанию при создании события выбрана первая система в списке
    if (value != undefined) {
        index = value.SystemId - 1;
    }
    for (var j = 0; j < system[index].CarSubsystems.length; j++) {
        var optionSubsystem = document.createElement('option');
        optionSubsystem.innerHTML = system[index].CarSubsystems[j].Name;
        if (value != undefined && value.SubSystemId == system[index].CarSubsystems[j].Id) {
            optionSubsystem.selected = true;
        }
        optionSubsystem.value = system[index].CarSubsystems[j].Id;
        selectSubsystem.append(optionSubsystem);
    }
}
/**удаление списка подсистем из таблицы с деталями автотранспорта при срабатывании события типа сменя системы
 * @param selectElement ссылка на список подсистем, которые нужно удалить
 */
function removeOptions(selectElement) {

    var i, L = selectElement.getElementsByTagName("option").length - 1;
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
    var tableRow = selectSystem.closest('tr');
    var selectSubsystem = tableRow.querySelector('.Subsystem');
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
function changeData(Id) {
    setCookie('vehicleId', Id);
    var location = window.location;
    location.search = "?vehicleId=" + Id;
}
//CreateRefuelEvent and RefuelEdit
/**срабатывает на событие смены название АЗС в списке при создании или редактировании события заправки автотранспорта
 * открывает поле для ввода заправки, которой нет в приведенном списке АЗС при выборе соотвествующего option
 * 
 * @param {any} e параметр, который был выбран при событии (option)
 */
function changeSelectListCreate(e) {
    var inputStation = e.target;
    if (inputStation.value == "1") {
        $(inputStation.parentElement.nextElementSibling).slideDown();
       
    }
    else if (inputStation.value != "1") {
        inputStation.parentElement.nextElementSibling.querySelector('input').value = '';
        $(inputStation.parentElement.parentElement.nextElementSibling).slideUp();
        
    }
}

/**
 * срабатывает при изменении поля АЗС (select) в окне редактирования заправки
 * @param {any} e номер выбранной заправки
 */

function changeSelectListEdit(e) {
    var inputStation = e.target;
    if (inputStation.value == "1") {
        $(inputStation.parentElement.nextElementSibling).slideDown();
    }
    else if (inputStation.value != "1") {
        inputStation.parentElement.nextElementSibling.querySelector('input').value = '';
        $(inputStation.parentElement.nextElementSibling).slideUp();
    }
}

//SubMenu BasicTemplate
/**срабатывает на событие нажатие на кнопку выбора события (Новая заправка или Новый ремонт) в шапке мастер страницы
 * делает видимым выбранное частичное представление, которое было скрыто
 * @param {any} str имя выбранного частичного представления в подменю
 * @param {any} vehicleId id транспортного средства, для которого создается новое событие
 */
//create new event
function popup(str, vehicleId) {
    if (str == "Новый ремонт")
        editRepair(0, true, vehicleId);

    else if (str == "Новая заправка") {
        $('#newRefuelWindow')
            .modal({
                autofocus: false,
                onApprove: function () {
                    RefuelCreateSubmit();
                    document.getElementById("newRefuelWindow").getElementsByTagName("form")[0].submit();
                },
                onVisible: () => {
                    $("#newRefuelWindow .ui.checkbox").checkbox();
                },
                onHidden: () => {
                    $("#newRefuelWindow form")[0].reset();
                }
            })
        fetch("/Refuel/GetDataForCreateEvent?vehicleId=" + vehicleId)
            .then(response => response.json())
            .then((data) => {
                document.getElementById("formCreate").querySelector("[name=" + "Mileage" + "]").value = data.LastMileage;
                document.getElementById("formCreate").querySelector("[name=" + "Fuel" + "]").value = data.LastFuel;
                $('#newRefuelWindow').modal('show');
            });
        activeCalendar(new Date().getDate());
    }
    else if (str == "Добавить новое транспортное средство") {
        $('#AddVehicle')
            .modal({
                onApprove: function () {
                    document.getElementById("AddVehicle").getElementsByTagName("form")[0].submit();
                }
            })
            .modal('show');
    }
    else if (str = "Новый расход") {
        $('#ExpenseEdit')
            .modal({
                autofocus: false,
                onApprove:
                    function () {
                        document.getElementById("ExpenseEdit").getElementsByTagName("form")[0].submit();
                    },
            }
            )
            .modal('show');
        fetch("/Expense/GetLastMileage?vehicleId=" + vehicleId)
            .then(response => response.text())
            .then((data) => {
                console.log(data);
                document.getElementById("formCreateExpense").querySelector("[name=" + "Mileage" + "]").value = data;
            })
        var header = document.querySelector('#ExpenseEdit .header');
        header.innerHTML = "Новый расход";
        activeCalendar(new Date().getDate());
    }
}

function RefuelCreateSubmit() {
    var elementsForm = document.getElementById('formCreate');
    elementsForm.children.FullTank.value = elementsForm.children[7].getElementsByTagName('input')[0].checked;
    elementsForm.children.ForgotRecordPreviousGasStation.value = elementsForm.children[9].getElementsByTagName('input')[0].checked;
    return true;
}

//delete events
//Repair (Index)
/**
 * срабатывает на событие нажатия на "крестик" в таблице с событиями ремонта
 * @param {any} id номер события, которое нужно удалить из таблицы с ремонтами, вызывается соотвествующий метод контроллера Repair  
 */
function deleteRepair(id) {
    if (confirm("Удалить эту запись о ремонте?")) {
        document.location = "/Repair/Delete?id=" + id;
    }   
}

//Refuel (Index)
/**
 * срабатывает на событие нажатия на "крестик" в таблице с событиями заправки
 * @param {any} id номер события, которое нужно удалить из таблицы с заправками, вызывается соотвествующий метод контроллера Refuel
 */
function deleteRefuel(id) {
    if (confirm("Удалить эту запись о заправке?")) {
        document.location = "/Refuel/Delete?id=" + id;
    }    
}

//Expense (Index)
/**
 * срабатывает на событие нажатия на "крестик" в таблице с событиями расходы
 * @param {any} id id номер события, которое нужно удалить из таблицы с заправками, вызывается соотвествующий метод контроллера Expense
 */
function deleteExpense(id) {
    if (confirm("Удалить эту запись о расходе?")) {
        document.location = "/Expense/Delete?id=" + id;
    }
}

//Common (Index)
/**
 * срабатывает на событие нажатия на "крестик" в общей таблице с событиями заправки и ремонта, 
 * вызывается метод DeleteEvent 
 * @param {any} record название удаляемого события
 * @param {any} id номер удаляемого события в соотвествующей событию таблице
 */
function deleteCommon(record, id) {
    var event = (record == "Refuel") ? "заправку" : "ремонт";
    if (confirm("Удалить " + event + "?")) {
        document.location = "/Common/DeleteEvent?record=" + record + "&id=" + id;
    }
}

/**
 * перейти на следующую страницу
 * @param {any} vehicleId
 * @param {any} pageNumber номер страницы, на которую надо реализовать переход
 */

function toNextPage(vehicleId, namePage, pageNumber)
{
    if (namePage == "Общая таблица") {
        document.location = "/Common/Index?vehicleId=" + vehicleId + "&pageNumber=" + pageNumber;
    }
    else if (namePage == "Заправки") {
        document.location = "/Refuel/Index?vehicleId=" + vehicleId + "&pageNumber=" + pageNumber;
    }
    else if (namePage == "Ремонты") {
        document.location = "/Repair/Index?vehicleId=" + vehicleId + "&pageNumber=" + pageNumber;
    }
    else if (namePage == "Расходы")
    {
        document.location = "/Expense/Index?vehicleId=" + vehicleId + "&pageNumber=" + pageNumber;
    }
}

//edit events

/**
 * изменение формы редактирования (только для просмотра)
 * @param {any} form id формы 
 * @param {any} nameForm новое название формы
 */
function toggleForm(form, nameForm) {
    form.classList.add('justText');
    var inputs = form.getElementsByTagName('input');
    for (var l = 0; l < inputs.length; l++) {
        inputs[l].setAttribute('disabled', 'disabled');
    }
    var buttons = form.parentElement.nextElementSibling.getElementsByClassName('button');
    for (var m = 0; m < buttons.length; m++) {
        buttons[m].style.display = 'none';
    }
    var name = form.parentElement.previousElementSibling;
    name.innerText = nameForm;
}

//RefuelEdit
/**
 * срабатывает на событие нажатия на знак редактирования ("карандаш") в таблице для редактирования событий заправки
 * открывает окно для редактирования события, где заполняет форму редактирования данными выбранного события (id)
 * @param {any} id номер события заправки для редактирования
 */
function editRefuel(id, Verified) {
    if (Verified) {
        fetch("/Refuel/Get?id=" + id)
            .then(response => response.json())
            .then((data) => {
                $('#EditRefuelData').modal({
                    autofocus: false,
                    onVisible: () => {
                        $('#EditRefuelData .ui.checkbox').checkbox();
                    },
                    onApprove: function () {
                        RefuelEditSubmit();
                        document.getElementById('EditRefuelData').getElementsByTagName("form")[0].submit();
                    }
                })
                    .modal('show');
                /*активируем календарь*/
                activeCalendar(data.Date);

                var elementsForm = document.getElementById('formEdit');
                elementsForm.Date.value = data.Date;
                elementsForm.Mileage.value = data.Mileage;
                elementsForm.Fuel.value = data.Fuel;
                elementsForm.Station.value = data.Station;
                if (data.Station == 1) {
                    elementsForm.CustomStation.value = data.CustomStation;
                    elementsForm.CustomStation.parentElement.style.display = "block";
                }
                else if (data.Station != 1 && elementsForm.CustomStation.parentElement.style.display != "none") {
                    elementsForm.CustomStation.parentElement.style.display = "none";
                }
                elementsForm.Volume.value = data.Volume;
                elementsForm.PricePerOneLiter.value = data.PricePerOneLiter;
                elementsForm.FullTankCheckbox.checked = data.FullTank;
                elementsForm.ForgotRecordPreviousGasStationCheckbox.checked = data.ForgotRecordPreviousGasStation;
                elementsForm.Id.value = data.Id;
            }, () => {
                alert("Произошла ошибка");
            });
    } else refuelShow(id);
    
}
/**
 * вызов происходит в режиме неверифицированного пользователя при нажатии значка "лупа"
 * @param {any} id id пользователя, чьи события просматриваются
 */
function refuelShow(id) {
    fetch("/Refuel/Get?id=" + id)
        .then(response => response.json())
        .then((data) => {
            $('#EventShow').modal({})
                .modal('show');
            var refuelDiv = document.getElementById("EventShow");
            var header = refuelDiv.getElementsByClassName("header");
            if (data.Station == 1) header[0].innerHTML = "Заправка " + data.CustomStation;
            else header[0].innerHTML = "Заправка " + data.StationName;
            var description = refuelDiv.getElementsByClassName("description");
            description[0].innerHTML = "<strong>Дата: </strong>" + data.Date + "<br>";
            description[0].innerHTML += "<strong>Пробег: </strong>" + data.Mileage + " км"+ "<br>"; 
            description[0].innerHTML += "<strong>Тип топлива: </strong>" + data.FuelName + "<br>"; 
            description[0].innerHTML += data.FullTank == true ? "<strong>Объем: </strong>" + data.Volume + " л, полный бак" + "<br>" : "<strong>Объем: </strong>" + data.Volume + "<br>";
            description[0].innerHTML += "<strong>Стоимость: </strong>" + Math.round(data.Cost) + " руб." + "<br>"; 
            if (data.ForgotRecordPreviousGasStation) {
                description[0].innerHTML += "<strong>Забыл внести предыдущую заправку</strong>";
            }
        })
}

/**
 * срабатывает на событие нажатия на кнопку "Сохранить изменения" в окне редактирования события
 * устанавливает значение полей типа checkbox в параметре value для отправки формы при сохранении изменений
 * */
function RefuelEditSubmit() {
    var elementsForm = document.getElementById('formEdit');
    elementsForm.children.FullTank.value = elementsForm.querySelector("[name=FullTankCheckbox]").checked;
    elementsForm.children.ForgotRecordPreviousGasStation.value = elementsForm.querySelector("[name=ForgotRecordPreviousGasStationCheckbox]").checked;
    return true;
}

//ExpenseEdit
/**
 * срабатывает на событие нажатия на знак редактирования ("карандаш") в таблице для редактирования событий расходы
 * открывает окно для редактирования события, где заполняет форму редактирования данными выбранного события (id)
 * */
function editExpense(id, Verified) {
    if (Verified) {
         fetch("/Expense/Get?id=" + id)
                .then(response => response.json())
                .then((data) => {
                    $('#ExpenseEdit').modal({
                        autofocus: false,
                        onVisible: () => {
                            $('#ExpenseEdit .ui.checkbox').checkbox();
                        },
                        onApprove: function () {
                            document.getElementById('ExpenseEdit').getElementsByTagName("form")[0].submit();
                        }
                    })
                        .modal('show');

                    activeCalendar(data.Date);
                        
                    var elementsForm = document.getElementById("formEditExpense");
                    elementsForm.Date.value = data.Date;
                    elementsForm.Mileage.value = data.Mileage;
                    elementsForm.TypeId.value = data.TypeId;
                    elementsForm.Sum.value = data.Sum;
                    elementsForm.Description.value = data.Description;
                    elementsForm.Comment.value = data.Comment;
                    elementsForm.Id.value = data.Id;
                }) 
    }
    else expenseShow(id);
      
}

function expenseShow(id) {
    fetch("/Expense/Get?id=" + id)
        .then(response => response.json())
        .then((data) => {
            $('#EventShow').modal({})
                .modal('show');
            var expenseDiv = document.getElementById("EventShow");

            var header = expenseDiv.getElementsByClassName("header");
            header[0].innerHTML = data.TypeName;

            var description = expenseDiv.getElementsByClassName("description");
            description[0].innerHTML = "<strong>Дата: </strong>" + data.Date + "<br>";
            if (data.Mileage != null) {
                description[0].innerHTML += "<strong>Пробег: </strong>" + data.Mileage + "<br>";
            }            
            description[0].innerHTML += "<strong>Сумма: </strong>" + data.Sum + "<br>";
            if (data.Description != null) {
                description[0].innerHTML += "<strong>Описание: </strong>" + data.Description + "<br>";
            }
            if (data.Comment != null) {
                description[0].innerHTML += "<strong>Комментарий: </strong>" + data.Comment;
            }
        })          
                
}

//RepairEdit
/**
 * преобразует склонированный input к правильному и заполняет его данными
 * @param {any} element ссылка на таблицу 
 * @param {any} oldName имя поля, которое заполняется данными 
 * @param {any} newName новое имя для поля
 * @param {any} newValue новое значение для поля
 */
function createCellRepairInput(element, oldName, newValue, newName) {
    var input = element.querySelector("[name=" +oldName+ "]");
    input.name = newName;
    input.value = newValue;
}

/**
 * срабатывает на событие нажатия на знак редактирования ("карандаш") в таблице для редактирования событий ремонта
 * открывает окно для редактирования события, где заполняет форму редактирования данными выбранного события (id)
 * @param {any} id номер события заправки для редактирования
 * @param {any} Verified true - если пользователь не авторизован
 */
function editRepair(id, Verified, vehicleId) {
    if (!Verified) repairShow(id, vehicleId)
    else {
        fetch("/Repair/Get?id=" + id + "&vehicleId=" + vehicleId)
            .then(response => response.json())
            .then((data) => {
                $('#EditRepairData').modal({
                    autofocus: false,
                    onApprove: function () {
                        var systemSelect = document.getElementById('EditRepairData').getElementsByClassName("System")[0];
                        var subsystemSelect = document.getElementById('EditRepairData').getElementsByClassName("Subsystem")[0];
                        var errorString = [];
                        if (systemSelect.value == '')
                            errorString.push('систему');
                        if (subsystemSelect.value == '')
                            errorString.push('подсистему')
                        if (errorString.length != 0) {
                            toastr.error('Вы не выбрали ' + errorString.join(' и '));
                            return false;                            
                        }
                        document.getElementById('EditRepairData').getElementsByTagName("form")[0].submit();
                    },
                    onHide: function () {
                        clearEvent("RepairFormEdit");
                    }
                }).modal('show');

                if (id == 0) {
                    var header = document.querySelector('#EditRepairData .header');
                    header.innerHTML = "Новый ремонт";
                    activeCalendar(new Date().getDate());
                } else { activeCalendar(data.Date); }
                var elementsForm = document.getElementById('RepairFormEdit');
                elementsForm.Date.value = data.Date;
                elementsForm.Mileage.value = data.Mileage;
                elementsForm.Repair.value = data.Repair;
                elementsForm.CarService.value = data.CarService;
                elementsForm.RepairCost.value = data.RepairCost;
                elementsForm.Comments.value = data.Comments;
                elementsForm.Id.value = data.Id;
                var mainTable = document.querySelector("#repairPartsTable");
                
                    mainTable.innerHTML = '';
                    var cloneTable = document.querySelector("#clone-repairPartsTable tr");
                    for (var i = 0; i < data.Parts.length; i++) {
                        var clone = cloneTable.cloneNode(true);
                        mainTable.append(clone);
                        createCellRepairInput(mainTable, "Name", data.Parts[i].Name, "Parts[" + i + "].Name");
                        createCellsSelect(mainTable, "Parts[" + i + "]", data.Parts[i]);
                        createCellRepairInput(mainTable, "CarManufacturer", data.Parts[i].CarManufacturer, "Parts[" + i + "].CarManufacturer");
                        createCellRepairInput(mainTable, "Article", data.Parts[i].Article, "Parts[" + i + "].Article");
                        createCellRepairInput(mainTable, "Price", data.Parts[i].Price, "Parts[" + i + "].Price");
                        /*ячейка со скрытым значением Id*/
                        var inputId = document.createElement('input');
                        inputId.name = "Parts[" + i + "].Id";
                        inputId.type = "hidden";
                        inputId.value = data.Parts[i].Id;
                        /*скрытая ячейка с булевым значением удаляется ли ячейка */
                        var inputDelete = document.createElement('input');
                        inputDelete.type = "hidden";
                        inputDelete.className = "inputDelete";
                        inputDelete.name = "Parts[" + i + "].IsDeleted";
                        /*ячейка с событием скрытия поля по нажатию крестик*/
                        var button = mainTable.querySelector('.deleteCarPartButton');
                        button.append(inputId);
                        button.append(inputDelete);
                        button.addEventListener("click", function (e) {
                            var td = e.target.closest('td');
                            var IsDeletedInput = td.getElementsByClassName("inputDelete");
                            IsDeletedInput[0].value = "true";
                            td.style.display = "none";
                        })                  
                    if (data.Parts.length > 0) {
                        mainTable.style.display = '';
                    }
                }
            }, () => {
                alert("Произошла ошибка");
            });
    }
}

function repairShow(id, vehicleId) {
    fetch("/Repair/Get?id=" + id + "&vehicleId=" + vehicleId)
        .then(response => response.json())
        .then((data) => {
            $('#EventShow').modal({})
                .modal('show');
            var expenseDiv = document.getElementById("EventShow");

            var header = expenseDiv.getElementsByClassName("header");
            header[0].innerHTML = data.Repair;

            var description = expenseDiv.getElementsByClassName("description");
            description[0].innerHTML = "<strong>Дата: </strong>" + data.Date + "<br>";
            if (data.Mileage != null) {
                description[0].innerHTML += "<strong>Пробег: </strong>" + data.Mileage + " км <br>";
            }
            var commonCostRepair = data.RepairCost;
            data.Parts.forEach(p => commonCostRepair += p.Price);
            description[0].innerHTML += "<strong>Общая стоимость: </strong>" + commonCostRepair + " руб. <br>";
            description[0].innerHTML += "<strong>Стоимость работ: </strong>" + data.RepairCost + " руб. <br>";
            description[0].innerHTML += "<strong>Сервис: </strong>" + data.CarService + "<br>";
            if (data.Comments != null) {
                description[0].innerHTML += "<strong>Комментарий: </strong>" + data.Comments + "<br>";
            }
            if (data.Parts.length > 0) {
                var ol = document.createElement('ol');
                description[0].innerHTML += "<strong>Запчасти: </strong>";
                for (var i = 0; i < data.Parts.length; i++) {
                    var li = document.createElement('li');
                    var nameSubsystem;
                    var Subsystem = system[data.Parts[i].SystemId - 1].CarSubsystems;
                    for (var j = 0; j < Subsystem.length; j++) {
                        if (Subsystem[j].Id == data.Parts[i].SubSystemId) {
                            nameSubsystem = Subsystem[j].Name;
                        }
                    }
                    li.innerHTML += (data.Parts[i].Name == null ? "" : data.Parts[i].Name) + "  (" + system[data.Parts[i].SystemId - 1].Name
                        + " / " + nameSubsystem + ") <br>" + "  " + (data.Parts[i].CarManufacturer == null ? "" : data.Parts[i].CarManufacturer)
                        + "  " + (data.Parts[i].Article == null ? "" : data.Parts[i].Article) + " за " +
                        data.Parts[i].Price + " руб. <br>";
                    ol.append(li);
                }
                description[0].append(ol);
            }           
        })
}
/**
 * срабатывает при нажатии на кнопку "отмена" или крестик в окне редактирования и создания ремонта,
 * скрывает таблицу с запчастями
 */
function hidePartsTable()
{
    var partsTable = document.querySelector("#repairPartsTable");
    partsTable.style.display = "none";
}

function editVehicle(Id) {
    fetch("/Vehicle/Get?Id=" + Id)
        .then(response => response.json())
        .then((data) => {
            $("#EditVehicleData").modal({
                onApprove: function () {
                    document.getElementById('EditVehicleData').getElementsByTagName("form")[0].submit();
                },
                onHide: function () {
                    clearEvent("VehicleFormEdit");
                }
            }).modal("show");
            var elementsForm = document.getElementById('VehicleFormEdit');
            elementsForm.Brand.value = data.Brand;
            elementsForm.Model.value = data.Model;
            elementsForm.ReleaseYear.value = data.ReleaseYear;
            elementsForm.Color.value = data.Color;
            elementsForm.Body.value = data.Body;
            elementsForm.Id.value = data.Id;
        }, () => {
            alert("Произошла ошибка");
        });
}
//Common (Index)
/**
 * срабатывает на событие нажатия на знак редактирования ("карандаш") в общей таблице 
 * в зависимости от параметра record вызывает соотвествующую функцию для редавтирования события
 * @param {any} record название редактируемого события
 * @param {any} id номер события в соотвествующей таблице событий
 */
function editCommon(record, id) {
    if (record == 'Refuel') {
        editRefuel(id, true);
    }
    else if (record == 'Repair') {
        editRepair(id, true);
    }
    else if (record == 'Expense') {
        editExpense(id, true);
    }
}

/**
 * нажатие на кнопку "лупа" в режиме неавторизованного пользователя,
 * для просмотра в детялах события ремонта или заправка
 * @param {any} record название события (ремонт или заправка)
 * @param {any} id статус неавторизованного пользователя (true - не авторизован)
 */
function showCommon(record, id) {
    if (record == 'Refuel') {
        editRefuel(id, false);
    }
    else if (record == 'Repair') {
        editRepair(id, false);
    }
    else if (record == 'Expense') {
        editExpense(id, false);
    }
}

/**
 * срабатывает на события нажатия на крестик в окнах создания или редактирования событий
 * очищает форму перед удалением 
 * @param {any} event элемент из соотвествующего окна, по которому произошло событие нажатия 
 */
function clearEvent(IdForm) {
    var element = document.getElementById(IdForm);
    element.reset();
}

/**
 * удаление транспортного средства из гаража
 * @param {any} id номер ТС в БД
 */
function deleteVehicle(id) {
    document.location = "/Vehicle/Delete?id=" + id;
}

//Home page 
/**
 * переход в общую таблицу личного кабинета
 */
function goToCommonTable() {
    document.location = "/Common/Index";
}

/**
 * переход на главную страницу
 * */
function goToHomePage() {
    document.location = "/";
}


function goToRegisterPage() {
    document.location = "/Registration/Index";
}