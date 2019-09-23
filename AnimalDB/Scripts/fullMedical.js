var fullMedical = (function () {
    var pub = {};

    pub.setup = function () {
        $('#btnnotes').click(function () {
            if ($(this).attr('class') === 'btn  btn-primary active') {
                $('.note td').hide();
            } else {
                $('.note td').show();
            }
        });

        $('#btnmeds').click(function () {
            if ($(this).attr('class') === 'btn  btn-primary active') {
                $('.med td').hide();
            } else {
                $('.med td').show();
            }
        });

        $('#btnsurg').click(function () {
            if ($(this).attr('class') === 'btn  btn-primary active') {
                $('.surg td').hide();
            } else {
                $('.surg td').show();
            }
        });

        $('#btnnotes').button('toggle');
        $('#btnmeds').button('toggle');
        $('#btnsurg').button('toggle');

    };

    return pub;
})();

$(document).ready(fullMedical.setup);