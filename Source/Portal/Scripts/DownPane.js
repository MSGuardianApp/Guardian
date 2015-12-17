function changeContent(temp) {
    //debugger;
    if (temp == "t") {
        $("#attachmentContent").removeClass("appear");
        $("#attachmentContent").addClass("disappear");
        $("#trackingContent").removeClass("disappear");
        $("#trackingContent").addClass("appear");

    }

    else if (temp == "a") {

        $("#trackingContent").removeClass("appear");
        $("#trackingContent").addClass("disappear");
        $("#attachmentContent").removeClass("disappear");
        $("#attachmentContent").addClass("appear");
    }

    else if (temp == "plus") {
        $("#downMore").removeClass("disappear");
        $("#downMore").addClass("appear");
        $("#plus").removeClass("appear");
        $("#plus").addClass("disappear");
        $("#minus").removeClass("disappear");
        $("#minus").addClass("appear");
        $('#downMore').addClass("setMargin");
        $("#trackingContent").removeClass("disappear");
        $("#trackingContent").addClass("appear");
        
    }

    else if (temp == "minus") {
        $("#downMore").removeClass("appear");
        $("#downMore").addClass("disappear");
        $("#plus").removeClass("disappear");
        $("#plus").addClass("appear");
        $("#minus").removeClass("appear");
        $("#minus").addClass("disappear");
    }
}