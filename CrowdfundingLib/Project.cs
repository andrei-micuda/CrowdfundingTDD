using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowdfundingLib
{
    public class Project
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Deadline { get; set; }

        public Project(string title,
                       string description,
                       DateTimeOffset deadline)
        {
            Title = title;
            Description = description;
            Deadline = deadline;
        }

        public override bool Equals(object? obj)
        {
            return obj is Project project &&
                   Title == project.Title &&
                   Description == project.Description &&
                   Deadline.Equals(project.Deadline);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, Description, Deadline);
        }
    }
}
