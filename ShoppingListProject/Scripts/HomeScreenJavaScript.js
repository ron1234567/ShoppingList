$(document).ready(function () {

    $("#identificationForm").validate({

        // Specify validation rules
        rules: {
            // The key name on the left side is the name attribute of an input field. Validation rules are defined
            // on the right side
            email: {
                required: true,
            }
        },
        // Specify validation error messages
        messages: {
            email: {
                required: "You have to enter your email"
            },
        },
        //When the user insert valid email address
        submitHandler: function () {

            var email = $("#email").val();
            $.ajax({
                type: "POST",
                url: "HomeScreen.aspx/CreateUser",
                data: '{email : "' + email + '"}',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function () {
                    //When the user's creation success we save his email in cookie and move to shopping list screen
                    document.cookie = "email=" + email + ";path=/";
                    window.location.href = "ShoppingListScreen.aspx"
                },
                error: function () {
                    alert("Error while setting your email");
                }
            });
            return false;
        }
    });
})