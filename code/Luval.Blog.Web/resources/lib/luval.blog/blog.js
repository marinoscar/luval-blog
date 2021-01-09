var blog = {
    editor: "",
    init: function () {

    },
    initEditor: function () {
        blog.editor = new SimpleMDE({
            element: document.getElementById("MarkDownEditor"),
			toolbar: [
				"undo", "redo", "bold", "italic", "strikethrough", "heading", "|", "quote", "unordered-list", "ordered-list", "table", "horizontal-rule", "|",
				"link", "image", {
					name: "custom",
					action: function openMedia(editor) {
						window.open('/Media', '_blank');
					},
					className: "fa fa-folder-open",
					title: "add media",
				}, "code", "clean-block", "|", "preview", "side-by-side", "fullscreen", "guide"
			],
        });
    }
}