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

        public void Search(string city, string gruppe, string year)
        {
            try
            {
                SqlCommand sqlCommand = new("Select Data.Id, Data.Kom_nr, City, Gruppe, Aarstal, Tal From Data Join Keynummer on Data.GruppeId = Keynummer.Id Join Kommune on Data.Kom_nr = Kommune.Kom_nr WHERE Aarstal LIKE @Year AND City LIKE @City AND Gruppe LIKE @Gruppe", connection);
                SqlCommand command = sqlCommand;
                //command.Parameters.Add(CreateParam("@KomNr", komNr + "%", SqlDbType.NVarChar));
                command.Parameters.Add(CreateParam("@City", city + "%", SqlDbType.NVarChar));
                command.Parameters.Add(CreateParam("@Gruppe", gruppe + "%", SqlDbType.NVarChar));
                command.Parameters.Add(CreateParam("@Year", year + "%", SqlDbType.NVarChar));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                list.Clear();
                while (reader.Read())
                {
                    list.Add(new Data(reader[0].ToString(),
                                      reader[1].ToString(),
                                      reader[2].ToString(),
                                      reader[3].ToString(),
                                      reader[4].ToString(),
                                      reader[5].ToString()));
                }

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

        public void Add(Data data)
        {
            string error = "";
            if (data.City.Length > 0 && data.Year.Length == 4 && data.Gruppe.Length > 0 && data.Num.Length >= 0)
            {

                if (GetIdWithCity(data.City, data.Gruppe, data.Year) == null)
                {
                    try
                    {
                        string komNr = KommuneRepository.GetKomNr(data.City);
                        string gruppeId = KeynummerRepository.GetId(data.Gruppe);
                        SqlCommand sqlCommand = new("INSERT INTO Data (Kom_nr, GruppeID, Aarstal, Tal) VALUES (@KomNr, @Gruppe, @Year, @Num)", connection);
                        SqlCommand command = sqlCommand;
                        command.Parameters.Add(CreateParam("@KomNr", komNr, SqlDbType.NVarChar));
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
                }
                else error = "Illegal value for KomNr";
                Console.WriteLine(error);
                // throw new DbException("Error in Data repositiory: " + error);
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
                    SqlCommand sqlCommand = new("UPDATE Data SET Kom_nr = @KomNr, GruppeId = @Gruppe, Aarstal = @Year, Tal = @Num WHERE Id = @DataId", connection);
                    SqlCommand command = sqlCommand;
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
                using (SqlCommand command = new("DELETE FROM Data WHERE Id = @DataId", connection))
                {
                    command.Parameters.Add(CreateParam("@DataId", dataId, SqlDbType.NVarChar));
                    connection.Open();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        list.Remove(new Data(dataId, "", "", "", "", ""));
                        OnChanged(DbOperation.DELETE, DbModeltype.Data);
                        return;
                    }
                }

                error = string.Format("Data could not be deleted");
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            throw new DbException("Error in Data repositiory: " + error);
        }

        public static string GetId(string komNr, string gruppe, string year)
        {
            SqlConnection connection = null;
            try
            {
                string GruppeID = KeynummerRepository.GetId(gruppe);
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
                SqlCommand sqlCommand = new("SELECT Id FROM Data WHERE GruppeId = @Gruppe AND Kom_Nr = @KomNr AND Aarstal = @Year", connection);
                SqlCommand command = sqlCommand;
                SqlParameter param = new("@Gruppe", SqlDbType.NVarChar);
                SqlParameter param2 = new("@KomNr", SqlDbType.NVarChar);
                SqlParameter param3 = new("@Year", SqlDbType.NVarChar);
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
        public static string? GetIdWithCity(string city, string gruppe, string year)
        {
            SqlConnection connection = null;
            try
            {
                string komNr = KommuneRepository.GetKomNr(city);
                string GruppeID = KeynummerRepository.GetId(gruppe);
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
                SqlCommand sqlCommand = new("SELECT Id FROM Data WHERE GruppeId = @Gruppe AND Kom_Nr = @KomNr AND Aarstal = @Year", connection);
                SqlCommand command = sqlCommand;
                SqlParameter param = new("@Gruppe", SqlDbType.NVarChar);
                SqlParameter param2 = new("@KomNr", SqlDbType.NVarChar);
                SqlParameter param3 = new("@Year", SqlDbType.NVarChar);
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
            return null;
        }
    }
}
