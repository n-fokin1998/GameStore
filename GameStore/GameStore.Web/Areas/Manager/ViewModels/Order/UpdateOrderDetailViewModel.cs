using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Areas.Manager.ViewModels.Order
{
    public class UpdateOrderDetailViewModel
    {
        public int Id { get; set; }

        public string GameName { get; set; }

        [Display(Name = "Quantity", ResourceType = typeof(GlobalRes))]
        public short Quantity { get; set; }

        public decimal Price { get; set; }

        public int GameId { get; set; }

        public int OrderId { get; set; }

        public float Discount { get; set; }
    }
}