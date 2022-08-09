using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LaborationRep.Models
{
    public class CollectionModel : DbContext
    {
        public CollectionModel() 
        {
            Database.EnsureCreated(); 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) 
        {
            options.UseSqlite("Data Source= collection.db");  
        }
     

        public DbSet<Artwork> Artworks { get; set; }

        public DbSet<Artist> Artists { get; set; }
    }
}
