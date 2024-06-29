﻿using Microsoft.EntityFrameworkCore;
using P02_FootballBetting.Data.Models;


namespace P02_FootballBetting.Data
{
   
    public class FootballBettingContext : DbContext
    {
        private const string ConnectionString = "Server=MENTAT\\SQLEXPRESS;Database=FootballBetting;Integrated Security=True;";

        public DbSet<Country> Countries { get; set; }

        public FootballBettingContext(DbContextOptions db) : base(db) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }


}
