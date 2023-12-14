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
        //IMemory memory = new FileMemory();
        //IMemory memory = new DBMemory();
        IMemory memory = new RamMemory();
        BindingContext = new ViewModel.ViewModel(memory);

    }

}


