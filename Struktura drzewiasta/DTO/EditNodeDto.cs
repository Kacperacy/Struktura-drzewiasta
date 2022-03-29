using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Struktura_drzewiasta.DTO
{
    public class EditNodeDto
    {
        [Required]
        public int SelectedNodeId { get; set; }
        [Remote(action:"VerifyEdit", controller: "Tree", AdditionalFields = nameof(TargetNode))]
        public string? NewName { get; set; }
        [Remote(action: "VerifyEdit", controller: "Tree", AdditionalFields = nameof(NewName))]
        public string? TargetNode { get; set; }
    }
}
