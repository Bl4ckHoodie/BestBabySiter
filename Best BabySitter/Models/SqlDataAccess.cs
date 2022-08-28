using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Best_BabySitter.Models
{
    public class SqlDataAccess
    {

        public static string getConnectionString(string connectionName = "defaultDB")
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }
        
       public static int  createAdvert(Advert newAdvert, int parentID)
        {
            try
            {
                string sql;
                if(newAdvert.Specification == null)
                    sql = "Insert INTO Advert (parent_ID, num_kids, specification, location, start_date, end_date, start_time, end_time, date_created) VALUES ("+parentID+", "+newAdvert.NumKids+", 'None', '"+newAdvert.Street+", "+newAdvert.City+"', '"+newAdvert.StartDate+"', '"+newAdvert.EndDate+"', '"+newAdvert.StartTime+"', '"+newAdvert.EndTime+"','"+newAdvert.DateCreated+"')";
                else
                    sql = "Insert INTO Advert (parent_ID, num_kids, specification, location, start_date, end_date, start_time, end_time, date_created) VALUES (" + parentID + ", " + newAdvert.NumKids + ", '"+newAdvert.Specification+"', '" + newAdvert.Street + ", " + newAdvert.City + "', '" + newAdvert.StartDate + "', '" + newAdvert.EndDate + "', '" + newAdvert.StartTime + "', '" + newAdvert.EndTime + "','" + newAdvert.DateCreated + "')";
                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
               
                com.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            
                return -1;
            }
            
        }
       
       public static int registerParent(Parent parent)
        {
            try
            {

                string sql;
                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                sql = "SELECT parent_ID FROM Parent WHERE email='"+parent.email+"'";
                OleDbCommand check = new OleDbCommand(sql, conn);
                OleDbDataReader data = check.ExecuteReader();
                if (!data.HasRows)
                {
                    sql = "INSERT INTO Parent (f_name, l_name, street, city, email, [password]) VALUES ('"+ parent.f_name +"', '" + parent.l_name + "', '" + parent.street + "', '" + parent.city + "', '" + parent.email + "', '" + parent.password + "')";
                    
                    OleDbCommand com = new OleDbCommand(sql, conn);

                    com.ExecuteNonQuery();
                    conn.Close();
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return -1;
        }


       public static int getParentID(Parent parent){
        try
            {

                string sql;
                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                sql = "SELECT parent_ID FROM Parent WHERE email='"+parent.email+"' AND password='"+parent.password+"';";
                OleDbCommand check = new OleDbCommand(sql, conn);
                OleDbDataReader data = check.ExecuteReader();
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        return int.Parse(data["parentID"]);
                    }
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return -1;
       }


       public static List<Advert> getOpenAdverts(int parentID)
        {
            List<Advert> adverts = new List<Advert>();
            try
            {
                string sql = "SELECT * FROM Advert WHERE parent_ID ="+parentID+" AND closed=False";
    
                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                OleDbDataReader data = com.ExecuteReader();
                while (data.Read())
                {
                    String[] location = data["location"].ToString().Split(',');
                    adverts.Add(new Advert
                    {
                        ID = int.Parse(data["Advert_ID"].ToString()),
                        NumKids = int.Parse(data["num_kids"].ToString()),
                        Specification = data["specification"].ToString(),
                        Street = location[0],
                        City = location[1],
                        DateCreated = DateTime.Parse(data["date_created"].ToString()),
                        StartTime = DateTime.Parse(data["start_time"].ToString()),
                        EndTime = DateTime.Parse(data["end_time"].ToString()),
                        StartDate = DateTime.Parse(data["start_date"].ToString()),
                        EndDate = DateTime.Parse(data["end_date"].ToString()),
                        AgeRange =""
                    }); 
                }
                conn.Close();
                return adverts;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return null;
            }
            
        }
    
    }
}