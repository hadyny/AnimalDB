var CheckBaseWeight = (function () { 
    var pub = {};
    var baseWeight = $("#BaseWeight").val();

    pub.setup = function () {
        $('#BaseWeight').on("focusout", function () {
            var base = this.value;
            var upperLimit = Number(baseWeight);
            upperLimit = upperLimit * 1.1;
            var lowerLimit = Number(baseWeight);
            lowerLimit = lowerLimit * .9;
            if ((baseWeight !== '') && (base >= upperLimit || base <= lowerLimit)) {
                window.alert("You have changed the base weight by more than 10%, please check that this value is correct.");
            }
        });
    }
    return pub;
})();

$(document).ready(CheckBaseWeight.setup);