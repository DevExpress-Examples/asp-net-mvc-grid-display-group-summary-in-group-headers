using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace Sample.Models {
    public class ConnectionRepository {
        public static OleDbConnection GetDataConnection() {
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}", HttpContext.Current.Server.MapPath("~/App_Data/Northwind.mdb"));
            return connection;
        }
    }
    public class NorthwindDataModel {
        public static DataTable GetProducts() {
            DataTable dataTableProducts = new DataTable();
            using (OleDbConnection connection = ConnectionRepository.GetDataConnection()) {
                OleDbDataAdapter adapter = new OleDbDataAdapter(string.Empty, connection);
                adapter.SelectCommand.CommandText = "SELECT * FROM [Products]";
                adapter.Fill(dataTableProducts);
            }
            return dataTableProducts;
        }

        public static DataTable GetCategories() {
            DataTable dataTableCategories = new DataTable();
            using (OleDbConnection connection = ConnectionRepository.GetDataConnection()) {
                OleDbDataAdapter adapter = new OleDbDataAdapter(string.Empty, connection);
                adapter.SelectCommand.CommandText = "SELECT * FROM [Categories]";
                adapter.Fill(dataTableCategories);
            }
            return dataTableCategories;
        }
    }
}