using SpaProject.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaProject.Data
{
    public interface ISpaRepository
    {
		IEnumerable<Product> GetAllProducts();
		Product GetProductById(int id);
		IEnumerable<Product> GetProductByCategory(string category);
		IEnumerable<Order> GetOrders(bool includeItems);
		IEnumerable<Order> GetOrdersById(int id);
	}
}
