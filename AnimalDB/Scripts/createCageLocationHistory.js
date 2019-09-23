var CreatCageLocationHistory = (function () {
    var pub = {};
    pub.setup = function () {
        $('#Rack_Id').change(function () {
            $('#CageLocation_Id').val('');
            if ($("#Rack_Id").val() !== '') {
                $('.selectGrid table').css('display', 'none');
                $('.selectGrid').css('display', 'block');
                $('#rack' + $('#Rack_Id').val()).css('display', 'block');
                $('#rackname').text($('#Rack_Id option:selected').text());
            }
            else {
                $('.selectGrid, .selectGrid table').css('display', 'none');
            }
        });

        $('#CageLocation_Id').change(function () {
            $('.selectGrid, .selectGrid table').css('display', 'none');
            $('#Rack_Id').val('');
        });

        var selectRadio = function (rack, x, y) {
            $('input#RackEntry_X').val(x);
            $('input#RackEntry_Y').val(y);
            $('input#SelectedRack').val(rack);
        };
    };

    return pub;
})();

$(document).ready(CreatCageLocationHistory.setup);