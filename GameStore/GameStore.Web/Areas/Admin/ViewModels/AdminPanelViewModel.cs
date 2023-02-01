using System.Collections.Generic;
using GameStore.BusinessLogicLayer.DTO;

namespace GameStore.Web.Areas.Admin.ViewModels
{
    public class AdminPanelViewModel
    {
        public IEnumerable<UserDTO> Users { get; set; }

        public IEnumerable<RoleDTO> Roles { get; set; }
    }
}