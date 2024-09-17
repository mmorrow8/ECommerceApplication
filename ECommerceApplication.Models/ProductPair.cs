using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceApplication.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ECommerceApplication.Models;
public class ProductPairs
{
    public Dictionary<(string product1, string product2), int> pairList = new Dictionary<(string, string), int>();

    public void Add(string product1, string product2)
    {
        var key = (product1, product2);

        if (pairList.ContainsKey(key))
        {
            pairList[key]++;
        }
        else
        {
            pairList[key] = 1;
        }
    }
}

public class PairAnalysis
{
    public List<string> products { get; set; } = new List<string>();
    public int frequency { get; set; }

    public PairAnalysis(string product1, string product2, int count)
    {
        products.Add(product1);
        products.Add(product2);
        frequency = count;
    }
}
