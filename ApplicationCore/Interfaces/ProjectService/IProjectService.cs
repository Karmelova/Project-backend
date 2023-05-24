using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.ProjectService
{
    public interface IProjectService
    {
        void Create(Project project);

        Project GetById(int id);

        void Update(Project project);

        void Delete(Project project);
    }
}