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
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Members
        //List all the memmebrs
        //8 - Members => Index
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = from members in _context.Members
                                       join membership in _context.MembershipCategories on members.MembershipCategoryNumber equals membership.MembershipCategoryNumber
                                       select new
                                       {
                                           MemberNumber = members.MemberNumber,
                                           MemberFirstName = members.MemberFirstName,
                                           MemberLastName = members.MemberLastName,
                                           MemberAddress = members.MemberAddress,
                                           MemberDOB = members.MemberDOB,
                                           Membership = membership.MembershipCategoryDescription,
                                           TotalAcceptLoans = membership.MembershipCategoryTotalLoans,
                                           TotalCurrentLoans = (from loans in _context.Loans
                                                                where loans.DateReturned == null
                                                                where loans.MemberNumber == members.MemberNumber
                                                                select loans).Count(),

                                       };
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: Members/Details/5
        //Member details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.MembershipCategory)
                .FirstOrDefaultAsync(m => m.MemberNumber == id);

            //Get datetime of 31 days ago
            var differenceDate = DateTime.Now.AddDays(-31);

            //get all data
            var data = from members in _context.Members
                       join loan in _context.Loans on members.MemberNumber equals loan.MemberNumber
                       where loan.DateOut >= differenceDate
                       where members.MemberNumber == id
                       join dvdcopy in _context.DVDCopies on loan.CopyNumber equals dvdcopy.CopyNumber
                       join dvdtitle in _context.DVDTitles on dvdcopy.DVDNumber equals dvdtitle.DVDNumber
                       select new
                       {
                           Loan = loan.LoanNumber,
                           CopyNumber = dvdcopy.CopyNumber,
                           Title = dvdtitle.DVDTitles,
                           DateOut = loan.DateOut,
                           DateReturn = loan.DateReturned

                       };
            if (member == null)
            {
                return NotFound();
            }
            if (data == null)
            {
                ViewData["Member"] = member;
                return View(member);
            }
            else
            {
                ViewData["Member"] = member;
                return View(data);
            }
            
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("MemberNumber,MemberLastName,MemberFirstName,MemberAddress,MemberDOB")] Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("MemberNumber,MemberLastName,MemberFirstName,MemberAddress,MemberDOB")] Member member)
        {
            if (id != member.MemberNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.MemberNumber))
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
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MemberNumber == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MemberNumber == id);
        }
        //Select member
        //3 - Members => SelectMembers => MemberLoans
        public async Task<IActionResult> SelectMember(Member members)
        {
            ViewData["MemberLastName"] = new SelectList(_context.Set<Member>(), "MemberLastName", "MemberLastName", members.MemberLastName);
            return View();
        }
        //List all the dvds loaned by selected members
        //3 - Members => SelectMembers => MemberLoans
        public async Task<IActionResult> MemberLoans()
        {
            string memberName = Request.Form["memberList"].ToString();
            var data = (from Member in _context.Members
                          join Loan in _context.Loans on Member.MemberNumber equals Loan.LoanNumber
                          join copy in _context.DVDCopies on Loan.LoanNumber equals copy.CopyNumber
                          join title in _context.DVDTitles on copy.CopyNumber equals title.DVDNumber
                          where Member.MemberLastName == memberName
                          select new
                          {
                              MemberNumber = Member.MemberNumber,
                              DVDNumber = title.DVDNumber,
                              Title = title.DVDTitles,
                              dateReturned = Loan.DateOut <= DateTime.Now.AddDays(31),
                              CopyNumber = copy.CopyNumber
                              
                          });
            return View(data);
        }
        //Lists all the members that have been inactive since last 31 days
        //12 - Members => MemberNotBorrowed
        public async Task<IActionResult> MemberNotBorrowed()
        {
            var notLoanedMembers = _context.Members.ToList().Except(from m in _context.Members.ToList()
                                                                   join l in _context.Loans.ToList().Where(l => DateTime.Now.Subtract(l.DateOut).TotalDays <= 31)
                                                                   on m.MemberNumber equals l.MemberNumber
                                                                   select m);
            var loanedMember = (from m in notLoanedMembers
                                join Loan in _context.Loans on m.MemberNumber equals Loan.MemberNumber
                                join DVDCopy in _context.DVDCopies on Loan.CopyNumber equals DVDCopy.CopyNumber
                                join DVDTitle in _context.DVDTitles on DVDCopy.DVDNumber equals DVDTitle.DVDNumber
                                select new
                                {
                                    DateOut = Loan.DateOut,
                                    Title = DVDTitle.DVDTitles,
                                    LoanDays = Convert.ToInt32(DateTime.Now.Subtract(Loan.DateOut).TotalDays),
                                    MemberAddress = m.MemberAddress,
                                    MemberFirstName = m.MemberFirstName,
                                    MemberLastName = m.MemberLastName,
                                });
            return View(loanedMember);

                               
        }
    }
}
