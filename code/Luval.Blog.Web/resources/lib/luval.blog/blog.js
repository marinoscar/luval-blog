var blog = {
    editor: "",
    init: function () {

    },
    initEditor: function () {
        blog.editor = new SimpleMDE({ element: document.getElementById("MarkDownEditor") });
    }
}