function getStoreId() {
    return sessionStorage.getItem("StoreID");
}

function getBaseAddress() {
    return sessionStorage.getItem("base");
}

$(document).ready(function () {
    if ($.cookie("base") !== null) {
        console.log("nije null");
    }
    else {
        console.log("je null");
    }
});

function setSlider(min, max) {
    $("#slider-range").slider({
        range: true,
        min: min,
        max: max,
        values: [min, max],
        slide: function (event, ui) {
            $("#amount").val("$" + ui.values[0] + " - $" + ui.values[1]);
        }
    });
    $("#amount").val("$" + $("#slider-range").slider("values", 0) +
        " - $" + $("#slider-range").slider("values", 1));
}

//ss

//$(function () {
//    $("#slider-range").slider({
//        range: true,
//        min: @ViewBag.Min,
//    max: @ViewBag.Max,
//    values: [@ViewBag.From, @ViewBag.To],
//    slide: function (event, ui) {
//        $("#amount").val("$" + ui.values[0] + " - $" + ui.values[1]);
//    },
//    change: function (event, ui) {



//        let array = ui.values;
//        array = array.join(",");

//        $("#PriceFilter").val(array);
//        $("#filter-form").submit();
//    }
//});
//$("#amount").val("$" + $("#slider-range").slider("values", 0) +
//    " - $" + $("#slider-range").slider("values", 1));
//            });