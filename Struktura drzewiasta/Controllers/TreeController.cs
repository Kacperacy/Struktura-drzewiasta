using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Struktura_drzewiasta.DTO;
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

        async Task<bool> CheckIfPossible(Node node, Node targetNode)
        {
            var nodeWithChildren = await _context
                .Nodes
                .Include(n => n.Children)
                .FirstOrDefaultAsync(n => n == node);

            if (nodeWithChildren?.Children.FirstOrDefault() is null)
            {
                return true;
            }

            if (nodeWithChildren.Children.Contains(targetNode))
            {
                return false;
            }

            foreach (var child in nodeWithChildren.Children)
            {
                if (! await CheckIfPossible(child, targetNode))
                {
                    return false;
                }
            }

            return true;
        }

        public IActionResult VerifyEdit(string name, string node)
        {
            if (String.IsNullOrEmpty(name) && String.IsNullOrEmpty(node))
            {
                return Json(false);
            }

            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditFormDto dto)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            if (dto.NewName is not null)
            {
                var node = await _context
                    .Nodes
                    .FirstOrDefaultAsync(n => n.Id == dto.SelectedNodeId);

                if (node == null)
                {
                    return NotFound();
                }

                node.Name = dto.NewName;

                await _context.SaveChangesAsync();
            }

            if (dto.TargetNode is not null)
            {
                var node = await _context
                    .Nodes
                    .FirstOrDefaultAsync(n => n.Id == dto.SelectedNodeId);

                if (node == null)
                {
                    return NotFound();
                }

                var targetParentId = int.Parse(dto.TargetNode.Split(".")[0]);

                var targetParentNode = await _context
                    .Nodes
                    .FirstOrDefaultAsync(n => n.Id == targetParentId);

                if (targetParentNode == null)
                {
                    return NotFound();
                }

                if (node == targetParentNode)
                {
                    return RedirectToAction("Index");
                }

                if (! await CheckIfPossible(node, targetParentNode))
                {
                    return BadRequest();
                }

                targetParentNode.Children.Add(node);

                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
