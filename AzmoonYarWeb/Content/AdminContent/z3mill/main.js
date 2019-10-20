$(function(){
	var availableTagsf = [];

	$('#myAutocomplete').autocomplete({
        source: availableTags,
        multiselect: true
    });
})