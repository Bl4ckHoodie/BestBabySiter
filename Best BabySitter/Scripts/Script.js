$(".event-advert").click(function(){
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
});
$(".edit-advert-btn").click(() => {
    if ($(".manage-advert-responses-btn").hasClass("active-tab")) {
        $(".manage-advert-responses-btn").removeClass("active-tab");
        $(".edit-advert-btn").addClass("active-tab");
        $(".edit-advert-container").show();
    }
});