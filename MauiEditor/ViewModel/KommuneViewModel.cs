using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiEditor.Model;
using MauiEditor.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiEditor.ViewModel
{
    public partial class KommuneViewModel : ObservableObject
    {
        private KommuneRepository repository = [];

        public KommuneViewModel()
        {
            Info = new ObservableCollection<Kommune>(repository);
        }

        [ObservableProperty]
        ObservableCollection<Kommune> info;


        [ObservableProperty]
        string komNr;

        [ObservableProperty]
        string city;


        [ICommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        [ICommand]
        void Clear()
        {
            
        }

        [ICommand]
        public void Search()
        {
            repository.Search(KomNr, City);
            Info = new ObservableCollection<Kommune>(repository);

            komNr = string.Empty;
            City = string.Empty;
        }

        [ICommand]
        public void Update()
        {
            repository.Update(KomNr, City);
        }

        [ICommand]
        public void Add()
        {
            repository.Add(KomNr, City);
        }
    }
}
