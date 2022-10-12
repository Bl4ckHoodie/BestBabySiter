$(".event-advert").click(function() {
    if ($(".selected-advert").length) {
        $(".selected-advert").removeClass("selected-advert");
    }
    $(this).addClass("selected-advert");

    
    if (!($(".edit-advert-btn").hasClass("active-tab")))
        $(".edit-advert-btn").addClass("active-tab");  
    if ($(".manage-advert-responses-btn").hasClass("active-tab"))
        $(".manage-advert-responses-btn").removeClass("active-tab");
    $(".edit-advert-reposonse-container").hide();
    $(".advert-repsonse-container").hide();
    if ($(".selected-response-sitter").length)
        $(".selected-response-sitter").removeClass("selected-response-sitter");
    $(".sitter-event-adverts").html("");
  
});

$(".manage-advert-responses-btn").click(() => {
    if ($(".edit-advert-btn").hasClass("active-tab")) {
        $(".manage-advert-responses-btn").addClass("active-tab");
        $(".edit-advert-btn").removeClass("active-tab");
        $(".edit-advert-container").hide();
        $(".edit-advert-reposonse-container").show();
        $(".sitter-event-adverts").html("");
        $.ajax({
            type: "POST",
            url: "/Parent/GetResponses",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response != null) {
                    for (var i = 0; i < response.length; i++) {

                        $(".sitter-event-adverts").append("<div class='sitter-response' onclick='changeSitter(" + i + ")'> <div class= 'advert-info' ><a class='advert-icon fa fa-user'></a>" + response[i].L_name + " " + response[i].f_name + "<br/><Label>" + response[i].street + ", " + response[i].city + "</Label></div > </div >");
                    }
                }

            },
            failure: function (response) {

            },
            error: function (response) {

            }
        });
    }
    
});

$(".edit-advert-btn").click(() => {
    if ($(".manage-advert-responses-btn").hasClass("active-tab")) {
        $(".manage-advert-responses-btn").removeClass("active-tab");
        $(".edit-advert-btn").addClass("active-tab");
        $(".edit-advert-container").show();
        $(".edit-advert-reposonse-container").hide();
        $(".advert-repsonse-container").hide();
        if ($(".selected-response-sitter").length)
            $(".selected-response-sitter").removeClass("selected-response-sitter");
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
    $(".edit-advert-container").show();
    
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

$(".update-advert").click(function () {

    let adv = JSON.stringify({ "NumKids": $(".cur-ad-num-kids").val(), "Specification": $(".cur-ad-specif").val(), "StartDate": $(".cur-ad-start-date").val(), "EndDate": $(".cur-ad-end-date").val(), "StartTime": $(".cur-ad-start-time").val(), "EndTime": $(".cur-ad-end-time").val(), "AgeRange": $(".cur-ad-age-range").val(), "City": $(".cur-ad-city").val(), "Street": $(".cur-ad-street").val()});
    $.ajax({
        type: "POST",
        url: "/Parent/UpdateAdvert",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ "adv": adv }),
        dataType: "json",
        success: function (response) {
            
        }
    });
});

function changeSitter(loc) {
    console.log(loc);
    $(".sitter-response").click(function () {
        if ($(".selected-response-sitter").length)
            $(".selected-response-sitter").removeClass("selected-response-sitter");
        $(this).addClass("selected-response-sitter");
    });
    $.ajax({
        type: "POST",
        url: "/Parent/GetResponses",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                $(".advert-response-sitter-name").html(response[loc].L_name + " " + response[loc].f_name);

                $(".advert-response-sitter-email").html(response[loc].email);
                $(".advert-response-sitter-phone").html(response[loc].contact_NO);
                $(".advert-response-city").html(response[loc].city);
                $(".advert-response-sitter-charge").html("R "+response[loc].chargePerService);
                $(".advert-response-sitter-duration").html(response[loc].service_Duration);
                $(".advert-response-sitter-about").html(response[loc].AboutMe);
                //$(".advert-response-sitter-education").html(response[loc].);

                
            }

        },
        failure: function (response) {

        },
        error: function (response) {

        }
    });
    $(".advert-repsonse-container").show();
}

