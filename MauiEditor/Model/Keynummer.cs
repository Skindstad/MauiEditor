using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiEditor.Model
{
    public class Keynummer : IDataErrorInfo, IComparable<Keynummer>
    {
        public string KeyId { get; set; } // ID
        public string Gruppe { get; set; } // Nøgletal


        public Keynummer()
        {
            KeyId = "";
            Gruppe = "";
        }

        public Keynummer(string id, string gruppe)
        {
            ;
            KeyId = id;
            Gruppe = gruppe;
        }

        public override bool Equals(object obj)
        {
            try
            {
                Keynummer keynummer = (Keynummer)obj;
                return KeyId.Equals(keynummer.KeyId);
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return KeyId.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} ", KeyId);
        }

        // Implementerer ordning af objekter, så der alene sammenlignes på postnummer.
        public int CompareTo(Keynummer keynummer)
        {
            return KeyId.CompareTo(keynummer.KeyId);
        }

        public bool IsValid
        {
            get { return Gruppe != null && Gruppe.Length > 0; }
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
            if (property.Equals("Gruppe")) return Gruppe != null && Gruppe.Length > 0 ? null : "Illegal gruppe";
            return null;
        }
    }
}
