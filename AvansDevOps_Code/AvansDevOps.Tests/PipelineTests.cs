using AvansDevOps.Domain.Pipeline;
using Xunit;

namespace AvansDevOps.Tests;

/// <summary>
/// Tests for the Development Pipeline (Composite Pattern).
/// Verifies that pipelines execute actions sequentially and
/// stop on failure.
/// </summary>
public class PipelineTests
{
    [Fact]
    public void Pipeline_AllActionsSucceed_ShouldReturnTrue()
    {
        var pipeline = new DevelopmentPipeline("Build Pipeline");
        pipeline.AddComponent(new PipelineAction("Checkout Code", PipelineActionType.Sources));
        pipeline.AddComponent(new PipelineAction("Install Packages", PipelineActionType.Package));
        pipeline.AddComponent(new PipelineAction("Build Solution", PipelineActionType.Build));
        pipeline.AddComponent(new PipelineAction("Run Tests", PipelineActionType.Test));

        bool result = pipeline.Execute();
        Assert.True(result);
    }

    [Fact]
    public void Pipeline_ActionFails_ShouldReturnFalse()
    {
        var pipeline = new DevelopmentPipeline("Build Pipeline");
        pipeline.AddComponent(new PipelineAction("Checkout Code", PipelineActionType.Sources));
        pipeline.AddComponent(new PipelineAction("Build Solution", PipelineActionType.Build)
        {
            ExecutionLogic = () => false // Simulate build failure
        });
        pipeline.AddComponent(new PipelineAction("Run Tests", PipelineActionType.Test));

        bool result = pipeline.Execute();
        Assert.False(result);
    }

    [Fact]
    public void Pipeline_ActionAfterFailure_ShouldNotExecute()
    {
        bool testExecuted = false;

        var pipeline = new DevelopmentPipeline("Build Pipeline");
        pipeline.AddComponent(new PipelineAction("Build", PipelineActionType.Build)
        {
            ExecutionLogic = () => false
        });
        pipeline.AddComponent(new PipelineAction("Test", PipelineActionType.Test)
        {
            ExecutionLogic = () => { testExecuted = true; return true; }
        });

        pipeline.Execute();
        Assert.False(testExecuted);
    }

    [Fact]
    public void Pipeline_EmptyPipeline_ShouldSucceed()
    {
        var pipeline = new DevelopmentPipeline("Empty Pipeline");
        bool result = pipeline.Execute();
        Assert.True(result);
    }

    [Fact]
    public void Pipeline_NestedPipelines_ShouldExecuteAll()
    {
        // COMPOSITE PATTERN: A pipeline can contain sub-pipelines
        var buildPipeline = new DevelopmentPipeline("Build Stage");
        buildPipeline.AddComponent(new PipelineAction("Checkout", PipelineActionType.Sources));
        buildPipeline.AddComponent(new PipelineAction("Build", PipelineActionType.Build));

        var testPipeline = new DevelopmentPipeline("Test Stage");
        testPipeline.AddComponent(new PipelineAction("Unit Tests", PipelineActionType.Test));
        testPipeline.AddComponent(new PipelineAction("SonarQube Analysis", PipelineActionType.Analyse));

        var mainPipeline = new DevelopmentPipeline("CI/CD Pipeline");
        mainPipeline.AddComponent(buildPipeline);
        mainPipeline.AddComponent(testPipeline);
        mainPipeline.AddComponent(new PipelineAction("Deploy to Azure", PipelineActionType.Deploy));

        bool result = mainPipeline.Execute();
        Assert.True(result);
    }

    [Fact]
    public void Pipeline_NestedPipelineFailure_ShouldStopOuter()
    {
        bool deployExecuted = false;

        var buildPipeline = new DevelopmentPipeline("Build Stage");
        buildPipeline.AddComponent(new PipelineAction("Build", PipelineActionType.Build)
        {
            ExecutionLogic = () => false
        });

        var mainPipeline = new DevelopmentPipeline("CI/CD Pipeline");
        mainPipeline.AddComponent(buildPipeline);
        mainPipeline.AddComponent(new PipelineAction("Deploy", PipelineActionType.Deploy)
        {
            ExecutionLogic = () => { deployExecuted = true; return true; }
        });

        bool result = mainPipeline.Execute();
        Assert.False(result);
        Assert.False(deployExecuted);
    }

    [Fact]
    public void Pipeline_RemoveComponent_ShouldWork()
    {
        var pipeline = new DevelopmentPipeline("Pipeline");
        var action = new PipelineAction("Build", PipelineActionType.Build);
        pipeline.AddComponent(action);
        Assert.Single(pipeline.Components);

        pipeline.RemoveComponent(action);
        Assert.Empty(pipeline.Components);
    }

    [Fact]
    public void Pipeline_AllActionTypes_ShouldBeSupported()
    {
        var pipeline = new DevelopmentPipeline("Full Pipeline");
        pipeline.AddComponent(new PipelineAction("Git Clone", PipelineActionType.Sources));
        pipeline.AddComponent(new PipelineAction("NuGet Restore", PipelineActionType.Package));
        pipeline.AddComponent(new PipelineAction("dotnet build", PipelineActionType.Build));
        pipeline.AddComponent(new PipelineAction("NUnit", PipelineActionType.Test));
        pipeline.AddComponent(new PipelineAction("SonarQube", PipelineActionType.Analyse));
        pipeline.AddComponent(new PipelineAction("Azure Deploy", PipelineActionType.Deploy));
        pipeline.AddComponent(new PipelineAction("Copy Files", PipelineActionType.Utility));

        Assert.Equal(7, pipeline.Components.Count);
        Assert.True(pipeline.Execute());
    }
}
