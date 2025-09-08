using TeamworkWeeklyReport.Models.Teamwork;

namespace TeamworkWeeklyReport.Services
{    
    public class GroupTasks{
        private readonly List<Tasks> _tasks;

        public GroupTasks(List<Tasks> tasks)
        {
            _tasks = tasks;
        }
        public Dictionary<long, List<Tasks>> GroupUserTasks(List<long> projectsIds){
            return _tasks
                .Where(task => projectsIds.Contains(task.ProjectId))
                .ToList()
                .GroupBy(task => task.ProjectOwnerId)
                .ToDictionary(task => task.Key, task => task.ToList());
        }        
    }
}
