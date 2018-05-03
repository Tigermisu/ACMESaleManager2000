using ACMESaleManager2000.DataRepositories;
using ACMESaleManager2000.DomainObjects;
using ACMESaleManager2000.Models;
using ACMESaleManager2000.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DomainServices
{
    public class SaleOrderService : Service<SaleOrder>, ISaleOrderService
    {
        protected readonly IItemRepository _itemRepository;
        protected readonly IEmailSender _emailSender;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected const int INVENTORY_EMAIL_THRESHOLD = 5;

        public SaleOrderService(IRepository<SaleOrder> repository, 
            IItemRepository itemRepository, 
            IEmailSender emailSender,
            UserManager<ApplicationUser> userManager) : base(repository)
        {
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public void SubtractFromItemInventory(int itemId, int quantity)
        {
            _itemRepository.ModifyStock(itemId, -quantity);
        }

        public bool VerifyStock(SaleOrder saleOrder)
        {
            List<ItemSaleOrder> itemSales = saleOrder.SoldItems.ToList();
            List<Item> items = _itemRepository.GetItems(itemSales.Select(i => i.ItemEntityId).ToArray());

            foreach (ItemSaleOrder itemSale in itemSales) {
                Item item = items.Where(i => i.Id == itemSale.ItemEntityId).First();
                if (itemSale.SoldQuantity > item.QuantityAvailable) {
                    return false;
                }

                if (item.QuantityAvailable - itemSale.SoldQuantity < INVENTORY_EMAIL_THRESHOLD) {
                    SendLowStockWarningEmail(item).Wait();
                }
            }

            return true;
        }

        private async Task<bool> SendLowStockWarningEmail(Item item) {
            string message = $"{item.Name}'s stock has gone below the threshold of {INVENTORY_EMAIL_THRESHOLD}!";
            var adminEmails = await _userManager.GetUsersInRoleAsync("Admin");
            var supervisorEmails = await _userManager.GetUsersInRoleAsync("Supervisor");

            adminEmails.Union(supervisorEmails).ToList().ForEach(user => {
                _emailSender.SendEmailAsync(user.Email, "Low stock in item!", message).Wait();
            });

            return true;
        }
    }
}
