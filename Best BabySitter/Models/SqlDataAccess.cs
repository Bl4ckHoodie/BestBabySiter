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


        /*Parent Queries
         * getParentID, getParentData, loginParent, registerParent
         */
        #region

        public static int getParentID(Parent parent)
        {
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
       
        public static Parent getParentData(int id)
        {
            Parent parent = null;
            try
            {
                string sql = @" SELECT        parent_ID, f_name, l_name, street, city, email, [password]
                                FROM Parent
                                WHERE(parent_ID =" + id + ")";

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
        
        public static Parent getParentActualData(int id)
        {
            Parent parent = null;
            try
            {
                string sql = @" SELECT        parent_ID, f_name, l_name, street, city, email, [password]
                                FROM Parent
                                WHERE(parent_ID =" + id + ")";

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
                        password = data["password"].ToString(),
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

            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}", ex.Message);
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

        public static int updateParentInfo(Parent parent)
        {
            try
            {

                string sql;
                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                if (parent.password != null)
                    sql = "UPDATE Parent SET f_name ='" + parent.f_name + "', l_name= '" + parent.l_name + "', street ='" + parent.street + "', city ='" + parent.city + "', [password] = '" + parent.password + "', email='"+parent.email+"'  WHERE (parent_ID=" + parent.parent_ID + ") ";
                else
                    sql = "UPDATE Parent SET f_name ='" + parent.f_name + "', l_name= '" + parent.l_name + "', street ='" + parent.street + "', city ='" + parent.city + "', email='" + parent.email + "' WHERE (parent_ID=" + parent.parent_ID + ") ";
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
        #endregion


        /*Advert Queries
         * Create, Close, getResponse, getOpenAdverts, updateAdvert,
         * getAdvert, getOpenAdvertsForSitter
         */
        #region
        public static int createAdvert(Advert newAdvert, int parentID)
        {
            try
            {
                string sql;
                if (newAdvert.Specification == null)
                    sql = "Insert INTO Advert (parent_ID, num_kids, specification, location, start_date, end_date, start_time, end_time, date_created,age_range) VALUES (" + parentID + ", " + newAdvert.NumKids + ", 'None', '" + newAdvert.Street + "," + newAdvert.City + "', '" + newAdvert.StartDate + "', '" + newAdvert.EndDate + "', '" + newAdvert.StartTime + "', '" + newAdvert.EndTime + "','" + newAdvert.DateCreated + "','" + newAdvert.AgeRange + "')";
                else
                    sql = "Insert INTO Advert (parent_ID, num_kids, specification, location, start_date, end_date, start_time, end_time, date_created,age_range) VALUES (" + parentID + ", " + newAdvert.NumKids + ", '" + newAdvert.Specification + "', '" + newAdvert.Street + "," + newAdvert.City + "', '" + newAdvert.StartDate + "', '" + newAdvert.EndDate + "', '" + newAdvert.StartTime + "', '" + newAdvert.EndTime + "','" + newAdvert.DateCreated + "','" + newAdvert.AgeRange + "')";
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

        public static int closeAdvert(Advert advert)
        {
            try
            {

                string sql;
                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                sql = "UPDATE Advert SET closed = True WHERE (Advert_ID=" + advert.ID + ") ";
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

        public static List<Sitter> getResponses(int advertID)
        {
            List<Sitter> sitters = new List<Sitter>();
            try
            {
                string sql = @"SELECT jobApplication.sitter_ID, Sitter.f_name, Sitter.L_name, Sitter.email, Sitter.AboutMe, Sitter.city, Sitter.profilePicPath, Sitter.contact_NO, Sitter.chargePerService, Sitter.street
                               FROM   ((jobApplication INNER JOIN
                               Sitter ON jobApplication.sitter_ID = Sitter.sitter_ID) INNER JOIN
                               Advert ON Advert.Advert_ID = jobApplication.advert_ID)
                               WHERE (jobApplication.advert_ID = " + advertID + ") AND (Advert.closed = False)";

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
                        chargePerService = int.Parse(data["chargePerService"].ToString()),
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
                string sql;
                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                sql = "UPDATE Advert SET closed=TRUE WHERE (parent_ID=" + parentID + " AND end_date<#"+DateTime.Now+"#) ";
                OleDbCommand update = new OleDbCommand(sql, conn);
                update.ExecuteNonQuery();
                conn.Close();
                sql = "SELECT * FROM Advert WHERE parent_ID =" + parentID + " AND closed=False";
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
                        AgeRange = data["age_range"].ToString()
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

        public static int updateAdvert(Advert advert)
        {
            try
            {

                string sql;
                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                sql = "UPDATE Advert SET num_kids =" + advert.NumKids + ", specification = '" + advert.Specification + "', location='" + advert.Street + "," + advert.City + "', start_date='" + advert.StartDate + "', end_date='" + advert.EndDate + "', start_time='" + advert.StartTime + "', end_time='" + advert.EndTime + "', age_range='" + advert.AgeRange + "' WHERE (Advert_ID=" + advert.ID + ") ";
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

        public static Advert getAdvert(int id)
        {
            try
            {
                string sql = "SELECT * FROM Advert WHERE Advert_ID =" + id + "";
                Advert advert = null;
                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                OleDbDataReader data = com.ExecuteReader();
                while (data.Read())
                {
                    String[] location = data["location"].ToString().Split(',');
                    advert = new Advert
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
                        AgeRange = data["age_range"].ToString()
                    };
                }
                conn.Close();
                return advert;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return null;
            }
        }

        public static List<Advert> getOpenAdvertsForSitter(int sitterID)
        {

            List<Advert> adverts = new List<Advert>();
            try
            {
                Sitter sitter = getSitterData(sitterID.ToString());
                string sql = @"SELECT        Advert_ID, parent_ID, num_kids, specification, location, date_created, start_time, end_time, start_date, end_date, closed, age_range
                                FROM Advert
                                WHERE(location LIKE '%" + sitter.city + "%') AND(closed = FALSE) AND start_date >=#" + DateTime.Now + "# AND (Advert_ID <> IIF(IsNull((SELECT advert_ID  FROM jobApplication WHERE(sitter_ID = " + sitter.sitter_ID + ") AND(advert_ID = Advert.Advert_ID))), 0, (SELECT advert_ID  FROM jobApplication WHERE(sitter_ID = " + sitter.sitter_ID + ") AND(advert_ID = Advert.Advert_ID))))";

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
                        AgeRange = data["age_range"].ToString()
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

        #endregion


        /*Admin Queries
         * getAdminData, LoginAdmin, getUnverified(Sitters),
         * getFlagged(Sitters), 
         */
        #region
        public static Admin getAdminData(object obj)
        {
            int id = int.Parse(obj.ToString());
            Admin admin = null;
            try
            {
                string sql = @" SELECT   *
                                FROM Admin
                                WHERE(Admin_ID =" + id + ")";

                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                OleDbDataReader data = com.ExecuteReader();
                while (data.Read())
                {

                    admin = new Admin
                    {
                        Admin_ID = int.Parse(data["Admin_ID"].ToString()),
                        f_name = data["f_name"].ToString(),
                        l_name = data["l_name"].ToString(),
                        email = data["email"].ToString(),
                        password = "Nice try :P",
                        
                    };
                }
                conn.Close();
                return admin;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return null;
            }


        }
        public static int LoginAdmin(Admin admin)
        {
            string sql;
            OleDbConnection conn = new OleDbConnection(getConnectionString());
            try
            {
                sql = "SELECT Admin_ID FROM Admin WHERE email='" + admin.email + "' AND [password]='" + admin.password + "'";
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                OleDbDataReader data = com.ExecuteReader();
                if (data.HasRows)
                {
                    int Admin_ID = 1;
                    while (data.Read())
                    {
                        Admin_ID = data.GetInt32(0);
                    }
                    return Admin_ID;

                }
                else
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}", ex.Message);
                return -1;
            }
        }
        public static List<Sitter> getUnverified()
        {
            List<Sitter> sitters = new List<Sitter>();
            try
            {
                //change Sql to get unverified sitters
                string sql = "SELECT * FROM Sitter WHERE verified = False";
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
                        chargePerService = int.Parse(data["chargePerService"].ToString()),
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

        public static List<Sitter> getFlagged()
        {
            List<Sitter> sitters = new List<Sitter>();
            try
            {
                //change Sql to get flagged sitters
                string sql = "SELECT * FROM Sitter";
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
                        chargePerService = int.Parse(data["chargePerService"].ToString()),
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

        public static int updateAdminInfo(Admin admin)
        {
            try
            {

                string sql;
                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                if (admin.password != "")
                    sql = "UPDATE Admin SET f_name =" + admin.f_name + ", l_name= '" + admin.l_name + "', email ='" + admin.email+ "', [password] = '" + admin.password + "'  WHERE (Admin_ID=" + admin.Admin_ID + ") ";
                else
                    sql = "UPDATE Admin SET f_name =" + admin.f_name + ", l_name= '" + admin.l_name + "', email ='" + admin.email + "' WHERE (Admin_ID=" + admin.Admin_ID + ") ";
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
        #endregion


        /*Sitter Queries
         * getSitterData, verifySitter, getSittersToSuspend
         */
        #region

        public static List<Sitter> getSittersToSuspend()
        {
            List<Sitter> sitters = new List<Sitter>();
            try
            {
                string sql = "SELECT * FROM Sitter";
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
                        chargePerService = int.Parse(data["chargePerService"].ToString()),
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

        public static Sitter getSitterData(string id)
        {
            Sitter sitter = null;
            try
            {
                string sql = @" SELECT   *
                                FROM Sitter
                                WHERE(sitter_ID =" + id + ")";

                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                OleDbDataReader data = com.ExecuteReader();
                while (data.Read())
                {
                    sitter = new Sitter
                    {
                        sitter_ID = int.Parse(data["sitter_ID"].ToString()),
                        f_name = data["f_name"].ToString(),
                        L_name = data["L_name"].ToString(),
                        password = "not happening buddy :P",
                        email = data["email"].ToString(),
                        AboutMe = data["AboutMe"].ToString(),
                        city = data["city"].ToString(),
                        Location = data["Location"].ToString(),
                        cv_filePath = data["cv_filePath"].ToString(),
                        hasProfile = bool.Parse(data["hasProfile"].ToString()),
                        suspended = bool.Parse(data["suspended"].ToString()),
                        verified = bool.Parse(data["verified"].ToString()),
                        service_Duration = data["service_Duration"].ToString(),
                        profilePicPath = data["profilePicPath"].ToString(),
                        contact_NO = data["contact_NO"].ToString(),
                        chargePerService = int.Parse(data["chargePerService"].ToString()),
                        street = data["street"].ToString()
                    };
                }
                conn.Close();
                return sitter;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return null;
            }
        }

        public static int verifySitter(string condition, int id)
        {
            if (int.Parse(condition) == 1)
            {
                try
                {

                    string sql;
                    OleDbConnection conn = new OleDbConnection(getConnectionString());
                    conn.Open();
                    sql = "UPDATE Sitter SET verified = TRUE  WHERE (sitter_ID=" + id + ") ";
                    OleDbCommand update = new OleDbCommand(sql, conn);
                    update.ExecuteNonQuery();
                    conn.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return 1;
            }
            else if (int.Parse(condition) == 0)
            {
                try
                {

                    string sql;
                    OleDbConnection conn = new OleDbConnection(getConnectionString());
                    conn.Open();
                    sql = "UPDATE Sitter SET verified = FALSE  WHERE (sitter_ID=" + id + ") ";
                    OleDbCommand update = new OleDbCommand(sql, conn);
                    update.ExecuteNonQuery();
                    conn.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static bool applyForJob(int sitterID, int advertID)
        {

            try
            {
                string sql = "INSERT INTO jobApplication (advert_ID, sitter_ID) VALUES(" + advertID + "," + sitterID + ")";
                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                com.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return false;
            }

        }

        public static int LoginSitter(Sitter sitter)
        {
            string sql;
            OleDbConnection conn = new OleDbConnection(getConnectionString());
            try
            {
                sql = "SELECT sitter_ID FROM Sitter WHERE email='" + sitter.email + "' AND [password]='" + sitter.password + "'";
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                OleDbDataReader data = com.ExecuteReader();
                if (data.HasRows)
                {
                    int sitter_ID = 1;
                    while (data.Read())
                    {
                        sitter_ID = data.GetInt32(0);
                    }
                    return sitter_ID;

                }
                else
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}", ex.Message);
                return -1;
            }
        }

        public static int registerSitter (Sitter sitter)
        {
            try
            {
                string sql;
                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                sql = "SELECT sitter_ID FROM Sitter WHERE email='" + sitter.email + "'";
                OleDbCommand check = new OleDbCommand(sql, conn);
                OleDbDataReader data = check.ExecuteReader();
                if (!data.HasRows)
                {
                    sql = "INSERT INTO Sitter (f_name, L_name, street, city,cv_filePath, email, [password]) VALUES ('" + sitter.f_name + "', '" + sitter.L_name + "', '" + sitter.street + "', '" + sitter.city + "','"+sitter.cv_filePath+"' ,'" + sitter.email + "', '" + sitter.password + "')";

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

        public static int updateSitterInfo(Sitter sitter)
        {
            try
            {

                string sql;
                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                if(sitter.password != null)
                    sql = "UPDATE Sitter SET f_name ='" + sitter.f_name + "', L_name= '" + sitter.L_name+ "', Location='" + sitter.street + "," + sitter.city + "', AboutMe = '"+sitter.AboutMe+"', contact_NO = '"+sitter.contact_NO+"', service_Duration = '"+sitter.service_Duration+"', chargePerService='"+sitter.chargePerService+"', profilePicPath='"+sitter.profilePicPath+"', email='"+sitter.email+"', street ='"+sitter.street+ "', city ='" + sitter.city + "', [password] = '" + sitter.password+"'  WHERE (sitter_ID=" + sitter.sitter_ID+ ") ";
                else
                    sql = "UPDATE Sitter SET f_name ='" + sitter.f_name + "', L_name= '" + sitter.L_name + "', Location='" + sitter.street + "," + sitter.city + "', AboutMe = '" + sitter.AboutMe + "', contact_NO = '" + sitter.contact_NO + "', service_Duration = '" + sitter.service_Duration + "', chargePerService='" + sitter.chargePerService + "', profilePicPath='" + sitter.profilePicPath + "', email='" + sitter.email + "', street ='" + sitter.street + "', city ='" + sitter.city + "'  WHERE (sitter_ID=" + sitter.sitter_ID + ") ";
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

        public static Sitter getSitterActualData(string id)
        {
            Sitter sitter = null;
            try
            {
                string sql = @" SELECT   *
                                FROM Sitter
                                WHERE(sitter_ID =" + id + ")";

                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                OleDbDataReader data = com.ExecuteReader();
                while (data.Read())
                {
                    sitter = new Sitter
                    {
                        sitter_ID = int.Parse(data["sitter_ID"].ToString()),
                        f_name = data["f_name"].ToString(),
                        L_name = data["L_name"].ToString(),
                        password = data["password"].ToString(),
                        email = data["email"].ToString(),
                        AboutMe = data["AboutMe"].ToString(),
                        city = data["city"].ToString(),
                        Location = data["Location"].ToString(),
                        cv_filePath = data["cv_filePath"].ToString(),
                        hasProfile = bool.Parse(data["hasProfile"].ToString()),
                        suspended =  bool.Parse(data["suspended"].ToString()),
                        verified = bool.Parse(data["verified"].ToString()),
                        service_Duration = data["service_Duration"].ToString(),
                        profilePicPath = data["profilePicPath"].ToString(),
                        contact_NO = data["contact_NO"].ToString(),
                        chargePerService = int.Parse(data["chargePerService"].ToString()),
                        street = data["street"].ToString()
                    };
                }
                conn.Close();
                return sitter;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return null;
            }
        }

        #endregion


        /*Slot Queries
         * getSlots, getSitterOpenSlots, getSitterAppointmentSlots, getSitterJobDone, getSitterCreatedSlotCount
         */
        #region
        public static List<Slot> getSlots(int parentID)
        {
            List<Slot> slots = new List<Slot>();
            Parent parent = getParentData(parentID);
            try
            {
                string sql = @"SELECT Slot.slot_ID,Slot.sitter_ID,Slot.[time], Slot.start_date 
                                FROM Slot INNER JOIN Sitter ON Sitter.sitter_ID = Slot.sitter_ID
                                WHERE Sitter.city = '" + parent.city.ToUpper() + "' AND Slot.start_date >=#" + DateTime.Now + "#;";

                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                OleDbDataReader data = com.ExecuteReader();
                while (data.Read())
                {
                    slots.Add(new Slot
                    {
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
        public static List<Slot> getSitterOpenSlots(int sitterID)
        {
            List<Slot> slots = new List<Slot>();
            Sitter sitter = getSitterData(sitterID.ToString());
            try
            {
                string sql = @"SELECT slot_ID, sitter_ID, start_time, end_time, start_date, end_date 
                                FROM Slot 
                                WHERE sitter_ID = " + sitterID + " AND closed=FALSE AND Slot.start_date >=#" + DateTime.Now + "#;";

                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                OleDbDataReader data = com.ExecuteReader();
                while (data.Read())
                {
                    slots.Add(new Slot
                    {
                        ID = int.Parse(data["slot_ID"].ToString()),
                        sitterID = int.Parse(data["sitter_ID"].ToString()),
                        start_date= DateTime.Parse(data["start_date"].ToString()),
                        end_date = DateTime.Parse(data["end_date"].ToString()),
                        start_time = DateTime.Parse(data["start_time"].ToString()),
                        end_time = DateTime.Parse(data["end_time"].ToString()),
                        city = sitter.city
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
        public static List<Appointment> getSitterAppointmentSlots(int sitterID)
        {
            List<Appointment> slots = new List<Appointment>();
            Sitter sitter = getSitterData(sitterID.ToString());
            try
            {
                string sql = @"SELECT  Slot.start_time, Slot.end_time, Slot.start_date, Slot.end_date, Parent.f_name, Parent.l_name,Parent.city, Parent.street, Parent.email 
                                FROM Slot 
                                INNER JOIN Booking ON Slot.slot_ID = Booking.slot_ID 
                                INNER JOIN Parent ON Parent.parent_ID = Booking.parent_ID
                                WHERE Slot.sitter_ID = " + sitterID + " AND Slot.closed=TRUE AND Booking.Booked=TRUE AND Slot.start_date >=#" + DateTime.Now + "#;";

                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                OleDbDataReader data = com.ExecuteReader();
                while (data.Read())
                {
                    slots.Add(new Appointment
                    {
                        appointee = data["f_name"].ToString().Substring(0,1).ToUpper()+" "+data["l_name"].ToString(),
                        StartDate = DateTime.Parse(data["start_date"].ToString()),
                        EndDate = DateTime.Parse(data["end_date"].ToString()),
                        StartTime = DateTime.Parse(data["start_time"].ToString()),
                        EndTime = DateTime.Parse(data["end_time"].ToString()),
                        City = data["city"].ToString(),
                        Street = data["street"].ToString(),
                        email = data["email"].ToString()
       
                    }); 
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

        public static int getSitterJobDone(int sitterID)
        {
            int count = 0;
            try
            {
                string sql = @"SELECT COUNT(slot_ID)
                                FROM Slot 
                                WHERE sitter_ID = " + sitterID + " AND closed=TRUE AND Slot.start_date <=#" + DateTime.Now + "#;";

                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                OleDbDataReader data = com.ExecuteReader();
                data.Read();
                count += data.GetInt32(0);
                conn.Close();

            }catch(Exception ex)
            {            
                Console.WriteLine("{0}", ex.Message);
                return 0;
            }
            return count;
         }
        public static int getSitterCreatedSlotCount(int sitterID)
        {
            int count = 0;
            try
            {
                string sql = @"SELECT COUNT(slot_ID)
                                FROM Slot 
                                WHERE sitter_ID = " + sitterID + " ";

                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                OleDbDataReader data = com.ExecuteReader();
                data.Read();
                count += data.GetInt32(0);
              
                conn.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}", ex.Message);
                return 0;
            }
            return count;
        }
        #endregion


        /*Other Queries
         * getAppointments, getEmails(Sitters)
         */
        #region

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
                        appointee = data["f_name"].ToString().Substring(0, 1).ToUpper() + ". " + data["l_name"].ToString().ToUpper(),
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

        public static List<string> getEmails(string city)
        {
            List<string> emails = new List<string>();
            try
            {
                string sql = "SELECT email FROM Sitter WHERE city='" + city + "'";

                OleDbConnection conn = new OleDbConnection(getConnectionString());
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                OleDbDataReader data = com.ExecuteReader();
                while (data.Read())
                {

                    emails.Add(data["email"].ToString());
                }
                conn.Close();
                return emails;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return null;
            }



        }

        #endregion


    }
}