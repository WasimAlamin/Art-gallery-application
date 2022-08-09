using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaborationRep.Models
{
    public class Artwork
    {
        public int Id { get; set; }

        public string ArtworkName { get; set; }

        public int Year { get; set; }

        public string Genre { get; set; }

        public string Technique { get; set; }

        public string Room { get; set; }

        public string Location { get; set; }

        public string Exhibition { get; set; }

        public int ArtistId { get; set; }

        public Artist  Artist { get; set; }
    }
}
