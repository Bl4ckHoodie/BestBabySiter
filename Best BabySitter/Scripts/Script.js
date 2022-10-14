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

    $(".create-advert-end-date").prop('disabled', true);
    $(".create-advert-start-time").prop('disabled', true);
    $(".create-advert-end-time").prop('disabled', true);
});

//========================================================================================
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
        $(".create-advert-end-date").prop('disabled', true);
        $(".create-advert-start-time").prop('disabled', true);
        $(".create-advert-end-time").prop('disabled', true);
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
function changeAdvertforSitter(id) {
    $(".edit-advert-container").show();
    $.ajax({
        type: "POST",
        url: "/Sitter/AdvertDetail",
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

$(".verify-sitter").click(function () {
    if ($(".selected-sitter").length)
        $(".selected-sitter").removeClass("selected-sitter");
    $(this).addClass("selected-sitter");
    $(".verify-block-sitter").show();
});

$(".create-advert-start-date").click(() => {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); 
    var yyyy = today.getFullYear();
    today = yyyy + '-' + mm + '-' + dd  ;
    $(".create-advert-start-date").prop("min", today);   
});

$(".create-advert-start-date").change(() => {
    if ($(".create-advert-start-date").val() != '') {
        $(".create-advert-end-date").prop('disabled', false);

        if ($('.create-advert-end-date').val() != '') {
            if ($('.create-advert-start-date').val() > $('.create-advert-end-date').val())
                $('.create-advert-end-date').val($('.create-advert-start-date').val());
        }
    }
});

$(".create-advert-end-date").click(() => {
    $(".create-advert-end-date").prop("min", $(".create-advert-start-date").val());
});

function getverifySitter(id) {

    $.ajax({
        type: "POST",
        url: "/Admin/getSitterToVerify",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id }),
        dataType: "json",
        success: function (response) {
            if (response != null) {
                $(".verify-sitter-info-lname").html(response.L_name);
                $(".verify-sitter-info-fname").html(response.f_name);
                $(".verify-sitter-info-email").html(response.email);
                //$(".verify-sitter-info-lname").html(response.L_name);
            }
        },
        failure: function (response) {
        },
        error: function (response) {
        }
    });
}

$(".create-advert-end-date").change(() => {
    if ($(".create-advert-end-date").val() != '') {
        if ($(".create-advert-start-date").prop("min") == $(".create-advert-end-date").val()) {
            var today = new Date();
            var hh = String(today.getHours()).padStart(2, '0');
            var mm = String(today.getMinutes()).padStart(2, '0');
            today = hh + ":" + mm;
            $(".create-advert-start-time").prop("min", today);          
        }else {
            $(".create-advert-end-time").prop('min', '');
        }
        $(".create-advert-start-time").prop('disabled', false);
        
    }
});

$(".create-advert-start-time").change(() => {
    if ($(".create-advert-start-time").val() != '') {
        $(".create-advert-end-time").prop('min', $(".create-advert-start-time").val());
        $(".create-advert-end-time").prop('disabled', false);
    }
});

$(".create-advert-default-location").change(() => {
    if ($(".create-advert-default-location").prop("checked")) {
        $.ajax({
            type: "POST",
            url: "/Parent/GetDefaultLocation",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response != null) {

                    $(".create-advert-street").val(response.street);
                    $(".create-advert-city").val(response.city);
                }
            },
            failure: function (response) {
            },
            error: function (response) {
            }
        });
    } else {
        $(".create-advert-street").val("");
        $(".create-advert-city").val("");
    }
});

$('.close-this-advert').click(() => {
    $(".loader-container").add('<div class="load-waiter"> <div class= "spinner"></div ></div >');
    $.ajax({
        type: "POST",
        url: "/Parent/CloseAdvert",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            location.reload();
        },
        failure: function (response) {
        },
        error: function (response) {
        }
    });
    $('.load-waiter').remove();
});

$('.advert-start-date').change(() => {
    if ($('.advert-start-date').val() != '') {
        var today = new Date();
        var dd = String(today.getDate()).padStart(2, '0');
        var mm = String(today.getMonth() + 1).padStart(2, '0');
        var yyyy = today.getFullYear();
        today = yyyy + '-' + mm + '-' + dd;
        $('.advert-start-date').prop("min", today);
        $('.advert-end-date').prop('min', $('.advert-start-date').val());
        if ($('.advert-end-date').val() != '') {
            if ($('.advert-start-date').val() > $('.advert-end-date').val())
                $('.advert-end-date').val($('.advert-start-date').val());
        }
    }
});
$('.cancel-create-advert').click(() => {
    $('.advert-start-date').prop("min",'');
    $('.advert-end-date').prop("min", '');
});
$(".create-advert-end-date").prop('disabled', true);
$(".create-advert-start-time").prop('disabled', true);
$(".create-advert-end-time").prop('disabled', true);


$(".decline-verify-sitter-btn").click(function() {
    var id = 0;
    $.ajax({
        type: "POST",
        url: "/Admin/verifySitter",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id }),
        dataType: "json",
        success: function (response) {
        },
        failure: function (response) {
        },
        error: function (response) {
        }
    });
    location.reload();
});
$(".accept-verify-sitter-btn").click(function () {
    var id = 1;
    $.ajax({
        type: "POST",
        url: "/Admin/verifySitter",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id }),
        dataType: "json",
        success: function (response) {
        },
        failure: function (response) {
        },
        error: function (response) {
        }
    });

    location.reload();
});

//=================================================================================/

$(".register-sitter-btn").click(() => {
    $(".loader-container").add('<div class="load-waiter"> <div class= "spinner"></div ></div >');
    var fileUpload = $("#sitter_cv").get(0);
    var files = fileUpload.files;

    var formData = new FormData();

    // Looping over all files and add it to FormData object  
    for (var i = 0; i < files.length; i++) {
        console.log('(files[i].name:' + files[i].name);
        formData.append('sitterCV', files[i]);
    }

        // You can update the jquery selector to use a css class if you want
        $.ajax({
            type: 'POST',
            url: '/Home/uploadCVFile',
            data: formData,
            processData: false,
            contentType: false,
            success: function (data) {
            }
        });
    $('.load-waiter').remove();
});

$("#show-pass").click(() => {
    if ($("#show-pass").prop("checked")) {
        $(".form-pass").prop("type", "text")
    } else {
        $(".form-pass").prop("type", "password");
    }
        });

