using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiEditor.Model;
using System.Configuration;

namespace MauiEditor.Repository
{
    public class DataRepository : Repository, IEnumerable<Data>
    {
        private List<Data> list = new List<Data>();

        public IEnumerator<Data> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Search(string komNr, string city, string gruppe, string year)
        {
            try
            {
                SqlCommand command = new SqlCommand("Select Data.Id, Data.Kom_nr, City, Gruppe, Aarstal, Tal From Data Join Keynummer on Data.GruppeId = Keynummer.Id Join Kommune on Data.Kom_nr = Kommune.Kom_nr WHERE Aarstal LIKE @Year AND City LIKE @City AND Gruppe LIKE @Gruppe", connection);
                //command.Parameters.Add(CreateParam("@KomNr", komNr + "%", SqlDbType.NVarChar));
                command.Parameters.Add(CreateParam("@City", city + "%", SqlDbType.NVarChar));
                command.Parameters.Add(CreateParam("@Gruppe", gruppe + "%", SqlDbType.NVarChar));
                command.Parameters.Add(CreateParam("@Year", year + "%", SqlDbType.NVarChar));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                list.Clear();
                while (reader.Read()) list.Add(new Data(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString()));
                OnChanged(DbOperation.SELECT, DbModeltype.Data);
            }
            catch (Exception ex)
            {
                throw new DbException("Error in Data repositiory: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
        }

        //public void Add(string phone, string fname, string lname, string addr, string code, string email, string title)
        //{
        //  string error = "";
        //  Contact contact = new Contact(phone, fname, lname, addr, code, "", email, title);
        //  if (contact.IsValid)
        //  {
        //    try
        //    {
        //      SqlCommand command = new SqlCommand("INSERT INTO Addresses (phone, lname, fname, addr, code, email, title) VALUES (@Phone, @Lname, @Fname, @Addr, @Code, @Mail, @Title)", connection);
        //      command.Parameters.Add(CreateParam("@Phone", phone, SqlDbType.NVarChar));
        //      command.Parameters.Add(CreateParam("@Lname", lname, SqlDbType.NVarChar));
        //      command.Parameters.Add(CreateParam("@Fname", fname, SqlDbType.NVarChar));
        //      command.Parameters.Add(CreateParam("@Addr", addr, SqlDbType.NVarChar));
        //      command.Parameters.Add(CreateParam("@Code", code, SqlDbType.NVarChar));
        //      command.Parameters.Add(CreateParam("@Mail", email, SqlDbType.NVarChar));
        //      command.Parameters.Add(CreateParam("@Title", title, SqlDbType.NVarChar));
        //      connection.Open();
        //      if (command.ExecuteNonQuery() == 1)
        //      {
        //        contact.City = ZipcodeRepository.GetCity(code);
        //        list.Add(contact);
        //        list.Sort();
        //        OnChanged(DbOperation.INSERT, DbModeltype.Contact);
        //        return;
        //      }
        //      error = string.Format("Address could not be inserted in the database");
        //    }
        //    catch (Exception ex)
        //    {
        //      error = ex.Message;
        //    }
        //    finally
        //    {
        //      if (connection != null && connection.State == ConnectionState.Open) connection.Close();
        //    }
        //  }
        //  else error = "Illegal value for address";
        //  throw new DbException("Error in Contact repositiory: " + error);
        //}

        public void Add(Data data)
        {
            string error = "";
            if (data.IsValid)
            {
                try
                {
                    string id = CountID();
                    int to = int.Parse(id);
                    id = (id + 1).ToString();
                    string gruppeId = KeynummerRepository.GetId(data.Gruppe);
                    SqlCommand command = new SqlCommand("INSERT INTO Data (Id, Kom_nr, GruppeID, Aarstal, Tal) VALUES (@Id, @KomNr, @Gruppe, @Year, @Num)", connection);
                    command.Parameters.Add(CreateParam("@Id", id, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@KomNr", data.KomNr, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Gruppe", gruppeId, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Year", data.Year, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Num", data.Num, SqlDbType.NVarChar));
                    connection.Open();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        data.City = KommuneRepository.GetCity(data.KomNr);
                        list.Add(data);
                        list.Sort();
                        OnChanged(DbOperation.INSERT, DbModeltype.Data);
                        return;
                    }
                    error = string.Format("KomNr could not be inserted in the database");
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
                finally
                {
                    if (connection != null && connection.State == ConnectionState.Open) connection.Close();
                }
            }
            else error = "Illegal value for KomNr";
            throw new DbException("Error in Data repositiory: " + error);
        }

        public void Update(string id, string komNr, string city, string gruppe, string year, string num)
        {
            Update(new Data(id, komNr, city, gruppe, year, num));
        }

        public void Update(Data data)
        {
            string error = "";
            if (data.IsValid)
            {
                try
                {
                    string dataId = GetId(data.KomNr, data.Gruppe, data.Year);
                    string gruppeId = KeynummerRepository.GetId(data.Gruppe);
                    SqlCommand command = new SqlCommand("UPDATE Data SET Kom_nr = @KomNr, GruppeId = @Gruppe, Aarstal = @Year, Tal = @Num WHERE Id = @DataId", connection);
                    command.Parameters.Add(CreateParam("@DataId", dataId, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@KomNr", data.KomNr, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Gruppe", gruppeId, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Year", data.Year, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Num", data.Num, SqlDbType.NVarChar));
                    connection.Open();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        UpdateList(data);
                        OnChanged(DbOperation.UPDATE, DbModeltype.Data);
                        return;
                    }
                    error = string.Format("Data could not be updated");
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
                finally
                {
                    if (connection != null && connection.State == ConnectionState.Open) connection.Close();
                }
            }
            else error = "Illegal value for Data";
            throw new DbException("Error in Data repositiory: " + error);
        }

        private void UpdateList(Data data)
        {
            for (int i = 0; i < list.Count; ++i)
                if (list[i].DataId.Equals(data.DataId))
                {
                    list[i].KomNr = data.KomNr;
                    list[i].City = KommuneRepository.GetCity(data.KomNr);
                    list[i].Gruppe = data.Gruppe;
                    list[i].Year = data.Year;
                    list[i].Num = data.Num;
                    break;
                }
        }

        public void Remove(Data data)
        {
            string error = "";
            try
            {
                string dataId = GetId(data.KomNr, data.Gruppe, data.Year);
                SqlCommand command = new SqlCommand("DELETE FROM Data WHERE Id = @DataId", connection);
                command.Parameters.Add(CreateParam("@DataId", dataId, SqlDbType.NVarChar));
                connection.Open();
                if (command.ExecuteNonQuery() == 1)
                {
                    list.Remove(new Data(dataId, "", "", "", "", ""));
                    OnChanged(DbOperation.DELETE, DbModeltype.Data);
                    return;
                }
                error = string.Format("Contact could not be deleted");
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            throw new DbException("Error in Contact repositiory: " + error);
        }

        public static string GetId(string komNr, string gruppe, string year)
        {
            SqlConnection connection = null;
            try
            {
                string GruppeID = KeynummerRepository.GetId(gruppe);
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
                SqlCommand command = new SqlCommand("SELECT Id FROM Data WHERE GruppeId = @Gruppe AND Kom_Nr = @KomNr AND Aarstal = @Year", connection);
                SqlParameter param = new SqlParameter("@Gruppe", SqlDbType.NVarChar);
                SqlParameter param2 = new SqlParameter("@KomNr", SqlDbType.NVarChar);
                SqlParameter param3 = new SqlParameter("@Year", SqlDbType.NVarChar);
                param.Value = GruppeID;
                param2.Value = komNr;
                param3.Value = year;
                command.Parameters.Add(param);
                command.Parameters.Add(param2);
                command.Parameters.Add(param3);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read()) return reader[0].ToString();
            }
            catch
            {
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            return "";
        }
        public static string CountID()
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
                SqlCommand command = new SqlCommand("SELECT COUNT(Id )FROM Data", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read()) return reader[0].ToString();
            }
            catch
            {
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            return "";
        }
    }
}
