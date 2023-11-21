using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using Microsoft.Maui.HotReload;
using elme.ViewModel;



namespace elme;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
        BindingContext = new ViewModel.ViewModel();

    }

}


