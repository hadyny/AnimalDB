var PrintAnimalManipulationReport = (function () {
    var pub = {};

    function printreport() {
        print_window = window.open("", "printwindow", "status=1,width=700,height=842");
        print_window.document.write('<html><head>');
        print_window.document.write('<title>Animal Manipulation Report</title>');
        print_window.document.write('<link href="../../Content/printcss" rel="stylesheet" />');
        print_window.document.write('<link href="../../Content/printcss2" rel="stylesheet" media="print" />');
        print_window.document.write('</head>');
        print_window.document.write('<body onload="window.print()"><div class="noprint">');
        print_window.document.write('<button onclick="window.close()">Close Preview</button>');
        print_window.document.write('</div>');
        print_window.document.write($('<div />').append($('.printmanipulationreport').clone()).html());
        print_window.document.write('<script src="../../bundles/jquery"></script>');
        print_window.document.write('<script>$("input, textarea").attr("readonly", "readonly");$("select").attr("disabled", true);<' + '/script>');
        print_window.document.write('</body></html>');
        print_window.document.close();
        print_window.focus();
    }

    pub.setup = function () {
        var button = document.querySelector("a.btn.noprint");
        button.onclick = printreport;
    }

    return pub;
})();

$(document).ready(PrintAnimalManipulationReport.setup);