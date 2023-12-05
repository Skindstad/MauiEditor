using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiEditor.Model
{
    public class Kommune : IDataErrorInfo, IComparable<Kommune>
    {
        public string KomNr { get; set; } // kom_nr

        public string City { get; set; } // by / Kommune

        public Kommune()
        {
            KomNr = "";
            City = "";
        }

        public Kommune(string komNr, string city)
        {
            KomNr = komNr;
            City = city;
        }

        public override bool Equals(object obj)
        {
            try
            {
                Kommune kommune = (Kommune)obj;
                return KomNr.Equals(kommune.KomNr);
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return KomNr.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} ", KomNr);
        }

        // Implementerer ordning af objekter, så der alene sammenlignes på postnummer.
        public int CompareTo(Kommune kommune)
        {
            return KomNr.CompareTo(kommune.KomNr);
        }

        public bool IsValid
        {
            get { return KomNr != null && NrOk(KomNr.Trim()) && City != null && City.Length > 0; }
        }

        string IDataErrorInfo.Error
        {
            get { return IsValid ? null : "Illegal model object"; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get { return Validate(propertyName); }
        }

        private string Validate(string property)
        {
            if (property.Equals("KomNr")) return KomNr != null && NrOk(KomNr.Trim()) ? null : "Illegal kom_nr";
            if (property.Equals("Kommune")) return City != null && City.Length > 0 ? null : "Illegal City";
            return null;
        }

        private bool NrOk(string komNr)
        {
            if (komNr.Length != 3) return false;
            foreach (char c in komNr) if (c < '0' || c > '9') return false;
            return true;
        }

    }
}
