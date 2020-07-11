using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Price { get; set; }
        public List<UserProj> UserProjs { get; set; }
        public Project()
        {
            UserProjs = new List<UserProj>();
        }
    }
}
