var TextEditor = function() {"use strict";
	//function to initiate ckeditor
	var ckEditorHandler = function() {
		CKEDITOR.disableAutoInline = true;
		$('textarea.ckeditor').ckeditor();

		CKFinder.setupCKEditor(null, null, { type: 'Images' });
	};

	return {
		//main function to initiate template pages
		init: function() {
			ckEditorHandler();
		}
	};
}();
