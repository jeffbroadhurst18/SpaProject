using Microsoft.AspNetCore.Mvc;
using SpaProject.Data.Items;
using System.Collections.Generic;

namespace SpaProject.Data
{
	public interface ISpaRepository
    {
		IEnumerable<Product> GetAllProducts();
		Product GetProductById(int id);
		IEnumerable<Product> GetProductByCategory(string category);
		IEnumerable<Order> GetOrders(bool includeItems);
		IEnumerable<Order> GetOrdersById(int id);
		void AddOrder(Order order);
		bool SaveAll();
		IEnumerable<Order> GetOrdersByUser(string user);
		int GetNextOrderId();
		IEnumerable<OrderItem> GetOrderItems(int id);
		Order UpdateOrder(Order order);
		StoreUser GetLastUser();
		Address GetAddress(string user);
	}
}
