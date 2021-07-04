document.addEventListener("DOMContentLoaded", ready);
function ready() {
    var vehicleIdCookie = getCookie('vehicleId');
    var elem = document.getElementById('vehicleSelect');
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