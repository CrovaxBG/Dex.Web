$(document).ready(function () {
    $(window).on("load", function () {
        $(window).scroll(function () {
            var windowBottom = $(this).scrollTop() + $(this).innerHeight();
            var index = 0;
            $(".fade").each(function () {
                console.log(index);
                var objectBottom = $(this).offset().top + $(this).outerHeight() - 50;

                if (objectBottom < windowBottom) {
                    if ($(this).css("opacity") == 0) { $(this).fadeTo(750 , 1); }
                }
                index++;
            });
        }).scroll();
    });
});