using ShoppingListProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShoppingListProject.ShoppingListPages
{
    public partial class ShoppingListScreen : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private static string email;
        private static DataTable TableData = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpCookie cookie = Request.Cookies["email"];
                if (cookie != null)
                {
                    //Get email from cookie and set expires time to 30 minute from current time
                    email = cookie.Value;
                    cookie.Expires = DateTime.Now.AddMinutes(30);
                    Response.AppendCookie(cookie);
                }
                //If email not exist in the cookie redirect to home screen
                else Response.Redirect("HomeScreen.aspx");
            }
        }

        //Get all items that belongs to current user from database 
        [WebMethod]
        public static Item[] GetAllItems()
        {
            var allItems = new List<Item>();
            using (var connection = new SqlConnection(connectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                const string query = "SELECT Id,ItemName,WasPurchased FROM [UserList] WHERE ([Email]= @email)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@email", email);
                    command.CommandType = CommandType.Text;
                    using (var sda = new SqlDataAdapter())
                    {
                        sda.SelectCommand = command;
                        TableData.Clear();
                        sda.Fill(TableData);
                        allItems.AddRange(from DataRow dtrow in TableData.Rows
                                          select new Item
                                          {
                                              Id = Convert.ToInt32(dtrow["Id"]),
                                              ItemName = dtrow["ItemName"].ToString(),
                                              WasPurchased = Convert.ToBoolean(dtrow["WasPurchased"])
                                          });
                    }

                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                }
            }
            return allItems.ToArray();
        }

        //Create item in database if it is not already exist(if it's exist return false else return true)
        [WebMethod]
        public static bool SaveItem(string itemName)
        {
            bool flag = false;
            if (itemName != "")
            {
                using (var connection = new SqlConnection(connectionString))
                {                 
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    const string query = "SELECT COUNT(*) FROM [UserList] WHERE ([Email]= @email AND [ItemName]=@ItemName)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@itemName", itemName);
                        int itemExist = (int)command.ExecuteScalar();

                        //If item doesn't exist in database create it
                        if (itemExist == 0)
                        {
                            const string query2 = "INSERT INTO[UserList] ([Email],[ItemName],[WasPurchased]) VALUES(@email, @itemName, 0)";
                            using (var command2 = new SqlCommand(query2, connection))
                            {
                                command2.CommandType = CommandType.Text;
                                command2.Parameters.AddWithValue("@email", email);
                                command2.Parameters.AddWithValue("@itemName", itemName);
                                command2.ExecuteNonQuery();

                                command2.Dispose();
                                flag = true;
                            }
                        }

                        command.Dispose();
                        connection.Dispose();
                        connection.Close();
                        return flag;
                    }
                }
            }
            else return flag;
        }

        //Delete item from database by id 
        [WebMethod]
        public static void DeleteItem(int itemId)
        {
            if (itemId > -1)
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    const string query = "Delete from [UserList] where ([Id]=@itemId)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@itemId", itemId);
                        command.ExecuteNonQuery();

                        command.Dispose();
                        connection.Dispose();
                        connection.Close();
                    }
                }
            }
        }

        //Update item's name in database by id if it is not already exist(if it's exist return false else return true)
        [WebMethod]
        public static bool UpdateItemName(int itemId, string itemName)
        {          
            bool flag = false;
            if (itemId > -1 && itemName != "")
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    const string query = "SELECT COUNT(*) FROM [UserList] WHERE ([Email]= @email AND [ItemName]=@ItemName)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@itemName", itemName);
                        int itemExist = (int)command.ExecuteScalar();

                        //Item doesn't exist.
                        if (itemExist == 0)
                        {
                            const string query2 = "UPDATE [UserList] SET [ItemName] = @itemName WHERE [Id] = @itemId";
                            using (var command2 = new SqlCommand(query2, connection))
                            {
                                command2.CommandType = CommandType.Text;
                                command2.Parameters.AddWithValue("@itemName", itemName);
                                command2.Parameters.AddWithValue("@itemId", itemId);
                                command2.ExecuteNonQuery();

                                command2.Dispose();
                                flag = true;
                            }
                        }

                        command.Dispose();
                        connection.Dispose();
                        connection.Close();
                        return flag;
                    }
                }
            }
            else return flag;
        }

        //Update item's status in database by id
        [WebMethod]
        public static void UpdateItemStatus(int itemId, int wasPurchased)
        {
            if (itemId > -1 && (wasPurchased == 1 || wasPurchased == 0))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    const string query = "UPDATE [UserList] SET [WasPurchased]=@WasPurchased WHERE [Id]=@itemId";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@WasPurchased", wasPurchased);
                        command.Parameters.AddWithValue("@itemId", itemId);
                        command.ExecuteNonQuery();

                        command.Dispose();
                        connection.Dispose();
                        connection.Close();
                    }
                }
            }
        }
    }
}