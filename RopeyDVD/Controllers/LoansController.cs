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
    public class LoansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Loans
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Loans.Include(l => l.DVDCopy).Include(l => l.LoanType).Include(l => l.Member);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Loans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .Include(l => l.DVDCopy)
                .Include(l => l.LoanType)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(m => m.LoanNumber == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // GET: Loans/Create
        public IActionResult Create()
        {
            ViewData["CopyNumber"] = new SelectList(from copy in _context.DVDCopies
                                                    join DVDTitle in _context.DVDTitles on copy.DVDNumber equals DVDTitle.DVDNumber
                                                    select new
                                                    {
                                                        CopyNumber = copy.CopyNumber,
                                                        Title = copy.CopyNumber + " - " + DVDTitle.DVDTitles,
                                                    }, "CopyNumber", "Title");
            ViewData["LoanTypeNumber"] = new SelectList(_context.LoanTypes, "LoanTypeNumber", "LoanTypes");
            ViewData["MemberNumber"] = new SelectList(from member in _context.Members
                                                      select new
                                                      {
                                                          MemberNumber = member.MemberNumber,
                                                          MemberName = member.MemberFirstName + " " + member.MemberLastName,
                                                      }, "MemberNumber", "MemberName"
                                                      );
            return View();
        }

        // POST: Loans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // create loan
        //6 - Loans => Create => Payment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LoanNumber,DateOut,DateDue,DateReturned,LoanTypeNumber,CopyNumber,MemberNumber")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                //details of members
                var memberDOB = from member in _context.Members
                                 join membership in _context.MembershipCategories on member.MembershipCategoryNumber equals membership.MembershipCategoryNumber
                                 where member.MemberNumber == loan.MemberNumber
                                 select new
                                 {
                                     DateOfBirth = member.MemberDOB,
                                     totalLoans = membership.MembershipCategoryTotalLoans
                                 };
                var data1 = memberDOB.FirstOrDefault();
                //get age restriction
                var ageRestricted = from copy in _context.DVDCopies
                                    join title in _context.DVDTitles on copy.DVDNumber equals title.DVDNumber
                                    join category in _context.DVDCategories on title.CategoryNumber equals category.CategoryNumber
                                    where copy.CopyNumber == loan.CopyNumber
                                    select new
                                    {
                                        restricted = category.AgeRestricted,
                                    };
                var data2 = ageRestricted.FirstOrDefault();

                //total loans taken by user
                var loansTaken = (from loans in _context.Loans
                                  where loans.MemberNumber == loan.MemberNumber
                                  select loans).Count();
                var dvdLoanTake = (from loans in _context.Loans
                                   where loans.CopyNumber == loan.CopyNumber
                                   select loans).Count();
                if(dvdLoanTake == 0)
                {
                    //Checking is date due is greater than date out
                    if (loan.DateDue > loan.DateOut)
                    {
                        //checking is loans taken is less than the loans allowed
                        if (loansTaken < data1.totalLoans)
                        {
                            var dt = data1.DateOfBirth;
                            var today = DateTime.Today;

                            //Age of the member
                            var age = today.Year - dt.Year;

                            if (dt.Date > today.AddYears(-age)) age--;

                            if (data2.restricted)
                            {
                                if (age >= 18)
                                {
                                    _context.Add(loan);
                                    await _context.SaveChangesAsync();
                                    return RedirectToAction(nameof(Index));
                                }
                                else
                                {
                                    //if users is aged under 18
                                    ViewData["DangerAlert"] = "This user is underage!!!";
                                }
                            }
                            else
                            {
                                _context.Add(loan);
                                await _context.SaveChangesAsync();
                                return RedirectToAction(nameof(Index));
                            }


                        }
                        else
                        {
                            ViewData["DangerAlert"] = "This user is not allowed to take more loans.!";
                        }
                    }
                    else
                    {
                        ViewData["DangerAlert"] = "Due Date can not be less than Date Out.!";
                    }
                }
                else
                {
                    ViewData["DangerAlert"] = "This copy has already been loaned.!";
                }
            }
            ViewData["CopyNumber"] = new SelectList(from copy in _context.DVDCopies
                                                    join dvdTitle in _context.DVDTitles on copy.DVDNumber equals dvdTitle.DVDNumber
                                                    select new
                                                    {
                                                        CopyNumber = copy.CopyNumber,
                                                        Title = copy.CopyNumber + " - " + dvdTitle.DVDTitles,

                                                    }, "CopyNumber", "Title"
                                                    );
            ViewData["LoanTypeNumber"] = new SelectList(_context.LoanTypes, "LoanTypeNumber", "LoanTypes", loan.LoanTypeNumber);
            ViewData["MemberNumber"] = new SelectList(from member in _context.Members
                                                      select new
                                                      {
                                                          MemberNumber = member.MemberNumber,
                                                          MemberName = member.MemberFirstName + " " + member.MemberLastName,

                                                      }, "MemberNumber", "MemberName"
                                                      );
            return View();
        }

        // GET: Loans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }
            ViewData["CopyNumber"] = new SelectList(from copy in _context.DVDCopies
                                                    join dvdtitle in _context.DVDTitles on copy.DVDNumber equals dvdtitle.DVDNumber
                                                    select new
                                                    {
                                                        CopyNumber = copy.CopyNumber,
                                                        Title = copy.CopyNumber + " - " + dvdtitle.DVDTitles
                                                    }, "CopyNumber", "Title");
            ViewData["LoanTypeNumber"] = new SelectList(_context.LoanTypes, "LoanTypeNumber", "LoanTypes");
            ViewData["MemberNumber"] = new SelectList(from member in _context.Members
                                                      select new
                                                      {
                                                          MemberNumber = member.MemberNumber,
                                                          MemberName = member.MemberFirstName + " " + member.MemberLastName,
                                                      }, "MemberNumber", "MemberName"
                                                      );
            return View(loan);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //DVD return is done by editing the loaned dvd
        //7 - Loans => Index => Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LoanNumber,DateOut,DateDue,DateReturned,LoanTypeNumber,CopyNumber,MemberNumber")] Loan loan)
        {
            if (id != loan.LoanNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if((loan.DateReturned - loan.DateDue).Value.TotalDays >= 0)
                    {
                        _context.Update(loan);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Payment", loan);
                    }
                    else
                    {
                        ViewData["DangerAlert"] = "Return date must not be less than due date.!";
                        ViewData["CopyNumber"] = new SelectList(from copy in _context.DVDCopies
                                                                join dvdtitle in _context.DVDTitles on copy.DVDNumber equals dvdtitle.DVDNumber
                                                                select new
                                                                {
                                                                    CopyNumber = copy.CopyNumber,
                                                                    Title = copy.CopyNumber + " - " + dvdtitle.DVDTitles
                                                                }, "CopyNumber", "Title");
                        ViewData["LoanTypeNumber"] = new SelectList(_context.LoanTypes, "LoanTypeNumber", "LoanTypes");
                        ViewData["MemberNumber"] = new SelectList(from member in _context.Members
                                                                  select new
                                                                  {
                                                                      MemberNumber = member.MemberNumber,
                                                                      MemberName = member.MemberFirstName + " " + member.MemberLastName
                                                                  }, "MemberNumber", "MemberName"
                                                                  );
                        return View(loan);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanExists(loan.LoanNumber))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["CopyNumber"] = new SelectList(from copy in _context.DVDCopies
                                                    join dvdtitle in _context.DVDTitles on copy.DVDNumber equals dvdtitle.DVDNumber
                                                    select new
                                                    {
                                                        CopyNumber = copy.CopyNumber,
                                                        Title = copy.CopyNumber + " - " + dvdtitle.DVDTitles
                                                    }, "CopyNumber", "Title");
            ViewData["LoanTypeNumber"] = new SelectList(_context.LoanTypes, "LoanTypeNumber", "LoanTypes");
            ViewData["MemberNumber"] = new SelectList(from member in _context.Members
                                                      select new
                                                      {
                                                          MemberNumber = member.MemberNumber,
                                                          MemberName = member.MemberFirstName + " " + member.MemberLastName
                                                      }, "MemberNumber", "MemberName"
                                                      );
            return View(loan);
        }
        //for payment details
        public async Task<IActionResult> Payment(Loan loan)
        {
            //Using LINQ to get loan details
            var loanDetails = (from loans in _context.Loans
                               join member in _context.Members on loans.MemberNumber equals member.MemberNumber
                               join membership in _context.MembershipCategories on member.MembershipCategoryNumber equals membership.MembershipCategoryNumber
                               join loanType in _context.LoanTypes on loans.CopyNumber equals loanType.LoanTypeNumber
                               join copy in _context.DVDCopies on loans.CopyNumber equals copy.CopyNumber
                               join dvdTitle in _context.DVDTitles on copy.DVDNumber equals dvdTitle.DVDNumber
                               where loans.LoanNumber == loan.LoanNumber
                               select new
                               {
                                   LoanNumber = loan.LoanNumber,
                                   CopyNumber = loans.CopyNumber,
                                   DateOut = loans.DateOut,
                                   DateDue = loans.DateDue,
                                   DateReturned = loan.DateReturned,
                                   Member = member.MemberFirstName + " " + member.MemberLastName,
                                   Membership = membership.MembershipCategoryDescription,
                                   LoanType = loanType.LoanTypes,
                                   LoanDuration = loanType.LoanDuration,
                                   DVDTitle = dvdTitle.DVDTitles,
                                   StandardCharge = dvdTitle.StandardCharge,
                                   PenaltyCharge = dvdTitle.PenaltyCharge

                               }).FirstOrDefault();
            return View(loanDetails);
        }
        // GET: Loans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .Include(l => l.DVDCopy)
                .Include(l => l.LoanType)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(m => m.LoanNumber == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanExists(int id)
        {
            return _context.Loans.Any(e => e.LoanNumber == id);
        }
    }
}
