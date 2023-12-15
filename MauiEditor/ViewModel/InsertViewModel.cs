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
        public static KeynummerRepository keynummerRepository = [];
        public static KommuneRepository kommuneRepository = [];
        public static AarstalRepository yearRepository = [];

        public InsertViewModel()
        {
          LoadCitys();
          LoadGruppes();
          LoadYears();
        }

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
        void Create()
        {
           Data data = new Data("", "", selectedKommune, selectedGruppe, selectedYear, Num);
            repository.Add(data);


            //KomNr = string.Empty;
            //Gruppe = string.Empty;
            //Year = string.Empty;
            Num = string.Empty;
        }


        [ICommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
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
