﻿using System.Net.Http.Json;
using TheBugInspector.Client.Models;
using TheBugInspector.Client.Services.Interfaces;

namespace TheBugInspector.Client.Services
{
    public class WASMProjectDTOService : IProjectDTOService
    {
        private readonly HttpClient _httpClient;

        public WASMProjectDTOService(HttpClient httpClient)
        {
            _httpClient = httpClient;  
        }

        public async Task<ProjectDTO> AddProjectAsync(ProjectDTO project, int companyId)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/projects", project);
            response.EnsureSuccessStatusCode();

            ProjectDTO? createdProject = await response.Content.ReadFromJsonAsync<ProjectDTO>();
            return createdProject!;
        }

        public async Task ArchiveProjectAsync(int projectId, int companyId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/projects/{projectId}/active", projectId);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync(int companyId)
        {
            IEnumerable<ProjectDTO> projects = [];

            try
            {
                projects = await _httpClient.GetFromJsonAsync<IEnumerable<ProjectDTO>>($"api/projects/{companyId}/active") ?? [];
                return projects;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return projects;
            }
        }

        public async Task<IEnumerable<ProjectDTO>> GetArchivedProjectsAsync(int companyId)
        {
            IEnumerable<ProjectDTO> projects = [];

            try
            {
                projects = await _httpClient.GetFromJsonAsync<IEnumerable<ProjectDTO>>($"api/projects/{companyId}/archived") ?? [];
                return projects;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return projects;
            }
        }

        public async Task<ProjectDTO?> GetProjectByIdAsync(int projectId, int companyId)
        {
            ProjectDTO? project = new();

            try
            {
                project = await _httpClient.GetFromJsonAsync<ProjectDTO>($"api/projects/{projectId}");
                return project;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return project;
            }
        }

        public async Task RestoreProjectAsync(int projectId, int companyId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/projects/{projectId}/archived", projectId);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateProjectAsync(ProjectDTO project, int companyId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/projects/{project.Id}", project);
            response.EnsureSuccessStatusCode();
        }
    }
}
