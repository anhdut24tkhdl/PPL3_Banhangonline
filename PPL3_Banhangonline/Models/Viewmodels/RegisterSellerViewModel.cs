using System.ComponentModel.DataAnnotations;

namespace PPL3_Banhangonline.Models.Viewmodels
{
    public class RegisterSellerViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên cửa hàng")]
        public string ShopName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
}
}