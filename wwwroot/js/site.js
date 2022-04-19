// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//$(document).ready(function () {
//    $('#btnSendEmail').click(function () {
//        $('#emailModel').css("display", "block");
//    });
//    //$('#btnSendEmail').on('click', function (event) {
//    //    $('#emailModel').modal('show')
//    //});
//});

var myModal = document.getElementById('emailModal')
var myInput = document.getElementById('btnSendEmail')

myModal.addEventListener('shown.bs.modal', function () {
    myInput.focus()
})
