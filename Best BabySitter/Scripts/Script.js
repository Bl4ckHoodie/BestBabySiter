$(".event-advert").click(function () {
    if ($(".selected-advert").length) {
        $(".selected-advert").removeClass("selected-advert");      
    }
    $(this).addClass("selected-advert");
    $(".edit-advert-container").show();
    if (!$(".edit-advert-btn").hasClass("active-tab"))
        $(".edit-advert-btn").addClass("active-tab");
    if ($(".manage-advert-responses-btn").hasClass("active-tab"))
        $(".manage-advert-responses-btn").removeClass("active-tab");
});

$(".manage-advert-responses-btn").click(() => {
    if ($(".edit-advert-btn").hasClass("active-tab")) {
        $(".manage-advert-responses-btn").addClass("active-tab");
        $(".edit-advert-btn").removeClass("active-tab");
        $(".edit-advert-container").hide();
    }
    $.ajax({
        type: "POST",
        url: "/Parent/GetResponses",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

        },
        failure: function (response) {

        },
        error: function (response) {

        }
    });
});

$(".edit-advert-btn").click(() => {
    if ($(".manage-advert-responses-btn").hasClass("active-tab")) {
        $(".manage-advert-responses-btn").removeClass("active-tab");
        $(".edit-advert-btn").addClass("active-tab");
        $(".edit-advert-container").show();
    }
});

function changeAdvert(id) {
    $.ajax({
        type: "POST",
        url: "/Parent/AdvertDetail",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id }),
        dataType: "json",
        success: function (response) {
            $(".cur-ad-city").val(response.City);
            $(".cur-ad-street").val(response.Street);
            $(".cur-ad-start-time").val(formatDate(response.StartTime, 0));
            $(".cur-ad-end-time").val(formatDate(response.EndTime, 0));
            $(".cur-ad-start-date").val(formatDate(response.StartDate, 1));
            $(".cur-ad-end-date").val(formatDate(response.EndDate, 1));
            $(".cur-ad-num-kids").val(response.NumKids);
            $(".cur-ad-age-range").val(response.AgeRange);
            $(".cur-ad-specif").val(response.Specification);
        },
        failure: function (response) {

        },
        error: function (response) {

        }
    });
}

function formatDate(date, f) {
    var d = date.toString().replace('/Date(', '');
    d = d.replace(')/', '');
    d = new Date(parseInt(d)),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear(),
        hours = '' + d.getHours(),
        minutes = '' + d.getMinutes();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;
    if (hours.length < 2)
        hours = '0' + hours;
    if (minutes.length < 2)
        minutes = '0' + minutes;
    if (f == 1)
        return [year, month, day].join('-');
    return [hours, minutes].join(':');
}

$(".update-advert").click(()=>{
    var ad = {"numKids":$(".cur-ad-num-kids").val() , "Specificatons":$(".cur-ad-specif").val() , "startDate":$(".cur-ad-start-date").val(), "endDate":$(".cur-ad-end-date").val() , "startTime":$(".cur-ad-start-time").val(),"endTime":$(".cur-ad-end-time").val(),"ageRange":$(".cur-ad-age-range").val(), "city": $(".cur-ad-city").val(), "street":$(".cur-ad-street").val()};
    $.ajax({
        type: "POST",
        url: "/Parent/UpdateAdvert",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(ad),
        dataType: "json",
        success: function (response) {
            
        },
        failure: function (response) {

        },
        error: function (response) {

        }
    });
});