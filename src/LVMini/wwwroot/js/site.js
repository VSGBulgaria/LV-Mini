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

function logErrorInConsole(err) {
    console.log('Error: ' + err);
}

function displayUsernameSign(isAvailable) {
    console.log(isAvailable);
    let available_sign = $('#username_available_sign');
    let unavailable_sign = $('#username_unavailable_sign');
    if (!isAvailable) {
        $(available_sign).css('display', 'inline-block');
        $(available_sign).css('color', 'green');
        $(unavailable_sign).css('display', 'none');
    } else {
        $(available_sign).css('display', 'none');
        $(unavailable_sign).css('color', 'red');
        $(unavailable_sign).css('display', 'inline-block');
    }
}

$('.modify-users-buttons').on('click', saveProfileChanges);

//Admin Edit Profile
function saveProfileChanges(ev) {
    let currentTargetId = ev.currentTarget.id;
    let currentUser = currentTargetId.replace(new RegExp('^btnModify'), '');
    let allLabels = $('.form-control');
    let labelsNeedForExtractingVhanges = [];
    for (let label of allLabels) {
        if (typeof label.id !== 'undefined' && label.id.endsWith(currentUser)) {
            labelsNeedForExtractingVhanges.push(label);
        }
    }
    let labelRegex = new RegExp('([\w]+)(TestUN12)');
    let properiesOfTheUserModel = [];
    let labelsSubfix = 'Input' + currentUser;
    for (let label of labelsNeedForExtractingVhanges) {
        properiesOfTheUserModel.push(label.id.replace(labelsSubfix, ''));
    }
    let user = {};
    for (let property of properiesOfTheUserModel) {
        if (property === 'IsActive') {
            user[property] = $('#' + property + 'Input' + currentUser).val().toLowerCase() === 'true';
        } else {
            user[property] = $('#' + property + 'Input' + currentUser).val();
        }
    }
    user['Username'] = currentUser;
    $.ajax({
        type: 'POST',
        url: '/Admin/ModifyUserInfo',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(user),
        success: replaceTheExistingData,
        error: logErrorInConsole
    });
    function replaceTheExistingData(isDataReplaced) {
        if (isDataReplaced) {
            //TODO: refactor the replacing 
            let oldFirstNameLabelId = 'currentUserFirstName' + currentUser;
            let oldLastNameLabelId = 'currentUserLastName' + currentUser;
            $('#' + oldFirstNameLabelId).text(changedFirstName);
            $('#' + oldLastNameLabelId).text(changedLastName);
            $('#hidden' + currentUser).css('display', 'none');
        }
    }
}

function isEmpty(str) {
    return (!str || 0 === str.length);
}


//My Profile 
$('#saveMyProfileChangesButton').on('click', function () {
    let currentUserChangedValues = $('.form-control');
    let usersChangedValues = {
        Email: undefined,
        FirstName: undefined,
        LastName: undefined
    };
    let emailId = 'email';
    let firstNameId = 'FirstName';
    let lastNameId = 'LastName';
    for (let userInput of currentUserChangedValues) {
        if (userInput.id.toUpperCase() === emailId.toUpperCase()) {
            usersChangedValues.Email = $(userInput).val();
        } if (userInput.id.toUpperCase() === firstNameId.toUpperCase()) {
            usersChangedValues.FirstName = $(userInput).val();
        } if (userInput.id.toUpperCase() === lastNameId.toUpperCase()) {
            usersChangedValues.LastName = $(userInput).val();
        }
    }


    checkIsDataCorrect(usersChangedValues);
    if (checkIsDataCorrect(usersChangedValues)) {
        sendChanges(usersChangedValues);
    }
    hideMyprofileChangesForm();

    function sendChanges(data) {
        $.ajax({
            type: 'POST',
            url: '/Accounts/ModifyMyProfileInfo',
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: replaceChangedValues,
            error: logErrorInConsole
        });
    }


    function checkIsDataCorrect(myProfileData) {
        let expr = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        let emailResult = expr.test(myProfileData['Email']);
        let firstNameResult = !isEmpty(myProfileData['FirstName']);
        let lastNameResult = !isEmpty(myProfileData['LastName']);
        return emailResult && firstNameResult && lastNameResult;
    }


    function replaceChangedValues(changesApplied) {
        if (changesApplied) {
            let oldDataFields = $('.control-label');
            for (let dataField of oldDataFields) {
                if ($(dataField).attr('class') === 'control-label') {
                    for (let prop in usersChangedValues) {
                        if ($(dataField).attr('for').toUpperCase() === prop.toUpperCase()) {
                            $(dataField).html(usersChangedValues[prop]);
                        }
                    }
                }
            }
        }
    }
});

$('#Cancel').on('click', hideMyprofileChangesForm);

function hideMyprofileChangesForm() {
    if (!$('#editMyProfile').hasClass('hidden')) {
        $('#editMyProfile').addClass('hidden');
    }
}


//MyProfile Edit Finctions
$('#btnEdit').click(function () {
    if ($('#editMyProfile').hasClass('hidden'))
        $('#editMyProfile').removeClass('hidden');
});

//Get Users Edit
$('.btnUEdit').click(function (ev) {
    let userIdetity = ev.currentTarget.id;
    let userIdentityAsJqueryString = '#hidden' + userIdetity;
    let hiddenUserTemplate = $(userIdentityAsJqueryString);
    $(hiddenUserTemplate).attr('class', 'col-lg-6');
    $(hiddenUserTemplate).css('display', 'inline-block');
});