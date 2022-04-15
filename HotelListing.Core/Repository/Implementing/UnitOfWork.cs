using HotelListing.Core.Repository.Interfaces;
using HotelListing.Data;
using System;
using System.Threading.Tasks;

namespace HotelListing.Core.Repository.Implementing
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
        private IGenericRepository<Country> counties;
        private IGenericRepository<Hotel> hotels;

        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
        }

        public IGenericRepository<Country> Counties =>
            counties ??= new GenericRepository<Country>(context);

        public IGenericRepository<Hotel> Hotels =>
            hotels ??= new GenericRepository<Hotel>(context);

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}