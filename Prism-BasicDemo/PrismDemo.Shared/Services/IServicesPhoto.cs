using PrismDemo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrismDemo.Services
{
  public interface IServicesPhoto
    {
        Task<List<Photo>> GetRecentPhoto(int count);
        Task<List<Photo>> SearchPhoto(int count, string searchphoto);
        Task<Location> CheckforGeolocation(string photo_id);
    }
}
