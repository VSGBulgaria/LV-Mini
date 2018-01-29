
//Footer In Login Page
(function () {
    $(document).ready(() => {
        $('.fa.fa-facebook').parent().click(e => {
            e.preventDefault();
            window.location = "https://www.facebook.com";
        });
    });
})();

(function () {
    $(document).ready(() => {
        $('.fa.fa-google-plus').parent().click(e => {
            e.preventDefault();
            window.location = 'https://www.plus.google.com/';
            });
    });
})();

(function () {
    $(document).ready(() => {
        $('.fa.fa-twitter').parent().click(e => {
            e.preventDefault();
            window.location = "https://www.twitter.com";
        });
    });
})();

(function () {
    $(document).ready(() => {
        $('.fa.fa-instagram').parent().click(e => {
            e.preventDefault();
            window.location = "https://www.instagram.com";
        });
    });
})();

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
let jqueryClickKwyWord = 'click';
let registerUsernameAvailableSignId = '#username_available_sign';
let registrationUsernameUnavailableSignId = '#username_unavailable_sign';
let minimumUsernameLenght = 3;
let jqueryButtonsModifyUserInfoClass = '.modify-users-buttons';
let buttonsModifyUserPrefix = 'btnModify';


//Validate Username
$(registerUsernameTagId).on(jqueryKeyUpKeyWord, checkForAvailableUsername);

function checkForAvailableUsername(ev) {
    ev.preventDefault();
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
    let available_sign = $(registerUsernameAvailableSignId);
    let unavailable_sign = $(registrationUsernameUnavailableSignId);
    if (!isAvailable) {
        $(available_sign).css(cssDisplayKeyWord, cssInlineBlockKeyWord);
        $(available_sign).css(cssColorKeyWord, cssGreenKeyWord);
        $(unavailable_sign).css(cssDisplayKeyWord, cssNoneKeyWord);
    } else {
        $(available_sign).css(cssDisplayKeyWord, cssNoneKeyWord);
        $(unavailable_sign).css(cssColorKeyWord, cssRedKeyWord);
        $(unavailable_sign).css(cssDisplayKeyWord, cssInlineBlockKeyWord);
    }
}

$(jqueryButtonsModifyUserInfoClass).on(jqueryClickKwyWord, saveProfileChanges);

