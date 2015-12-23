(function($){	
	var obj;
	
	$.fn.hk_popUp = function(obj){
		obj = $.extend({
		}, obj || {});
		
			
		$(document).on('click','.maskbox_trigger', function(e) {
			e.preventDefault();
			
			
			var self_rel = $(this).attr("rel");
			var self_div = $('#'+self_rel);
						
			var	maskHeight = $(document).height();
			var	maskWidth =	$(window).width();	
			var windowHeight = $(window).height();
			var scroll_top = $(window).scrollTop();
			//console.log(scroll_top);
			
			var lightbox ='<div id="maskbox"></div>';
			$('body').append(lightbox);
			
			$('#maskbox').css({'width':maskWidth,'height':maskHeight});  
						
			var mask_body_width = self_div.width();
			var mask_body_height = self_div.height();
			
			mask_body_left = (maskWidth  - mask_body_width) * 0.5;
			mask_body_top = (windowHeight - mask_body_height) * 0.5 + scroll_top;			
			if(mask_body_top < 60) mask_body_top = 60;
			self_div.addClass('mask_body');
			self_div.css({
				"z-index":"110",
				position:"absolute",
				width:""+ mask_body_width +"px",
				height:""+ mask_body_height +"px",
				top:""+ mask_body_top +"px", 
				left:""+ mask_body_left +"px" 
			}).fadeIn(300);
			
		});
		
		$(document).on('click','#maskbox,.btn_close',function(e) {
			e.preventDefault();
			$('#maskbox').remove();
			$('.mask_body').css('display', 'none');

			var self_rel = $(this).attr("rel");
			var self_div = $('#' + self_rel);
			self_div.html("");
		});
		
	
		
		$(window).resize(function() {
			
			if ($('#maskbox').length > 0) {
				$('#maskbox').css({'width':$(this).width() ,'height':$(this).height()});
			}
		});

	
	}
	
})(jQuery);

$(document).hk_popUp();

/*
<a href="#" rel="google_map" id="" class="maskbox_trigger">
*/