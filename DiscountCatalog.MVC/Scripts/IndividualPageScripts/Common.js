$(document).ready(function () {

    //$("body").animate({
    //    opacity: '1'
    //}, 1000);

    if ($.cookie("scroll") !== null) {
        $(document).scrollTop($.cookie("scroll"));
        $.cookie("scroll", "0");
    }

    $('.stay').on("click", function () {
        $.cookie("scroll", $(document).scrollTop());

    });

    $('.stay-bottom').on("click", function () {
        //$('html,body').animate({ scrollTop: 9999 }, 'slow'); 
        $.cookie("scroll", 9999); //nije dobro
    });

});



function PreviewImage() {
    let oFReader = new FileReader();
    oFReader.readAsDataURL(document.getElementById("default-file-browse").files[0]);

    if (IsFileImage(document.getElementById("default-file-browse").files[0]) >= 0) {
        oFReader.onload = function (oFEvent) {
            ShowFormContent(oFEvent);
        };
    }
    else {
        showClientError("Image is not valid.");
        location.reload();
    }
}

function IsFileImage(file) {
    const acceptedImageTypes = ['image/gif', 'image/jpeg', 'image/png'];
    return file && $.inArray(file['type'], acceptedImageTypes);
}

function ShowFormContent(oFEvent) {
    document.getElementById("uploadedPreview").src = oFEvent.target.result;
    document.getElementById("form-actions").style.display = "block";
}

function HideFormContent() {
    document.getElementById("uploadedPreview").src = "";
    document.getElementById("form-actions").style.display = "none";
}

function Clear() {
    document.getElementById("image-form").reset();
    HideFormContent();
}

function showClientError(message) {
    document.cookie = "Flash.Error=" + message + "; Path=/";
}
