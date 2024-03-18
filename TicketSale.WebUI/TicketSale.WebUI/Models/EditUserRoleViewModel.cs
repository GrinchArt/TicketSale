using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketSale.WebUI.Models
{
    public class EditUserRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public string SelectedRole { get; set; }
    }
}
