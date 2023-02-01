using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Display(Name = "UserName", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "EmailErrorMessage")]
        public string Email { get; set; }

        [Display(Name = "Password", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "PasswordErrorMessage")]
        public string Password { get; set; }

        [Display(Name = "ConfirmPassword", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "ConfirmPasswordErrorMessage")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public int[] Roles { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}