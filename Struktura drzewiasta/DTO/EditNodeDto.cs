using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Struktura_drzewiasta.Models;

namespace Struktura_drzewiasta.DTO
{
    public class EditNodeDto
    {
        [Required]
        public int? SelectedNodeId { get; set; }
        [Display(Name = "Name")]
        public string? NewName { get; set; }
        [Display(Name = "Parent")]
        public string? TargetNode { get; set; }
        public List<Node>? Nodes { get; set; }
    }
}