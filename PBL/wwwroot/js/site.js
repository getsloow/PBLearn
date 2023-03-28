// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var $cell = $('.card');

//open and close card when clicked on card
$cell.find('.js-expander').click(function () {

    var $thisCell = $(this).closest('.card');

    if ($thisCell.hasClass('is-collapsed')) {
        $cell.not($thisCell).removeClass('is-expanded').addClass('is-collapsed').addClass('is-inactive');
        $thisCell.removeClass('is-collapsed').addClass('is-expanded');

        if ($cell.not($thisCell).hasClass('is-inactive')) {
            //do nothing
        } else {
            $cell.not($thisCell).addClass('is-inactive');
        }

    } else {
        $thisCell.removeClass('is-expanded').addClass('is-collapsed');
        $cell.not($thisCell).removeClass('is-inactive');
    }
});

//close card when click on cross
$cell.find('.js-collapser').click(function () {

    var $thisCell = $(this).closest('.card');

    $thisCell.removeClass('is-expanded').addClass('is-collapsed');
    $cell.not($thisCell).removeClass('is-inactive');

});

function updateText() {
    var input = document.getElementById("file");
    var textChange = document.getElementById("textChange");
    var uploadButton = document.getElementById("uploadButton");

    if (input.files.length > 0) {
        textChange.innerHTML = "Uploaded: " + input.files[0].name;
        uploadButton.removeAttribute("disabled");
    } else {
        textChange.innerHTML = "Drag and drop a file here or click to choose a file.";
        uploadButton.setAttribute("disabled", true);
    }
}
var dropArea = document.getElementById("drop-area");

dropArea.addEventListener("dragenter", function (event) {
    event.preventDefault();
    dropArea.style.backgroundColor = "#eee";
});

dropArea.addEventListener("dragover", function (event) {
    event.preventDefault();
    dropArea.style.backgroundColor = "#eee";
});

dropArea.addEventListener("dragleave", function (event) {
    event.preventDefault();
    dropArea.style.backgroundColor = "";
});

dropArea.addEventListener("drop", function (event) {
    event.preventDefault();
    dropArea.style.backgroundColor = "";

    var fileInput = document.getElementById("file");
    fileInput.files = event.dataTransfer.files;
});

dropArea.addEventListener("click", function () {
    var fileInput = document.getElementById("file");
    fileInput.click();
});