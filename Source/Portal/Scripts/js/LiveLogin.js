WL.init({
    client_id: GuardianConfig.LiveClientID,
    redirect_uri: GuardianConfig.PortalUrl,
    scope: ["wl.signin", "wl.emails"]
    //response_type: "token",

});
WL.ui({
    name: "signin",
    element: "logoutLink",
    onloggedout: Logout,
    onerror: function (err) {
        //alert(JSON.stringify(err) + 'live error');
    },
    type: "custom",
    sign_in_text: "Sign in",
    sign_out_text: "Sign out"
});
WL.ui({
    name: "signin",
    element: "loginLink",
    onloggedin: onLogin,
    //onloggedout: Logout,
    onerror: function (err) {
        //alert(JSON.stringify(err) + 'live error');
    },
    type: "custom",
    sign_in_text: "Sign in",
    sign_out_text: "Sign out"
});

function onLogin(session) {
    WL.login({
        scope: ["wl.signin", "wl.emails"]
    }).then(function (response) {
        $("#errorLabelLogin").text("");
        if (!session.error) {
            WL.api({
                path: "me",
                method: "GET"
            }).then(
                function (response) {
                    if (response.emails === undefined || response.emails.account === undefined) {
                        $("#errorLabelLogin").text("Login attempt didn't succeeded! Please try again ")
                        Logout();
                        return;
                    }
                    var boolgroup = $("#GroupLogin").is(":checked");
                    var strUser = "User";
                    var typeuser = " (User)";
                    if (boolgroup == true) {
                        typeuser = " (Group Admin)";
                        strUser = "Group";
                    }

                    setSession('gSession', response);
                    setSession('userName', response.first_name + " " + response.last_name + typeuser);
                    setSession('authentication_token', session.session.authentication_token);
                    setSession('email', response.emails.account);
                    var result = false;
                    if (strUser == "Group") {
                        setSession('userType', 'group');
                        setSession('userTypeAuth', 'a');
                        $("#groupSetting").show();
                        result = getProfile(response.emails.account, 'group');
                    }
                    else if (strUser == "User") {
                        setSession('userType', 'user');
                        setSession('userTypeAuth', 'u');
                        $("#groupSetting").hide();
                        result = getProfile(response.emails.account, 'user');
                    }
                    if (result) {
                        document.getElementById("loggedInName").innerText = getSession("userName");
                        HideLoginPopup();
                        $('#defultLoginLink').hide();
                    }
                },
                function (responseFailed) {
                    document.getElementById("errorLabel").innerText =
                        "Error: " + responseFailed.error.message;
                }
            );
        }
        else {
            document.getElementById("errorLabel").innerText =
                "Error logging in: " + session.error_description;
        }
    });
}

function Logout() {
    removeSession('gSession');
    clear();
    document.getElementById('loggedInName').innerText = "Guest";
    $("#groupSetting").hide();
    WL.logout();
    clearUserList();
    $('#defultLoginLink').show();
    ShowLoginPopup();

}

function BypassLiveLogin() {
    var boolgroup = $("#GroupLogin").is(":checked");
    var strUser = "User";
    var typeuser = " (User)";
    if (boolgroup == true) {
        typeuser = " (Group Admin)";
        strUser = "Group";
    }

    var result = false;
    if (strUser == "Group") {
        setSession('authentication_token', "<RelaceWithAuthToken>");
        if (getSession('authentication_token').indexOf('<') != 0) {
            setSession('email', "sochyd@live.com");
            setSession('userName', "SOCHYD" + typeuser);
            setSession('gSession', "Debug");

            setSession('userType', 'group');
            setSession('userTypeAuth', 'a');
            $("#groupSetting").show();
            result = getProfile("sochyd@live.com", 'group');
        }
    }
    else if (strUser == "User") {
        setSession('authentication_token', "<RelaceWithAuthToken>");
        if (getSession('authentication_token').indexOf('<') != 0) {
            setSession('email', "vrreddy.d@live.com");
            setSession('userName', "VR" + typeuser);
            setSession('gSession', "Debug");

            setSession('userType', 'user');
            setSession('userTypeAuth', 'u');
            $("#groupSetting").hide();
            result = getProfile("vrreddy.d@live.com", 'user');
        }
    }
    if (result) {
        document.getElementById("loggedInName").innerText = getSession("userName");
        HideLoginPopup();
        $('#defultLoginLink').hide();
    }
}