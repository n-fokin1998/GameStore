using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "EmailErrorMessage")]
        public string Email { get; set; }

        [Display(Name = "Password", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsPersistent { get; set; }
    }
}