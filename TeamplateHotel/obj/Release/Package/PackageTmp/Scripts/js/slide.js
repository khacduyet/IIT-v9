$('.owl-banner_index').owlCarousel({
    lazyLoad: true,
    loop: true,
    nav: true,
    dots: false,
    margin: 0,
    autoplay: true,
    autoplayTimeout: 8000,
    animateOut: 'fadeOut',
    animateIn: 'fadeIn',
    smartSpeed: 450,
    navText: ["<i class='fal fa-chevron-left'></i>", "<i class='fal fa-chevron-right'></i>"],
    responsive: {
        0: {
            items: 1,
        },
        600: {
            items: 1,
        },
        1000: {
            items: 1,
        },
        1900: {
            items: 1
        }
    }
})
$('.owl-rooms_index').owlCarousel({
    loop: true,
    fade: true,
    nav: true,
    dots: false,
    autoplayHoverPause: true,
    autoplay: true,
    smartSpeed: 500,
    autoHeight: true,
    margin: 15,
    items: 1,
    center: true,
    URLhashListener: true,
    autoplayHoverPause: true,
    startPosition: 'URLHash',
    navText: ["<i class='fal fa-chevron-left'></i>", "<i class='fal fa-chevron-right'></i>"],
});

$('.owl-rooms-small_index').owlCarousel({
        lazyLoad: true,
        loop: true,
        nav: true,
        dots: false,
        margin: 10,
        center: true,
        autoplay: true,
        autoplayTimeout: 8000,
        animateOut: 'fadeOut',
        animateIn: 'fadeIn',
        smartSpeed: 450,

        responsive: {
            0: {
                items: 2,
            },
            600: {
                items: 4,
            },
            1000: {
                items: 4,
            },
            1900: {
                items: 5,
            }
        }
    })
    // $('.owl-ecotourism_image').owlCarousel({
    //     loop: true,
    //     nav: false,
    //     dots: false,
    //     center: true,
    //     autoplay: true,
    //     autoplayTimeout: 5000,
    //     URLhashListener: true,
    //     autoplayHoverPause: true,
    //     startPosition: 'URLHash',
    //     items: 1,


// })

$('.owl-feeback').owlCarousel({
    loop: true,
    nav: false,
    dots: true,
    margin: 15,
    stagePadding: 0,
    autoplay: true,
    animateOut: 'fadeOut',
    animateIn: 'fadeIn',
    navText: ["<i class='fal fa-chevron-left'></i>", "<i class='fal fa-chevron-right'></i>"],
    responsive: {
        0: {
            items: 1,
        },
        600: {
            items: 1,
        },
        1000: {
            items: 1,
        },
        1200: {
            items: 1
        }
    }
})
$('.owl-service_index').owlCarousel({
    loop: true,
    nav: false,
    dots: true,
    margin: 15,
    autoplay: true,
    animateOut: 'fadeOut',
    animateIn: 'fadeIn',
    navText: ["<i class='fal fa-chevron-left'></i>", "<i class='fal fa-chevron-right'></i>"],
    responsive: {
        0: {
            items: 1,
            stagePadding: 0,

        },
        600: {
            items: 1,
            stagePadding: 0,

        },
        1000: {
            items: 1,
            stagePadding: 0,

        },
        1200: {
            items: 1,
            stagePadding: 150,

        }
    }
})

$('.owl-gallery').owlCarousel({
    items: 1,
    loop: true,
    center: true,
    margin: 0,
    dots: false,
    autoplay: true,
    autoplayTimeout: 8000,
    animateOut: 'fadeOut',
    animateIn: 'fadeIn',
    smartSpeed: 450,
    callbacks: true,
    nav: true,
    navText: ["<i class='fal fa-chevron-left'></i>", "<i class='fal fa-chevron-right'></i>"],
    URLhashListener: true,
    autoplayHoverPause: true,
    startPosition: 'URLHash'
});
$('.owl-detai-room').owlCarousel({
    items: 1,
    loop: true,
    center: true,
    margin: 0,
    dots: true,
    autoplay: true,
    autoplayTimeout: 8000,
    animateOut: 'fadeOut',
    animateIn: 'fadeIn',
    smartSpeed: 450,
    callbacks: true,
    nav: false,
    // navText: ["<i class='fal fa-chevron-left'></i>", "<i class='fal fa-chevron-right'></i>"],
    URLhashListener: true,
    autoplayHoverPause: true,
    startPosition: 'URLHash'
});

$('.owl-system').owlCarousel({
    loop: true,
    nav: false,
    dots: true,
    margin: 20,
    stagePadding: 0,
    autoplay: true,
    animateOut: 'fadeOut',
    animateIn: 'fadeIn',
    navText: ["<i class='fal fa-chevron-left'></i>", "<i class='fal fa-chevron-right'></i>"],
    responsive: {
        0: {
            items: 1,
        },
        600: {
            items: 2,
        },
        1000: {
            items: 3,
        },
        1200: {
            items: 3
        }
    }
})

$('.owl-other-article').owlCarousel({
    loop: true,
    nav: true,
    dots: true,
    margin: 20,
    stagePadding: 0,
    autoplay: true,
    animateOut: 'fadeOut',
    animateIn: 'fadeIn',
    navText: ["<i class='fal fa-chevron-left'></i>", "<i class='fal fa-chevron-right'></i>"],
    responsive: {
        0: {
            items: 1,
        },
        600: {
            items: 2,
        },
        1000: {
            items: 3,
        },
        1200: {
            items: 3
        }
    }
})