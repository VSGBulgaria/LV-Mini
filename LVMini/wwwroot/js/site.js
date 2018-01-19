// Write your JavaScript code.
//Functions For The LogIn Form


function checkForm(form) {
    re = /^[A-Za-z0-9_]{4,20}$/;
    rep = /^[A-Za-z0-9]{6,20}$/;
    if (!re.test(form.usr.value)) {
        alert("Error: Username must contain only letters, numbers and underscores!");
        form.usr.focus();
        return false;
    }

    if (!rep.test(form.pass.value)) {
        alert("Error:Password is not valid !");
        form.pass.focus();
        return false;
    }
}
//Function For facebook Login

var finished_rendering = function () {
    console.log("finished rendering plugins");
    var facebook = document.getElementById("facebook");
    facebook.removeAttribute("style");
    facebook.removeChild(facebook.childNodes[0]);
}
//FB.Event.subscribe('xfbml.render', finished_rendering);

// For Map Location
function initMap() {
    var location = { lat: 42.142517, lng: 24.720753 };
    var map = new google.maps.Map(document.getElementById("map"), {
        zoom: 15,
        center: location
    });
    var marker = new google.maps.Marker({
        position: location,
        map: map
    });
}

//Variables
var url = '/Accounts/CheckUser';
var emailurl = '/Accounts/CheckEmail';
var expr = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

//Validate Username
$('#Username').on('keyup', CheckValid);

function CheckValid(ev) {
    ev.preventDefault()
    let data = $('#Username').val();
    $.ajax({
        url: '/Accounts/CheckUser',
            contentType: "application/json",
                data: { name: data },
        success: function (response) {
            console.log(response);
        },
        error: function (data) {
            console.log(data);
        }
    });
}
//Validate Email
$('#Email').on('keyup', CheckEmail);

function CheckEmail(ev) {
    ev.preventDefault()
    let data = $('#Email').val();
    $.ajax({
        url: emailurl,
            contentType: "application/json",
            data: { email: data },
        success: function (response) {
            console.log(response);
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function OpenUserInView(userid) {
    console.log(userid);
    $.ajax({
        url: '/Accounts/DisplayUserInfo',
        contentType: "application/json",
        data: { username: userid },
        success: function (response) {
            console.log(response);
        },
        error: function (data) {
            console.log(data);
        }
    })
}
//For Get Users Page
    //$(document).ready(function () {

    //    $('.star').on('click', function () {
    //        $(this).toggleClass('star-checked');
    //    });

    //$('.ckbox label').on('click', function () {
    //    $(this).parents('tr').toggleClass('selected');
    //});

    //    $('.btn-filter').on('click', function () {
    //        var $target = $(this).data('target');
    //        if ($target != 'all') {
    //    $('.table tr').css('display', 'none');
    //$('.table tr[data-status="' + $target + '"]').fadeIn('slow');
    //        } else {
    //    $('.table tr').css('display', 'none').fadeIn('slow');
    //}
    //    });

    //});
