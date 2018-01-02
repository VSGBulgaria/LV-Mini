var div = document.getElementById('errors');
//var firstname = document.getElementById('firstname');       //getting firstname input field
//var  addr= document.getElementById('addr');                 //getting addr input field
//var zip = document.getElementById('zip');
//var state = document.getElementById('state');
var name = document.getElementById('name');
var email = document.getElementById('email');               // getting email input field
var username = document.getElementById('username');         //getting username input field 
var password = document.getElementById('password');
var confirm = document.getElementById('confirm');

function formValidator() {

    //Checks each input it this order

    if (isAlphabet(name, "Please enter only letters")) {               
        if (emailvalidator(email, "Email unvalid") && notEmpty(email, "Email field should be filled")) {
            if (lengthRestriction(username, 6, 8) && notEmpty(username, "Username should be filled")) {
                if (lengthRestriction(password, 7, 9) && notEmpty(password, "Enter a password")) {
                    if (lengthRestriction(confirm, 7, 9) && notEmpty(confirm, "Repeat Password")) {
                        // if (GetSelectedItem()) {
                        // if (CheckBox()) {
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
        };
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