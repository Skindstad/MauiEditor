using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiEditor.Model;
using MauiEditor.Repository;
using MauiEditor.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiEditor.ViewModel
{
   public partial class MainViewModel: ObservableObject
    {
        public static DataRepository repository = [];

        public MainViewModel()
        {
            DataList = new ObservableCollection<Data>(repository);
        }

        [ObservableProperty]
        ObservableCollection<Data> dataList;


            // Entry
            [ObservableProperty]
             string city;

             [ObservableProperty]
             string gruppe;

             [ObservableProperty]
             string year;


            [ObservableProperty]
            string dataId;

            [ObservableProperty]
            string komNr;

            [ObservableProperty]
            string num;

        
        [ICommand]
        void Search()
                 {
                    repository.Search(City, Gruppe, Year);
                    DataList = new ObservableCollection<Data>(repository);

                     City = string.Empty;
                     Gruppe = string.Empty;
                     Year = string.Empty;
                 }


        /*private bool CanSearch()
        {
          
            return City.Length > 0 ||
                Gruppe.Length > 0 || Year.Length > 0;
        }*/

        [ICommand]
        async Task Insert()
        {
            await Shell.Current.GoToAsync(nameof(InsertDataView));
        }

        [ICommand]
        async Task Kommune()
        {
            await Shell.Current.GoToAsync(nameof(KommuneManagerView));
        }

        [ICommand]
        async Task UpdateData()
        {
            await Shell.Current.GoToAsync(nameof(UpdateDataView));
        }

        [ICommand]
        void Clear() 
        {
            if (string.IsNullOrWhiteSpace(City) || string.IsNullOrWhiteSpace(Gruppe) || string.IsNullOrWhiteSpace(Year)) return;
            City = string.Empty;
            Gruppe = string.Empty;
            Year = string.Empty;
        }

        [ICommand]
        void Exit()
        {

        }


    }
}
