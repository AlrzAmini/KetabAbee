var timeoutTime = 1800000;
var timeoutTimer = setTimeout(ShowTimeOutWarning, timeoutTime);
$(document).ready(function () {
    $('body').bind('mousedown keydown', function (event) {
        clearTimeout(timeoutTimer);
        timeoutTimer = setTimeout(window.ShowTimeOutWarning, timeoutTime);
    });
});