/*
    Script description:
    This script provides the client-side functionality for the sidebar nav collapse.
*/

$(document).ready(showActiveSidemenu);

function showActiveSidemenu() {
    // get current url
    var pathname = window.location.pathname;
    var sidemenuItem = $('#sidebar .sidebar-submenu a[href="' + pathname + '"]');

    if (sidemenuItem.length > 0) {
        sidemenuItem.parent().addClass('show');
        sidemenuItem.addClass('active');
    }
}

$(window).on("load resize", responsiveViewUpdate);

function responsiveViewUpdate() {
    var viewportWidth = $(window).width();

    // hide sidebar on smaller than 769px
    if (viewportWidth < 769) {
        $('#sidebar').addClass('inactive');
        $('#content').addClass('active');
        $('#sidebarCollapse').removeClass('d-none'); // show toggle button
    }
    else {
        $('#sidebar').removeClass('inactive');
        $('#content').removeClass('active');  
        $('#sidebarCollapse').addClass('d-none'); // hide toggle button
    }
}

$('#sidebarCollapse').on('click', function () {
    $('#sidebar').toggleClass('inactive'); // toggle sidebar
    $('#content').toggleClass('active'); // toggle full content
    $('.collapse.show').toggleClass('show'); // collapse submenus
    $('a[aria-expanded=true]').attr('aria-expanded', 'false'); // submenus closed
});
