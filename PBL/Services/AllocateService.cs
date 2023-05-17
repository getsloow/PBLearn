using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL.Repositories.Interfaces;
using PBL.Services.Interfaces;
using PBL.Repositories;
using Microsoft.AspNet.Identity;

namespace PBL.Services
{
    public class AllocateService : IAllocateService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public AllocateService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public void Allocate(int Id, string userEmail)
        {
            var project = _repositoryWrapper.ProjectRepository.FindByCondition(c => c.Id == Id).FirstOrDefault();

            project!.UserEmail = userEmail;
            _repositoryWrapper.ProjectRepository.Update(project);
            _repositoryWrapper.Save();
        }
        public void DeAllocate(int Id)
        {
            var project = _repositoryWrapper.ProjectRepository.FindByCondition(c => c.Id == Id).FirstOrDefault();
            project!.UserEmail = null;
            _repositoryWrapper.ProjectRepository.Update(project);
            _repositoryWrapper.Save();
        }


    }
}
