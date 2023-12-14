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
        public static KeynummerRepository keynummerRepository = [];
        public static KommuneRepository kommuneRepository = [];
        public static AarstalRepository yearRepository = [];



        public MainViewModel()
        {
            DataList = new ObservableCollection<Data>(repository);
            LoadCitys();
            LoadGruppes();
            LoadYears();
        }

        [ObservableProperty]
        ObservableCollection<Data> dataList;


       // [ObservableProperty]
       // ObservableCollection<string> gruppeList;


        // Items
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

        // Picker
            [ObservableProperty]
            List<string> kommuneList;

            [ObservableProperty]
            string selectedKommune;

            [ObservableProperty]
            List<string> gruppeList;

            [ObservableProperty]
            string selectedGruppe;

            [ObservableProperty]
            List<string> yearList;

            [ObservableProperty]
            string selectedYear;



        [ICommand]
        void Search()
                 {
                    repository.Search(selectedKommune, selectedGruppe, selectedYear);
            DataList = new ObservableCollection<Data>(repository);

                 }

        

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

        private void LoadCitys()
        {
          kommuneList = kommuneRepository.GetCitys();
        }
        private void LoadGruppes()
        {
            gruppeList = keynummerRepository.GetGruppers();
        }
        private void LoadYears()
        {
            yearList = yearRepository.GetYears();
        }

    }
}
