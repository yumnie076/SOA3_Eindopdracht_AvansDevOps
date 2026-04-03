namespace AvansDevOps.Domain.Report;

// DECORATOR PATTERN: Concrete component.
// The base report body containing sprint data.
public class ReportBody : IReportComponent
{
    private readonly Sprint _sprint;

    public ReportBody(Sprint sprint)
    {
        _sprint = sprint;
    }

    public string Render()
    {
        var lines = new List<string>
        {
            $"Sprint Report: {_sprint.Name}",
            $"Period: {_sprint.StartDate:dd-MM-yyyy} - {_sprint.EndDate:dd-MM-yyyy}",
            $"Status: {_sprint.GetStateName()}",
            "",
            "Team Composition:",
        };

        foreach (var member in _sprint.Members)
        {
            lines.Add($"  - {member.Person.Name} ({member.Role})");
        }

        lines.Add("");
        lines.Add("Backlog Items:");

        int totalPoints = 0;
        foreach (var item in _sprint.BacklogItems)
        {
            lines.Add($"  - [{item.GetStateName()}] {item.Title} ({item.StoryPoints} SP)");
            totalPoints += item.StoryPoints;
        }

        lines.Add("");
        lines.Add($"Total Story Points: {totalPoints}");

        int doneItems = _sprint.BacklogItems.Count(bi => bi.GetStateName() == "Done");
        lines.Add($"Done Items: {doneItems}/{_sprint.BacklogItems.Count}");

        // Burndown data
        lines.Add("");
        lines.Add("Burndown Chart Data:");
        int remaining = totalPoints;
        int daysInSprint = (_sprint.EndDate - _sprint.StartDate).Days;
        if (daysInSprint > 0)
        {
            double idealBurnRate = (double)totalPoints / daysInSprint;
            lines.Add($"  Ideal burn rate: {idealBurnRate:F1} SP/day");
        }

        // Effort points per developer
        lines.Add("");
        lines.Add("Effort Points per Developer:");
        var developerPoints = _sprint.BacklogItems
            .Where(bi => bi.AssignedDeveloper != null)
            .GroupBy(bi => bi.AssignedDeveloper!.Name)
            .Select(g => new { Developer = g.Key, Points = g.Sum(bi => bi.StoryPoints) });

        foreach (var dp in developerPoints)
        {
            lines.Add($"  - {dp.Developer}: {dp.Points} SP");
        }

        return string.Join("\n", lines);
    }
}
