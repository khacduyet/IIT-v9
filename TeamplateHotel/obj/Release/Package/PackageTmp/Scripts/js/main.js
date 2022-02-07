$(document).ready(function () {
    $('#nav > li > a').click(function () {
        if ($(this).attr('class') != 'active') {
            $('#nav li ul').slideUp();
            $(this).next().slideToggle();
            $('#nav li a').removeClass('active');
            $(this).addClass('active');
        }
    });
});
// count
$(document).ready(function () {
    $('.minus').click(function () {
        var $input = $(this).parent().find('input');
        var count = parseInt($input.val()) - 1;
        count = count < 1 ? 1 : count;
        $input.val(count);
        $input.change();
        return false;
    });
    $('.plus').click(function () {
        var $input = $(this).parent().find('input');
        $input.val(parseInt($input.val()) + 1);
        $input.change();
        return false;
    });
});

// date picker
$(document).ready(function () {
    $(".check-in").datepicker({
        dateFormat: "d, M, yy",
    });

    $(".check-out").datepicker({
        dateFormat: "d, M, yy",
    });
});

// // fullpage
// $(document).ready(function() {

//     //initialising fullpage.js in the jQuery way
//     $('#fullpage').fullpage({

//         navigation: true,

//         slidesNavigation: true,

//     });

// });
$(document).ready(function () {
    // Create two variables with names of months and days of the week in the array
    var monthNames = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"];
    var dayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]

    // Create an object newDate()
    var newDate = new Date();
    // Retrieve the current date from the Date object
    newDate.setDate(newDate.getDate());
    // At the output of the day, date, month and year    
    $('#Date').html(newDate.getDate() + '-' + monthNames[newDate.getMonth()] + '-' + newDate.getFullYear());

    setInterval(function () {
        // Create an object newDate () and extract the second of the current time
        var seconds = new Date().getSeconds();
        // Add a leading zero to the value of seconds
        $("#sec").html((seconds < 10 ? "0" : "") + seconds);
    }, 1000);

    setInterval(function () {
        // Create an object newDate () and extract the minutes of the current time
        var minutes = new Date().getMinutes();
        // Add a leading zero to the minutes
        $("#min").html((minutes < 10 ? "0" : "") + minutes);
    }, 1000);

    setInterval(function () {
        // Create an object newDate () and extract the clock from the current time
        var hours = new Date().getHours();
        // Add a leading zero to the value of hours
        $("#hours").html((hours < 10 ? "0" : "") + hours);
    }, 1000);

});
$(document).ready(function () {
    // Create two variables with names of months and days of the week in the array
    var monthNames = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"];
    var dayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]

    // Create an object newDate()
    var newDate = new Date();
    // Retrieve the current date from the Date object
    newDate.setDate(newDate.getDate());
    // At the output of the day, date, month and year    
    $('#Date').html(newDate.getDate() + '-' + monthNames[newDate.getMonth()] + '-' + newDate.getFullYear());

    setInterval(function () {
        // Create an object newDate () and extract the second of the current time
        var seconds = new Date().getSeconds();
        // Add a leading zero to the value of seconds
        $("#sec").html((seconds < 10 ? "0" : "") + seconds);
    }, 1000);

    setInterval(function () {
        // Create an object newDate () and extract the minutes of the current time
        var minutes = new Date().getMinutes();
        // Add a leading zero to the minutes
        $("#min").html((minutes < 10 ? "0" : "") + minutes);
    }, 1000);

    setInterval(function () {
        // Create an object newDate () and extract the clock from the current time
        var hours = new Date().getHours();
        // Add a leading zero to the value of hours
        $("#hours").html((hours < 10 ? "0" : "") + hours);
    }, 1000);

});


$(document).ready(function () {
    $(document).on('click', '.plus', function () {
        $('.count').val(parseInt($('.count').val()) + 1);
    });
    $(document).on('click', '.minus', function () {
        $('.count').val(parseInt($('.count').val()) - 1);
        if ($('.count').val() == 0) {
            $('.count').val(1);
        }
    });
});


$(document).ready(function () {
    $(document).on('click', '.plus2', function () {
        $('.count2').val(parseInt($('.count2').val()) + 1);
    });
    $(document).on('click', '.minus2', function () {
        $('.count2').val(parseInt($('.count2').val()) - 1);
        if ($('.count2').val() < 0) {
            $('.count2').val(0);
        }
    });
});

// Gallery image hover
$(".img-wrapper").hover(
    function () {
        $(this).find(".img-overlay").animate({ opacity: 1 }, 600);
    },
    function () {
        $(this).find(".img-overlay").animate({ opacity: 0 }, 600);
    }
);

// Lightbox
var $overlay = $('<div id="overlay"></div>');
var $image = $("<img>");
var $prevButton = $('<div id="prevButton"><i class="fa fa-chevron-left"></i></div>');
var $nextButton = $('<div id="nextButton"><i class="fa fa-chevron-right"></i></div>');
var $exitButton = $('<div id="exitButton"><i class="fa fa-times"></i></div>');

// Add overlay
$overlay.append($image).prepend($prevButton).append($nextButton).append($exitButton);
$("#gallery").append($overlay);

// Hide overlay on default
$overlay.hide();

// When an image is clicked
$(".img-overlay").click(function (event) {
    // Prevents default behavior
    event.preventDefault();
    // Adds href attribute to variable
    var imageLocation = $(this).prev().attr("href");
    // Add the image src to $image
    $image.attr("src", imageLocation);
    // Fade in the overlay
    $overlay.fadeIn("slow");
});

// When the overlay is clicked
$overlay.click(function () {
    // Fade out the overlay
    $(this).fadeOut("slow");
});

// When next button is clicked
$nextButton.click(function (event) {
    // Hide the current image
    $("#overlay img").hide();
    // Overlay image location
    var $currentImgSrc = $("#overlay img").attr("src");
    // Image with matching location of the overlay image
    var $currentImg = $('#image-gallery img[src="' + $currentImgSrc + '"]');
    // Finds the next image
    var $nextImg = $($currentImg.closest(".image").next().find("img"));
    // All of the images in the gallery
    var $images = $("#image-gallery img");
    // If there is a next image
    if ($nextImg.length > 0) {
        // Fade in the next image
        $("#overlay img").attr("src", $nextImg.attr("src")).fadeIn(800);
    } else {
        // Otherwise fade in the first image
        $("#overlay img").attr("src", $($images[0]).attr("src")).fadeIn(800);
    }
    // Prevents overlay from being hidden
    event.stopPropagation();
});

// When previous button is clicked
$prevButton.click(function (event) {
    // Hide the current image
    $("#overlay img").hide();
    // Overlay image location
    var $currentImgSrc = $("#overlay img").attr("src");
    // Image with matching location of the overlay image
    var $currentImg = $('#image-gallery img[src="' + $currentImgSrc + '"]');
    // Finds the next image
    var $nextImg = $($currentImg.closest(".image").prev().find("img"));
    // Fade in the next image
    $("#overlay img").attr("src", $nextImg.attr("src")).fadeIn(800);
    // Prevents overlay from being hidden
    event.stopPropagation();
});

// When the exit button is clicked
$exitButton.click(function () {
    // Fade out the overlay
    $("#overlay").fadeOut("slow");
});
// count up
// counter in meeting rooms page
$(function () {
    $(".count").each(function () {
        $(this)
            .prop("Counter", 0)
            .animate({
                Counter: $(this).text(),
            }, {
                duration: 4000,
                easing: "swing",
                step: function (now) {
                    $(this).text(Math.ceil(now));
                },
            });
    });
});
// hover service
$("figure").mouseenter(function () {
    $(this).addClass("active");
})
$("figure").mouseleave(function () {
    $(this).removeClass("active");
})