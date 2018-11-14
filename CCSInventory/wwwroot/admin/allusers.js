/*
    Script description:
    This script provides the client-side functionality for displaying, filtering, 
    and sorting the list of users.
*/

// JavaScript Variable defined in view: var listOfUsers;

"use strict";

// This function updates the table body of #userList with the array of users provided.
function updateUserList(userList){
    var tbody = $("#userList");

    // Update the counts under the table:
    var userCount = "";
    if(userList.length < 1){
        userCount = "No users match this filter";
    } else if(userList.length == 1){
        userCount = "Showing 1 user";
    } else {
        userCount = `Showing ${userList.length} users`
    }
    $("#userCounts").text(userCount);

    // Populate the table, one user per row.
    var content = ""; // All the HTML will be concatenated here before insertion all at once.
    $.each(userList, function(i, u){
        // Some of the values have null, so replace them with empty strings
        const empty = '<span class="faded">Empty</span>';
        var row = `
            <tr>
                <td><a href="/admin/edituser/${u.UserID}" class="btn btn-danger">Edit</a></td>
                <td>${u.Username ? u.Username : empty}</td>
                <td>${u.FullNameLastFirst ? u.FullNameLastFirst : empty}</td>
                <td>${u.Email ? u.Email : empty}</td>
                <td>${u.UserRole ? u.UserRole : empty}</td>
                <td>${u.UserNote ? _.escape(u.UserNote) : empty}</td>
                <td>${u.ModifiedDateVisible ? u.ModifiedDateVisible : empty}</td>
            </tr>`;
        content += row;
    });
    tbody.html(content);
}

// Sort `list` of users by `key` and update the tbody#userList
function sortBy(key, tableHeader, list){

    // Toggle between ascending and descending sort
    let ascOrDesc = 0;
    if(tableHeader.dataset.sort == "asc"){
        // descending
        ascOrDesc = -1;
    } else {
        // ascending
        ascOrDesc = 1;
    }

    // For previous sort, remove the caret.
    $(".sortLink").each(function(i){
        this.dataset.sort = "";
        this.classList.remove("sortAsc", "sortDesc");
    });

    // For the next sort, set the class.
    tableHeader.dataset.sort = ascOrDesc == 1 ? "asc" : "desc";
    tableHeader.classList.add(ascOrDesc == 1 ? "sortAsc" : "sortDesc");

    // String sort
    list.sort(function(a, b){
        return ("" + a[key]).localeCompare("" + b[key]) * ascOrDesc;
    });

    updateUserList(list);
}

// function filterBy(key, text){
//     console.log(`Filtering for ${key}: ${text}`)
//     if(("" + text).length === 0){
//         filteredUsers = listOfUsers;
//     } else {
//         filteredUsers = listOfUsers.filter(function(u){ return u[key].indexOf(text.toLowerCase()) !== -1});
//     }
// }

// I think this pattern is called "Currying"?
function sortHandler(key){
    return function(event){
        event.preventDefault();
        event.stopPropagation();
        sortBy(key, event.target, filteredUsers);
        return false;
    };
}

// function filterHandler(key){
//     return _.throttle(function(event){
//         filterBy(key, event.target.value);
//     }, 250, { leading: true, trailing: true });
// }

// This function runs when the DOM is ready.  
$(document).ready(function(){
    // Set up event handlers for sorting by table header.
    $("#sortUsername").click(sortHandler("Username"));
    $("#sortName").click(sortHandler("FullNameLastFirst"));
    $("#sortEmail").click(sortHandler("Email"));
    $("#sortUserRole").click(sortHandler("UserRole"));
    $("#sortUserNote").click(sortHandler("UserNote"));
    $("#sortModifiedDate").click(sortHandler("ModifiedDateInt"));
    
    // // Event handlers for filtering
    // $("#filterUsername").on("input", filterHandler("Username"));

    // Display Users for the first time:
    if(!listOfUsers || listOfUsers.Length < 1){
        $("#noUsersMsg").removeClass("hidden");
    } else {
        sortBy("Username", $("#sortUsername")[0], [...listOfUsers]);
        window.filteredUsers = listOfUsers;
    }
});
