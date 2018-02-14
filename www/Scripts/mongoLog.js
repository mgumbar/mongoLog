$(document).ready(function () {
    //http://johannburkard.de/blog/programming/javascript/highlight-javascript-text-higlighting-jquery-plugin.html
    $("#highlightme1").change(function () {
        $(document).find("span.highlightYellow").each(function () {
            this.parentNode.firstChild.nodeName;
            with (this.parentNode) {
                replaceChild(this.firstChild, this);
                normalize();
            }
        }).end();

        $('tr').highlight($("#highlightme1").val(), 'highlightYellow');
    });

    $("#highlightme2").change(function () {
        $(document).find("span.highlightBlue").each(function () {
            this.parentNode.firstChild.nodeName;
            with (this.parentNode) {
                replaceChild(this.firstChild, this);
                normalize();
            }
        }).end();
        $('tr').highlight($("#highlightme2").val(), 'highlightBlue');
    });

    $("#highlightme3").change(function () {
        $(document).find("span.highlightGreen").each(function () {
            this.parentNode.firstChild.nodeName;
            with (this.parentNode) {
                replaceChild(this.firstChild, this);
                normalize();
            }
        }).end();
        $('tr').highlight($("#highlightme3").val(), 'highlightGreen');
    });
});

