using CommunityToolkit.Mvvm.ComponentModel;
using MauiEditor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiEditor.ViewModel
{
    public partial class UpdateDataViewModel : ObservableObject
    {
        protected Aarstal AarstalDato;
        protected Kommune kommune;
        protected Keynummer noegleNummer;
        protected Data data;

        public UpdateDataViewModel() 
        {

        }


        public string Aarstal
        {
            get ; set;
        }
        public string Tal
        {
            get;
            set ;
        }
        public string Gruppe
        {
            get;set;
        }
        public string City
        {
            get;set;
        }
        public string KomId
        {
            get;set;
        }
    }
}
