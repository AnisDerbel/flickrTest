using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using PrismDemo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using PrismDemo.Services;

namespace PrismDemo.ViewModels
{
   public class PivotPageViewModel : ViewModel
    {

       public ICommand PhotoChanged { get; set; }

        public DelegateCommand MapCommand { get; set; }
        public List<Photo> _listphoto;
        public List<Photo> Listphoto
        {
            get
            {
                return _listphoto;
            }
            set
            {
                SetProperty(ref _listphoto, value);
            }
        }

        public int _selectedPos;
        public int SelectedPos
        {
            get
            {
                return _selectedPos;
            }
            set
            {
                SetProperty(ref _selectedPos, value);
            }
        }


        private Visibility _visibleMap = Visibility.Collapsed;
        public Visibility VisibleMap
        {
            get { return _visibleMap; }
            set
            {

                SetProperty(ref _visibleMap, value);
            }
        }


         private readonly INavigationService _navigationService;
         private readonly IServicesPhoto _servicesPhoto;

         public PivotPageViewModel(INavigationService navigationService, IServicesPhoto servicephoto)
         {
             _navigationService = navigationService;
             _servicesPhoto = servicephoto;
             PhotoChanged = new DelegateCommand<SelectionChangedEventArgs>(OnPhotoChanged);

             MapCommand = new DelegateCommand(() =>
             {
                 selectedlocation.title = selectedphoto.title;
                 _navigationService.Navigate("Map", selectedlocation);
             });
         }

         private async void OnPhotoChanged(SelectionChangedEventArgs obj)
         {
             if (obj.AddedItems.Count > 0)
             {
                  selectedphoto = obj.AddedItems[0] as Photo;
                  Statique.selectedpos = Statique.listphotodesc.IndexOf(selectedphoto);
                
                  if ((selectedlocation = await _servicesPhoto.CheckforGeolocation(selectedphoto.id)) != null)
                 {
                     VisibleMap = Visibility.Visible;

                 }
                 else
                 {
                     VisibleMap = Visibility.Collapsed;

                 }
                  
             }
         }
         Photo selectedphoto;
         Location selectedlocation;
         public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
         {

             if (navigationMode == NavigationMode.Back)
             {
                 Listphoto = Statique.listphotodesc;
                 SelectedPos = Statique.selectedpos;
                 VisibleMap = Visibility.Visible;
                 return;
             }
                

               string id = navigationParameter as string;
               selectedlocation = new Location();
              selectedphoto = Statique.listphotodesc.Where(x => x.id == id).ToList().FirstOrDefault();

            //  Listphoto = null;
             Listphoto = Statique.listphotodesc;
           
             Statique.selectedpos = Listphoto.IndexOf(selectedphoto);
             SelectedPos = Statique.selectedpos;
             if ((selectedlocation = await _servicesPhoto.CheckforGeolocation(selectedphoto.id))!=null)
             {
                 VisibleMap = Visibility.Visible;
             }
             else
             {
                 VisibleMap = Visibility.Collapsed;

             }
          
          
         }

       
    

    }
}
