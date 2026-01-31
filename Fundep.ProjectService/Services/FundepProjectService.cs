using Fundep.ProjectService.Contracts;
using Fundep.ProjectService.Models;
using Fundep.ProjectService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fundep.ProjectService.Services
{
    public class FundepProjectService : IFundepProjectService
    {
        private readonly IXmlStore<Coordinator> _coordinators;
        private readonly IXmlStore<Project> _projects;

        public FundepProjectService(string appDataPath)
        {
            _coordinators = Stores.Coordinators(appDataPath);
            _projects = Stores.Projects(appDataPath);
        }

        public List<Coordinator> GetCoordinators() => _coordinators.LoadAll().OrderBy(c => c.Name).ToList();

        public Coordinator GetCoordinatorById(string id) =>
            GetCoordinators().FirstOrDefault(c => string.Equals(c.Id, id, StringComparison.OrdinalIgnoreCase));

        public void CreateCoordinator(Coordinator coordinator)
        {
            ValidateCoordinator(coordinator);
            var list = _coordinators.LoadAll();

            if (list.Any(c => string.Equals(c.Name, coordinator.Name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Já existe um coordenador com esse nome.");

            list.Add(coordinator);
            _coordinators.SaveAll(list);
        }

        public void UpdateCoordinator(Coordinator coordinator)
        {
            ValidateCoordinator(coordinator);
            var list = _coordinators.LoadAll();
            var idx = list.FindIndex(c => string.Equals(c.Id, coordinator.Id, StringComparison.OrdinalIgnoreCase));
            if (idx < 0) throw new InvalidOperationException("Coordenador não encontrado.");

            if (list.Any(c => !string.Equals(c.Id, coordinator.Id, StringComparison.OrdinalIgnoreCase)
                              && string.Equals(c.Name, coordinator.Name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Já existe um coordenador com esse nome.");

            list[idx].Name = coordinator.Name;
            _coordinators.SaveAll(list);
        }

        public void DeleteCoordinator(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id do coordenador é obrigatório.");
            var list = _coordinators.LoadAll();
            if (list.Count <= 1) throw new InvalidOperationException("Não é possível excluir o último coordenador.");

            var projects = _projects.LoadAll();
            if (projects.Any(p => string.Equals(p.CoordinatorId, id, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Não é possível excluir: existem projetos vinculados a este coordenador.");

            list = list.Where(c => !string.Equals(c.Id, id, StringComparison.OrdinalIgnoreCase)).ToList();
            _coordinators.SaveAll(list);
        }

        public List<Project> GetProjects() => _projects.LoadAll().OrderBy(p => p.ProjectNumber).ThenBy(p => p.SubProjectNumber).ToList();

        public Project GetProjectByKey(string projectNumber, string subProjectNumber) =>
            GetProjects().FirstOrDefault(p =>
                string.Equals(p.ProjectNumber, projectNumber, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(p.SubProjectNumber, subProjectNumber, StringComparison.OrdinalIgnoreCase));

        public void CreateProject(Project project)
        {
            ValidateProject(project);
            EnsureCoordinatorExists(project.CoordinatorId);

            var list = _projects.LoadAll();
            if (list.Any(p =>
                string.Equals(p.ProjectNumber, project.ProjectNumber, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(p.SubProjectNumber, project.SubProjectNumber, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Já existe um projeto com este número e subprojeto.");

            list.Add(project);
            _projects.SaveAll(list);
        }

        public void UpdateProject(Project project)
        {
            ValidateProject(project);
            EnsureCoordinatorExists(project.CoordinatorId);

            var list = _projects.LoadAll();
            var idx = list.FindIndex(p =>
                string.Equals(p.ProjectNumber, project.ProjectNumber, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(p.SubProjectNumber, project.SubProjectNumber, StringComparison.OrdinalIgnoreCase));

            if (idx < 0) throw new InvalidOperationException("Projeto não encontrado.");

            list[idx].Name = project.Name;
            list[idx].CoordinatorId = project.CoordinatorId;
            list[idx].Balance = project.Balance;
            _projects.SaveAll(list);
        }

        public void DeleteProject(string projectNumber, string subProjectNumber)
        {
            if (string.IsNullOrWhiteSpace(projectNumber)) throw new ArgumentException("Número do projeto é obrigatório.");
            if (string.IsNullOrWhiteSpace(subProjectNumber)) throw new ArgumentException("Número do subprojeto é obrigatório.");

            var list = _projects.LoadAll();
            list = list.Where(p =>
                !(string.Equals(p.ProjectNumber, projectNumber, StringComparison.OrdinalIgnoreCase) &&
                  string.Equals(p.SubProjectNumber, subProjectNumber, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            _projects.SaveAll(list);
        }

        public List<Project> SearchProjects(string projectNumberFilter, string nameFilter)
        {
            var hasNumber = !string.IsNullOrWhiteSpace(projectNumberFilter);
            var hasName = !string.IsNullOrWhiteSpace(nameFilter);

            if (!hasNumber && !hasName)
                throw new ArgumentException("Informe ao menos um filtro de busca (número do projeto ou nome).");

            var list = GetProjects();

            if (hasNumber)
                list = list.Where(p => (p.ProjectNumber ?? "").IndexOf(projectNumberFilter, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            if (hasName)
                list = list.Where(p => (p.Name ?? "").IndexOf(nameFilter, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            return list;
        }

        private void ValidateCoordinator(Coordinator c)
        {
            if (c == null) throw new ArgumentException("Coordenador é obrigatório.");
            if (string.IsNullOrWhiteSpace(c.Id)) throw new ArgumentException("Id do coordenador é obrigatório.");
            if (string.IsNullOrWhiteSpace(c.Name)) throw new ArgumentException("Nome do coordenador é obrigatório.");
        }

        private void ValidateProject(Project p)
        {
            if (p == null) throw new ArgumentException("Projeto é obrigatório.");
            if (string.IsNullOrWhiteSpace(p.ProjectNumber)) throw new ArgumentException("Número do projeto é obrigatório.");
            if (string.IsNullOrWhiteSpace(p.SubProjectNumber)) throw new ArgumentException("Número do subprojeto é obrigatório.");
            if (string.IsNullOrWhiteSpace(p.Name)) throw new ArgumentException("Nome do projeto é obrigatório.");
            if (string.IsNullOrWhiteSpace(p.CoordinatorId)) throw new ArgumentException("Coordenador do projeto é obrigatório.");
            if (p.Balance < 0) throw new ArgumentException("Saldo do projeto não pode ser negativo.");
        }

        private void EnsureCoordinatorExists(string coordinatorId)
        {
            var exists = _coordinators.LoadAll().Any(c => string.Equals(c.Id, coordinatorId, StringComparison.OrdinalIgnoreCase));
            if (!exists) throw new InvalidOperationException("Coordenador selecionado não existe.");
        }
    }
}
