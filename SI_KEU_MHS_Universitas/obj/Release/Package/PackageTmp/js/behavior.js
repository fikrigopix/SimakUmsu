$(document).ready(function () {    
    //add bullet between list
    $(function () {
        var lastElement = false;
        $(".nav-footer ul li").each(function () {
            if (lastElement && lastElement.offset().top != $(this).offset().top) {
                $(lastElement).addClass("nobullet");
            }
            lastElement = $(this);
        }).last().addClass("nobullet");
    });

});


$(document).on('click', '.yamm .dropdown-menu', function (e) {
    e.stopPropagation()
})

$(document).ready(function () {
    $(".trigger-mm").click(function () {
        $(".panel-mm").toggle();
        $(".container-mm").addClass("animated fadeInLeft");
        $(this).toggleClass("active");
        return false;
    });
});