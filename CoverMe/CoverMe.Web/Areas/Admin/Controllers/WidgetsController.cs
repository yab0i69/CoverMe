using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoverCore.BreadCrumbs.Services;
using RoverCore.Datatables.DTOs;
using RoverCore.Datatables.Extensions;
using CoverMe.Web.Controllers;
using CoverMe.Infrastructure.Common.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CoverMe.Domain.Entities;
using CoverMe.Infrastructure.Persistence.DbContexts;

namespace CoverMe.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class WidgetsController : BaseController<WidgetsController>
{
    public class WidgetIndexViewModel
    {
        [Key]
        public int WidgetId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Available { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
    }

    private const string createBindingFields = "WidgetId,Name,Description,Available,Count,Price";
    private const string editBindingFields = "WidgetId,Name,Description,Available,Count,Price";
    private const string areaTitle = "Admin";

    private readonly ApplicationDbContext _context;

    public WidgetsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Admin/Widgets
    public IActionResult Index()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .Then("Manage Widget");

        // Fetch descriptive data from the index dto to build the datatables index
        var metadata = DatatableExtensions.GetDtMetadata<WidgetIndexViewModel>();

        return View(metadata);
    }

    // GET: Admin/Widgets/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage Widget", "Index", "Widgets", new { Area = "Admin" })
            .Then("Widget Details");

        if (id == null)
        {
            return NotFound();
        }

        var widget = await _context.Widget
            .FirstOrDefaultAsync(m => m.WidgetId == id);
        if (widget == null)
        {
            return NotFound();
        }

        return View(widget);
    }

    // GET: Admin/Widgets/Create
    public IActionResult Create()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage Widget", "Index", "Widgets", new { Area = "Admin" })
            .Then("Create Widget");

        return View();
    }

    // POST: Admin/Widgets/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(createBindingFields)] Widget widget)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Widget", "Index", "WidgetsController", new { Area = "Admin" })
        .Then("Create Widget");

        // Remove validation errors from fields that aren't in the binding field list
        ModelState.Scrub(createBindingFields);

        if (ModelState.IsValid)
        {
            _context.Add(widget);
            await _context.SaveChangesAsync();

            _toast.Success("Created successfully.");

            return RedirectToAction(nameof(Index));
        }
        return View(widget);
    }

    // GET: Admin/Widgets/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Widget", "Index", "Widgets", new { Area = "Admin" })
        .Then("Edit Widget");

        if (id == null)
        {
            return NotFound();
        }

        var widget = await _context.Widget.FindAsync(id);
        if (widget == null)
        {
            return NotFound();
        }


        return View(widget);
    }

    // POST: Admin/Widgets/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind(editBindingFields)] Widget widget)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Widget", "Index", "Widgets", new { Area = "Admin" })
        .Then("Edit Widget");

        if (id != widget.WidgetId)
        {
            return NotFound();
        }

        Widget model = await _context.Widget.FindAsync(id);

        model.Name = widget.Name;
        model.Description = widget.Description;
        model.Available = widget.Available;
        model.Count = widget.Count;
        model.Price = widget.Price;
        // Remove validation errors from fields that aren't in the binding field list
        ModelState.Scrub(editBindingFields);

        if (ModelState.IsValid)
        {
            try
            {
                await _context.SaveChangesAsync();
                _toast.Success("Updated successfully.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WidgetExists(widget.WidgetId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(widget);
    }

    // GET: Admin/Widgets/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Widget", "Index", "Widgets", new { Area = "Admin" })
        .Then("Delete Widget");

        if (id == null)
        {
            return NotFound();
        }

        var widget = await _context.Widget
            .FirstOrDefaultAsync(m => m.WidgetId == id);
        if (widget == null)
        {
            return NotFound();
        }

        return View(widget);
    }

    // POST: Admin/Widgets/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var widget = await _context.Widget.FindAsync(id);
        _context.Widget.Remove(widget);
        await _context.SaveChangesAsync();

        _toast.Success("Widget deleted successfully");

        return RedirectToAction(nameof(Index));
    }

    private bool WidgetExists(int id)
    {
        return _context.Widget.Any(e => e.WidgetId == id);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GetWidget(DtRequest request)
    {
        try
        {
            var query = _context.Widget;
            var jsonData = await query.GetDatatableResponseAsync<Widget, WidgetIndexViewModel>(request);

            return Ok(jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating Widget index json");
        }

        return StatusCode(500);
    }

}

