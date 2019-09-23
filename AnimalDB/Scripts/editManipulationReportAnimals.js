var EditManipulationReportAnimals = (function () {
    var pub = {};

    pub.setup = function () {
        $('#toggleAll').click(function (e) {
            e.preventDefault();
            $('input[type="checkbox"]').each(function (i, el) {
                el.checked = !el.checked;
            });
        });
        $('#selectAll').click(function (e) {
            e.preventDefault();
            $('input[type="checkbox"]').each(function (i, el) {
                el.checked = true;
            });
        });
        $('#selectNone').click(function (e) {
            e.preventDefault();
            $('input[type="checkbox"]').each(function (i, el) {
                el.checked = false;
            });
        });
        $('#recategorizeSeleceted').click(function (e) {
            e.preventDefault();
            var selected = $('input[type="checkbox"]:checked');
            if (selected.length == 0) {
                return;
            }

            var form = document.querySelector("form#recategorizeForm");
            var hiddenField, animalId;

            selected.each(function (i, el) {
                animalId = el.getAttribute('data-id');
                hiddenField = document.createElement('input');
                hiddenField.setAttribute('type', 'hidden');
                hiddenField.setAttribute('name', 'animalIds[' + i + ']');
                hiddenField.setAttribute('value', animalId);
                form.appendChild(hiddenField);
            });

            form.submit();
        });
    }

    return pub;
})();

$(document).ready(EditManipulationReportAnimals.setup);