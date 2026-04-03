# Avans DevOps - SOA3 Eindopdracht

Scrum/DevOps projectmanagement systeem (application core) voor de eindopdracht Softwareontwerp & -architectuur 3 (INVT3.3).

## Projectstructuur

```
AvansDevOps/
├── AvansDevOps.sln                    # Solution file
├── AvansDevOps/                       # Application Core
│   ├── Domain/
│   │   ├── Project.cs                 # Root aggregate
│   │   ├── Person.cs                  # Persoon + rollen
│   │   ├── BacklogItem.cs             # Backlog item (State + Observer)
│   │   ├── Activity.cs                # Activiteit binnen backlog item
│   │   ├── Sprint.cs                  # Abstract sprint (State + Observer)
│   │   ├── ReviewSprint.cs            # Sprint met review (Factory Method)
│   │   ├── ReleaseSprint.cs           # Sprint met release (Factory Method)
│   │   ├── SprintMember.cs
│   │   ├── States/                    # STATE PATTERN
│   │   │   ├── IBacklogItemState.cs
│   │   │   ├── TodoState.cs
│   │   │   ├── DoingState.cs
│   │   │   ├── ReadyForTestingState.cs
│   │   │   ├── TestingState.cs
│   │   │   ├── TestedState.cs
│   │   │   ├── DoneState.cs
│   │   │   ├── ISprintState.cs
│   │   │   ├── SprintCreatedState.cs
│   │   │   ├── SprintInProgressState.cs
│   │   │   ├── SprintFinishedState.cs
│   │   │   ├── SprintReleasingState.cs
│   │   │   ├── SprintReleasedState.cs
│   │   │   ├── SprintCancelledState.cs
│   │   │   ├── SprintReviewState.cs
│   │   │   └── SprintClosedState.cs
│   │   ├── Notifications/             # OBSERVER PATTERN
│   │   │   ├── ISubscriber.cs
│   │   │   ├── INotificationPublisher.cs
│   │   │   ├── EmailSubscriber.cs
│   │   │   └── SlackSubscriber.cs
│   │   ├── Factory/                   # FACTORY METHOD PATTERN
│   │   │   ├── ISprintFactory.cs
│   │   │   ├── ReviewSprintFactory.cs
│   │   │   └── ReleaseSprintFactory.cs
│   │   ├── Pipeline/                  # COMPOSITE PATTERN
│   │   │   ├── IPipelineComponent.cs
│   │   │   ├── PipelineAction.cs
│   │   │   └── DevelopmentPipeline.cs
│   │   ├── Report/                    # STRATEGY + DECORATOR PATTERNS
│   │   │   ├── IReportExportStrategy.cs
│   │   │   ├── PdfExportStrategy.cs
│   │   │   ├── PngExportStrategy.cs
│   │   │   ├── IReportComponent.cs
│   │   │   ├── ReportBody.cs
│   │   │   ├── ReportDecorator.cs
│   │   │   ├── HeaderDecorator.cs
│   │   │   ├── FooterDecorator.cs
│   │   │   └── SprintReport.cs
│   │   ├── Forum/
│   │   │   ├── Forum.cs
│   │   │   ├── Discussion.cs
│   │   │   └── Message.cs
│   │   └── SCM/
│   │       └── Repository.cs
│   └── Interfaces/
│       └── IRepositories.cs
├── AvansDevOps.Tests/                 # Unit Tests
│   ├── BacklogItemStateTests.cs
│   ├── SprintStateTests.cs
│   ├── NotificationTests.cs
│   ├── PipelineTests.cs
│   ├── ReportTests.cs
│   ├── ForumTests.cs
│   └── SprintFactoryTests.cs
├── diagrams/                          # UML Diagrammen (Mermaid)
│   ├── class-diagram-domain.mmd
│   ├── class-diagram-patterns.mmd
│   ├── state-diagram-backlogitem.mmd
│   └── state-diagram-sprint.mmd
└── .github/workflows/
    └── build-and-analyze.yml          # CI/CD Pipeline
```

## Design Patterns (6)

| # | Pattern         | Type        | Toepassing                              |
|---|-----------------|-------------|-----------------------------------------|
| 1 | State           | Behavioral  | BacklogItem lifecycle + Sprint lifecycle |
| 2 | Observer        | Behavioral  | Notificaties (Email, Slack)             |
| 3 | Factory Method  | Creational  | Sprint-aanmaak (Review/Release)         |
| 4 | Composite       | Structural  | Development Pipeline                    |
| 5 | Strategy        | Behavioral  | Report export (PDF, PNG)                |
| 6 | Decorator       | Structural  | Report headers/footers                  |

## Bouwen en Testen

```bash
# Restore dependencies
dotnet restore

# Build
dotnet build

# Run tests
dotnet test

# Run tests met coverage
dotnet test --collect:"XPlat Code Coverage"
```

## SonarCloud

De CI/CD pipeline is geconfigureerd via GitHub Actions. Bij elke push naar `main` wordt automatisch:
1. De code gebouwd
2. Tests uitgevoerd met coverage
3. SonarCloud analyse gedraaid

Target: **Quality Gate A** (Sonar way).
