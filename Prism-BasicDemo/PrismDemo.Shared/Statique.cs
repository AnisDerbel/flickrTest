using PrismDemo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;

namespace PrismDemo
{
   public static class Statique
    {
       public static List<Photo> listphotodesc = new List<Photo>();
       public static int selectedpos;
      
       internal static bool ConnectedToInternet()
       {
           try
           {
               ConnectionProfile connectionProfile = NetworkInformation.GetInternetConnectionProfile();
               if (connectionProfile == null)
                   return false;
               else
                   return connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
           }
           catch
           {
               return false;
           }
       }
    }
}
