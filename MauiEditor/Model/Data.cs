using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiEditor.Model
{
    public class Data : IDataErrorInfo, IComparable<Data>
    {
        public string DataId { get; set; } // Id
        public string KomNr { get; set; } // kom_nr
        public string City { get; set; } // By
        public string Gruppe { get; set; } // NøgleTal
        public string Year { get; set; } // Year
        public string Num { get; set; } // Tal

        public Data()
        {
            DataId = "";
            KomNr = "";
            City = "";
            Gruppe = "";
            Year = "";
            Num = "";
        }

        public Data(string dataId, string komNr, string city, string gruppe, string year, string num)
        {
            DataId = dataId;
            KomNr = komNr;
            City = city;
            Gruppe = gruppe;
            Year = year;
            Num = num;
        }

        public override bool Equals(object obj)
        {
            try
            {
                Data data = (Data)obj;
                return DataId.Equals(data.DataId);
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return DataId.GetHashCode();
        }

        /*  public override string ToString()
          {
              return string.Format("{0} ", DataID);
          }*/

        // Implementerer ordning af objekter, så der alene sammenlignes på postnummer.
        public int CompareTo(Data data)
        {
            return DataId.CompareTo(data.DataId);
        }
        private static readonly string[] validatedProperties = { "KomNr", "City", "Gruppe", "Year", "Num" };
        public bool IsValid
        {
            get
            {
                foreach (string property in validatedProperties) if (GetError(property) != null) return false;
                return true;
            }
        }

        string IDataErrorInfo.Error
        {
            get { return IsValid ? null : "Illegal model object"; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get { return Validate(propertyName); }
        }

        private string GetError(string name)
        {
            foreach (string property in validatedProperties) if (property.Equals(name)) return Validate(name);
            return null;
        }

        private string Validate(string name)
        {
            switch (name)
            {
                case "KomNr": return ValidateKomNr();
                case "City": return ValidateCity();
                case "Gruppe": return ValidateGruppe();
                case "Year": return ValidateYear();
                case "Num": return ValidateTal();
            }
            return null;
        }

        private string? ValidateKomNr()
        {
            if (KomNr.Length != 3) return "Kom_nr must be a number of 3 digits";
            foreach (char c in KomNr) if (c < '0' || c > '9') return "KomNr must be a number";
            return null;
        }
        private string? ValidateCity()
        {
            if (City == null || City.Length == 0) return "City can not be empty";
            return null;
        }
        private string? ValidateGruppe()
        {
            if (Gruppe == null || Gruppe.Length == 0) return "Gruppe can not be empty";
            return null;
        }
        private string? ValidateYear()
        {
            if (Year.Length != 4) return "Year must be a number of 4 digits";
            foreach (char c in Year) if (c < '0' || c > '9') return "Year must be a number of 4 digits";
            return null;
        }
        private string? ValidateTal()
        {
            foreach (char c in Num) if (c < '0' || c > '9') return "Number must be a number";
            return null;
        }
    }
}
