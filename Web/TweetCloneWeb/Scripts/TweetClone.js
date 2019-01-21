// TweetClone js application functions
var TweetClone = {};

// Sign up to application
TweetClone.signup = function () { 
    var form = $('#signupForm');
    //form.validate();
    var formData = form.serialize();
    if (form[0].checkValidity())
        {
    $.post('/Profile/Signup',formData,function (result) {
            if (result)
                $('#msgSignup').html(result);
            else
                window.location.href = '/Tweet/Stream';
        });    
    }
}

// login to application
TweetClone.login = function () {
    var formData = $('#loginForm').serialize();
    $.post('/Profile/Login',formData,function (result) {
            if (result === true)
                window.location.href = '/Tweet/Stream';
            else
                $('#msgLogin').show();
        });
}

// update info and tweet board
TweetClone.update = function() {
    var formData = $('#tweetForm').serialize();
    $.post('Update', formData, function (view) {
        $("#tweetBoard").html(view);
        TweetClone.refreshInfo();       
    });
    $('#tweetForm')[0].reset();
}

// follow other user
TweetClone.follow = function () {
    var formData = $('#followForm').serialize();
    $.post('Follow', formData, function (view) {
        $("#tweetInfo").html(view);
        TweetClone.refreshBoard();
    });
    $('#followForm')[0].reset();
}

// refresh info bar
TweetClone.refreshInfo = function () {
    $.get("RefreshInfo", function (view) {
        $("#tweetInfo").html(view);
    });
}

// refresh tweet board
TweetClone.refreshBoard = function () {
    $.get("RefreshBoard", function (view) {
        $("#tweetBoard").html(view);
    });
}

// update profile
TweetClone.updateProfile = function () {
    var formData = $('#profileForm').serialize();
    $.post('/Profile/Save', formData, function (result) {
        if (result)
            $('#msg').html(result);
        else {
            $('#msg').html("Profile changed saved.");
        }            
    });
}

// delete profile
TweetClone.deleteProfile = function () {
    $.post("/Profile/Delete", function (result) {
        $("#msg").html("");
        if (result === false) {
            $("#msg").html("Profile could not be removed. Please contact administrator.");
        }
        else
        {
            $("#mainProfContainer").html(result);
        }
            
    });
}