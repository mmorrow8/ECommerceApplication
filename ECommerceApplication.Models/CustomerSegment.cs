using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerceApplication.Models;
public class CustomerSegment
{
    public string customerId { get; set; }
    
    [JsonIgnore]
    public double amount { get; set; }

    public string segment 
    {
        get
        {
            string returnValue;

            if (amount < 5000) 
                returnValue = "Bronze";
            else if (amount >= 10000)
                returnValue = "Gold";
            else
                returnValue = "Silver";

            return returnValue;
        }
    }
}
