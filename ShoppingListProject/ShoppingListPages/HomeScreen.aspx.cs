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
    public partial class HomeScreen : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //Create user in database if he is not already exist
        [WebMethod]
        public static void CreateUser(string email)
        {
            if (email != "")
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    const string query = "SELECT COUNT(*) FROM [User] WHERE ([Email]= @email)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@email", email);
                        int userExist = (int)command.ExecuteScalar();

                        //If user doesn't exist in database create him
                        if (userExist == 0)
                        {
                            const string query2 = "INSERT INTO [User] ([Email]) VALUES(@email)";
                            using (var command2 = new SqlCommand(query2, connection))
                            {
                                command2.CommandType = CommandType.Text;
                                command2.Parameters.AddWithValue("@email", email);
                                command2.ExecuteNonQuery();

                                command2.Dispose();
                            }
                        }

                        command.Dispose();
                        connection.Dispose();
                        connection.Close();
                    }
                }
            }
        }
    }
}