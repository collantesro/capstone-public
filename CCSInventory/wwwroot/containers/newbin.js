"use strict";

function binBox(isDisabled){
    $("#binNumberInput").prop("disabled", isDisabled);
}

$("#autogenCheckbox").change(function(event){
    event.preventDefault();
    binBox(this.checked);
});

binBox($("#autogenCheckbox").prop("checked", true));
