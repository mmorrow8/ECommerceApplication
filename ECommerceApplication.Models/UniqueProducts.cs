using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApplication.Models;
public class UniqueProducts
{
    public string productId { get; set; }
    public string productName { get; set; }
    public string category { get; set; }
}

public class UniqueProductsComparer : IEqualityComparer<UniqueProducts>
{
    public bool Equals(UniqueProducts x, UniqueProducts y)
    {
        return x.productId == y.productId && x.productName == y.productName && x.category == y.category;
    }

    public int GetHashCode(UniqueProducts obj)
    {
        return obj.productId.GetHashCode() ^ obj.productName.GetHashCode() ^ obj.category.GetHashCode();
    }
}
