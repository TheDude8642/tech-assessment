using System;

namespace Models;
public class Order
{
    public Guid Id { get; set; }
    public string Customer { get; set; }
    public string Product { get; set; }
    public int Quantity { get; set; }
    public DateTime OrderDate { get; set; }
    public bool Canceled { get; set; }
}