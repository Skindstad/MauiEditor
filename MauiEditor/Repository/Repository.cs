using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace MauiEditor.Repository
{
    // Definerer typen af hhv. den databaseoperation, der er udført og af hvilket repository.
    public enum DbOperation { SELECT, INSERT, UPDATE, DELETE };
    public enum DbModeltype { Data, Keynummer, Kommune, Aarstal }

    // EventArgs for en databaseoperation.
    public class DbEventArgs : EventArgs
    {
        public DbOperation Operation { get; private set; }                    // databaseoperation
        public DbModeltype Modeltype { get; private set; }                    // repository

        public DbEventArgs(DbOperation operation, DbModeltype modeltype)
        {
            Operation = operation;
            Modeltype = modeltype;
        }
    }
    // Exception type programmets repositories.
    public class DbException : Exception
    {
        public DbException(string message)
          : base(message)
        {
        }
    }

    public delegate void DbEventHandler(object sender, DbEventArgs e);

    // Basisklasse til et Repository.
    // Klassens konstruktør er defineret protected, da det ikke giver nogen mening at instantiere klassen.
    public class Repository
    {
        public event DbEventHandler RepositoryChanged;
        protected SqlConnection connection = null;

        protected Repository()
        {
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
            }
            catch (Exception ex)
            {
                throw new DbException("Error in repositiory: " + ex.Message);
            }
        }

        public void OnChanged(DbOperation opr, DbModeltype mt)
        {
            if (RepositoryChanged != null) RepositoryChanged(this, new DbEventArgs(opr, mt));
        }

        protected SqlParameter CreateParam(string name, object value, SqlDbType type)
        {
            SqlParameter param = new SqlParameter(name, type);
            param.Value = value;
            return param;
        }
    }
}
