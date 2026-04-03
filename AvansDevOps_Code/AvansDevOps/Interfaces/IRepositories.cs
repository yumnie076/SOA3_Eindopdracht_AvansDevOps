namespace AvansDevOps.Interfaces;

using AvansDevOps.Domain;

// Clean Architecture: Repository interfaces in the application core.
// Infrastructure implementations (database, file, etc.) depend on these
// abstractions, not the other way around (Dependency Inversion Principle).

public interface IProjectRepository
{
    Project? GetById(int id);
    List<Project> GetAll();
    void Add(Project project);
    void Update(Project project);
    void Delete(int id);
}

public interface IBacklogItemRepository
{
    BacklogItem? GetById(int id);
    List<BacklogItem> GetBySprintId(int sprintId);
    void Add(BacklogItem item);
    void Update(BacklogItem item);
    void Delete(int id);
}

public interface ISprintRepository
{
    Sprint? GetById(int id);
    List<Sprint> GetByProjectId(int projectId);
    void Add(Sprint sprint);
    void Update(Sprint sprint);
    void Delete(int id);
}
