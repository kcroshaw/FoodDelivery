using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Category> Category { get; }
        public IGenericRepository<FoodType> FoodType { get; }
        public IGenericRepository<MenuItem> MenuItem { get; }
        public IGenericRepository<ApplicationUser> ApplicationUser { get; }
        public IGenericRepository<ShoppingCart> ShoppingCart { get; }
        //Save changes to database source
        //This is a test comment
        int Commit();
        Task<int> CommitAsync();
    }
}
