$(document).ready(function () {
    $(window).on("load", function () {
        $(window).scroll(function () {
            var windowBottom = $(this).scrollTop() + $(this).innerHeight();
            $(".fade").each(function () {
                var objectBottom = $(this).offset().top + $(this).outerHeight() - 50;

                if (objectBottom < windowBottom) {
                    if ($(this).css("opacity") == 0) { $(this).fadeTo(750, 1); }
                }
            });
        }).scroll();
    });
});

$(document).ready(function () {
    delay(
        $("#projectsListDiv li").each(function (i) {
        $(this).delay(100 * i).fadeIn(5000);
    }),1000);
});

function displayBusyIndicator() {
    $('.loading').show();
}

$(document).ready(function () {
    $(document).on('submit', 'form',
        function () {
            if ($(document.activeElement).attr('id') == 'buttonBusyIndicator') {
                $(document.activeElement).prop('disabled', true);
                displayBusyIndicator();
            }
        });
});

function delay(callback, ms) {
    var timer = 0;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 0);
    };
}

function hasClass(elem, className) {
    return elem.className.split(' ').indexOf(className) > -1;
}