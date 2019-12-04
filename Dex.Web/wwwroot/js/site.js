$(document).ready(function () {
    $(window).on("load", function () {
        $(window).scroll(function () {
            var windowBottom = $(this).scrollTop() + $(this).innerHeight();
            var index = 0;
            $(".fade").each(function () {
                var objectBottom = $(this).offset().top + $(this).outerHeight() - 50;

                if (objectBottom < windowBottom) {
                    if ($(this).css("opacity") == 0) { $(this).fadeTo(750, 1); }
                }
                index++;
            });
        }).scroll();
    });
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

$(document).ready(function () {
    $('#submitForm').click(function (evt) {
        evt.preventDefault();
        var searchText = encodeURIComponent($('#searchBar').val());
        $.ajax({
            url: "/downloads/index?search=" + searchText + "&handler=SearchProject",
            method: 'POST',
            dataType: 'text',
            headers: {
                RequestVerificationToken:
                    $('input:hidden[name="__RequestVerificationToken"]').val()
            }
        });
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