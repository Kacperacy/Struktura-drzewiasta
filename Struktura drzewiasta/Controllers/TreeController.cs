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
            ViewData["Sort"] = HttpContext.Request.Query["sort"];

            var nodes = await _context
            .Nodes
            .OrderBy(n => n.Id)
            .ToListAsync();

            return View(nodes);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var node = await _context
                .Nodes
                .FirstOrDefaultAsync(n => n.Id == id);

            if (node == null)
            {
                return RedirectToAction("Index");
            }

            _context.Nodes.RemoveRange(node);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateNodeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var node = new Node()
            {
                Name = dto.Name
            };

            if (dto.ParentNode is null)
            {
                await _context.Nodes.AddAsync(node);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var parentNode = _context
                .Nodes
                .FirstOrDefault(n => n.Id == int.Parse(dto.ParentNode));

            if (parentNode == null)
            {
                return NotFound();
            }

            parentNode.Children.Add(node);
            await _context.Nodes.AddAsync(node);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private async Task<bool> CheckIfPossible(Node node, Node targetNode)
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
            return Json(!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(node));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditNodeDto dto)
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
                    return RedirectToAction("Index");
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
