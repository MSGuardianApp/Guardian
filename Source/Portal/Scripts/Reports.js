
window.onload = function () {

    document.getElementById("AuthID").value = getSession('authentication_token');
    document.getElementById("UType").value = getSession('userTypeAuth');

    if (document.getElementById("GroupId") != null)
        document.getElementById("GroupId").value = getSession('GroupID');
};


