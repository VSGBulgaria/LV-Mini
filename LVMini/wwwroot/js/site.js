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
let url = '/Accounts/CheckUser';
let emailurl = '/Accounts/CheckEmail';
let defaultContentType = 'application/json';
let expr = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
let registerUsernameTagId = '#Username';
let registerEmailTagId = '#Email';
let cssDisplayKeyWord = 'display';
let cssColorKeyWord = 'color';
let cssNoneKeyWord = 'none';
let cssInlineBlockKeyWord = 'inline-block';
let cssRedKeyWord = 'red';
let cssGreenKeyWord = 'green';
let jqueryKeyUpKeyWord = 'keyup';
let registerUsernameAvailableSignId = '#username_available_sign';
let registrationUsernameUnavailableSignId = '#username_unavailable_sign';
let minimumUsernameLenght = 3;


//Validate Username
$(registerUsernameTagId).on(jqueryKeyUpKeyWord, checkForAvailableUsername);

function checkForAvailableUsername(ev) {
    ev.preventDefault()
    let data = $(registerUsernameTagId).val();
    if (data.length >= minimumUsernameLenght) {
        $.ajax({
            url: url,
            contentType: defaultContentType,
            data: { name: data },
            success: displayUsernameSign,
            error: logErrorInConsole
        });
    } else {
        $(registerUsernameAvailableSignId).css(cssDisplayKeyWord, cssNoneKeyWord);
        $(registrationUsernameUnavailableSignId).css(cssDisplayKeyWord, cssInlineBlockKeyWord);
        $(registrationUsernameUnavailableSignId).css(cssColorKeyWord, cssRedKeyWord);
    }
}

function logErrorInConsole(err) {
    console.log('Error: ' + err);
}

function displayUsernameSign(isAvailable) {
    console.log(isAvailable);
    let av_sign = $(registerUsernameAvailableSignId);
    let unav_sign = $(registrationUsernameUnavailableSignId);
    if (!isAvailable) {
        $(av_sign).css(cssDisplayKeyWord, cssInlineBlockKeyWord);
        $(av_sign).css(cssColorKeyWord, cssGreenKeyWord);
        $(unav_sign).css(cssDisplayKeyWord, cssNoneKeyWord);
    } else {
        $(av_sign).css(cssDisplayKeyWord, cssNoneKeyWord);
        $(unav_sign).css(cssColorKeyWord, cssRedKeyWord);
        $(unav_sign).css(cssDisplayKeyWord, cssInlineBlockKeyWord);
    }
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
