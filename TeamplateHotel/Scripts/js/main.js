$("img:not(.banner__item img)").each(function () {
  $(this).attr("loading", "lazy");
});

document.body.addEventListener(
  "load",
  (e) => {
    if (e.target.tagName != "IMG") {
      return;
    }
    // Remove the blurry placeholder.
    e.target.style.backgroundImage = "none";
  },
  /* capture */
  true
);

$(document).ready(function () {

  /* --- langugle dropdown --- */
  $(".lang__menu-icon").click(function (e) {

    $(".lang__menu_drop").toggleClass("active");
  });

  var $lang__menu = $('.lang__menu');
  $(document).mouseup(e => {

    if (!$lang__menu.is(e.target) // if the target of the click isn't the container...
      &&
      $lang__menu.has(e.target).length === 0) // ... nor a descendant of the container
    {
      $(".lang__menu_drop").removeClass("active");
    }
  });


  /* ---------- MENU ------- */
  $(window).scroll(function () {
    if ($(window).scrollTop() > 1) {
      $(".header").addClass("fixed");
    }
    else {
      $(".header").removeClass("fixed");
    }
  });

  $(".nav-icon").click(function (e) {
    $(".nav-mobile").toggleClass("active");
  });

  $(".arrow-icon").click(function (e) {
    e.preventDefault();
    var menu_this = $(this).closest(".menu-mobile li").find(".menu__child");
    var $this = $(this);

    $(this).closest('.menu-mobile').find('.arrow-icon').not($this).removeClass("rotate");
    $(this).closest(".menu-mobile").find(".menu__child").not(menu_this).slideUp();

    if ($(this).hasClass("rotate")) {
      $(this).removeClass("rotate");
      menu_this.slideUp();
    }
    else {

      $(this).addClass("rotate");
      menu_this.slideDown();
    }
  });


  function responsive() {
    if ($(window).width() < 1023) {

    }
    else {
      $(".nav-mobile").removeClass("active");
      $(".arrow-icon").removeClass("rotate");
      $(".menu__child").slideUp();
    }
  };

  responsive();

  $(window).resize(function () {
    responsive();
  });


  // ------Back to top -------
  $(window).scroll(function () {
    if ($(window).scrollTop() > 450) {
      $('.back-top').addClass("show")
    }
    else {
      $('.back-top').removeClass("show")
    }
  });
  $('.back-top').click(function () {
    $("html, body").animate({ scrollTop: 0 }, 1000);
  });

  // Video home
  $(".video_btn").click(function (e) {
    e.preventDefault();
    // $(this).closest(".video_container").addClass("active")
    $(".bg_holder").hide();
  });

  // Datepicker

  $("#checkIn").daterangepicker({
    autoUpdateInput: false,
    singleDatePicker: true,
    autoApply: true,
    minDate: moment(),
    locale: {
      format: 'DD/MM/YYYY'
    }
  });

  $("#checkOut").daterangepicker({
    autoUpdateInput: false,
    singleDatePicker: true,
    autoApply: true,
    // minDate: moment(),
    minDate: moment().add(1, "days"),
    locale: {
      format: 'DD/MM/YYYY'
    }
  });

  $("#checkIn").on("apply.daterangepicker", function (e, picker) {
    $(this).val(picker.startDate.format('DD/MM/YYYY'));
    var mindate = $(this).val();
    var date = moment(mindate, "DD/MM/YYYY");

    $("#checkOut").daterangepicker({
      minDate: date.add(1, "days"),
      singleDatePicker: true,
      autoApply: true,
      locale: {
        format: 'DD/MM/YYYY'
      }
    });
  });

  $('#checkOut').on('apply.daterangepicker', function (ev, picker) {
    $(this).val(picker.startDate.format('DD/MM/YYYY'));
  });

  $('#checkOut').on('cancel.daterangepicker', function (ev, picker) {
    $(this).val('');
  });


  // counter About Us page
  $(".counter").counterUp({
    time: 1000
  });

  // accordion
  $('[data-collapse-panel]').hide();
  var acParent = $('.single-accordion');
  var showAc = $('.single-accordion.show');
  $('[data-collapse]').on('click', function (event) {
    event.preventDefault();
    var AcCurrent = ($(this).attr('href'));
    var acNew = AcCurrent.replace('#', '');
    $(this).parent('.single-accordion').toggleClass('show');
    var findAccordion = $(document).find("[id='" + acNew + "']").slideToggle();
    $(this).parent('.single-accordion').siblings().children('[data-collapse-panel]').slideUp();
    $(this).parent('.single-accordion').siblings().removeClass('show')
  });
  if ($('.single-accordion').hasClass('show')) {
    showAc.find('[data-collapse-panel]').slideDown()
  }
  $('.show-collapse').slideDown();



  // gallery dropdown
  $(".drop-button").click(function (e) {
    $(this).parent(".nav-select").find(".order-dropdown").toggle();
  });

  var $menu1 = $('.nav-select');
  $(document).mouseup(e => {
    if (!$menu1.is(e.target) // if the target of the click isn't the container...
      &&
      $menu1.has(e.target).length === 0) // ... nor a descendant of the container
    {
      $(".order-dropdown").hide();
    }
  });

  $(".order-link").click(function (e) {

    var get_text = $(this).text();

    $(this).closest(".nav-select").find(".drop-filter").text(get_text);
    $(this).closest(".order-dropdown").hide();
  });
});

// gallery home (Filter)
$(window).on('load', function () {

  var $grid = $('.gallery_item1').isotope({
    itemSelector: '.grid-item',
    percentPosition: true,
    masonry: {
      columnWidth: '.grid-item'
    }
  });

  $('.filter li').on('click', function () {
    $(this).addClass('active').siblings().removeClass('active');
    var filterValue = $(this).attr('data-filter');
    $('.grid').isotope({
      filter: filterValue
    });
  });


});
