using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
                .OrderBy(n => n.Id)
                .ToListAsync();
            return View(nodes);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var node = _context
                .Nodes
                .Where(n => n.Id == id);

            if (node == null)
            {
                return NotFound();
            }

            _context.Nodes.RemoveRange(node);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string? name, string? parent)
        {
            var isRoot = _context
                .Nodes
                .Any();

            if (name is null || (parent is null && isRoot))
            {
                return BadRequest();
            }

            if (parent is null)
            {
                var root = new Node()
                {
                    Name = name
                };

                await _context.Nodes.AddAsync(root);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var parentId = int.Parse(parent.Split(".")[0]);

            var node = new Node()
            {
                Name = name
            };

            var parentNode = _context
                .Nodes
                .FirstOrDefault(n => n.Id == parentId);

            if (parentNode == null)
            {
                return NotFound();
            }

            parentNode.Children.Add(node);
            await _context.Nodes.AddAsync(node);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, string? name, string? parent)
        {
            if (id is null)
            {
                return BadRequest();
            }

            if (name is not null)
            {
                var node = await _context
                    .Nodes
                    .FirstOrDefaultAsync(n => n.Id == id);

                if (node == null)
                {
                    return NotFound();
                }

                node.Name = name;

                await _context.SaveChangesAsync();
            }

            if (parent is not null)
            {
                var node = await _context
                    .Nodes
                    .FirstOrDefaultAsync(n => n.Id == id);

                if (node == null)
                {
                    return NotFound();
                }

                var parentId = int.Parse(parent.Split(".")[0]);

                var parentNode = await _context
                    .Nodes
                    .FirstOrDefaultAsync(n => n.Id == parentId);

                if (parentNode == null)
                {
                    return NotFound();
                }

                if (node == parentNode)
                {
                    return RedirectToAction("Index");
                }

                parentNode.Children.Add(node);

                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
