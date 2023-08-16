// Convert Json Date ("\/Date(1297246301973)\/") to Datetime format :: Import date.format.js in project
function ConvertJson2DatetimeFormat(JsonDateString) {
    var result = "";
    if (JsonDateString != null) {
        var IsDate = JsonDateString.toString();
        var nowDate = new Date(parseInt(IsDate.substr(6)));
        result = nowDate.format("dd/mm/yyyy");
    }
    else {
        var IsDate = new Date();
        result = IsDate.format("dd/mm/yyyy");
    }
    return result;
}

function ConvertJson2DatetimeFormatOrNull(JsonDateString) {
    var result = "";
    if (JsonDateString != null) {
        var IsDate = JsonDateString.toString();
        var nowDate = new Date(parseInt(IsDate.substr(6)));
        result = nowDate.format("dd/mm/yyyy");
    }
    else {
        result = "";
    }
    return result;
}

function ConvertJson2DatetimeFormat2(JsonDateString, IsFormat) {
    var result = "";
    if (JsonDateString != null) {
        var IsDate = JsonDateString.toString();
        var nowDate = new Date(parseInt(IsDate.substr(6)));
        result = nowDate.format(IsFormat);
    }
    else {
        var IsDate = new Date();
        result = IsDate.format(IsFormat);
    }

    //var IsDate = JsonDateString.toString();    
    //var result = "";
    //if (IsDate != "")
    //{
    //    var nowDate = new Date(parseInt(IsDate.substr(6)));
    //    result = nowDate.format("dd/mm/yyyy");
    //}
    return result;
}

function ConvertString2DatetimeFormat(StringDate, Format) {
    var initial = StringDate.split(/\//);
    //server
    //var result = ([initial[1], initial[0], initial[2]].join('/'));

    //local
    var result = ([initial[0], initial[1], initial[2]].join('/'));
    return result;
}

function ConvertString2DatetimeFormatInDate(StringDate) {
    var initial = StringDate.split(/\//);
    //server
    //var result = ([initial[0], initial[1], initial[2]].join('/'));

    //local
    var result = ([initial[1], initial[0], initial[2]].join('/'));
    return result;
}

function ConvertObj2Boolean(IsObj) {
    var result = false;
    if (IsObj || IsObj == "true" || IsObj == "True" || IsObj == 1) { result = true; }
    return result;
}

function ConvertString2DecimalFormat(yourNumber) {
    if (isNaN(yourNumber)) { yourNumber = "0.00" }
    var n = yourNumber.toString().split(".");    
    n[0] = n[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return n.join(".");
}

function getDataListForValue(selector) {
    var result = [];
    $(selector).each(function () {
        result.push($(this).val());
    });
    return result;
}

function getDataListForText(selector) {
    var result = [];
    $(selector).each(function () {
        result.push($(this).text());
    });
    return result;
}

function getDataListForData(selector, dataname) {
    var result = [];
    $(selector).each(function () {
        var temp = $(this).data(dataname);
        result.push(temp);
    });
    return result;
}

function chkNumber(event) {
    var vchar = String.fromCharCode(event.keyCode);
    if (vchar == "." && (vchar < '0' || vchar > '9')) {
        event.preventDefault();
    } else {
        event.onKeyPress = vchar;
    }
}

function isNullReturnBlank(yourValue){
    var vchar = '';
    if (yourValue != null) {
        vchar = yourValue;
    }
    return vchar;
}


