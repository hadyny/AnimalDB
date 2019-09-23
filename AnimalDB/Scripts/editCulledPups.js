var EditCulledPups = (function () {
    var pub = {};

    pub.setup = function () {
        $('select').select2({ allowClear: true, matcher: matchCustom, placeholder: "Scan or Select..." });

        $('#NumFemale').keyup(function () {
            if (!isNaN($('#AmountCulled').val()) && !isNaN($('#NumFemale').val())) {
                var total = +$('#AmountCulled').val() - +$('#NumFemale').val();
                $('#NumMale').val(total);
            }
        });

        $('#NumMale').keyup(function () {
            if (!isNaN($('#AmountCulled').val()) && !isNaN($('#NumMale').val())) {
                var total = +$('#AmountCulled').val() - +$('#NumMale').val()
                $('#NumFemale').val(total);
            }
        });
    };

    return pub;
})();

$(document).ready(EditCulledPups.setup);