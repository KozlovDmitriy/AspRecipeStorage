using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity.Migrations;

namespace AspRecipeStorage.Models
{
    public partial class ApplicationDbContext 
    {
        public void Seed()
        {
            this.Database.ExecuteSqlCommand("DELETE FROM [RecipeStep]");
            this.Database.ExecuteSqlCommand("DELETE FROM [Recipe]");
            this.Database.ExecuteSqlCommand("DELETE FROM [UserUserRole]");
            this.Database.ExecuteSqlCommand("DELETE FROM [Users]");
            this.Database.ExecuteSqlCommand("DELETE FROM [UserRoles]");
            this.Database.ExecuteSqlCommand("DELETE FROM [DishType]");

            var roles = new List<UserRole> {
                new UserRole { Id = 1, Name = "Admin" },
                new UserRole { Id = 2, Name = "User" }
            };
            this.UserRoles.AddRange(roles);

            this.DishType.AddRange(new List<DishType> {
                    new DishType { Name = "Пироги" },
                    new DishType { Name = "Блины" },
                    new DishType { Name = "Печенье" },
                    new DishType { Name = "Пончики" },
                    new DishType { Name = "Вафли" },
                    new DishType { Name = "Торты" },
                    new DishType { Name = "Круассаны" },
                    new DishType { Name = "Пирожные" },
                    new DishType { Name = "Пироги (сластенка)" },
                    new DishType { Name = "Пряники" },
                    new DishType { Name = "Блинчатые пироги и пирожки" },
                    new DishType { Name = "Оладьи" },
                    new DishType { Name = "Пончики" },
                    new DishType { Name = "Хлеб" },
                    new DishType { Name = "Кулич" },
                    new DishType { Name = "Чебуреки" },
                    new DishType { Name = "Пицца" },
                    new DishType { Name = "Пирожки" },
                    new DishType { Name = "Булочки" },
                    new DishType { Name = "Кексы" },
                    new DishType { Name = "Начинки" },
                    new DishType { Name = "Тесто" },
                    new DishType { Name = "Хачапури" },
                    new DishType { Name = "Ватрушки" }
                }
            );

            var user = new User
            {
                Email = "admin@admin.admin",
                UserName = "admin@admin.admin",
                PasswordHash = "AERPoxTkc6T4usk9X+rF2gzUfBOlsjmmcbNENT84uVljw87kItnCao6didax+mT/Lw==",
                SecurityStamp = "028213dc-181b-4c58-b42e-293512414225",
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                Roles = roles.Take(1).ToList()
            };

            this.Users.Add(user);
  
            this.SaveChanges();
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}