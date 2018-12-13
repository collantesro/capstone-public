"use strict";

/* Bin Number */
// Flips the disabled property on the BinNumber textbox according to the argument
function binBox(isDisabled) {
    $("#binNumberInput").prop("disabled", isDisabled);
}

// Event listener on the AutoGenerate checkbox when it's clicked:
// Enables the BinNumber box when this is unchecked and vice versa.
$("#autogenCheckbox").change(function (event) {
    binBox(this.checked);
});

// Initial state on page load:
if ($("#binNumberInput").val().length > 0) {
    // If there's text in binNumber (say from a bad model reloading the page), 
    // keep the box unchecked.
    $("#autogenCheckbox").prop("checked", false);
    binBox(false);
} else {
    $("#autogenCheckbox").prop("checked", true);
    binBox(true);
}

// To easily find a category by its CategoryID,
// or to find a Subcategory by its ID and then its parent:
var categoryMap = new Map();
var subcategoryMap = new Map();

categories.forEach(function (c) {
    categoryMap.set(c.CategoryID, c);
    c.Subcategories.forEach(function (s) {
        subcategoryMap.set(s.SubcategoryID, {
            SubcategoryID: s.SubcategoryID,
            SubcategoryName: s.SubcategoryName,
            Category: {
                CategoryID: c.CategoryID,
                CategoryName: c.CategoryName
            }
        });
    });
});

// This function loads the Subcategories for the selected Category.
function updateSubcategories() {
    var current = parseInt($("#categorySelect").val());
    console.log("Current value for select: ", current);
    var category = categoryMap.get(current);
    if (category) {
        console.log("Subcategories:", category.Subcategories);
        var defaultSelected = false;
        var options = [];
        $.each(category.Subcategories, function (i, s) {
            var newOption = document.createElement("option");
            newOption.value = s.SubcategoryID;
            newOption.text = _.escape(s.SubcategoryName);
            if (_.includes(s.SubcategoryName, "Unsorted") && !defaultSelected) {
                defaultSelected = true;
                newOption.defaultSelected = true;
                options.unshift(newOption);
            } else {
                options.push(newOption);
            }
        });
        $("#subcategorySelect").empty().append(options);
    } else {
        // Invalid category
        $("#subcategorySelect").empty();
    }
    subcategoryChanged();
}

function subcategoryChanged() {
    // This function updates the hidden input with id #subcategoryHidden with the proper value.
    // var categoryValue = $("#categorySelect").val();
    var subcategoryValue = $("#subcategorySelect").val();

    if (window.subcategoryAfterFirstRun != true) {
        window.subcategoryAfterFirstRun = true;
        var hiddenValue = $("#subcategoryHidden").val();
        var parsedValue = parseInt(hiddenValue);
        if (hiddenValue.length > 0 && parsedValue > 0) {
            // find the subcategory/category combo it fits with and set them.
            var subcategory = subcategoryMap.get(parsedValue);
            if (!subcategory) {
                console.log("Invalid subcategory: ", parsedValue);
            } else {
                var categorySelect = $("#categorySelect")[0];
                if (categorySelect) {
                    for (var i = 0; i < categorySelect.options.length; i++) {
                        if (categorySelect.options[i].value == subcategory.Category.CategoryID) {
                            categorySelect.selectedIndex = i;
                            updateSubcategories(); // this overwrites the hidden value, but we still have the subcategory value here.
                            var subcategorySelect = $("#subcategorySelect")[0];
                            for (var j = 0; j < subcategorySelect.options.length; j++) {
                                if (subcategorySelect.options[j].value == subcategory.SubcategoryID) {
                                    subcategorySelect.selectedIndex = j;
                                    subcategoryChanged();
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            console.log(`End of first run of subcategoryChanged() with value in hidden input.  parsedValue=${parsedValue}, current value of hidden input=${$("#subcategoryHidden").val()}`);
        } else {
            $("#subcategoryHidden").val(subcategoryValue);
            console.log(`End of first run of subcategoryChanged() WITHOUT value in hidden input. hidden input=${$("#subcategoryHidden").val()}`);
        }
    } else {
        // Setting value of hidden select
        $("#subcategoryHidden").val(subcategoryValue);
    }
}

$("#categorySelect").change(function (event) {
    if (_.includes(this.options[this.selectedIndex].text, "Pantry Pack")) {
        $("#unitsLabel").text("Quantity");
    } else {
        $("#unitsLabel").text("Cases");
    }
    updateSubcategories();
});

$("#subcategorySelect").change(subcategoryChanged);

function insertCategories() {
    var options = [];
    $.each(categories, function (i, c) {
        var newOption = document.createElement("option");
        newOption.value = c.CategoryID;
        newOption.text = _.escape(c.CategoryName);
        options.push(newOption);
    });
    $("#categorySelect").empty().append(options);
    updateSubcategories();
}

insertCategories();
