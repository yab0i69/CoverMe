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

namespace CoverMe.Web.Areas.Dashboard.Controllers;

[Area("Dashboard")]
[Authorize(Roles = "Admin")]
public class CoverageRequestsController : BaseController<CoverageRequestsController>
{
	public class CoverageRequestIndexViewModel 
	{
		[Key]            
	    public int Id { get; set; }
	    public string Firstname { get; set; }
	    public string Lastname { get; set; }
	    public DateTime Date { get; set; }
	    public bool Approval { get; set; }
	}

	private const string createBindingFields = "Id,Firstname,Lastname,Date,Approval";
    private const string editBindingFields = "Id,Firstname,Lastname,Date,Approval";
    private const string areaTitle = "Dashboard";

    private readonly ApplicationDbContext _context;

    public CoverageRequestsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Dashboard/CoverageRequests
    public IActionResult Index()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
			.Then("Manage CoverageRequest");       
		
		// Fetch descriptive data from the index dto to build the datatables index
		var metadata = DatatableExtensions.GetDtMetadata<CoverageRequestIndexViewModel>();
		
		return View(metadata);
   }

    // GET: Dashboard/CoverageRequests/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage CoverageRequest", "Index", "CoverageRequests", new { Area = "Dashboard" })
            .Then("CoverageRequest Details");            

        if (id == null)
        {
            return NotFound();
        }

        var coverageRequest = await _context.CoverageRequest
            .FirstOrDefaultAsync(m => m.Id == id);
        if (coverageRequest == null)
        {
            return NotFound();
        }

        return View(coverageRequest);
    }

    // GET: Dashboard/CoverageRequests/Create
    public IActionResult Create()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage CoverageRequest", "Index", "CoverageRequests", new { Area = "Dashboard" })
            .Then("Create CoverageRequest");     

       return View();
	}

    // POST: Dashboard/CoverageRequests/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(createBindingFields)] CoverageRequest coverageRequest)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage CoverageRequest", "Index", "CoverageRequestsController", new { Area = "Dashboard" })
        .Then("Create CoverageRequest");     
        
        // Remove validation errors from fields that aren't in the binding field list
        ModelState.Scrub(createBindingFields);           

        if (ModelState.IsValid)
        {
            _context.Add(coverageRequest);
            await _context.SaveChangesAsync();
            
            _toast.Success("Created successfully.");
            
                return RedirectToAction(nameof(Index));
            }
        return View(coverageRequest);
    }

    // GET: Dashboard/CoverageRequests/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage CoverageRequest", "Index", "CoverageRequests", new { Area = "Dashboard" })
        .Then("Edit CoverageRequest");     

        if (id == null)
        {
            return NotFound();
        }

        var coverageRequest = await _context.CoverageRequest.FindAsync(id);
        if (coverageRequest == null)
        {
            return NotFound();
        }
        

        return View(coverageRequest);
    }

    // POST: Dashboard/CoverageRequests/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind(editBindingFields)] CoverageRequest coverageRequest)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage CoverageRequest", "Index", "CoverageRequests", new { Area = "Dashboard" })
        .Then("Edit CoverageRequest");  
    
        if (id != coverageRequest.Id)
        {
            return NotFound();
        }
        
        CoverageRequest model = await _context.CoverageRequest.FindAsync(id);

        model.Firstname = coverageRequest.Firstname;
        model.Lastname = coverageRequest.Lastname;
        model.Date = coverageRequest.Date;
        model.Approval = coverageRequest.Approval;
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
                if (!CoverageRequestExists(coverageRequest.Id))
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
        return View(coverageRequest);
    }

    // GET: Dashboard/CoverageRequests/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage CoverageRequest", "Index", "CoverageRequests", new { Area = "Dashboard" })
        .Then("Delete CoverageRequest");  

        if (id == null)
        {
            return NotFound();
        }

        var coverageRequest = await _context.CoverageRequest
            .FirstOrDefaultAsync(m => m.Id == id);
        if (coverageRequest == null)
        {
            return NotFound();
        }

        return View(coverageRequest);
    }

    // POST: Dashboard/CoverageRequests/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var coverageRequest = await _context.CoverageRequest.FindAsync(id);
        _context.CoverageRequest.Remove(coverageRequest);
        await _context.SaveChangesAsync();
        
        _toast.Success("CoverageRequest deleted successfully");

        return RedirectToAction(nameof(Index));
    }

    private bool CoverageRequestExists(int id)
    {
        return _context.CoverageRequest.Any(e => e.Id == id);
    }


	[HttpPost]
	[ValidateAntiForgeryToken]
    public async Task<IActionResult> GetCoverageRequest(DtRequest request)
    {
        try
		{
			var query = _context.CoverageRequest;
			var jsonData = await query.GetDatatableResponseAsync<CoverageRequest, CoverageRequestIndexViewModel>(request);

            return Ok(jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating CoverageRequest index json");
        }
        
        return StatusCode(500);
    }

}

