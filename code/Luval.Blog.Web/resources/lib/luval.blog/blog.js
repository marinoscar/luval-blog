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
        blog.setAlertMessage('Auto save is on, post will be saved every 5 minutes');
        setTimeout(function () {
            blog.savePost();
        },((1000 * 60) * 5));
    },
    setAlertMessage: function (message, type) {
        var banner = $('#alertbanner');
        var textEl = $('#alertmessage');
        banner.removeClass('alert-success');
        banner.removeClass('alert-warning');
        banner.removeClass('alert-danger');
        banner.removeClass('alert-primary');
        banner.addClass('alert-' + type);
        textEl.html(message);
        banner.show().delay(30000).fadeOut();
    },
    savePost: function () {
        var postObject = blog.getPost();
        $.ajax({
            type: 'post',
            url: '/Blog/SavePost',
            data: postObject,
            success: function (data, status, jqXHR) {
                if (jqXHR.status == "200") {
                    if (data.isSuccess == true)
                        blog.setAlertMessage(data.message, 'success');
                    else
                        blog.setAlertMessage(data.message, 'danger');
                    return;
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (jqXHR.status == "200") {
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
            Content: blog.editor.value(),
            UtcPublishDate: $('#UtcPublishDate').val()
		}
    },
    initShowdown: function () {
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
    },
    getHtml: function (markdown) {
        var converter = new showdown.Converter({
            tables: true, strikethrough: true, extensions: ['bootstrap-tables']
        });
        converter.setFlavor('github');
        return converter.makeHtml(markdown);
    },
    getPostContent: function () {

        var postAreas = $('[data-postarea]');

        postAreas.each(function (index, post) {

            var id = $(post).find('input[type=hidden]').val();
            var element = $(post).find('[data-post]')[0];
            $.getJSON("/Blog/PostContent/" + id, function (data) {

                if (data == null || data == undefined)
                    return;
                //var el = document.getElementById('BlogPostContent');
                //el.innerHTML = blog.getHtml(data.content);
                element.innerHTML = blog.getHtml(data.content);

            });
        });
    }
}