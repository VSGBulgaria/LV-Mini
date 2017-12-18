// Write your JavaScript code.
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