using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System.Net.Http;
using PrismDemo.Models;
using Newtonsoft.Json;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Linq;
using System.Windows.Input;
using PrismDemo.Services;
using Windows.UI.Popups;

namespace PrismDemo.ViewModels
{
    public class MainPageViewModel : ViewModel
    {
        #region declarations

      
        public DelegateCommand SearchCommand { get; private set; }
        public ICommand Selectionphoto { get; set; }

        #endregion

        #region Getter & setter

        public IncrementalLoadingNotificationsCollection _listPhoto;
        public IncrementalLoadingNotificationsCollection ListPhoto
        {
            get
            {
                return _listPhoto;
            }
            set
            {
                SetProperty(ref _listPhoto, value);
            }
        }

      


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





        private string _labelBar = "search";
        public string LabelBar
        {
            get { return _labelBar; }
            set
            {

                SetProperty(ref _labelBar, value);
            }
        }

        private SymbolIcon _iconBar = new SymbolIcon(Symbol.Find);
        public SymbolIcon IconBar
        {
            get { return _iconBar; }
            set
            {

                SetProperty(ref _iconBar, value);
            }
        }



        private Visibility _visibleProgress = Visibility.Visible;
         public Visibility VisibleProgress
        {
            get { return _visibleProgress; }
            set
            {

                SetProperty(ref _visibleProgress, value);
            }
        }

        private Visibility _visibleInfo = Visibility.Collapsed;
         public Visibility VisibleInfo
        {
            get { return _visibleInfo; }
            set
            {

                SetProperty(ref _visibleInfo, value);
            }
        }
        private Visibility _searchVisible = Visibility.Collapsed;
        public Visibility SearchVisible
        {
            get { return _searchVisible; }
            set
            {

                SetProperty(ref _searchVisible, value);
            }
        }

        private string _searchPhoto;
        public string SearchPhoto
        {
            get { return _searchPhoto; }
            set
            {
                SetProperty(ref _searchPhoto, value);
                LoadContents(_searchPhoto);
            }
        }

     

        #endregion

        private  void LoadContents(string _searchPhoto)
        {
            ListPhoto = new IncrementalLoadingNotificationsCollection(_servicesPhoto, _searchPhoto,this);
        }

        private readonly INavigationService _navigationService;
        private readonly IServicesPhoto _servicesPhoto;

        public MainPageViewModel(INavigationService navigationService,IServicesPhoto servicephoto)
        {
            _navigationService = navigationService;
            _servicesPhoto = servicephoto;

            Selectionphoto = new DelegateCommand<ItemClickEventArgs>(OnSelectionPhoto);


            SearchCommand = new DelegateCommand(() =>
            {
                switch (SearchVisible)
                {
                    case Visibility.Collapsed:
                        SearchVisible = Visibility.Visible;
                        LabelBar = "cancel";
                        IconBar = new SymbolIcon(Symbol.Cancel);
                        break;

                    case Visibility.Visible:
                          SearchVisible = Visibility.Collapsed;
                        LabelBar = "search";
                        IconBar = new SymbolIcon(Symbol.Find);
                        SearchPhoto = "";
                        break;

                    default:
                        break;
                }

            });
        }

        private void OnSelectionPhoto(ItemClickEventArgs obj)
        {
            if(obj.ClickedItem!=null)
            {
                Photo selectedphoto = obj.ClickedItem as Photo;
                _navigationService.Navigate("Pivot", selectedphoto.id);
            }
        }
      
        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {

            if (navigationMode == NavigationMode.Back)
                return;

            if (Statique.ConnectedToInternet())
            {
                ListPhoto = new IncrementalLoadingNotificationsCollection(_servicesPhoto,"",this);
            }
            else
            {
                MessageDialog msg = new MessageDialog("Please check your network!");
                await  msg.ShowAsync();
            }
            //if (Statique.ConnectedToInternet())
            //{


            //    Statique.listphotodesc = await _servicesPhoto.GetRecentPhoto();

            //    if (Statique.listphotodesc != null || Statique.listphotodesc.Count!=0)
            //    {

            //        Listphoto = null;
            //        Listphoto = Statique.listphotodesc;
            //        VisibleProgress = Visibility.Collapsed;
            //    }

            //    else
            //    {
            //        MessageDialog msg = new MessageDialog("An error has occured!");
            //        await msg.ShowAsync();
            //    }
            //}
            //else
            //{
            //    MessageDialog msg = new MessageDialog("Please check your network!");
            //    await  msg.ShowAsync();
            //}


         }

        

        }


    }


