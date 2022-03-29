﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Struktura_drzewiasta.DTO
{
    public class CreateNodeDto
    {
        [Required]
        [Display(Name ="Name")]
        [StringLength(64)]
        public string Name { get; set; }
        [Display(Name ="Parent")]
        public List<SelectListItem> ParentNode { get; set; }
    }
}