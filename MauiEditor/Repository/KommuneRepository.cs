using MauiEditor.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiEditor.Repository
{
    public class KommuneRepository : Repository, IEnumerable<Kommune>
    {
        private readonly List<Kommune> list = [];

        public IEnumerator<Kommune> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Search(string komNr, string city)
        {
            try
            {
                SqlCommand command = new("SELECT Kom_nr, City FROM Kommune WHERE Kom_nr LIKE @KomNr AND City LIKE @City ", connection);
                command.Parameters.Add(CreateParam("@KomNr", komNr + "%", SqlDbType.NVarChar));
                command.Parameters.Add(CreateParam("@City", city + "%", SqlDbType.NVarChar));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                list.Clear();
                while (reader.Read())
                {
                    list.Add(new Kommune(reader[0].ToString(),
                                         reader[1].ToString()));
                }

                OnChanged(DbOperation.SELECT, DbModeltype.Kommune);
            }
            catch (Exception ex)
            {
                throw new DbException("Error in Kommune repositiory: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
        }

        public void Add(string komNr, string city)
        {
            string error = "";
            city = city.Trim();
            Kommune kommune = new(komNr, city);
            if (kommune.IsValid)
            {
                try
                {
                    SqlCommand command = new("INSERT INTO Kommune (Kom_nr, City) VALUES (@KomNr, @City)", connection);
                    command.Parameters.Add(CreateParam("@KomNr", komNr, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@City", city, SqlDbType.NVarChar));
                    connection.Open();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        list.Add(kommune);
                        list.Sort();
                        OnChanged(DbOperation.INSERT, DbModeltype.Kommune);
                        return;
                    }
                    error = string.Format("{0} And {1} could not be inserted in the database", komNr, city);
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
            else error = "Illegal value for Kommune";
            //throw new DbException("Error in Kommune repositiory: " + error);
        }

        public void Update(string komNr, string city)
        {
            string error = "";
            city = city.Trim();
            if (city.Length > 0)
            {
                try
                {
                    SqlCommand command = new("UPDATE Kommune SET City = @City WHERE Kom_nr = @KomNr", connection);
                    command.Parameters.Add(CreateParam("@KomNr", komNr, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@City", city, SqlDbType.NVarChar));
                    connection.Open();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        for (int i = 0; i < list.Count; ++i)
                            if (list[i].KomNr.Equals(komNr))
                            {
                                list[i].City = city;
                                break;
                            }
                        OnChanged(DbOperation.UPDATE, DbModeltype.Kommune);
                        return;
                    }
                    error = string.Format("Kommune {0} could not be update", komNr);
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
            else error = "Illegal value for city";
            //throw new DbException("Error in Kommune repositiory: " + error);
        }

        public void Remove(string komNr)
        {
            string error = "";
            try
            {
                SqlCommand command = new("DELETE FROM Data WHERE Kom_nr = @KomNr", connection);
                command.Parameters.Add(CreateParam("@KomNr", komNr, SqlDbType.NVarChar));
                SqlCommand command2 = new("DELETE FROM Kommune WHERE Kom_nr = @KomNr", connection);
                command2.Parameters.Add(CreateParam("@KomNr", komNr, SqlDbType.NVarChar));
                connection.Open();
                command.ExecuteNonQuery();
                if (command2.ExecuteNonQuery() == 1)
                {
                    list.Remove(new Kommune(komNr, ""));
                    OnChanged(DbOperation.DELETE, DbModeltype.Kommune);
                    return;
                }
                error = string.Format("Kommune {0} could not be deleted", komNr);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            throw new DbException("Error in Kommune repositiory: " + error);
        }

        public static string GetCity(string komNr)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
                SqlCommand command = new("SELECT City FROM Kommune WHERE Kom_nr = @KomNr", connection);
                SqlParameter param = new("@KomNr", SqlDbType.NVarChar)
                {
                    Value = komNr
                };
                command.Parameters.Add(param);
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
        public static string GetKomNr(string city)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
                SqlCommand command = new("SELECT Kom_nr FROM Kommune WHERE City = @City", connection);
                SqlParameter param = new("@City", SqlDbType.NVarChar)
                {
                    Value = city
                };
                command.Parameters.Add(param);
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
        public List<string>? GetCitys()
        {
            SqlConnection connection = null;
            List<string> list = new();
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
                SqlCommand command = new("SELECT City FROM Kommune", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                list.Clear();
                while (reader.Read())
                {
                    list.Add(reader[0].ToString());
                }
                return list;
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
