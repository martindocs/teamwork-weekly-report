using TeamworkWeeklyReport.Models.Teamwork;

namespace TeamworkWeeklyReport.Services
{    
    public class GroupTasks{
        private readonly List<Tasks> _tasks;

        public GroupTasks(List<Tasks> tasks)
        {
            _tasks = tasks;
        }

        public Dictionary<long, List<Tasks>> GroupUserTasks(List<long> projectsIds) => _tasks.Where(o => projectsIds.Contains(o.ProjectId)).ToList().GroupBy(o => o.ProjectOwnerId).ToDictionary(g => g.Key, g => g.ToList());
        
    }
}
