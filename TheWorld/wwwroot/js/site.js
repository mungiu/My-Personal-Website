﻿// site.js
// hiding the code from the "global scope" inside of "(function () )""
(function () {
    
    ////$
    //var ele = $("#username");
    //ele.text("[PN] ProfessionNoob");
    ////$
    ////now working - color not changing on mouseover
    //var main = $("#main");
    //main.on("mouseenter", function () {
    //    main.style = "background-color: #888;"
    //});
    //main.on("mouseleave", function () {
    //    main.style = "";
    //});
    ////$
    //var menuItems = $("ul.menu li a");
    //menuItems.on("click", function () {
    //    var me = $(this);
    //    alert(me.text());
    //});

    var $sidebarAndWrapper = $("#sidebar,#wrapper");
    var $icon = $("#sidebarToggle i.fa");

    $("#sidebarToggle") .on("click", function () {
        $sidebarAndWrapper.toggleClass("hide-sidebar");
        if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
            $icon.removeClass("fa-angle-left");
            $icon.addClass("fa-angle-right");
        } else {
            $icon.addClass("fa-angle-left");
            $icon.removeClass("fa-angle-right");

        }
    });
})();