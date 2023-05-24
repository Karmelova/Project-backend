using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.ProjectService;
using ApplicationCore.Models;

namespace WebAPI.Services
{
    public class ProjectServiceEF : IProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectServiceEF(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Create(Project project)
        {
            throw new NotImplementedException();
        }

        public Project GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Project project)
        {
            throw new NotImplementedException();
        }

        public void Delete(Project project)
        {
            throw new NotImplementedException();
        }
    }
}