$(document).ready(function(){
	//select
	var params = {
		changedEl: "select",
		visRows: 12,
		scrollArrows: true
	}
	cuSel(params);
	
	// Checkbox
	$('.checkbox').not('.disabled').click(function() {
		if ( $(this).parent().find('input').attr('checked') ) {	
			$(this).removeClass('checked');
			$(this).parent().find('input').attr('checked',false);
		}else{
			$(this).addClass('checked');
			$(this).parent().find('input').attr('checked',true);
		}
	});
	
	$('.radio').click(function(){
		if ( $(this).hasClass('checked')){
			return false;
		}else{
			$(this).parents('.radio-list:first').children("li").find(".radio:first").removeClass('checked');
			$(this).closest('li').siblings('li').find('input').attr('checked',false);
			$(this).find('input').attr('checked',true);
			$(this).addClass('checked');
		}
	});
	$(".radio-list label").click(function(){
		if ($(this).siblings("span").hasClass('checked')){
			return false;
		}else{
			$(this).closest('.radio-list').find('.radio').removeClass('checked');
			$(this).closest('li').siblings('li').find('input').attr('checked',false);
			$(this).siblings("span").find('input').attr('checked',true);
			$(this).siblings("span").addClass('checked');
		}
	});
	// Popup
	//$('.window-popup, .popup-bg, .bg-window').hide(); 
	$(".popup-bg").css({opacity: .2});
	alignCenter($('.window-popup')); 
	$(window).resize(function() {
		alignCenter($('.window-popup')); 
	});
	$('.open-popup').click(function() {
		$('.popup-bg, .window-popup, .bg-window').fadeIn(300); 
		return false;
	});
	$('.close-popup').click(function() {
		$('.popup-bg, .window-popup, .bg-window').fadeOut(300); 
	});
	$('#cancellogin').click(function () {
	    $($(this).parents('form')[0]).find('input[type!="submit"]').val(null);
        $('.close-popup').click();
    });
	// function centering
	function alignCenter(elem) {
		elem.css({
			left: ($(window).width() - elem.outerWidth()) / 2 + 'px', 
			top: ($(window).height() - elem.outerHeight()) / 2 + 'px' 
		});
	}
	$('#subscribe').popover({
		'placement': 'bottom',
		'trigger': 'hover',
		html:true,
		content:function(){
			return $('#subscribe-popover').html()
		}
	});
	$('#subscribed').popover({
		'placement': 'bottom',
		'trigger': 'hover',
		html:true,
		content:function(){
			return $('#subscribed-popover').html()
		}
	});
	$('#lk').popover({
		'placement': 'bottom',
		'trigger': 'hover',
		html:true,
		content:function(){
			return $('#lk-popover').html()
		}
	});
	//tabs
	$('.nav-tabs li:first a, #tab.nav-tabs li:first a').tab("show");
	$('.nav-tabs a').click(function (e) {
		e.preventDefault();
		$(this).tab('show');
	});
	
	//carousel
	var slider = $('.bxslider').bxSlider({
		mode: 'horizontal',
		autoControls: true,
		minSlides: 1,
		pager: false,
		adaptiveHeight: true
	});
	
	//open block solutions
	$(".solutions").click(function(){
		$(this).parents(".content-slide").find(".b-solution").slideToggle("fast", function(){
			current = slider.getCurrentSlide();
			/*slider.reloadSlider();
			slider.goToSlide(current);*/
			//slider.find("li").eq(current).css("height", "100%");
			$(".bx-viewport").css("height", "100%");
			
		});
		if($(this).hasClass("op")){
			$(this).parent("div").hide().next("div").show();
		}else{
			$(this).parent("div").hide().prev("div").show();
		}
	});
});