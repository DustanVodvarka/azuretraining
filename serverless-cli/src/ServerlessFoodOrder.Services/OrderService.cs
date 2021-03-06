﻿using Microsoft.Azure.Documents;
using ServerlessFoodOrder.Data;
using ServerlessFoodOrder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ServerlessFoodOrder.Services
{
    public class OrderService
    {
        private readonly IServiceBus serviceBus;
        private DataRepository<Order> repo;
        private MenuService menuService;

        public OrderService(IConfigurationRoot config, IServiceBus serviceBus)
        {
            this.serviceBus = serviceBus;
            this.repo = new DataRepository<Order>(config);
            this.menuService = new MenuService(config);
        }

        public async Task<List<Order>> GetActiveOrders()
        {
            var orders = (await this.repo.GetItemsAsync(x => x.CompletedTime == null)).ToList();
            return orders;
        }

        public async Task<List<Order>> GetCompletedOrders()
        {
            var orders = (await this.repo.GetItemsAsync(x => x.CompletedTime != null)).ToList();
            return orders;
        }

        public async Task<Document> SaveOrder(Order order)
        {
            if (String.IsNullOrEmpty(order.Id))
            {
                var result = await this.repo.CreateItemAsync(order);

                var message = new OrderPlacedEvent()
                {
                    Name = order.Name,
                    OrderId = order.Id,
                    OrderItems = order.OrderItems
                        .Select(i => new OrderPlacedItem()
                        {
                            MenuItemId = i.MenuOption.Id,
                            MenuItemName = i.MenuOption.Name,
                            Quantity = i.Quantity
                        })
                        .ToArray()
                };
                await serviceBus.PublishAsync(message);

                return result;
            }

            return await this.repo.UpdateItemAsync(order.Id, order);
        }

        public async Task<Document> CompleteOrder(string id)
        {
            var order = await this.repo.GetItemAsync(id);
            order.CompletedTime = DateTime.UtcNow.ToString("o");
            return await this.SaveOrder(order);
        }
    }
}
