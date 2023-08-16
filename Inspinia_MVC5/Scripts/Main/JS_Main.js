
//----------------------------------------------Load Master Page---------------------------------------------
//โหลดส่วนของหน้าจอ เมนูด้านซ้าย
function loadLeftMenu(url) {
    var EmpID = $("#EmpID").val();
    $('#MenuBar').load(url, {
        EmpID: EmpID
    }
        , function () {

    });
}

//โหลดส่วนของหน้าจอ เมนูด้านบน
function loadTopMenu(url) {
    $('#TopBar').load(url, function () {

    });
}

//โหลดส่วนของหน้าจอ บอดี้
function loadInBodyPage(url) {
    $('#BodyBar').load(url, function () {
        $('#application').text($('#isapplicationname').text());
        $('#menu').text($('#ismenu').text());
        $('#submenu').text($('#issubmenu').text());
    });
}


//โหลดส่วนของลิ้งที่เป็นรายงานแสดงหน้าจอออกไป
function loadNewPage(url)
{
    window.open(url, '_blank');
}

function loadPage(url)
{
    window.open(url, "_self");
}

function loadReport(url)
{
    $('#BodyBar').load(url, function () {

    });
}
//------------------------------------------End Load Master Page---------------------------------------------

