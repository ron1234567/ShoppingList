function GetAllItems() {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ShoppingListScreen.aspx/GetAllItems",
        data: {},
        dataType: "json",
        success: function (data) {
            //Build item's tables
            $('#itemsWerePurchased tbody tr').remove();
            $('#itemsWereNotPurchased tbody tr').remove();
            for (var i = 0; i < data.d.length; i++) {
                if (data.d[i].WasPurchased) {
                    $("#itemsWerePurchased").append(
                        "<tr><td><input type='checkbox' class='WerePurchasedCheckBox' checked='checked' name='itemCheckBox'"
                        + " item-id='" + data.d[i].Id + "'/></td><td class='WerePurchasedTD'>" + data.d[i].ItemName
                        + "</td><td><i class='fas fa-edit editButton' data-toggle='modal'" + " data-target='#itemModal' item-name='"
                        + data.d[i].ItemName + "' item-id='" + data.d[i].Id + "'></i> <i class='fas fa-trash-alt deleteButton'"
                        + "item-id='" + data.d[i].Id + "'></i></td></tr>");
                }
                else {
                    $("#itemsWereNotPurchased").append(
                        "<tr><td><input type='checkbox' class='WereNotPurchasedCheckBox' name='itemCheckBox'"
                        + " item-id='" + data.d[i].Id + "'/></td><td class='WereNotPurchasedTD'>" + data.d[i].ItemName +
                        "</td><td><i class='fas fa-edit editButton' data-toggle='modal' data-target='#itemModal' item-name='"
                        + data.d[i].ItemName + "' item-id='" + data.d[i].Id + "'></i> <i class='fas fa-trash-alt deleteButton'"
                        + "item-id='" + data.d[i].Id + "'></i></td></tr>");
                }
            }
        },
        error: function () {
            alert("Error while Showing data");
        }
    });
}
GetAllItems();
var itemName, itemId;

//When the user click on edit icon, save item's id and load item's name to modal
$(document).on("click", ".editButton", function () {
    $('#itemModal').focus();

    itemId = $(this).attr("item-id");
    itemName = $(this).attr("item-name");

    $("#updateButton").attr("update-id", itemId);
    $("#itemName").val(itemName);

    $("label.error").hide();
    $(".error").removeClass("error");
});

//When the user click on delete icon, delete this item by id and load again items to tables
$(document).on("click", ".deleteButton", function () {

    if (confirm("Are you sure to delete this task ?") == true) {

        itemId = $(this).attr("item-id");
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ShoppingListScreen.aspx/DeleteItem",
            data: '{itemId: ' + itemId + '}',
            dataType: "json",
            success: function () {
                alert("Task Deleted successfully");
                GetAllItems();
            },
            error: function () {
                alert("Error while deletion data of :" + itemId);
            }
        });
    }
    else alert("You have canceled the deletion");
});

//When the user click on item's checkbox in were purchased list,we changed its status to were not purchased by id 
//and save it in database.in the end load again items to tables
$(document).on("click", ".WerePurchasedCheckBox", function () {

    itemId = $(this).attr("item-id");
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ShoppingListScreen.aspx/UpdateItemStatus",
        data: '{itemId: ' + itemId + ',wasPurchased:0}',
        dataType: "json",
        success: function () {
            GetAllItems();
        },
        error: function () {
            alert("Error while changing data of :" + itemId);
        }
    });
});

//When the user click on item's checkbox in were not purchased list,we changed its status to were purchased by id 
//and save it in database.in the end load again items to tables
$(document).on("click", ".WereNotPurchasedCheckBox", function () {

    itemId = $(this).attr("item-id");

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ShoppingListScreen.aspx/UpdateItemStatus",
        data: '{itemId: ' + itemId + ',wasPurchased:1}',
        dataType: "json",
        success: function () {
            GetAllItems();
        },
        error: function () {
            alert("Error while changing data of :" + itemId);
        }
    });
});

$(document).ready(function () {

    $("#insertForm").validate({
        // Specify validation rules
        rules: {
            // The key name on the left side is the name attribute of an input field. Validation rules are defined
            // on the right side
            newItem: {
                required: true,
            }
        },
        // Specify validation error messages
        messages: {
            newItem: {
                required: "You have to enter the item's name"
            },
        },
        //When the user insert valid item's name, save this item in database if it not exist already
        submitHandler: function () {

            itemName = $("#newItem").val();
            $.ajax({
                type: "POST",
                url: "ShoppingListScreen.aspx/SaveItem",
                data: '{itemName : "' + itemName + '"}',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.d) {
                        alert("Item has been added successfully");
                        GetAllItems();
                    }
                    else alert("The item you try to insert is already exist in your list!")
                    $("#newItem").val("");
                },
                error: function () {
                    alert("Error while inserting data");
                }
            });
            return false;
        }
    });

    $("#updateItemForm").validate({
        // Specify validation rules
        rules: {
            // The key name on the left side is the name attribute of an input field. Validation rules are defined
            // on the right side
            itemName: {
                required: true,
            }
        },
        // Specify validation error messages
        messages: {
            itemName: {
                required: "You have to enter the item's name"
            },
        },
        //When the user insert valid item's name, update this item in database if it not exist already
        submitHandler: function () {

            //If the user did not change item's name
            if (itemName == $("#itemName").val()) {
                alert("The item's name will not change");
                location.reload();
            }
            else {
                if (confirm("Are you sure to update this task ?") == true) {

                    itemId = $("#updateButton").attr("update-id");
                    itemName = $("#itemName").val();
                    $.ajax({
                        type: "POST",
                        url: "ShoppingListScreen.aspx/UpdateItemName",
                        data: '{itemId: ' + itemId + ', itemName : "' + itemName + '"}',
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d) {
                                alert("Item has been Updated successfully");
                            }
                            else alert("This item's name is already exist in your list!");
                            location.reload();
                        },
                        error: function () {
                            alert("Error while Updating data of :" + itemId);
                        }
                    });
                }
                else alert("You have canceled the changes");
            }
            return false;
        }
    });
})