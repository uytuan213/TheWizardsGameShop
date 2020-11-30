using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheWizardsGameShop.Models
{
    public class TestGame
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameId { get; set; }

        [Display(Name = "Status")]
        public string GameStatusCode { get; set; }

        [Display(Name = "Category")]
        [Required]
        public short GameCategoryId { get; set; }

        [Display(Name = "Platform")]
        [Required]
        public short GamePlatformId { get; set; }

        [Display(Name = "Name")]
        [Required]
        public string GameName { get; set; }

        [Display(Name = "Description")]
        public string GameDescription { get; set; }

        [Display(Name = "Price")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$",
            ErrorMessage = "Must be numeric with optional two decimal places")]
        public decimal GamePrice { get; set; }

        [Display(Name = "Quantity")]
        [RegularExpression(@"^\d*$",
            ErrorMessage = "Must be numeric")]
        public short GameQty { get; set; }

        [Display(Name = "Path")]
        public string GameDigitalPath { get; set; }

        [Display(Name = "Category")]
        public virtual GameCategory GameCategory { get; set; }

        [Display(Name = "Platform")]
        public virtual Platform GamePlatform { get; set; }

        [Display(Name = "Status")]
        public virtual GameStatus GameStatusCodeNavigation { get; set; }
    }
}
