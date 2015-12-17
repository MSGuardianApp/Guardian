WL.Event.subscribe("auth.login", onGroupLogin);
WL.init({
    client_id: GuardianConfig.LiveClientID,
    redirect_uri: GuardianConfig.PortalUrl,
    scope: "wl.signin",
    response_type: "token"
});
WL.ui({
    name: "signin",
    element: "groupLoginLink"
});


function onGroupLogin(session) {
    // alert('inside onlogin before calls');
    // alert('Session' + JSON.stringify(session));
    var session = WL.getSession();
    // alert(session);


    if (!session.error) {
        WL.api({
            path: "me",
            method: "GET"
        }).then(
            function (response) {
                // alert('inside onlogin');
                // alert('response' + JSON.stringify(response));
                document.getElementById("txtLiveAuthID").value = session.authentication_token;
                setSession('gSession', response);
                setSession('userName', response.first_name + " " + response.last_name + " (Group Admin)")
                document.getElementById("loggedInName").innerText = getSession("userName");
                $("#groupSetting").show();
                $("#LiveDetails").show();
                setSession('email', response.emails.account);
                setSession('userType', 'group');

                getProfile(response.emails.account, 'group');
                

            },
            function (responseFailed) {
                // alert('inside error');
                document.getElementById("errorLabel").innerText =
                    "Error: " + responseFailed.error.message;
            }
        );
    }
    else {
        // alert('onLogin session error');
        document.getElementById("errorLabel").innerText =
            "Error logging in: " + session.error_description;
    }

}

function Logout() {
    var session = WL.getSession();
    //alert(session);
    removeSession('gSession');
    clear();
    //document.getElementById('loggedInName').innerText = "Guest";
    //WL.logout();
    //clearUserList();
    // alert('inside logout');

}