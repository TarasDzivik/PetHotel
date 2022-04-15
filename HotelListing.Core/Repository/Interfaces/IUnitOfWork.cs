using HotelListing.Data;
using System;
using System.Threading.Tasks;

namespace HotelListing.Core.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Country> Counties { get; }
        IGenericRepository<Hotel> Hotels { get; }
        Task Save();
    }
}