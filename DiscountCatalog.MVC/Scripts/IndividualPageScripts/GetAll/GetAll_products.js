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