window.onload = function() {
    var Hash = new jsSHA("SHA-512", "TEXT");

    document.getElementById('loginForm').onsubmit = function() {
        var usernamebox = document.getElementByName('Username');
        var passwordbox = document.getElementByName('Password');

        var username = usernamebox.value;
        var password = passwordbox.value;

        Hash.update(username + password);
        password = Hash.getHash("TEXT");

        passwordbox.value = password;
    };

    document.getElementById('signupForm').onsubmit = function() {
        var usernamebox = document.getElementByName('Username');
        var passwordbox = document.getElementByName('Password');
        var repasswordbox = document.getElementByName('RetypePassword');

        var username = usernamebox.value;
        var password = passwordbox.value;
        var repassword = repasswordbox.value;

        Hash.update(username + password);
        password = Hash.getHash("TEXT");

        Hash.update(username + repassword);
        repassword = Hash.getHash("TEXT");

        passwordbox.value = password;
        repasswordbox.value = repassword;
    };
};​