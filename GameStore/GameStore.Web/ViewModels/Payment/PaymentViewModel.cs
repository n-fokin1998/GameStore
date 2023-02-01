using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GameStore.BusinessLogicLayer.Domain.Enums;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels
{
    public class PaymentViewModel
    {
        public int InvoiceNumber { get; set; }

        public string AccountNumber { get; set; }

        public List<OrderDetailsDTO> OrderDetails { get; set; }

        public DateTime Date { get; set; }

        public string Sum { get; set; }

        [Display(Name = "CardHolder", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public string CardHolder { get; set; }

        [Display(Name = "CardNumber", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public string CardNumber { get; set; }

        [Display(Name = "ExpiresDate", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public DateTime ExpiresDate { get; set; }

        [Display(Name = "SecureCode", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public short SecureCode { get; set; }

        public bool IsFailed { get; internal set; }

        public int Successed { get; internal set; }

        public int ShipperId { get; set; }

        public PaymentType PaymentType { get; set; }

        public IEnumerable<SelectListItem> ShippersList { get; set; }
    }
}