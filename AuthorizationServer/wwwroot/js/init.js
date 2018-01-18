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
