/*
    Script description:
    This script provides the client-side functionality for the sidebar nav collapse.
*/


// Hide submenus
$('#body-row .collapse').collapse('hide');

// collapse/expand icon
$('#collapse-icon').addClass('fa-angle-double-left');

// collapse click
$('[data-toggle=sidebar-collapse]').click(function () {
    SidebarCollapse();
});

// handles collapsing/expanding the sidebar
function SidebarCollapse() {
    $('.menu-collapsed').toggleClass('d-none');
    $('.sidebar-submenu').toggleClass('d-none');
    $('.submenu-icon').toggleClass('d-none');
    $('#sidebar-container').toggleClass('sidebar-expanded sidebar-collapsed');

    // Treating d-flex/d-none on separators with title
    var SeparatorTitle = $('.sidebar-separator-title');
    if (SeparatorTitle.hasClass('d-flex')) {
        SeparatorTitle.removeClass('d-flex');
    } else {
        SeparatorTitle.addClass('d-flex');
    }

    // Collapse/Expand icon
    $('#collapse-icon').toggleClass('fa-angle-double-left fa-angle-double-right');
}
