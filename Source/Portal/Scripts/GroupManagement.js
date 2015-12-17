var AuthID;
var Utype;

$(document).ready(function () {
    AuthID = getSession('authentication_token');
    Utype = getSession('userTypeAuth');
});



function NavigateToGroupManagement() {
    window.location.href = "CreateGroup.aspx?AuthID=" + AuthID + "&Utype=" + Utype;
}