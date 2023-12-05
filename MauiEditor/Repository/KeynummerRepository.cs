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
    public class KeynummerRepository : Repository, IEnumerable<Keynummer>
    {
        private readonly List<Keynummer> list = [];

        public IEnumerator<Keynummer> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Search(string id, string gruppe)
        {
            try
            {
                SqlCommand command = new("SELECT Id, Gruppe FROM Keynummer WHERE Id LIKE @KeyId AND Gruppe LIKE @Gruppe", connection);
                command.Parameters.Add(CreateParam("@Id", id + "%", SqlDbType.NVarChar));
                command.Parameters.Add(CreateParam("@Gruppe", gruppe + "%", SqlDbType.NVarChar));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                list.Clear();
                while (reader.Read()) list.Add(new Keynummer(reader[0].ToString(),
                                                             reader[1].ToString()));
                OnChanged(DbOperation.SELECT, DbModeltype.Keynummer);
            }
            catch (Exception ex)
            {
                throw new DbException("Error in Keynummer repositiory: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
        }

        public void Add(string gruppe)
        {
            string error = "";
            gruppe = gruppe.Trim();
            int count = list.Count;
            string keyId = (count + 1).ToString();
            Keynummer keynummer = new(keyId, gruppe);
            if (keynummer.IsValid)
            {
                try
                {
                    SqlCommand command = new("INSERT INTO Keynummer (Id, Gruppe) VALUES (@Id, @Gruppe)", connection);
                    command.Parameters.Add(CreateParam("@Id", keyId, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Gruppe", gruppe, SqlDbType.NVarChar));
                    connection.Open();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        list.Add(keynummer);
                        list.Sort();
                        OnChanged(DbOperation.INSERT, DbModeltype.Keynummer);
                        return;
                    }
                    error = string.Format("{0} could not be inserted in the database", gruppe);
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
            else error = "Illegal value for gruppe";
            throw new DbException("Error in Keynnummer repositiory: " + error);
        }

        public void Update(string key, string gruppe)
        {
            string error = "";
            gruppe = gruppe.Trim();
            if (gruppe.Length > 0)
            {
                try
                {
                    SqlCommand command = new("UPDATE Keynummer SET Gruppe = @Gruppe WHERE Id = @KeyId", connection);
                    command.Parameters.Add(CreateParam("@KeyId", key, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Gruppe", gruppe, SqlDbType.NVarChar));
                    connection.Open();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        for (int i = 0; i < list.Count; ++i)
                            if (list[i].KeyId.Equals(key))
                            {
                                list[i].Gruppe = gruppe;
                                break;
                            }
                        OnChanged(DbOperation.UPDATE, DbModeltype.Keynummer);
                        return;
                    }
                    error = string.Format("Keynummer {0} could not be update", key);
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
            else error = "Illegal value for gruppe";
            throw new DbException("Error in Keynummer repositiory: " + error);
        }

        public void Remove(string key)
        {
            string error = "";
            try
            {
                SqlCommand command = new("DELETE FROM Keynummer WHERE Id = @KeyId", connection);
                command.Parameters.Add(CreateParam("@KeyId", key, SqlDbType.NVarChar));
                connection.Open();
                if (command.ExecuteNonQuery() == 1)
                {
                    list.Remove(new Keynummer(key, ""));
                    OnChanged(DbOperation.DELETE, DbModeltype.Keynummer);
                    return;
                }
                error = string.Format("Keynummer {0} could not be deleted", key);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            throw new DbException("Error in Keynummer repositiory: " + error);
        }

        public static string GetGruppe(string key)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
                SqlCommand command = new("SELECT Gruppe FROM Keynummer WHERE Id = @KeyId", connection);
                SqlParameter param = new("@KeyId", SqlDbType.NVarChar)
                {
                    Value = key
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
        public static string GetId(string gruppe)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
                SqlCommand command = new("SELECT Id FROM Keynummer WHERE Gruppe = @Gruppe", connection);
                SqlParameter param = new("@Gruppe", SqlDbType.NVarChar)
                {
                    Value = gruppe
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
    }
}
