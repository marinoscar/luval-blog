var blog = {
    editor: "",
    init: function () {

    },
    initEditor: function () {
        blog.editor = new SimpleMDE({
            element: document.getElementById("MarkDownEditor"),
            toolbar: [
                {
                    name: "savePost",
                    action: function (editor) {
                        blog.savePost();
                    },
                    className: "fa fa-floppy-o",
                    title: "Save Post"
                }, "|", "undo", "redo", "bold", "italic", "strikethrough", "heading", "|",
                "quote", "unordered-list", "ordered-list", "table", "horizontal-rule", "|",
				"link", "image", {
					name: "addMedia",
					action: function openMedia(editor) {
						window.open('/Media', '_blank');
					},
					className: "fa fa-folder-open",
					title: "add media",
				}, "code", "clean-block", "|", "preview", "side-by-side", "fullscreen", "guide"
			],
        });
	},
    savePost: function () {
        var postObject = blog.getPost();
        $.ajax({
            type: 'post',
            url: '/Blog/SavePost',
            data: postObject,
            success: function (data, status, jqXHR) {
                if (jqXHR.status == "200") {
                    blog.onSuccess();
                    return;
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (jqXHR.status == "200") {
                    blog.onSuccess();
                    return;
                }
                alert('Failed to save the post ' + jqXHR.status + ' ' + jqXHR.statusText);
            },
            dataType: "json"
        });
    },
	getPost: function () {
		return {
			Id: $('#Id').val(),
            Title: $('#Title').val(),
			Content: blog.editor.value()
		}
    },
    onSuccess: function () {
        alert('Post saved succesfully');
    },
    getPostContent: function () {
        var id = $('#PostId').val();
        $.getJSON("/Blog/PostContent/" + id, function (data) {

            if (data == null || data == undefined)
                return;

            var el = document.getElementById('BlogPostContent');
            showdown.extension('bootstrap-tables', function () {
                return [{
                    type: "output",
                    filter: function (html, converter, options) {
                        // parse the html string
                        var liveHtml = $('<div></div>').html(html);
                        $('table', liveHtml).each(function () {
                            var table = $(this);
                            // table bootstrap classes
                            table.addClass('table table-striped table-bordered')
                                // make table responsive
                                .wrap('<div class="class table-responsive"></div>');
                        });
                        return liveHtml.html();
                    }
                }];
            });
            var converter = new showdown.Converter({
                tables: true, strikethrough: true, extensions: ['bootstrap-tables']
            });
            converter.setFlavor('github');
            var html = converter.makeHtml(data.content);

            el.innerHTML = html;
        });
    }
}