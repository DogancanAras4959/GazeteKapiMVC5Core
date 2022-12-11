
function InfinitiySroll(iTable, iAction, iParams) {

    this.table = iTable;        // Reference to the table where data should be added
    this.action = iAction;      // Name of the conrtoller action
    this.params = iParams;      // Additional parameters to pass to the controller
    this.loading = false;       // true if asynchronous loading is in process

    this.AddTableLines = function (firstItem) {

        this.loading = true;
        this.params.firstItem = firstItem;

        $.ajax({
            type: 'POST',
            url: self.action,
            data: self.params,
            dataType: "html"
        })
            .done(function (result) {
                if (result) {
                    $("#" + self.table).append(result);
                    self.loading = false;
                }

            })
            .fail(function (xhr, ajaxOptions, thrownError) {
                console.log("Error in AddTableLines:", thrownError);
            })
            .always(function () {
                // $("#footer").css("display", "none"); // hide loading info
            });
    }

    var self = this;

    //function isScrolledIntoView(elem) {
    //    var docViewTop = $(window).scrollTop();
    //    var docViewBottom = docViewTop + $(window).height();

    //    var elemTop = $(elem).offset().top;
    //    var elemBottom = elemTop + $(elem).height();

    //    return ((elemBottom <= docViewBottom) && (elemTop >= docViewTop));
    //}

    //$(window).scroll(function () {
    //    if ($(window).scrollTop() == $(document).height() - $(window).height()) {
    //        //User is currently at the bottom of the page
    //        if (!self.loading) {
    //            var itemCount = $('#' + self.table + ' tr').length - 1;
    //            self.AddTableLines(itemCount);
    //        }
    //    }
    //});

    window.onscroll = function () { onScroll() };
    function onScroll() {
        if (document.body.scrollTop > 50 || document.documentElement.scrollTop > 50) {
            if (!self.loading) {
                var itemCount = $('#' + self.table + ' tr').length - 1;
                self.AddTableLines(itemCount);
            }
        }
    }

    //var footerHeight = 0, footerTop = 0, $footer = $("#myFooter");
    //positionFooter();
    //function positionFooter() {

    //    footerHeight = $footer.height();
    //    footerTop = ($(window).scrollTop() + $(window).height() - footerHeight) + "px";

    //    if (($(document.body).height() + footerHeight) < $(window).height()) {
    //        $footer.css({ position: "absolute" }).animate({ top: footerTop }, -1)
    //        if (!self.loading) {
    //            var itemCount = $('#' + self.table + ' tr').length - 1;
    //            self.AddTableLines(itemCount);
    //        }
    //    } else {
    //        $footer.css({ position: "static" })
    //    }
    //}

    //$(window).scroll(positionFooter).resize(positionFooter)


    //window.onscroll = function (ev) {
    //    if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight) {
    //        //User is currently at the bottom of the page
    //        if (!self.loading) {
    //            var itemCount = $('#' + self.table + ' tr').length - 1;
    //            self.AddTableLines(itemCount);
    //        }
    //    }
    //};

    this.AddTableLines(0);
}
