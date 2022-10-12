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

        public static int createAdvert(Advert newAdvert, int parentID)
        {
            try
            {
                string sql;
                if (newAdvert.Specification == null)
                    sql = "Insert INTO Advert (parent_ID, num_kids, specification, location, start_date, end_date, start_time, end_time, date_created) VALUES (" + parentID + ", " + newAdvert.NumKids + ", 'None', '" + newAdvert.Street + "," + newAdvert.City + "', '" + newAdvert.StartDate + "', '" + newAdvert.EndDate + "', '" + newAdvert.StartTime + "', '" + newAdvert.EndTime + "','" + newAdvert.DateCreated + "')";
                else
                    sql = "Insert INTO Advert (parent_ID, num_kids, specification, location, start_date, end_date, start_time, end_time, date_created) VALUES (" + parentID + ", " + newAdvert.NumKids + ", '" + newAdvert.Specification + "', '" + newAdvert.Street + "," + newAdvert.City + "', '" + newAdvert.StartDate + "', '" + newAdvert.EndDate + "', '" + newAdvert.StartTime + "', '" + newAdvert.EndTime + "','" + newAdvert.DateCreated + "')";
                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);

                com.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch (Exception ex)
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
                sql = "SELECT parent_ID FROM Parent WHERE email='" + parent.email + "'";
                OleDbCommand check = new OleDbCommand(sql, conn);
                OleDbDataReader data = check.ExecuteReader();
                if (!data.HasRows)
                {
                    sql = "INSERT INTO Parent (f_name, l_name, street, city, email, [password]) VALUES ('" + parent.f_name + "', '" + parent.l_name + "', '" + parent.street + "', '" + parent.city + "', '" + parent.email + "', '" + parent.password + "')";

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

        public static int updateAdvert(Advert advert){
            try
            {

                string sql;
                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                sql = "UPDATE Advert SET num_kids =" + advert.NumKids + ", specification = '" + advert.Specification + "', location='" + advert.Street + "," + advert.City + "', start_date='" + advert.StartDate + "', end_date='" + advert.EndDate + "', start_time='" + advert.StartTime + "', end_time='" + advert.EndTime + "' WHERE (Advert_ID=" + advert.ID + ") ";
                OleDbCommand update = new OleDbCommand(sql, conn);
                update.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return -1;
        }

        public static int getParentID(Parent parent) {
            try
            {

                string sql;
                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                sql = @"SELECT  parent_ID
                        FROM Parent
                        WHERE(email = '" + parent.email + "') AND([password] = '" + parent.password + "')'";
                OleDbCommand check = new OleDbCommand(sql, conn);
                OleDbDataReader data = check.ExecuteReader();
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        return int.Parse(data["parentID"].ToString());
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

        public static List<Sitter> getResponses(int advertID)
        {
            List<Sitter> sitters = new List<Sitter>();
            try
            {
                string sql = @"SELECT jobApplication.sitter_ID, Sitter.f_name, Sitter.L_name, Sitter.email, Sitter.AboutMe, Sitter.city, Sitter.profilePicPath, Sitter.contact_NO, Sitter.chargePerService, Sitter.street
                               FROM   ((jobApplication INNER JOIN
                               Sitter ON jobApplication.sitter_ID = Sitter.sitter_ID) INNER JOIN
                               Advert ON Advert.Advert_ID = jobApplication.advert_ID)
                               WHERE (jobApplication.advert_ID = "+advertID+") AND (Advert.closed = False)";

                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                OleDbDataReader data = com.ExecuteReader();
                while (data.Read())
                {
                    sitters.Add(new Sitter
                    {
                        sitter_ID = int.Parse(data["sitter_ID"].ToString()),
                        f_name = data["f_name"].ToString(),
                        L_name = data["L_name"].ToString(),
                        password = "not happening buddy :P",
                        email = data["email"].ToString(),
                        AboutMe = data["AboutMe"].ToString(),
                        city = data["city"].ToString(),
                        profilePicPath = data["profilePicPath"].ToString(),
                        contact_NO = data["contact_NO"].ToString(),
                        chargePerService =  int.Parse(data["chargePerService"].ToString()),
                        street = data["street"].ToString()
                    });
                }
                conn.Close();
                return sitters;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return null;
            }
        }
        public static List<Advert> getOpenAdverts(int parentID)
        {
            List<Advert> adverts = new List<Advert>();
            try
            {
                string sql = "SELECT * FROM Advert WHERE parent_ID =" + parentID + " AND closed=False";

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
                        AgeRange = ""
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

        public static List<Appointment> getAppointments(int userID, int role)
        {
            List<Appointment> appointments = new List<Appointment>();
            try
            {
                string sql;
                if (role == 1)
                    sql = @"SELECT Advert.Advert_ID, Advert.parent_ID, Advert.location, Advert.start_time, Advert.end_time, Advert.start_date, Advert.closed, jobApplication.sitter_ID, Sitter.f_name, Sitter.L_name
                            FROM((Advert INNER JOIN
                                jobApplication ON jobApplication.advert_ID = Advert.Advert_ID) INNER JOIN
                                Sitter ON jobApplication.sitter_ID = Sitter.sitter_ID)
                            WHERE(Advert.parent_ID =" + userID + ") AND(Advert.closed = True) AND(Advert.start_date >= #" + DateTime.Now + "#)";
                else
                    sql = @"SELECT  Advert.Advert_ID, Advert.parent_ID, Advert.location, Advert.start_time, Advert.end_time, Advert.start_date, Advert.end_date, jobApplication.sitter_ID, Parent.f_name, Parent.l_name
                            FROM((Advert INNER JOIN
                                jobApplication ON jobApplication.advert_ID = Advert.Advert_ID) INNER JOIN
                                 Parent ON Parent.parent_ID = Advert.parent_ID)
                            WHERE(Advert.closed = True) AND(Advert.start_date >= #" + DateTime.Now + "#) AND (jobApplication.sitter_ID = " + userID + ")";

                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                OleDbDataReader data = com.ExecuteReader();
                while (data.Read())
                {
                    String[] location = data["location"].ToString().Split(',');
                    appointments.Add(new Appointment
                    {
                        appointee = data["f_name"].ToString().Substring(0,1).ToUpper() + ". " + data["l_name"].ToString().ToUpper(),
                        Street = location[0],
                        City = location[1],
                        StartTime = DateTime.Parse(data["start_time"].ToString()),
                        EndTime = DateTime.Parse(data["end_time"].ToString()),
                        StartDate = DateTime.Parse(data["start_date"].ToString()),
                    });
                }
                conn.Close();
                return appointments;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return null;
            }

        }

        public static Parent getParentData(int id)
        {
            Parent parent = null;
            try
            {
                string sql = @" SELECT        parent_ID, f_name, l_name, street, city, email, [password]
                                FROM Parent
                                WHERE(parent_ID ="+id+")";

                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                OleDbDataReader data = com.ExecuteReader();
                while (data.Read())
                {

                    parent = new Parent
                    {
                        parent_ID = int.Parse(data["parent_ID"].ToString()),
                        f_name = data["f_name"].ToString(),
                        l_name = data["l_name"].ToString(),
                        email = data["email"].ToString(),
                        password = "Nice try :P",
                        city = data["city"].ToString(),
                        street = data["street"].ToString()
                    };
                }
                conn.Close();
                return parent;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return null;
            }

        } 

         public static List<Slot> getSlots(int parentID)
        {
            List<Slot> slots = new List<Slot>();
            Parent parent = getParentData(parentID);
            try
            {
                string sql = @"SELECT Slot.slot_ID,Slot.sitter_ID,Slot.[time], Slot.start_date 
                                FROM Slot INNER JOIN Sitter ON Sitter.sitter_ID = Slot.sitter_ID
                                WHERE Sitter.city = '" + parent.city.ToUpper()+"' AND Slot.start_date >=#"+DateTime.Now+"#;";

                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                OleDbDataReader data = com.ExecuteReader();
                while (data.Read())
                {
                    slots.Add(new Slot {
                        ID = int.Parse(data["slot_ID"].ToString()),
                        sitterID = int.Parse(data["sitter_ID"].ToString()),
                        date = DateTime.Parse(data["start_date"].ToString()),
                        time = DateTime.Parse(data["time"].ToString()),
                        city = parent.city
                    }); ;
                }
                conn.Close();
                return slots;
            }
            catch (Exception ex)
            
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return slots;
            }
        }

        public static int LoginParent(Parent parent)
        {
            string sql;
            OleDbConnection conn = new OleDbConnection(getConnectionString());
            try
            {
                sql = "SELECT parent_ID FROM Parent WHERE email='" + parent.email + "' AND [password]='" + parent.password + "'";
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                OleDbDataReader data = com.ExecuteReader();
                if (data.HasRows)
                {
                    int parent_ID = 1;
                    while (data.Read())
                    {
                        parent_ID = data.GetInt32(0);
                    }
                    return parent_ID;

                }
                else
                {
                    return -1;
                }
                
            }catch(Exception ex)
            {
                Console.WriteLine("{0}", ex.Message);
                return -1;
            }
        }
    }
}