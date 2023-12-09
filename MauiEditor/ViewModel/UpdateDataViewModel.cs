using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiEditor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiEditor.ViewModel
{
    public partial class UpdateDataViewModel : ObservableObject
    {
        protected Data data;


        public customRelayCommand CloseCommand => new customRelayCommand(execute => OnClose(), canExecute => { return true; });
        public customRelayCommand UpdateCommand => new customRelayCommand(execute => Update());

        public UpdateDataViewModel() 
        {
            // CloseCommand = new RelayCommand(p => { if (CloseHandler != null) CloseHandler(); });


        }


        public string Aarstal
        {
            get; set;
            /*
            get { return data.Year; }
            set
            {
                if (!Aarstal.Equals(value))
                {
                    Aarstal = value;
                    OnPropertyChanged("Aarstal");
                }
            }
            */
                
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
            get;
            set;
        }
        public string KomId
        {
            get;set;
        }

        private void OnClose()
        {
            // object sender, EventArgs e
            // returns to previus page
            Shell.Current.GoToAsync("..");
        }

        private void Update()
        {

        }

    }

    public class customRelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler? CanExecuteChanged;

        public customRelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            execute(parameter);
        }
    }
}
