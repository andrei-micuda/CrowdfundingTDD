namespace CrowdfundingLib
{
    public class CrowdfundingPlatform
    {
        public List<Project> Projects { get; set; }

        public CrowdfundingPlatform()
        {
            Projects = new List<Project>();
        }

        public Project AddProject(Project project)
        {
            if(DateTimeOffset.Compare(DateTimeOffset.UtcNow, project.Deadline) > 0)
            {
                throw new ArgumentException("Project deadline cannot be in the past");
            }
            Projects.Add(project);

            return project;
        }
    }
}