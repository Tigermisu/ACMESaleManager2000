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
using ACMESaleManager2000.ViewModels;

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
            services.AddTransient<IRepository<Item>, ItemRepository>();

            services.AddTransient<ISaleOrderRepository, SaleOrderRepository>();
            services.AddTransient<IRepository<SaleOrder>, SaleOrderRepository>();

            services.AddTransient<IPurchaseOrderRepository, PurchaseOrderRepository>();
            services.AddTransient<IRepository<PurchaseOrder>, PurchaseOrderRepository>();

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
                cfg.CreateMap<ItemEntity, Item>().MaxDepth(1);
                cfg.CreateMap<PurchaseOrderEntity, PurchaseOrder>().MaxDepth(1);
                cfg.CreateMap<SaleOrderEntity, SaleOrder>().MaxDepth(1);

                cfg.CreateMap<Item, ItemEntity>().MaxDepth(1);
                cfg.CreateMap<PurchaseOrder, PurchaseOrderEntity>().MaxDepth(1);
                cfg.CreateMap<SaleOrder, SaleOrderEntity>().MaxDepth(1);

                cfg.CreateMap<SaleOrderViewModel, SaleOrder>().MaxDepth(1);
                cfg.CreateMap<PurchaseOrderViewModel, PurchaseOrder>().MaxDepth(1);
                cfg.CreateMap<ItemViewModel, Item>().MaxDepth(1);

                cfg.CreateMap<SaleOrder, SaleOrderViewModel>().MaxDepth(1);
                cfg.CreateMap<PurchaseOrder, PurchaseOrderViewModel>().MaxDepth(1);
                cfg.CreateMap<Item, ItemViewModel>().MaxDepth(1);


                cfg.CreateMap<ItemPurchaseOrderEntity, ItemPurchaseOrder>().MaxDepth(1);
                cfg.CreateMap<ItemSaleOrderEntity, ItemSaleOrder>().MaxDepth(1);
                cfg.CreateMap<ItemPurchaseOrder, ItemPurchaseOrderViewModel>().MaxDepth(1);
                cfg.CreateMap<ItemSaleOrder, ItemSaleOrderViewModel>().MaxDepth(1);

                cfg.CreateMap<ItemPurchaseOrder, ItemPurchaseOrderEntity>().MaxDepth(1);
                cfg.CreateMap<ItemSaleOrder, ItemSaleOrderEntity>().MaxDepth(1);
                cfg.CreateMap<ItemPurchaseOrderViewModel, ItemPurchaseOrder>().MaxDepth(1);
                cfg.CreateMap<ItemSaleOrderViewModel, ItemSaleOrder>().MaxDepth(1);
            });
        }

        // Role creation from 
        // Temi Lajumoke @ https://stackoverflow.com/questions/42471866/how-to-create-roles-in-asp-net-core-and-assign-them-to-users
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var defaultUsersSection = Configuration.GetSection("DefaultUsers");
            string[] roleNames = { "Admin", "Supervisor", "User" };
            string[] users = { "admin", "supervisor", "user" };
            IdentityResult roleResult;

            foreach (string roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            foreach (string userName in users) {

                var _user = await UserManager.FindByEmailAsync(defaultUsersSection[userName]);

                if (_user == null)
                {
                    var newUser = new ApplicationUser
                    {

                        UserName = defaultUsersSection[userName],
                        Email = defaultUsersSection[userName],
                    };

                    string userPWD = defaultUsersSection["password"];

                    var createNewUser = await UserManager.CreateAsync(newUser, userPWD);
                    if (createNewUser.Succeeded)
                    {
                        string roleName = userName.First().ToString().ToUpper() + userName.Substring(1);
                        await UserManager.AddToRoleAsync(newUser, roleName);
                    }
                }
            }
        }
    }
}
