using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiEditor.Model;
using MauiEditor.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiEditor.ViewModel
{
    public partial class InsertViewModel : ObservableObject
    {
        public static DataRepository repository = [];

        public InsertViewModel()
        {
          
        }

        // Entry
        [ObservableProperty]
        string gruppe;

        [ObservableProperty]
        string year;

        [ObservableProperty]
        string komNr;

        [ObservableProperty]
        string num;


        [ICommand]
        void Create()
        {
           Data data = new Data("", KomNr, "", Gruppe, Year, Num);
            repository.Add(data);


            KomNr = string.Empty;
            Gruppe = string.Empty;
            Year = string.Empty;
            Num = string.Empty;
        }


        [ICommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
