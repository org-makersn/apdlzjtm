$(function() {
	$('#Edate').datepicker({
		changeMonth: false,
		changeYear: false,
		yearRange: '2000:2020',
		showOn: 'button', buttonImage: '/_common/js/calendar/image/datepicker.gif', buttonImageOnly: true,
		altField: '#Edate', altFormat: 'yy-mm-dd',
		defaultDate: '+0m+0d' 
	});

	$('#Sdate').datepicker({
		changeMonth: false,	changeYear: false,
		yearRange: '2000:2020',
		showOn: 'button', buttonImage: '/_common/js/calendar/image/datepicker.gif', buttonImageOnly: true,
		altField: '#Sdate', altFormat: 'yy-mm-dd',
		defaultDate: '+0m+0d' 
	});	
});