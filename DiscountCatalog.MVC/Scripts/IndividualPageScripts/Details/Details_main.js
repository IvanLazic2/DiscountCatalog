//function PreviewImage() {
//    let oFReader = new FileReader();
//    oFReader.readAsDataURL(document.getElementById("default-file-browse").files[0]);

//    if (IsFileImage(document.getElementById("default-file-browse").files[0]) >= 0) {
//        oFReader.onload = function (oFEvent) {
//            ShowFormContent(oFEvent);
//        };
//    }
//    else {
//        showClientError("Image is not valid.");
//        location.reload();
//    }
//}

//function IsFileImage(file) {
//    const acceptedImageTypes = ['image/gif', 'image/jpeg', 'image/png'];
//    return file && $.inArray(file['type'], acceptedImageTypes);
//}

//function ShowFormContent(oFEvent) {
//    document.getElementById("uploadedPreview").src = oFEvent.target.result;
//    document.getElementById("form-actions").style.display = "block";
//}

//function HideFormContent() {
//    document.getElementById("uploadedPreview").src = "";
//    document.getElementById("form-actions").style.display = "none";
//}

//function Clear() {
//    document.getElementById("image-form").reset();
//    HideFormContent();
//}

//function showClientError(message) {
//    document.cookie = "Flash.Error=" + message + "; Path=/";
//}
