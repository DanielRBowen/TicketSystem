var PayPalFinalAmount = 0;  //used to globally get the amount of attendees

// Populates the quantity select drop-downs with options "" - 10
$(function () {
    var maxTicketsPerCustomer = 10;
    var option = "<option value=\"{#}\">{#}</option>";

    for (var i = 0; i <= maxTicketsPerCustomer; i++) {
        $("select.TicketQuantity").append(option.replace(/\{#\}/g, i));
    }

    $("select.TicketQuantity").change();
});

function calculatePrice() {
    var attendeeCount = $("option:selected", this).val();
    var FinalCount = (PayPalFinalAmount + ".00").toString();
    return FinalCount;
}

// Update the table and the global values when the user changes ticket quantities:
$(".TicketQuantity").change(function () {
    calculateRowTotal(this);
});

function calculateRowTotal(el) {
    var tr = $(el).closest("tr");
    var rowTotalSpan = $(tr).find("span.RowTotal");
    var rowTotalInput = $(tr).find("input.RowTotal");
    var price = $(tr).find(".TicketPrice").val().substr(1); // Trim the currency symbol.
    var quantity = $(tr).find(".TicketQuantity option:selected").val();
    var totalPrice = parseFloat(price) * parseFloat(quantity);

    if (isNaN(totalPrice)) {
        rowTotalSpan.html("0");
        rowTotalInput.val(0);
    }
    else {
        rowTotalSpan.html(totalPrice);
        rowTotalInput.val(totalPrice);
    }

    calculateTableTotal(el);
}

function calculateTableTotal(el) {
    var table = $(el).closest("table");
    var total = 0;
    var val = 0;

    // Total Price
    $.each(table.find("input.RowTotal"), function (index, input) {
        val = $(input).val();
        total += (val == "" ? 0 : parseFloat(val));
    });

    table.find("span.ColumnTotal.TicketPrice").html(total);
    table.find("input.ColumnTotal.TicketPrice").val(total);
    PayPalFinalAmount = total;  //used to globally set the total price

    // Total Quantity of Tickets
    total = 0;
    $.each(table.find("select.TicketQuantity"), function (index, select) {
        val = $("option:selected", select).val();

        total += (val == "" ? 0 : parseFloat(val));
    });
    table.find("span.ColumnTotal.TicketQuantity").html(total);

    var hidePaymentDiv = isNaN(total) || parseFloat(total) < 1;
    $("#ready-tickets").toggleClass("hidden", hidePaymentDiv);
    $("#payment-div").toggleClass("hidden", hidePaymentDiv);
}


// Uses AJAX to query the UserAccountId from the DB, filtering by the customer's email.
function getUserAccount(el, ev) {
    // Prevent the default action from happening (such as if the browser thinks it should submit the form).
    ev.preventDefault();
    // Compile the form data in the first form into a POST string
    var postData = $("#user-form").serialize();

    // Submit the first form's data using AJAX to the UserAccount Controller's customized action to create a user account
    $.ajax({
        url: "/UserAccounts/CheckUserAccount", // Controller/Function: this function checks the user account and creates one if it doesn't exist. It returns a JSON object with the ID.
        data: postData, // The user info form data
        type: "POST", // The type of request to send: POST
        dataType: "text", // They type of response to expect: JSON
        success: function (data) {
            //alert("Success!"); // For Debugging (remove once it works)
            data = JSON.parse(data);
            // alert(data.UserAccountId);

            // Assign the response value to the hidden UserAccountId field in the second form
            // (relies on the response being a JSON object with a UserAcountID attribute)
            $("#userAccountId").val(data.UserAccountId);

            // Submit the second form (to add tickets to the DB)
            // The controller function/action (asp-action) that this submits to is in the form element.
            // That function will need to be developed in the controller.
            // The function/action should then call the confirmation action (Stephen in working on that)
            $("#ticket-form").submit();
            // url = "/Tickets/PurchaseConfirmation/" + data.UserAccountId;
            // window.location.href = url;
        },
        failure: function () {
            alert("Failed"); // For debugging (hopefully we won't need to see this)
        }
    });
}