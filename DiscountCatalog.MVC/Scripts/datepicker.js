//$(function () {
//    $('.datetimepicker1').datepicker({
//        format: "dd.mm.yyyy.",
//        todayHighlight: true
//    });
//});

$(document).ready(function () {
    $(function () {
        $('.datepicker').datetimepicker({
            useCurrent: false, //Important! See issue #1075
            format: "DD.MM.YYYY."
        });
    });
});

$(document).ready(function () {
    $(function () {
        $('.timepicker').datetimepicker({
            useCurrent: false, //Important! See issue #1075
            format: "HH:mm"
        });
    });
});
