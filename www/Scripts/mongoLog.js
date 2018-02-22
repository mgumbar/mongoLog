$(document).ready(function () {
    //http://johannburkard.de/blog/programming/javascript/highlight-javascript-text-higlighting-jquery-plugin.html
    $("#highlightme1").change(function () {
        $(document).find("span.highlightGreen").each(function () {
            this.parentNode.firstChild.nodeName;
            with (this.parentNode) {
                replaceChild(this.firstChild, this);
                normalize();
            }
        }).end();

        $('tr').highlight($("#highlightme1").val(), 'highlightGreen');
    });

    $("#highlightme2").change(function () {
        $(document).find("span.highlightYellow").each(function () {
            this.parentNode.firstChild.nodeName;
            with (this.parentNode) {
                replaceChild(this.firstChild, this);
                normalize();
            }
        }).end();
        $('tr').highlight($("#highlightme2").val(), 'highlightYellow');
    });

    $("#highlightme3").change(function () {
        $(document).find("span.highlightRed").each(function () {
            this.parentNode.firstChild.nodeName;
            with (this.parentNode) {
                replaceChild(this.firstChild, this);
                normalize();
            }
        }).end();
        $('tr').highlight($("#highlightme3").val(), 'highlightRed');
    });
    $("div.panel-heading a.pull-right").click(function (event) {
        $("div.collapse in").collapse('hide');
    });
    //.active, .success, .info, .warning, .danger
    var trActive = $('tr.active').length;
    var trSuccess = $('tr.success').length;
    var trInfo = $('tr.info').length;
    var trWarning = $('tr.warning').length;
    var trDanger = $('tr.danger').length;
        var ctx = document.getElementById("myChart").getContext('2d');
var myChart = new Chart(ctx, {
            type: 'bar',
    data: {
            labels: ["Error", "Info", "Success", "Warning"],
        datasets: [{
            label: 'Error',
            data: [trDanger],
            backgroundColor: [
                'rgba(255, 99, 132, 0.2)'
            ],
            borderColor: [
                'rgba(255,99,132,1)'
            ],
            borderWidth: 1
        },
            {
                label: 'Info',
                data: [trInfo],
                backgroundColor: [
                    'rgba(54, 162, 235, 0.2)'
                ],
                borderColor: [
                    'rgba(54, 162, 235, 1)'
                ],
                borderWidth: 1
            },
            {
                label: 'Success',
                data: [trSuccess],
                backgroundColor: [
                    'rgba(75, 192, 192, 0.2)'
                ],
                borderColor: [
                    'rgba(75, 192, 192, 1)'
                ],
                borderWidth: 1
            },
            {
                label: 'Warning',
                data: [trWarning],
                backgroundColor: [
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }
        ]
    },
    options: {
            scales: {
            yAxes: [{
            ticks: {
            beginAtZero:true
                }
            }]
        }
    }
        });

});

