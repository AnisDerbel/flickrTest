using PrismDemo.Models;
using PrismDemo.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using System.Linq;
using Windows.UI.ViewManagement;
using Windows.UI;
using PrismDemo.ViewModels;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Mvvm;
using Windows.UI.Xaml.Controls;

namespace PrismDemo
{
   public class IncrementalLoadingNotificationsCollection : ObservableCollection<Photo>, ISupportIncrementalLoading
{
    private MainPageViewModel _mainviewmodel;
    private IServicesPhoto _notificationService;
    private string _searchphoto;

    public IncrementalLoadingNotificationsCollection(IServicesPhoto notificationService, string searchphoto, MainPageViewModel mainviewmodel)
    {
         HasMoreItems = true;
        _notificationService = notificationService;
        _searchphoto = searchphoto;
        _mainviewmodel = mainviewmodel;
    }


    public bool HasMoreItems
    {
        get;
        private set;
    }

    public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
    {
        if(Statique.ConnectedToInternet())
        return InnerLoadMoreItemsAsync(count).AsAsyncOperation();
        else
        {
#if WINDOWS_PHONE_APP
        ShowStatusBar("Network error!");
#endif
            return null;
        }
    }
    private int _photoStartIndex = 1;
   
       
     

    private async Task<LoadMoreItemsResult> InnerLoadMoreItemsAsync(uint expectedCount)
    {

#if WINDOWS_PHONE_APP
        ShowStatusBar("Loading...");
#endif
        List<Photo> notifications=null;
        var actualCount = 1;

        _mainviewmodel.VisibleProgress = Visibility.Visible;
        try
        {
           if(_searchphoto=="")
           {
               notifications = await _notificationService.GetRecentPhoto(_photoStartIndex);

           }
           else
           {
               var list = Statique.listphotodesc.Where(x=> x.search == false).ToList();
               foreach (var item in list)
               {
                   Remove(item);
               }

               notifications = await _notificationService.SearchPhoto(_photoStartIndex,_searchphoto);
               _mainviewmodel.VisibleInfo = (notifications.Count == 0) ? Visibility.Visible : Visibility.Collapsed;

           }

        
          
        }
        catch (Exception)
        {
            HasMoreItems = false;
            throw;
        }

        if (notifications != null && notifications.Any())
        {
            foreach (var notification in notifications)
            {
   
                Add(notification);


            }
#if WINDOWS_PHONE_APP
        HideStatusBar();
#endif

            actualCount += notifications.Count;
            _photoStartIndex += (int)actualCount;


        }
        else
        {
            HasMoreItems = false;
        }

        _mainviewmodel.VisibleProgress = Visibility.Collapsed;




        return new LoadMoreItemsResult
        {
            Count = (uint)actualCount
        };

    }
#if WINDOWS_PHONE_APP
    StatusBarProgressIndicator progressbar;
    public async void ShowStatusBar(string text)
    {
        StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
        progressbar = statusBar.ProgressIndicator;
        progressbar.Text = text;
        statusBar.ForegroundColor = Colors.Black;
        await progressbar.ShowAsync();
        await statusBar.ShowAsync();
    }

    public async void HideStatusBar()
    {
        await progressbar.HideAsync();
    }
#endif

}



}
