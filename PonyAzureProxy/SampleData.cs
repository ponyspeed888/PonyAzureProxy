using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PonyAzureProxy
{
    public class SampleData
    {
        public static void Initialize(ApplicationDbContext context, UserManager<IdentityUser> _userManager)
        //public static void Initialize(IServiceProvider serviceProvider )
        {
            //var context = serviceProvider.GetService<ApplicationDbContext>();

            string[] roles = new string[] { "Owner", "Administrator", "Manager", "Editor", "Buyer", "Business", "Seller", "Subscriber" };

            foreach (string role in roles)
            {
                var roleStore = new RoleStore<IdentityRole>(context);

                if (!context.Roles.Any(r => r.Name == role))
                {
                    roleStore.CreateAsync(new IdentityRole(role));
                }
            }


            var uid = "e@a.com";
            var user = new IdentityUser
            {
                Email = uid,
                NormalizedEmail = uid.ToUpper(),
                UserName = uid,
                NormalizedUserName = uid.ToUpper(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };


            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<IdentityUser>();
                var hashed = password.HashPassword(user, "1");
                user.PasswordHash = hashed;

                var userStore = new UserStore<IdentityUser>(context);
                var result = userStore.CreateAsync(user);

            }

            //IdentityUser user1 = await _userManager.FindByEmailAsync(uid);
            //await _userManager.AddToRolesAsync(user1, new string[] { "Administrator" });


            ////IdentityUser user = await _userManager.FindByEmailAsync(email);
            _userManager.AddToRolesAsync(user, new string[] { "Administrator" });



            //AssignRoles(serviceProvider, user.Email, new string [] { "Administrator" });

            context.SaveChangesAsync();
        }

        //public static async Task<IdentityResult> AssignRoles(IServiceProvider services, string email, string[] roles)
        //{
        //    UserManager<IdentityUser> _userManager = services.GetService<UserManager<IdentityUser>>();
        //    IdentityUser user = await _userManager.FindByEmailAsync(email);
        //    var result = await _userManager.AddToRolesAsync(user, roles);

        //    return result;
        //}

    }
}