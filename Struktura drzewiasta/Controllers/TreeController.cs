using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Struktura_drzewiasta.Models;

namespace Struktura_drzewiasta.Controllers
{
    public class TreeController : Controller
    {
        private readonly TreeDbContext _context;

        public TreeController(TreeDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var nodes = await _context
                .Nodes
                .OrderBy(n => n.ParentId)
                .ToListAsync();
            nodes.Insert(0,nodes[^1]);
            nodes.RemoveAt(nodes.Count-1);
            return View(nodes);
        }
    }
}
