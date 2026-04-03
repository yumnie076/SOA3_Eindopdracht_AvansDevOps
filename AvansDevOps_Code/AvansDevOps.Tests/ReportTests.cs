using AvansDevOps.Domain;
using AvansDevOps.Domain.Report;
using Moq;
using Xunit;

namespace AvansDevOps.Tests;

/// <summary>
/// Tests for the Report system (Strategy Pattern for export, Decorator Pattern for headers/footers).
/// </summary>
public class ReportTests
{
    private ReviewSprint CreateSprintWithData()
    {
        var sprint = new ReviewSprint("Sprint 1", new DateTime(2025, 1, 1), new DateTime(2025, 1, 14));
        var dev = new Person("Alice", "alice@avans.nl");
        sprint.AddMember(dev, Role.Developer);
        sprint.AddMember(new Person("Bob", "bob@avans.nl"), Role.ScrumMaster);

        var item1 = new BacklogItem("Login Feature", "Implement login", 5);
        item1.AssignDeveloper(dev);
        var item2 = new BacklogItem("Dashboard", "Build dashboard", 8);
        item2.AssignDeveloper(dev);

        sprint.AddBacklogItem(item1);
        sprint.AddBacklogItem(item2);
        return sprint;
    }

    // === Strategy Pattern tests ===

    [Fact]
    public void PdfExportStrategy_ShouldProducePdfFormat()
    {
        var sprint = CreateSprintWithData();
        var body = new ReportBody(sprint);
        var strategy = new PdfExportStrategy();
        var report = new SprintReport(body, strategy);

        string output = report.Generate();

        Assert.Contains("[PDF DOCUMENT]", output);
        Assert.Contains("Sprint Report: Sprint 1", output);
        Assert.Equal("PDF", report.GetFormatName());
    }

    [Fact]
    public void PngExportStrategy_ShouldProducePngFormat()
    {
        var sprint = CreateSprintWithData();
        var body = new ReportBody(sprint);
        var strategy = new PngExportStrategy();
        var report = new SprintReport(body, strategy);

        string output = report.Generate();

        Assert.Contains("[PNG IMAGE]", output);
        Assert.Equal("PNG", report.GetFormatName());
    }

    [Fact]
    public void SprintReport_ChangeStrategy_ShouldUseNewFormat()
    {
        var sprint = CreateSprintWithData();
        var body = new ReportBody(sprint);
        var report = new SprintReport(body, new PdfExportStrategy());

        Assert.Equal("PDF", report.GetFormatName());

        report.SetExportStrategy(new PngExportStrategy());
        Assert.Equal("PNG", report.GetFormatName());

        string output = report.Generate();
        Assert.Contains("[PNG IMAGE]", output);
    }

    [Fact]
    public void SprintReport_WithMockedStrategy_ShouldCallExport()
    {
        var sprint = CreateSprintWithData();
        var body = new ReportBody(sprint);
        var mockStrategy = new Mock<IReportExportStrategy>();
        mockStrategy.Setup(s => s.Export(It.IsAny<string>())).Returns("Mocked export");
        mockStrategy.Setup(s => s.FormatName).Returns("MOCK");

        var report = new SprintReport(body, mockStrategy.Object);
        string output = report.Generate();

        mockStrategy.Verify(s => s.Export(It.IsAny<string>()), Times.Once);
        Assert.Equal("Mocked export", output);
    }

    // === Decorator Pattern tests ===

    [Fact]
    public void ReportBody_ShouldContainSprintInfo()
    {
        var sprint = CreateSprintWithData();
        var body = new ReportBody(sprint);

        string content = body.Render();

        Assert.Contains("Sprint Report: Sprint 1", content);
        Assert.Contains("Alice", content);
        Assert.Contains("Login Feature", content);
        Assert.Contains("Dashboard", content);
        Assert.Contains("Total Story Points: 13", content);
    }

    [Fact]
    public void HeaderDecorator_ShouldAddHeaderToReport()
    {
        var sprint = CreateSprintWithData();
        IReportComponent component = new ReportBody(sprint);
        component = new HeaderDecorator(component, "Avans Hogeschool", "DevOps Project", "/img/logo.png");

        string content = component.Render();

        Assert.Contains("Avans Hogeschool", content);
        Assert.Contains("Project: DevOps Project", content);
        Assert.Contains("[Logo: /img/logo.png]", content);
        Assert.Contains("Sprint Report: Sprint 1", content);
    }

    [Fact]
    public void FooterDecorator_ShouldAddFooterToReport()
    {
        var sprint = CreateSprintWithData();
        IReportComponent component = new ReportBody(sprint);
        component = new FooterDecorator(component, "v1.0", new DateTime(2025, 1, 15), "CONFIDENTIAL");

        string content = component.Render();

        Assert.Contains("Sprint Report: Sprint 1", content);
        Assert.Contains("Version: v1.0", content);
        Assert.Contains("15-01-2025", content);
        Assert.Contains("CONFIDENTIAL", content);
    }

    [Fact]
    public void HeaderAndFooterDecorator_Combined_ShouldWork()
    {
        var sprint = CreateSprintWithData();
        IReportComponent component = new ReportBody(sprint);
        component = new HeaderDecorator(component, "Avans", "SOA3 Project");
        component = new FooterDecorator(component, "v2.0", new DateTime(2025, 1, 20));

        string content = component.Render();

        // Header should be at the top
        int headerPos = content.IndexOf("Avans");
        int bodyPos = content.IndexOf("Sprint Report");
        int footerPos = content.IndexOf("Version: v2.0");

        Assert.True(headerPos < bodyPos);
        Assert.True(bodyPos < footerPos);
    }

    [Fact]
    public void FullReport_WithDecoratorsAndStrategy_ShouldWork()
    {
        var sprint = CreateSprintWithData();
        IReportComponent component = new ReportBody(sprint);
        component = new HeaderDecorator(component, "Avans", "SOA3");
        component = new FooterDecorator(component, "v1.0", DateTime.Now);

        var report = new SprintReport(component, new PdfExportStrategy());
        string output = report.Generate();

        Assert.Contains("[PDF DOCUMENT]", output);
        Assert.Contains("Avans", output);
        Assert.Contains("Sprint Report", output);
        Assert.Contains("Version: v1.0", output);
    }

    // === Report content data ===

    [Fact]
    public void ReportBody_ShouldShowEffortPerDeveloper()
    {
        var sprint = CreateSprintWithData();
        var body = new ReportBody(sprint);

        string content = body.Render();

        Assert.Contains("Effort Points per Developer", content);
        Assert.Contains("Alice: 13 SP", content);
    }

    [Fact]
    public void ReportBody_ShouldShowBurndownData()
    {
        var sprint = CreateSprintWithData();
        var body = new ReportBody(sprint);

        string content = body.Render();

        Assert.Contains("Burndown Chart Data", content);
        Assert.Contains("Ideal burn rate", content);
    }
}
