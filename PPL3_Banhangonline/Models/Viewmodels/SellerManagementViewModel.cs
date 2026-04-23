using PPL3_Banhangonline.Models;

public class SellerManagementViewModel
{
    public List<Order> RegularOrders { get; set; } = new List<Order>();
    public List<RescueRegistration> RescueOrders { get; set; } = new List<RescueRegistration>();
}