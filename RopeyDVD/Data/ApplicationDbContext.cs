﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RopeyDVD.Models;
using RopeyDVD.Models.ViewModels;

namespace RopeyDVD.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<Actor>? Actors { get; set; }
        public DbSet<Studio>? Studios { get; set; }
        public DbSet<Producer>? Producers { get; set; }
        public DbSet<DVDCategory>? DVDCategories { get; set; }
        public DbSet<LoanType>? LoanTypes { get; set; }
        public DbSet<MembershipCategory>? MembershipCategories { get; set; }
        public DbSet<UserRegister>? Users { get; set; }
        public DbSet<Member>? Members { get; set; }
        public DbSet<DVDTitle>? DVDTitles { get; set; }
        public DbSet<DVDCopy>? DVDCopies { get; set; }
        public DbSet<Loan>? Loans { get; set; }
        public DbSet<CastMember>? CastMembers { get; set; }
        public DbSet<RopeyDVD.Models.ViewModels.UserDetailsViewModel> UserDetailsViewModel { get; set; }


    }
}
