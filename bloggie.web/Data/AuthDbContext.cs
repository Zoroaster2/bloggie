using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace bloggie.web.Data
{

    //: means inharite from and IdentityDbContext is package of using here
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Seed Roles (User, Admin, superAdmin)
            var adminRoleId = "b2f6b74d-25ca-4e3c-bfb0-2725b6a6e529";
            var superAdminRoleId = "656dad3a-f73b-4725-b7ff-ff64e3ae3efc";
            var userRoleId = "94c52faf-a6c3-45a3-b49f-8787d59c812b";

            var roles = new List<IdentityRole>
            {
                //Here We Created the Users
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId

                },
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
            //Seed SuperAdminUser
            var superAdminId = "0c007db2-dbe5-42d6-b8b4-2a7a334a890c";
            //Here we Give the super admin the accsess that needs
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@bloggie.com",
                Email = "superadmin@bloggie.com",
                NormalizedEmail = "superadmin@bloggie.com".ToUpper(),
                NormalizedUserName = "superadmin@bloggie.com".ToUpper(),
                Id = superAdminId
            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword
                (superAdminUser, "superadmin@123");
            builder.Entity<IdentityUser>().HasData(superAdminUser);
            //Add All Roles To SuperAdminUser
            var superAdminroles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminId
                },
                   new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId
                },
                      new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminId
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(superAdminroles);
        }
    }
}



//Console.WriteLine(Guid.NewGuid()); whrit this in c# interactive to creat new Guid for Id
//for migraton when we have one db is == ""Add migration"Name of Migration""
// for migration for identity becuse we have two DbContex We Must Use this Command == "Add migration"Name of Migration" - Context"AuthDbContext"" and then "Update Database" -Context "AuthdbContext"!
