#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RopeyDVD.Data;
using RopeyDVD.Models;

namespace RopeyDVD.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DVDCopiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DVDCopiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DVDCopies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DVDCopies.Include(d => d.DVDTitle);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DVDCopies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var dVDCopy = await _context.DVDCopies
                .Include(d => d.DVDTitle)
                .FirstOrDefaultAsync(m => m.CopyNumber == id);
            if (dVDCopy == null)
            {
                return NotFound();
            }
            var latestLoan = from loan in _context.Loans
                             join Member in _context.Members on loan.MemberNumber equals Member.MemberNumber
                             where loan.CopyNumber == id
                             orderby loan.DateOut descending
                             select new
                             {
                                 MemberName = Member.MemberFirstName + " " + Member.MemberLastName,
                                 Loan = loan
                             };
            var data = latestLoan.FirstOrDefault();
            if (data == null)
            {
                ViewData["loanData"] = data.Loan;
                ViewData["lastLoanMemberName"] = data.MemberName;
            }
            return View(dVDCopy);
        }

        // GET: DVDCopies/Create
        public IActionResult Create()
        {
            ViewData["DVDNumber"] = new SelectList(_context.DVDTitles, "DVDNumber", "DVDNumber");
            return View();
        }

        // POST: DVDCopies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CopyNumber,DatePurchased,DVDNumber")] DVDCopy dVDCopy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dVDCopy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DVDNumber"] = new SelectList(_context.DVDTitles, "DVDNumber", "DVDNumber", dVDCopy.DVDNumber);
            return View(dVDCopy);
        }

        // GET: DVDCopies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dVDCopy = await _context.DVDCopies.FindAsync(id);
            if (dVDCopy == null)
            {
                return NotFound();
            }
            ViewData["DVDNumber"] = new SelectList(_context.DVDTitles, "DVDNumber", "DVDNumber", dVDCopy.DVDNumber);
            return View(dVDCopy);
        }

        // POST: DVDCopies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CopyNumber,DatePurchased,DVDNumber")] DVDCopy dVDCopy)
        {
            if (id != dVDCopy.CopyNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dVDCopy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DVDCopyExists(dVDCopy.CopyNumber))
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
            ViewData["DVDNumber"] = new SelectList(_context.DVDTitles, "DVDNumber", "DVDNumber", dVDCopy.DVDNumber);
            return View(dVDCopy);
        }

        // GET: DVDCopies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dVDCopy = await _context.DVDCopies
                .Include(d => d.DVDTitle)
                .FirstOrDefaultAsync(m => m.CopyNumber == id);
            if (dVDCopy == null)
            {
                return NotFound();
            }

            return View(dVDCopy);
        }

        // POST: DVDCopies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dVDCopy = await _context.DVDCopies.FindAsync(id);
            _context.DVDCopies.Remove(dVDCopy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DVDCopyExists(int id)
        {
            return _context.DVDCopies.Any(e => e.CopyNumber == id);
        }
        public async Task<IActionResult> SelectDate(Member members)
        {
            //ViewData["SelectedDate"] = new SelectList(_context.Set<Member>(), "MemberFirstName", "MemberFirstName", members.MemberFirstName);
            return View();
        }
        //Shows older copies
        //10 - DVDCopies => OlderCopiesDVD
        public async Task<IActionResult> OlderCopiesDVD()
        {
            var loanedCopiesDVD = (from loan in _context.Loans
                                 where loan.DateReturned == null
                                 select loan.CopyNumber).Distinct();
            var notloanedCopiesDVD = (from copy in _context.DVDCopies
                                    join DVDTitle in _context.DVDTitles on copy.DVDNumber equals DVDTitle.DVDNumber
                                    where !(loanedCopiesDVD).Contains(copy.CopyNumber)
                                    select new
                                    {
                                        Copy = copy.CopyNumber,
                                        DVDTitle = DVDTitle.DVDTitles,
                                        DatePurchased = copy.DatePurchased
                                    }
                                    );
            return View(await notloanedCopiesDVD.ToListAsync());
        }
        //Remove older copies
        //10 - DVDCopies => OlderCopiesDVD
        public async Task<IActionResult> RemovedOldCopies()
        {
            var loanedCopiesDVD = (from loan in _context.Loans
                                   where loan.DateReturned == null
                                   select loan.CopyNumber).Distinct();
            var notloanedCopiesDVD = (from copy in _context.DVDCopies
                                      join DVDTitle in _context.DVDTitles on copy.DVDNumber equals DVDTitle.DVDNumber
                                      where !(loanedCopiesDVD).Contains(copy.CopyNumber)
                                      select new
                                      {
                                          Copy = copy.CopyNumber,
                                          DVDTitle = DVDTitle.DVDTitles,
                                          DatePurchased = copy.DatePurchased
                                      }
                                   );
            foreach(var copy in notloanedCopiesDVD.ToList())
            {
                if(DateTime.Now.Subtract(copy.DatePurchased).Days > 365)
                {
                    var remove = (from removeCopy in _context.DVDCopies
                                  where removeCopy.CopyNumber == copy.Copy
                                  select removeCopy).FirstOrDefault();
                    _context.DVDCopies.Remove(remove);
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
        //Loaned out copies
        //11 - DVDCopies => SelectDate => LoanedOutDVDCopies
        public async Task<IActionResult> LoanedOutDVDCopies()
        {

            if(Request.Form.Count() == 2 )
            {
                ViewData["SelectedDate"] = Request.Form["SearchDate"].ToString();
                DateTime searchingDate = DateTime.Parse(Request.Form["SearchDate"].ToString());
                var loanedOut = from loan in _context.Loans
                                join copy in _context.DVDCopies on loan.CopyNumber equals copy.CopyNumber
                                join DVDTitle in _context.DVDTitles on copy.DVDNumber equals DVDTitle.DVDNumber
                                join member in _context.Members on loan.MemberNumber equals member.MemberNumber
                                orderby loan.DateOut
                                where loan.DateOut.Date == searchingDate.Date
                                select new
                                 {
                                    Loan = loan.LoanNumber,
                                    DVDTitle = DVDTitle.DVDTitles,
                                    Copy = copy.CopyNumber,
                                    Member = member.MemberFirstName + " " + member.MemberLastName,
                                    DateOut = loan.DateOut
                                  };
                ViewData["TotalLoans"] = loanedOut.ToList().Count();
                return View( await loanedOut.ToListAsync());
            }
            else
            {
                ViewData["SelectedDate"] = DateTime.Today.ToString("yyyy-MM-dd");
                var loanedOut = from loan in _context.Loans
                                join copy in _context.DVDCopies on loan.CopyNumber equals copy.CopyNumber
                                join DVDTitle in _context.DVDTitles on copy.DVDNumber equals DVDTitle.DVDNumber
                                join member in _context.Members on loan.MemberNumber equals member.MemberNumber
                                orderby loan.DateOut
                                where loan.DateOut.Date == DateTime.Today.Date
                                select new
                                {
                                    Loan = loan.LoanNumber,
                                    DVDTitle = DVDTitle.DVDTitles,
                                    Copy = copy.CopyNumber,
                                    Member = member.MemberFirstName + " " + member.MemberLastName,
                                    DateOut = loan.DateOut
                                };
                ViewData["TotalLoans"] = loanedOut.ToList().Count();
                return View(await loanedOut.ToListAsync());
            }
        }
    }
}
