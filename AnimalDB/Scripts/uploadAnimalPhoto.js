var UploadAnimalPhoto = (function () {
    var pub = {};
    var animalId = $("#Id").val();

    pub.setup = function () {
        $("#fileuploader").uploadFile({
            url: "../Animals/UploadPhoto/" + animalId,
            fileName: "file",
            allowedTypes: "jpg",
            dragdropWidth: "170px",
            onSuccess: function (files, data, xhr) {
                $('.animalImg').css('display', 'block');
                $('.animalImg').attr('src', '../../Content/AnimalImages/' + animalId + '.jpg');
                $('.ajax-file-upload-statusbar').css('display', 'none');
                $('#RemovePhoto').css('display', 'block');
                $('#fileuploaderdiv').css('display', 'none');
            }
        });

        $('#RemovePhoto').click(function () {
            $.post("../Animals/RemovePhoto/" + animalId)
                .done(function (result) {
                    if (result === "True") {
                        $('.animalImg').css('display', 'none');
                        $('#RemovePhoto').css('display', 'none');
                        $('#fileuploaderdiv').css('display', 'inline-block');
                    }
                });

        });
    };

    return pub;
})();

$(document).ready(UploadAnimalPhoto.setup);