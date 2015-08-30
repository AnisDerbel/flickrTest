using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using PrismDemo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Navigation;

namespace PrismDemo.ViewModels
{
    public class MapPageViewModel : ViewModel
    {

         private readonly INavigationService _navigationService;
         public MapPageViewModel(INavigationService navigationService)
         {
             _navigationService = navigationService;

         }

         public  override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
         {
            
         }
    }
}
