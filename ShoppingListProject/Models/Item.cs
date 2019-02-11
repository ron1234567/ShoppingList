using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingListProject.Models
{
    //Class that represent Item
    public class Item
    {
        public int Id { get; set; }

        public string ItemName { get; set; }

        public bool WasPurchased { get; set; }
    }
}