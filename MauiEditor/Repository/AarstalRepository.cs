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
    public class AarstalRepository : Repository, IEnumerable<Aarstal>
    {
        private readonly List<Aarstal> list = [];

        public IEnumerator<Aarstal> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Search(string year)
        {
            try
            {
                SqlCommand sqlCommand = new("SELECT * FROM Aarstal WHERE Years LIKE @Year", connection);
                SqlCommand command = sqlCommand;
                command.Parameters.Add(Repository.CreateParam("@Year", year + "%", SqlDbType.NVarChar));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                list.Clear();
                while (reader.Read()) list.Add(new Aarstal(reader[0].ToString()));
                OnChanged(DbOperation.SELECT, DbModeltype.Aarstal);
            }
            catch (Exception ex)
            {
                throw new DbException("Error in Year repositiory: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
        }

        public void Add(string year)
        {
            string error = "";
            Aarstal aarstal = new(year);
            if (aarstal.IsValid)
            {
                try
                {
                    SqlCommand sqlCommand = new("INSERT INTO Aarstal (Years) VALUES (@Year)", connection);
                    SqlCommand command = sqlCommand;
                    command.Parameters.Add(Repository.CreateParam("@Year", year, SqlDbType.NVarChar));
                    connection.Open();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        list.Add(aarstal);
                        list.Sort();
                        OnChanged(DbOperation.INSERT, DbModeltype.Aarstal);
                        return;
                    }
                    error = string.Format("{0} could not be inserted in the database", year);
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
            else error = "Illegal value for year";
            throw new DbException("Error in year repositiory: " + error);
        }

        public void Update(string oldYear, string newYear)
        {
            string error = "";
            if (oldYear.Length > 0 && newYear.Length > 0)
            {
                try
                {
                    SqlCommand sqlCommand = new("UPDATE Aarstal SET Years = @NewYear WHERE Years = @OldYear", connection);
                    SqlCommand command = sqlCommand;
                    command.Parameters.Add(Repository.CreateParam("@OldYear", oldYear, SqlDbType.NVarChar));
                    command.Parameters.Add(Repository.CreateParam("@NewYear", newYear, SqlDbType.NVarChar));
                    connection.Open();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        for (int i = 0; i < list.Count; ++i)
                            if (list[i].Year.Equals(oldYear))
                            {
                                list[i].Year = newYear;
                                break;
                            }
                        OnChanged(DbOperation.UPDATE, DbModeltype.Aarstal);
                        return;
                    }
                    error = string.Format("Aarstal could not be update {0} => {1}", oldYear, newYear);
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
            else error = "Illegal value for year";
            throw new DbException("Error in Aarstal repositiory: " + error);
        }

        public void Remove(string year)
        {
            string error = "";
            try
            {
                SqlCommand sqlCommand = new("DELETE FROM Aarstal WHERE Years = @Year", connection);
                SqlCommand command = sqlCommand;
                command.Parameters.Add(Repository.CreateParam("@Year", year, SqlDbType.NVarChar));
                connection.Open();
                if (command.ExecuteNonQuery() == 1)
                {
                    list.Remove(new Aarstal(year));
                    OnChanged(DbOperation.DELETE, DbModeltype.Aarstal);
                    return;
                }
                error = string.Format("Aarstal {0} could not be deleted", year);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            throw new DbException("Error in Aarstal repositiory: " + error);
        }

        public static string GetYear(string year)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
                SqlCommand sqlCommand = new("SELECT Year FROM Aarstal WHERE Years = @Year", connection);
                SqlCommand command = sqlCommand;
                SqlParameter sqlParameter = new("@Year", SqlDbType.NVarChar)
                {
                    Value = year
                };
                SqlParameter param = sqlParameter;
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
