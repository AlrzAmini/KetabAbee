$(function () {
    var areas = document.getElementsByTagName("TEXTAREA");
    for (var i = 0; i < areas.length; i++) {
        areas[i].oninvalid = function (e) {
            e.target.setCustomValidity("");
            if (!e.target.validity.valid) {
                e.target.setCustomValidity(e.target.getAttribute("data-error"));
            }
        };
    }
});
$(function () {
    var inputs = document.getElementsByTagName("INPUT");
    for (var i = 0; i < inputs.length; i++) {
        inputs[i].oninvalid = function (e) {
            e.target.setCustomValidity("");
            if (!e.target.validity.valid) {
                e.target.setCustomValidity(e.target.getAttribute("data-error"));
            }
        };
    }
});