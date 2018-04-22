using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ACMESaleManager2000.Data;
using ACMESaleManager2000.Models;
using ACMESaleManager2000.Services;
using ACMESaleManager2000.DomainServices;
using ACMESaleManager2000.DataRepositories;
using ACMESaleManager2000.DomainObjects;
using ACMESaleManager2000.DataEntities;

namespace ACMESaleManager2000
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<ISaleOrderService, SaleOrderService>();
            services.AddTransient<IPurchaseOrderService, PurchaseOrderService>();

            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<ISaleOrderRepository, SaleOrderRepository>();
            services.AddTransient<IPurchaseOrderRepository, PurchaseOrderRepository>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            CreateRoles(serviceProvider).Wait();
            
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            AutoMapper.Mapper.Initialize(cfg => {
                cfg.CreateMap<ItemEntity, Item>();
                cfg.CreateMap<PurchaseOrderEntity, PurchaseOrder>();
                cfg.CreateMap<SaleOrderEntity, SaleOrder>();
            });
        }

        // Role creation from 
        // Temi Lajumoke @ https://stackoverflow.com/questions/42471866/how-to-create-roles-in-asp-net-core-and-assign-them-to-users
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Admin", "Supervisor", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var _user = await UserManager.FindByEmailAsync(Configuration["AdminUserEmail"]);           

            if (_user == null)
            {
                var poweruser = new ApplicationUser
                {

                    UserName = Configuration["AdminUserName"],
                    Email = Configuration["AdminUserEmail"],
                };

                string userPWD = Configuration["AdminUserPassword"];

                var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
                if (createPowerUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(poweruser, "Admin");
                }
            }
        }
    }
}
