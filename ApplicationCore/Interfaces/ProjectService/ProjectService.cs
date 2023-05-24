using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.ProjectService
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectService _projectService;

        public ProjectService(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public void Create(Project project)
        {
            _projectService.Create(project);
        }

        public Project GetById(int id)
        {
            return _projectService.GetById(id);
        }

        public void Update(Project project)
        {
            _projectService.Update(project);
        }

        public void Delete(Project project)
        {
            _projectService.Delete(project);
        }
    }
}