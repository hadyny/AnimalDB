var CreateNewAnimal = (function () {
    var pub = {};
    pub.setup = function () {
        $('form').find('input').keypress(function (e) {
            if (e.which === 13) {
                var els = $('input,select');
                var next = els.eq(els.index(this) + 1);
                next.focus();
                return false;
            }
        });
    }

    return pub;
})();

$(document).ready(CreateNewAnimal.setup);