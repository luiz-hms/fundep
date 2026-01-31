using Fundep.ProjectService.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Fundep.ProjectService.Contracts
{
    [ServiceContract]
    public interface IFundepProjectService
    {
        // Coordinators
        [OperationContract] List<Coordinator> GetCoordinators();
        [OperationContract] Coordinator GetCoordinatorById(string id);
        [OperationContract] void CreateCoordinator(Coordinator coordinator);
        [OperationContract] void UpdateCoordinator(Coordinator coordinator);
        [OperationContract] void DeleteCoordinator(string id);

        // Projects
        [OperationContract] List<Project> GetProjects();
        [OperationContract] Project GetProjectByKey(string projectNumber, string subProjectNumber);
        [OperationContract] void CreateProject(Project project);
        [OperationContract] void UpdateProject(Project project);
        [OperationContract] void DeleteProject(string projectNumber, string subProjectNumber);
        [OperationContract] List<Project> SearchProjects(string projectNumberFilter, string nameFilter);
    }
}
