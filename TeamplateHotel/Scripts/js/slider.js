$(document).ready(function () {
  $(".banner__slide").owlCarousel({
    items: 1,
    loop: true,
    nav: false,
    dots: false,
    autoplay: true,
    autoplayTimeout: 4000,
    autoplaySpeed: 800,
    // slideTransition: "ease-in-out",
    navSpeed: 1000,
    // animateOut: "animate__fadeOut",
    // animateIn: "animate__fadeIn",
    responsiveClass: true,
    navText: [
      '<i class="fal fa-chevron-left"></i>',
      '<i class="fal fa-chevron-right"></i>',
    ],
  });

  // $(".room-slide").owlCarousel({
  //   margin: 25,
  //   nav: false,
  //   autoplayTimeout: 4000,
  //   autoplaySpeed: 600,
  //   navSpeed: 1000,
  //   responsiveClass: true,
  //   navText: [
  //     '<i class="fal fa-chevron-left"></i>',
  //     '<i class="fal fa-chevron-right"></i>',
  //   ],
  //   responsive: {
  //     0: {
  //       loop: true,
  //       items: 1,
  //       autoplay: true,
  //       autoWidth: false,
  //     },
  //     640: {
  //       loop: true,
  //       items: 2,
  //       autoplay: true,
  //       autoWidth: false,
  //       mouseDrag: false,
  //     },
  //     1000: {
  //       loop: false,
  //       items: 3,
  //       autoplay: false,
  //       autoWidth: true,
  //       mouseDrag: false,
  //     }
  //   }
  // });

  $('.room-detail-gallery-1').owlCarousel({
    loop: true,
    margin: 10,
    nav: false,
    items: 1,
    dots: false,
    center: false,
    // URLhashListener:true,
    animateOut: 'fadeOut',
    navText: [
      '<i class="fas fa-long-arrow-left"></i>',
      '<i class="fas fa-long-arrow-right"></i>',
    ],
    responsive: {
      0: {
        items: 1
      },
      600: {
        items: 1
      },
      1000: {
        items: 1
      },
      1025: {
        items: 1
      },
      1400: {
        items: 1
      }
    }
    // autoplayHoverPause:true,
    // startPosition: 'URLHash'
  });

  $('.room-detail-gallery-2').owlCarousel({
    loop: true,
    margin: 5,
    nav: true,
    items: 4,
    dots: false,
    // center: true,
    // URLhashListener:true,
    // autoplayHoverPause:true,
    // startPosition: 'URLHash',
    animateOut: 'fadeOut',
    navText: [
      '<i class="fas fa-long-arrow-left"></i>',
      '<i class="fas fa-long-arrow-right"></i>',
    ],
    responsive: {
      0: {
        items: 1
      },
      319: {
        items: 1
      },
      600: {
        items: 2
      },
      1000: {
        items: 3
      },
      1025: {
        items: 4
      }
    }
  });


});

