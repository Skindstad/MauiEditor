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
        private Kommune model = new Kommune();

        public KommuneViewModel()
        {
            repository.RepositoryChanged += ModelChanged;
            Search();
            Info = new ObservableCollection<Kommune>(repository);
        }

        [ObservableProperty]
        ObservableCollection<Kommune> info;


        [ObservableProperty]
        string komNr;

        [ObservableProperty]
        string city;


        public void ModelChanged(object sender, DbEventArgs e)
        {
            if (e.Operation != DbOperation.SELECT)
            {
                Clear();
            }
            Info = new ObservableCollection<Kommune>(repository);
        }

        [ICommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        public Kommune SelectedModel
        {
            get => model;
            set
            {
                model = value;
                OnPropertyChanged(nameof(KomNr));
                OnPropertyChanged(nameof(City));

                // Set the values in the Entry controls
                KomNr = model?.KomNr;
                City = model?.City;

                OnPropertyChanged(nameof(SelectedModel));
            }
        }

        [ICommand]
        void Clear()
        {
            KomNr = string.Empty;
            City = string.Empty;
            Search();
        }

        [ICommand]
        public void Search()
        {
            repository.Search(KomNr, City);
            Info = new ObservableCollection<Kommune>(repository);
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
