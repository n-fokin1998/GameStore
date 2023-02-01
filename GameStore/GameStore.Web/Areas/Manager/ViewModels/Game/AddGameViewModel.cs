using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Areas.Manager.ViewModels
{
    public class AddGameViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Key", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public string Key { get; set; }

        [Display(Name = "Name", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public string NameEn { get; set; }

        [Display(Name = "Name", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public string NameRu { get; set; }

        [Display(Name = "Description", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public string DescriptionRu { get; set; }

        [Display(Name = "Description", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public string DescriptionEn { get; set; }

        [Display(Name = "Price", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public decimal Price { get; set; }

        [Display(Name = "UnitsInStock", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public short UnitsInStock { get; set; }

        [Display(Name = "PublishDate", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public DateTime PublishDate { get; set; }

        public DateTime UploadDate { get; set; }

        public int? PublisherId { get; set; }

        [Display(Name = "Discontinued", ResourceType = typeof(GlobalRes))]
        public bool Discontinued { get; set; }

        public string ImageReference { get; set; }

        public int[] Genres { get; set; }

        public int[] PlatformTypes { get; set; }

        public IEnumerable<SelectListItem> GenresList { get; set; }

        public IEnumerable<SelectListItem> PlatformTypesList { get; set; }

        public SelectList PublishersList { get; set; }
    }
}