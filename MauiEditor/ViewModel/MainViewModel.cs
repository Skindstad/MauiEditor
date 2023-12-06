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
using System.Windows.Input;

namespace MauiEditor.ViewModel
{
   public partial class MainViewModel: ObservableObject
    {

       public RelayCommand InsertDataCammand { get; private set; }
        public RelayCommand KommuneCommand { get; private set; }
        public RelayCommand ClearCommand { get; private set; }
        public RelayCommand ExitCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }

        //public static DataRepository repository = new DataRepository();
        public static DataRepository repository = new DataRepository();

        public MainViewModel()
        {
            SearchCommand = new Command(p => Search()/*, p => CanSearch()*/);
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

        private void Search()
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
    }
}
