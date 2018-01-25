// Write your JavaScript code.

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

function saveProfileChanges(ev) {
    let firstNameInputModifyUserPrefix = 'FirstNameInput';
    let lastNameInputModifyUserPrefix = 'LastNameInput';
    let currentTargetId = ev.currentTarget.id;
    let currentUser = currentTargetId.replace(new RegExp('^' + buttonsModifyUserPrefix), '');
    let changedFirstName = $('#' + firstNameInputModifyUserPrefix + currentUser).val();
    let changedLastName = $('#' + lastNameInputModifyUserPrefix + currentUser).val();
    if (!isEmpty(changedFirstName.trim()) && !isEmpty(changedLastName.trim())) {
        let user = {
            UserName: currentUser,
            FirstName: changedFirstName,
            LastName: changedLastName
        };
        console.log(user);
        $.ajax({
            type: 'POST',
            url: '/Accounts/ModifyUserInfo',
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify(user),
            success: replaceTheExistingData,
            error: logErrorInConsole
        });
    }

    function replaceTheExistingData(isDataReplaced) {
        if (isDataReplaced) {
            let oldFirstNameLabelId = 'currentUserFirstName' + currentUser;
            let oldLastNameLabelId = 'currentUserLastName' + currentUser;
            $('#' + oldFirstNameLabelId).text(changedFirstName);
            $('#' + oldLastNameLabelId).text(changedLastName);
            $('#hidden' + currentUser).css('display', 'none');
        }
    }

    function isEmpty(str) {
        return (!str || 0 === str.length);
    }
}


//MyProfile Edit Finctions
$("#btnEdit").click(function () {
    if ($("#editMyProfile").hasClass("hidden"))
        $("#editMyProfile").removeClass("hidden");
});

//Get Users Edit
$('.btnUEdit').click(function (ev) {
    let userIdetity = ev.currentTarget.id;
    let userIdentityAsJqueryString = '#hidden' + userIdetity;
    let hiddenUserTemplate = $(userIdentityAsJqueryString);
    $(hiddenUserTemplate).attr('class', 'col-lg-6');
});
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
