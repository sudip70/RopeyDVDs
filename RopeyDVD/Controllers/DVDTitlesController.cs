#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RopeyDVD.Data;
using RopeyDVD.Models;

namespace RopeyDVD.Controllers
{
    public class DVDTitlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DVDTitlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DVDTitles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DVDTitles.Include(d => d.DVDCategory).Include(d => d.Producer).Include(d => d.Studio);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DVDTitles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dVDTitle = await _context.DVDTitles
                .Include(d => d.DVDCategory)
                .Include(d => d.Producer)
                .Include(d => d.Studio)
                .FirstOrDefaultAsync(m => m.DVDNumber == id);
            if (dVDTitle == null)
            {
                return NotFound();
            }

            return View(dVDTitle);
        }

        // GET: DVDTitles/Create
        public IActionResult Create()
        {
            ViewData["CategoryNumber"] = new SelectList(_context.DVDCategories, "CategoryNumber", "CategoryNumber");
            ViewData["ProducerNumber"] = new SelectList(_context.Producers, "ProducerNumber", "ProducerNumber");
            ViewData["StudioNumber"] = new SelectList(_context.Studios, "StudioNumber", "StudioNumber");
            return View();
        }

        // POST: DVDTitles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DVDNumber,DVDTitles,DateReleased,StandardCharge,PenaltyCharge,CategoryNumber,StudioNumber,ProducerNumber")] DVDTitle dVDTitle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dVDTitle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryNumber"] = new SelectList(_context.DVDCategories, "CategoryNumber", "CategoryNumber", dVDTitle.CategoryNumber);
            ViewData["ProducerNumber"] = new SelectList(_context.Producers, "ProducerNumber", "ProducerNumber", dVDTitle.ProducerNumber);
            ViewData["StudioNumber"] = new SelectList(_context.Studios, "StudioNumber", "StudioNumber", dVDTitle.StudioNumber);
            return View(dVDTitle);
        }

        // GET: DVDTitles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dVDTitle = await _context.DVDTitles.FindAsync(id);
            if (dVDTitle == null)
            {
                return NotFound();
            }
            ViewData["CategoryNumber"] = new SelectList(_context.DVDCategories, "CategoryNumber", "CategoryNumber", dVDTitle.CategoryNumber);
            ViewData["ProducerNumber"] = new SelectList(_context.Producers, "ProducerNumber", "ProducerNumber", dVDTitle.ProducerNumber);
            ViewData["StudioNumber"] = new SelectList(_context.Studios, "StudioNumber", "StudioNumber", dVDTitle.StudioNumber);
            return View(dVDTitle);
        }

        // POST: DVDTitles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DVDNumber,DVDTitles,DateReleased,StandardCharge,PenaltyCharge,CategoryNumber,StudioNumber,ProducerNumber")] DVDTitle dVDTitle)
        {
            if (id != dVDTitle.DVDNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dVDTitle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DVDTitleExists(dVDTitle.DVDNumber))
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
            ViewData["CategoryNumber"] = new SelectList(_context.DVDCategories, "CategoryNumber", "CategoryNumber", dVDTitle.CategoryNumber);
            ViewData["ProducerNumber"] = new SelectList(_context.Producers, "ProducerNumber", "ProducerNumber", dVDTitle.ProducerNumber);
            ViewData["StudioNumber"] = new SelectList(_context.Studios, "StudioNumber", "StudioNumber", dVDTitle.StudioNumber);
            return View(dVDTitle);
        }

        // GET: DVDTitles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dVDTitle = await _context.DVDTitles
                .Include(d => d.DVDCategory)
                .Include(d => d.Producer)
                .Include(d => d.Studio)
                .FirstOrDefaultAsync(m => m.DVDNumber == id);
            if (dVDTitle == null)
            {
                return NotFound();
            }

            return View(dVDTitle);
        }

        // POST: DVDTitles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dVDTitle = await _context.DVDTitles.FindAsync(id);
            _context.DVDTitles.Remove(dVDTitle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DVDTitleExists(int id)
        {
            return _context.DVDTitles.Any(e => e.DVDNumber == id);
        }
        public async Task<IActionResult> DVDDetailsIndex()
        {
            var data = from DVDTitle in _context.DVDTitles
                       join DVDCategory in _context.DVDCategories on DVDTitle.CategoryNumber equals DVDCategory.CategoryNumber
                       join Studio in _context.Studios on DVDTitle.StudioNumber equals Studio.StudioNumber
                       orderby DVDTitle.DateReleased
                       select new
                       {
                           Title = DVDTitle.DVDTitles,
                           Category = DVDCategory.CategoryDescription,
                           Studio = Studio.StudioName,
                           Producer = DVDTitle.Producer.ProducerName,
                           Cast = from casts in DVDTitle.CastMember
                                  join actors in _context.Actors on casts.ActorNumber equals actors.ActorNumber
                                  group actors by new { casts.DVDNumber } into g
                                  select
                                    string.Join(",", g.OrderBy(c => c.ActorSurName).Select(x => (x.ActorFirstName + "" + x.ActorFirstName))),
                           Release = DVDTitle.DateReleased.ToString("dd MM yyyy"),
                       };
            data.OrderBy(c => c.Cast);
            return View(data);
        }
        public async Task<IActionResult> SelectActors(Actor actors)
        {
            ViewData["ActorSurName"] = new SelectList(_context.Set<Actor>(), "ActorSurName", "ActorSurName", actors.ActorSurName);
            return View();
        }
        public async Task<IActionResult> ShowDVDsofActors()
        {
            string actorName = Request.Form["actorList"].ToString();
            var data = from CastMember in _context.CastMembers
                       join Actor in _context.Actors on CastMember.ActorNumber equals Actor.ActorNumber
                       where Actor.ActorSurName == actorName
                       join DVDTitle in _context.DVDTitles
                        on CastMember.DVDNumber equals DVDTitle.DVDNumber
                       select new
                       {
                           Title = DVDTitle.DVDTitles,
                           Cast = from casts in DVDTitle.CastMember
                                  join Actor in _context.Actors on casts.ActorNumber equals Actor.ActorNumber
                                  group Actor by new { casts.DVDNumber } into g
                                  select
                                    String.Join(",", g.OrderBy(c => c.ActorNumber).Select(x => (x.ActorFirstName + "" + x.ActorSurName))),

                       };
            return View(data);
        }
        public async Task<IActionResult> ShowDVDCopiesofActors()
        {
            string actorName = Request.Form["actorList"].ToString();
            var data = from CastMember in _context.CastMembers
                       join Actor in _context.Actors on CastMember.ActorNumber equals Actor.ActorNumber
                       where Actor.ActorSurName == actorName
                       join DVDTitle in _context.DVDTitles on CastMember.DVDNumber equals DVDTitle.DVDNumber
                       select new
                       {
                           Title = DVDTitle.DVDTitles,
                           Cast = from casts in DVDTitle.CastMember
                                  join Actor in _context.Actors on casts.ActorNumber equals Actor.ActorNumber
                                  group Actor by new { casts.DVDNumber } into g
                                  select
                                  String.Join(",", g.OrderBy(c => c.ActorSurName).Select(x => (x.ActorFirstName + " " + x.ActorSurName))),
                           NumberOfCopies = (from DVDCopy in _context.DVDCopies
                                             join dt in _context.DVDTitles on DVDCopy.DVDNumber equals dt.DVDNumber
                                             join cm in _context.CastMembers on dt.DVDNumber equals cm.DVDNumber
                                             join a in _context.Actors on cm.ActorNumber equals a.ActorNumber
                                             where a.ActorSurName == actorName
                                             join l in _context.Loans on DVDCopy.CopyNumber equals l.CopyNumber
                                             where l.DateReturned == null
                                             select DVDCopy
                                                    ).Count()
                       };
            return View(data);

        }
        public async Task<IActionResult> DVDsNotLoaned()
        {
            var differenceDate = DateTime.Now.AddDays(-31);
            var loandedCopyIn31Days = (from loan in _context.Loans
                                       where loan.DateOut >= differenceDate
                                       select loan.CopyNumber).Distinct();
            var notloanedCopyDVD = (from copy in _context.DVDCopies
                                    join DVDTitle in _context.DVDTitles on copy.DVDNumber equals DVDTitle.DVDNumber
                                    join loan in _context.Loans on copy.CopyNumber equals loan.CopyNumber
                                    where !(loandedCopyIn31Days).Contains(copy.CopyNumber)
                                    select new
                                    {
                                        CopyNumber = copy.CopyNumber,
                                        Title = DVDTitle.DVDTitles,
                                        Loans = loan.DateOut,
                                    });
            return View(notloanedCopyDVD);
        }

    }
}
