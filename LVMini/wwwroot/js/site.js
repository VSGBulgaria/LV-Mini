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

//Functions For Registration Form
var div = document.getElementById('errors');
var email = document.getElementById('email');               // getting email input field
var username = document.getElementById('username');         //getting username input field 
var password = document.getElementById('password');
var confirm = document.getElementById('confirm');

function formValidator() {

    //Checks each input it this order

    if (isAlphabet(name, "Please enter only letters")) {
        if (emailvalidator(email, "Email unvalid")) {
            if (lengthRestriction(username, 6, 8)) {
                if (lengthRestriction(password, 7, 9)) {
                    if (lengthRestriction(confirm, 7, 9)) {
                        alert("Registrtion succesfuly completed");
                        return true;
                    }
                }
            }
        }
    }


    false;

    function returnError(errorMassage) {
        div.innerHTML = "</hr><p>" + errorMassage + "</hr><p>";
    }

    function clearError(elem) {
        elem.onfocus = function () {
            div.innerHTML = "";
        }
    }

    function notEmpty(elem, errorMassage) {
        if (elem.value.length === 0) {
            returnError(errorMassage);
            clearError(elem);
            return false;
        }
        return true;
    }

    function isNumeric(elem, errorMassage) {
        var numericExp = /^[0 - 9]+$/;
        if (elem.value.match(mumericExp)) {
            return true;
        } else {
            returnError(errorMassage);
            clearError(elem);
            return false;
        }
    }

    function isAphabet(elem, errorMassage) {
        var alphaExp = /^[a - z A - Z]+$/;
        if (elem.value.match(alphaExp)) {
            return true;
        } else {
            returnError(errorMassage);
            clearError(elem);
            return false;
        }
    }

    function isAlphanumeric(elem, errorMassage) {
        var alphaExp = /^[0-9 a-z A-Z]+$/;
        if (elem.value.match(alphaExp)) {
            return true;
        } else {
            returnError(errorMassage);
            clearError(elem);
            return false;
        }
    }

    function lengthRestriction(elem, errorMassage) {
        var uInput = elem.value;
        if (uInput.length >= min && uInput.length <= max) {
            return true;
        } else {
            returnError("The length of the field must be between " + min + " and " + max);
            clearError(elem);
            return false;
        }

    }

    function emailValidator(elem, errorMassage) {
        var emailExp = /^[\w\-\.\+] + \@ [A-Z a-z 0-9\.\-] + \.[A-Z a-z 0-9]{2,4}$/;
        if (elem.value.match(emailExp)) {
            return true;
        } else {
            returnError(errorMassage);
            clearError(elem);
            return false;
        }
    }
}