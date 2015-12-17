
var SubjectUser = { "userIndex": "-1", "pin": "-1", "info": { "infobox": "-1", "description": "-1", "address": "-1" }, "currentLocation": { "lat": "-1", "long": "-1" } }

function reverseGeocodeRequest(Lat, Long, callbackFunction) {
    var request =
    {
        location: new Microsoft.Maps.Location(Lat, Long),
        callback: callbackFunction,
        errorCallback: onReverseGeocodeFailed
    };
    SearchManager.reverseGeocode(request);
}
function onReverseGeocodeSuccess(result) {
    if (result) {
        result.name
    }
    else {
    }
}
function onReverseGeocodeFailed(result) {
    //alert('failed');
}

function portalUIFunctionality() {
    $('#startDateRange').datepicker();
    $('#endDateRange').datepicker();
}
function DateFormat(date) {
    var currentTimeStamp = (parseInt(date.getMonth()) + parseInt('1')).toString() + '-' + date.getDate() + '-' + date.getFullYear();
    return currentTimeStamp;
}

function DateDeserialize(dateTicks) {
    var date = ConvertTicksToDate(dateTicks);
    //currentTimeStamp = date.toLocaleString("en-GB");
    var currentTimeStamp = pad(date.getDate(), 2) + '/' + pad((parseInt(date.getMonth()) + parseInt('1')), 2) + '/' + date.getFullYear() + '  ' + pad(date.getHours(), 2) + ':' + pad(date.getMinutes(), 2) + ':' + pad(date.getSeconds(), 2);
    return currentTimeStamp;
}
function datediff(fromDate, toDate, interval) {
    /*
     * DateFormat month/day/year hh:mm:ss
     * ex.
     * datediff('01/01/2011 12:00:00','01/01/2011 13:30:00','seconds'); - obsolete
     * datediff('635181033340135207','635181033350135207','seconds'); 
     */
    var second = 1000, minute = second * 60, hour = minute * 60, day = hour * 24, week = day * 7;
    fromDate = ConvertTicksToDate(fromDate);
    toDate = ConvertTicksToDate(toDate);
    var timediff = toDate - fromDate;
    if (isNaN(timediff)) return NaN;
    switch (interval) {
        case "years": return toDate.getFullYear() - fromDate.getFullYear();
        case "months": return (
            (toDate.getFullYear() * 12 + toDate.getMonth())
            -
            (fromDate.getFullYear() * 12 + fromDate.getMonth())
        );
        case "weeks": return Math.floor(timediff / week);
        case "days": return Math.floor(timediff / day);
        case "hours": return Math.floor(timediff / hour);
        case "minutes": return Math.floor(timediff / minute);
        case "seconds": return Math.floor(timediff / second);
        default: return undefined;
    }
}
function ConvertTicksToDate(ticks) {

    var d = new Date(((ticks - 621355968000000000) / 10000) - 19800000);
    if (d.toString().indexOf('UTC') > 0) {
        var Sign = d.toString().substr(d.toString().indexOf('UTC') + 3, 1);
        var hOff = d.toString().substr(d.toString().indexOf('UTC') + 4, 2);
        var hIOff = parseInt(hOff);

        var mOff = d.toString().substr(d.toString().indexOf('UTC') + 6, 2);
        var mIOff = parseInt(mOff);

        d = new Date(d.setHours(d.getHours() - hIOff));
        d = new Date(d.setMinutes(d.getMinutes() - mIOff));
        return d;
    }
    else
        return d;
}

function pad(number, length) {

    var str = '' + number;
    while (str.length < length) {
        str = '0' + str;
    }
    return str;
}

