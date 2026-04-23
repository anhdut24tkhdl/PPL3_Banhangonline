using System.ComponentModel.DataAnnotations;

namespace PPL3_Banhangonline.Models.Viewmodels
{
    public class SellerProfileViewModel
    {
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string? Name { get; set; }

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string? Phone { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }

        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        [Display(Name = "Tuổi")]
        [Range(1, 120, ErrorMessage = "Tuổi không hợp lệ")]
        public int? Age { get; set; }
    }
}
