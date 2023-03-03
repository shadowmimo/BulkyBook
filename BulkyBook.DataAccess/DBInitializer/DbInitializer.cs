﻿using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.DBInitializer{
    public class DbInitializer : IDbInitializer {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db) {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }
        public void Initialize() {
            // migrations if they are not applied
            try {
                if (_db.Database.GetPendingMigrations().Count()>0) { 
                    _db.Database.Migrate();
                }
            }catch(Exception ex) {

            }
            // create roles if they are not created

            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult()) {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Indi)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Comp)).GetAwaiter().GetResult();
                // if roles are not created, then we will create admin user as well

                _userManager.CreateAsync(new ApplicationUser {
                    UserName="admin@admin.com",
                    Email="admin@admin.com",
                    Name="Ulises Reyes",
                    PhoneNumber="555555555",
                    StreetAddress="test 123",
                    State="DN",
                    PostalCode="33333",
                    City="Mexico"

                },"Admin123@").GetAwaiter().GetResult();

                ApplicationUser user = _db.Users.FirstOrDefault(u=>u.Email == "admin@admin.com");
                _userManager.AddToRoleAsync(user,SD.Role_Admin).GetAwaiter().GetResult();


            }
            return;
        }
    }
}
