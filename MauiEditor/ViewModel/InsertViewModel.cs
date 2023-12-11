﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiEditor.ViewModel
{
    public partial class InsertViewModel : ObservableObject
    {
        [ICommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
