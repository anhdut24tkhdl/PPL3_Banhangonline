namespace PPL3_Banhangonline.Models.Viewmodels
{
    public class SellerProductManagementViewModel
    {
        public List<Product> RegularProducts { get; set; } = new List<Product>();
        public List<RescueCampaign> RescueCampaigns { get; set; } = new List<RescueCampaign>();
    }
}