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
    var $div = $('.validation-summary-errors');
    if ($div.length === 0) {
        console.log("hita ovo");
        $div = $('<div class="validation-summary-errors">');
        $div.html('<ul id="flash-messages" class="alert"></ul>');
        // Put the $div somewhere
        $div.appendTo($('#image-form'));
        $div.find('ul').append($('<li>').text(message));
    }
}