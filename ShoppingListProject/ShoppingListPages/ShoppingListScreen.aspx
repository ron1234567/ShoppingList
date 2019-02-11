<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShoppingListScreen.aspx.cs" Inherits="ShoppingListProject.ShoppingListPages.ShoppingListScreen" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My Shopping List</title>

    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/css/bootstrap.min.css" integrity="sha384-GJzZqFGwb1QTTN6wy59ffF1BuGJpLSa9DkKMp0DgiMDm4iYMj70gZWKYbI706tWS" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.1/css/all.css" integrity="sha384-fnmOCqbTlWIlj8LyTjo7mOUStjsKC4pOpQbqyi7RrhN7udi9RwhKkMHpvLbHG9Sr" crossorigin="anonymous"/>
    <link href="../Content/StyleSheet.css" rel="stylesheet" />
</head>
<body>

    <div class="container">
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <h1>My Shopping List</h1>

                 <%-- Form to get item's name from user --%>
                <form id="insertForm" method="post">
                    <br />
                    <input id="newItem" name="newItem" type="text" placeholder="Insert item to your list" class="form-control"  />
                    <br />
                    <label for="newItem" class="error"></label>
                    <br />
                    <input id="insertButton" name="insertButton" class="btn btn-primary" type="submit" value="Insert" />
                </form>
                <br />

                <%-- Table for items that still were not purchased--%>
                <table border="1" id="itemsWereNotPurchased" class="table table-striped table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="itemsWereNotPurchased_info">
                    <thead>
                        <tr role="row">
                            <th class="sorting_asc" tabindex="0" aria-controls="itemsWereNotPurchased_info" rowspan="1" colspan="3" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">The items you need to purchase :</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>

                <%-- Table for items that were purchased already--%>
                <table border="1" id="itemsWerePurchased" class="table table-striped table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="itemsWerePurchased_info">
                    <thead>
                        <tr role="row">
                            <th class="sorting_asc" tabindex="0" aria-controls="itemsWerePurchased_info" rowspan="1" colspan="3" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">The items you already purchased :</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
        </div>
    </div>

    <%-- Modal for update item's name --%>
    <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" id="itemModal">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h2 class="modal-title"></h2>
                </div>
                <form id="updateItemForm" role="form">
                    <div class="modal-body">
                        <div class="panel-body">
                            <p>
                                <label for="itemName">Please change the item's name here:</label>
                                <input type="text" name="itemName" id="itemName" class="form-control" />
                            </p>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="submit" id="updateButton" class="btn btn-primary" update-id="">Update</button>
                    </div>
                </form>
            </div>
        </div>
    </div>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.6/umd/popper.min.js" integrity="sha384-wHAiFfRlMFy6i5SRaxvfOCifBUQy1xHdJ/yoi7FRNXMRBu5WHdZYu1hA6ZOblgut" crossorigin="anonymous"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/js/bootstrap.min.js" integrity="sha384-B0UglyR+jN6CkvvICOB2joaf5I4l3gm9GU6Hc1og6Ls7i6U/mkkaduKaBhlAXv9k" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.19.0/jquery.validate.min.js"></script> 
    <script src="../Scripts/ShoppingListScreenJavaScript.js"></script>
</body>
</html>
