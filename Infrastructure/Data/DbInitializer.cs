﻿using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if(context.Category.Any())
            {
                return; //DB has been seeded
            }
            var Category = new List<Category>
            {
                new Category{ Name = "Soup", DisplayOrder = 1},
                new Category{ Name = "Salad", DisplayOrder=2},
                new Category{ Name = "Entrees", DisplayOrder=3},
                new Category{ Name = "Dessert", DisplayOrder=4},
                new Category{ Name = "Beverages", DisplayOrder=5}
            };

            foreach (Category c in Category)
            {
                context.Category.Add(c);
            }
            context.SaveChanges();

            var foodtype = new List<FoodType>
            {
                new FoodType{ Name = "Beef"},
                new FoodType{ Name = "Chicken"},
                new FoodType{ Name = "Veggie"},
                new FoodType{ Name = "Sugar Free"},
                new FoodType{ Name = "Seafood"}
            };
            foreach (FoodType c in foodtype)
            {
                context.FoodType.Add(c);
            }
            context.SaveChanges();
        }
    }
}
