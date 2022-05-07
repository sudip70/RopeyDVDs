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
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            return View(await _context.Members.ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
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
        public async Task<IActionResult> SelectMember(Member members)
        {
            ViewData["MemberFirstName"] = new SelectList(_context.Set<Member>(), "MemberFirstName", "MemberFirstName", members.MemberFirstName);
            return View();
        }
        public async Task<IActionResult> MemberLoans()
        {
            string memberName = Request.Form["memberList"].ToString();
            var data = (from Member in _context.Members
                          join Loan in _context.Loans on Member.MemberNumber equals Loan.LoanNumber
                          join copy in _context.DVDCopies on Loan.LoanNumber equals copy.CopyNumber
                          join title in _context.DVDTitles on copy.CopyNumber equals title.DVDNumber
                          select new
                          {
                              MemberNumber = Member.MemberNumber,
                              DVDNumber = title.DVDNumber,
                              Title = title.DVDTitles,
                              dateReturned = Loan.DateOut <= DateTime.Now.AddDays(31),
                              CopyNumber = copy.CopyNumber
                              
                          });

            //var results = _context.Members.Include(m => m.Loan)
            //    .ThenInclude(l => l.DVDCopy)
            //    .ThenInclude(c => c.DVDTitle)
            //    .Where(m => m.Loan.All(l => l.DateOut <= DateTime.Now.AddDays(31)))
            //    .Where(m => m.MemberFirstName.Contains(searchString)).FirstOrDefault();
            //ViewData["member"] = results;
            //if(results == null)
            //{
            //    ViewData["loans"] = new List<Loan>();
            //}
            //else
            //{
            //    ViewData["loans"] = results.Loan;
            //}
            return View(data);
        }
    }
}
