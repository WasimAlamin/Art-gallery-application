using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaborationRep.Models
{
    public class Artist
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<Artwork> Art { get; set; }
    }
}
