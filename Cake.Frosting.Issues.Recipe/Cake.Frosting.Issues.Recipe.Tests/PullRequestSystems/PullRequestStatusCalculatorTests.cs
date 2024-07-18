namespace Cake.Frosting.Issues.Recipe.Tests.PullRequestSystems;

public sealed class PullRequestStatusCalculatorTests
{
    public sealed class TheGetPullRequestStatesMethod
    {
        public sealed class TheShouldSetPullRequestStatusParameter
        {
            [Fact]
            public void Should_Set_Suceeded_Status_If_Neither_Issues_Nor_Providers_Are_Reported()
            {
                // Given
                var issueProvidersAndRuns = new List<(IIssueProvider, string)>();
                var issues = new List<IIssue>();
                var shouldSetPullRequestStatus = true;
                var shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun = false;
                string buildIdentifier = null;

                // When
                var result =
                    PullRequestStatusCalculator.GetPullRequestStates(
                        issueProvidersAndRuns,
                        issues,
                        shouldSetPullRequestStatus,
                        shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun,
                        buildIdentifier);

                // Then
                var status = result.ShouldHaveSingleItem();
                status.Name.ShouldBe("Issues");
                status.Genre.ShouldBe("Cake.Issues.Recipe");
                status.State.ShouldBe(PullRequestStatusState.Succeeded);
                status.Description.ShouldBe("No issues found");
            }

            [Fact]
            public void Should_Set_Succeeded_Status_If_No_Issue_Is_Reported()
            {
                // Given
                var issueProvider = new FakeIssueProvider(new FakeLog());
                var run = "Run 1";
                var issueProvidersAndRuns = new List<(IIssueProvider, string)>()
                {
                    new (issueProvider, run)
                };
                var issues = new List<IIssue>();
                var shouldSetPullRequestStatus = true;
                var shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun = false;
                string buildIdentifier = null;

                // When
                var result =
                    PullRequestStatusCalculator.GetPullRequestStates(
                        issueProvidersAndRuns,
                        issues,
                        shouldSetPullRequestStatus,
                        shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun,
                        buildIdentifier);

                // Then
                var status = result.ShouldHaveSingleItem();
                status.Name.ShouldBe("Issues");
                status.Genre.ShouldBe("Cake.Issues.Recipe");
                status.State.ShouldBe(PullRequestStatusState.Succeeded);
                status.Description.ShouldBe("No issues found");
            }

            [Fact]
            public void Should_Set_Failed_Status_If_Issue_And_Provider_Is_Reported()
            {
                // Given
                var issueProvider = new FakeIssueProvider(new FakeLog());
                var run = "Run 1";
                var issueProvidersAndRuns = new List<(IIssueProvider, string)>()
                {
                    new (issueProvider, run)
                };
                var issues = new List<IIssue>
                {
                    IssueBuilder
                        .NewIssue("Message", issueProvider.ProviderType, issueProvider.ProviderName)
                        .ForRun(run)
                        .Create(),
                };
                var shouldSetPullRequestStatus = true;
                var shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun = false;
                string buildIdentifier = null;

                // When
                var result =
                    PullRequestStatusCalculator.GetPullRequestStates(
                        issueProvidersAndRuns,
                        issues,
                        shouldSetPullRequestStatus,
                        shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun,
                        buildIdentifier);

                // Then
                var status = result.ShouldHaveSingleItem();
                status.Name.ShouldBe("Issues");
                status.Genre.ShouldBe("Cake.Issues.Recipe");
                status.State.ShouldBe(PullRequestStatusState.Failed);
                status.Description.ShouldBe("Found 1 issues");
            }

            [Fact]
            public void Should_Set_Failed_Status_If_Issue_But_No_Provider_Is_Reported()
            {
                // Given
                var issueProvidersAndRuns = new List<(IIssueProvider, string)>();
                var issues = new List<IIssue>
                {
                    IssueBuilder
                        .NewIssue("Message", "ProviderType", "Provider Name")
                        .Create(),
                };
                var shouldSetPullRequestStatus = true;
                var shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun = false;
                string buildIdentifier = null;

                // When
                var result =
                    PullRequestStatusCalculator.GetPullRequestStates(
                        issueProvidersAndRuns,
                        issues,
                        shouldSetPullRequestStatus,
                        shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun,
                        buildIdentifier);

                // Then
                var status = result.ShouldHaveSingleItem();
                status.Name.ShouldBe("Issues");
                status.Genre.ShouldBe("Cake.Issues.Recipe");
                status.State.ShouldBe(PullRequestStatusState.Failed);
                status.Description.ShouldBe("Found 1 issues");
            }
        }

        public sealed class TheShouldSetSeparatePullRequestStatusForEachIssueProviderAndRunParameter
        {
            [Fact]
            public void Should_Set_Succeeded_Status_If_No_Issue_Is_Reported()
            {
                // Given
                var issueProvider = new FakeIssueProvider(new FakeLog());
                var issueProvidersAndRuns = new List<(IIssueProvider, string)>()
                {
                    new (issueProvider, null)
                };
                var issues = new List<IIssue>();
                var shouldSetPullRequestStatus = false;
                var shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun = true;
                string buildIdentifier = null;

                // When
                var result =
                    PullRequestStatusCalculator.GetPullRequestStates(
                        issueProvidersAndRuns,
                        issues,
                        shouldSetPullRequestStatus,
                        shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun,
                        buildIdentifier);

                // Then
                var status = result.ShouldHaveSingleItem();
                status.Name.ShouldBe("Issues-Fake Issue Provider");
                status.Genre.ShouldBe("Cake.Issues.Recipe");
                status.State.ShouldBe(PullRequestStatusState.Succeeded);
                status.Description.ShouldBe("No issues found for Fake Issue Provider");
            }

            [Fact]
            public void Should_Set_Failed_Status_If_Issue_Is_Reported()
            {
                // Given
                var issueProvider = new FakeIssueProvider(new FakeLog());
                var run = "Run 1";
                var issueProvidersAndRuns = new List<(IIssueProvider, string)>()
                {
                    new (issueProvider, run)
                };
                var issues = new List<IIssue>
                {
                    IssueBuilder
                        .NewIssue("Message", issueProvider.ProviderType, issueProvider.ProviderName)
                        .ForRun(run)
                        .Create(),
                };
                var shouldSetPullRequestStatus = false;
                var shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun = true;
                string buildIdentifier = null;

                // When
                var result =
                    PullRequestStatusCalculator.GetPullRequestStates(
                        issueProvidersAndRuns,
                        issues,
                        shouldSetPullRequestStatus,
                        shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun,
                        buildIdentifier);

                // Then
                var status = result.ShouldHaveSingleItem();
                status.Name.ShouldBe("Issues-Fake Issue Provider (Run 1)");
                status.Genre.ShouldBe("Cake.Issues.Recipe");
                status.State.ShouldBe(PullRequestStatusState.Failed);
                status.Description.ShouldBe("Found 1 issues for Fake Issue Provider (Run 1)");
            }

            [Fact]
            public void Should_Set_Status_For_Each_Provider()
            {
                // Given
                var issueProvider = new FakeIssueProvider(new FakeLog());
                var issueProvider2 = new FakeIssueProvider2(new FakeLog());
                var issueProvidersAndRuns = new List<(IIssueProvider, string)>()
                {
                    new (issueProvider, null),
                    new (issueProvider2, null)
                };
                var issues = new List<IIssue>
                {
                    IssueBuilder
                        .NewIssue("Message", issueProvider.ProviderType, issueProvider.ProviderName)
                        .Create(),
                };
                var shouldSetPullRequestStatus = false;
                var shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun = true;
                string buildIdentifier = null;

                // When
                var result =
                    PullRequestStatusCalculator.GetPullRequestStates(
                        issueProvidersAndRuns,
                        issues,
                        shouldSetPullRequestStatus,
                        shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun,
                        buildIdentifier);

                // Then
                result.Count().ShouldBe(2);

                result.ShouldContain(x =>
                    x.Name == "Issues-Fake Issue Provider" &&
                    x.Genre == "Cake.Issues.Recipe" &&
                    x.State == PullRequestStatusState.Failed &&
                    x.Description == "Found 1 issues for Fake Issue Provider");

                result.ShouldContain(x =>
                    x.Name == "Issues-Fake Issue Provider 2" &&
                    x.Genre == "Cake.Issues.Recipe" &&
                    x.State == PullRequestStatusState.Succeeded &&
                    x.Description == "No issues found for Fake Issue Provider 2");
            }

            [Fact]
            public void Should_Set_Status_For_Each_Run()
            {
                // Given
                var issueProvider = new FakeIssueProvider(new FakeLog());
                var run1 = "Run 1";
                var run2 = "Run 2";
                var issueProvidersAndRuns = new List<(IIssueProvider, string)>()
                {
                    new (issueProvider, run1),
                    new (issueProvider, run2)
                };
                var issues = new List<IIssue>
                {
                    IssueBuilder
                        .NewIssue("Message", issueProvider.ProviderType, issueProvider.ProviderName)
                        .ForRun(run1)
                        .Create(),
                };
                var shouldSetPullRequestStatus = false;
                var shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun = true;
                string buildIdentifier = null;

                // When
                var result =
                    PullRequestStatusCalculator.GetPullRequestStates(
                        issueProvidersAndRuns,
                        issues,
                        shouldSetPullRequestStatus,
                        shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun,
                        buildIdentifier);

                // Then
                result.Count().ShouldBe(2);

                result.ShouldContain(x =>
                    x.Name == "Issues-Fake Issue Provider (Run 1)" &&
                    x.Genre == "Cake.Issues.Recipe" &&
                    x.State == PullRequestStatusState.Failed &&
                    x.Description == "Found 1 issues for Fake Issue Provider (Run 1)");

                result.ShouldContain(x =>
                    x.Name == "Issues-Fake Issue Provider (Run 2)" &&
                    x.Genre == "Cake.Issues.Recipe" &&
                    x.State == PullRequestStatusState.Succeeded &&
                    x.Description == "No issues found for Fake Issue Provider (Run 2)");
            }

            [Fact]
            public void Should_Handle_Issue_Provider_With_Different_Type_But_Same_Run()
            {
                // Given
                var issueProvider = new FakeIssueProvider(new FakeLog());
                var issueProvider2 = new FakeIssueProvider2(new FakeLog());
                var run = "Run 1";
                var issueProvidersAndRuns = new List<(IIssueProvider, string)>()
                {
                    new (issueProvider, run),
                    new (issueProvider2, run)
                };
                var issues = new List<IIssue>
                {
                    IssueBuilder
                        .NewIssue("Message", issueProvider.ProviderType, issueProvider.ProviderName)
                        .ForRun(run)
                        .Create(),
                };
                var shouldSetPullRequestStatus = false;
                var shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun = true;
                string buildIdentifier = null;


                // When
                var result =
                    PullRequestStatusCalculator.GetPullRequestStates(
                        issueProvidersAndRuns,
                        issues,
                        shouldSetPullRequestStatus,
                        shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun,
                        buildIdentifier);

                // Then
                result.Count().ShouldBe(2);

                result.ShouldContain(x => 
                    x.Name == "Issues-Fake Issue Provider (Run 1)" &&
                    x.Genre == "Cake.Issues.Recipe" &&
                    x.State == PullRequestStatusState.Failed &&
                    x.Description == "Found 1 issues for Fake Issue Provider (Run 1)");

                result.ShouldContain(x =>
                    x.Name == "Issues-Fake Issue Provider 2 (Run 1)" &&
                    x.Genre == "Cake.Issues.Recipe" &&
                    x.State == PullRequestStatusState.Succeeded &&
                    x.Description == "No issues found for Fake Issue Provider 2 (Run 1)");
            }

            [Fact]
            public void Should_Handle_Issue_Provider_With_Same_Type_But_Different_Name()
            {
                // Given
                var issueProvider = new FakeIssueProvider(new FakeLog());
                var run = "Run 1";
                var issueProvidersAndRuns = new List<(IIssueProvider, string)>()
                {
                    new (issueProvider, run)
                };
                var issues = new List<IIssue>
                {
                    IssueBuilder
                        .NewIssue("Message", issueProvider.ProviderType, "DifferentIssueProviderName")
                        .ForRun(run)
                        .Create(),
                };
                var shouldSetPullRequestStatus = false;
                var shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun = true;
                string buildIdentifier = null;


                // When
                var result =
                    PullRequestStatusCalculator.GetPullRequestStates(
                        issueProvidersAndRuns,
                        issues,
                        shouldSetPullRequestStatus,
                        shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun,
                        buildIdentifier);

                // Then
                var status = result.ShouldHaveSingleItem();
                status.Name.ShouldBe("Issues-DifferentIssueProviderName (Run 1)");
                status.Genre.ShouldBe("Cake.Issues.Recipe");
                status.State.ShouldBe(PullRequestStatusState.Failed);
                status.Description.ShouldBe("Found 1 issues for DifferentIssueProviderName (Run 1)");
            }

            [Fact]
            public void Should_Handle_Everything_Together()
            {
                // Given
                var issueProvider = new FakeIssueProvider(new FakeLog());
                var issueProvider2 = new FakeIssueProvider2(new FakeLog());
                var run1 = "Run 1";
                var run2 = "Run 2";
                var issueProvidersAndRuns = new List<(IIssueProvider, string)>()
                {
                    new (issueProvider, run1),
                    new (issueProvider, run2),
                    new (issueProvider2, run2)
                };
                var issues = new List<IIssue>
                {
                    IssueBuilder
                        .NewIssue("Message 1", issueProvider.ProviderType, issueProvider.ProviderName)
                        .ForRun(run1)
                        .Create(),
                    IssueBuilder
                        .NewIssue("Message 2", issueProvider.ProviderType, issueProvider.ProviderName)
                        .ForRun(run1)
                        .Create(),
                    IssueBuilder
                        .NewIssue("Message", issueProvider2.ProviderType, "DifferentIssueProviderName")
                        .ForRun(run2)
                        .Create(),
                    IssueBuilder
                        .NewIssue("Message", "AnotherIssueProvider", "Another Issue Provider")
                        .Create(),
                };
                var shouldSetPullRequestStatus = false;
                var shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun = true;
                string buildIdentifier = null;


                // When
                var result =
                    PullRequestStatusCalculator.GetPullRequestStates(
                        issueProvidersAndRuns,
                        issues,
                        shouldSetPullRequestStatus,
                        shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun,
                        buildIdentifier)
                    .ToList();

                // Then
                result.Count.ShouldBe(4);

                result.ShouldContain(x =>
                    x.Name == "Issues-Fake Issue Provider (Run 1)" &&
                    x.Genre == "Cake.Issues.Recipe" &&
                    x.State == PullRequestStatusState.Failed &&
                    x.Description == "Found 2 issues for Fake Issue Provider (Run 1)");

                result.ShouldContain(x =>
                    x.Name == "Issues-Fake Issue Provider (Run 2)" &&
                    x.Genre == "Cake.Issues.Recipe" &&
                    x.State == PullRequestStatusState.Succeeded &&
                    x.Description == "No issues found for Fake Issue Provider (Run 2)");

                result.ShouldContain(x =>
                    x.Name == "Issues-DifferentIssueProviderName (Run 2)" &&
                    x.Genre == "Cake.Issues.Recipe" &&
                    x.State == PullRequestStatusState.Failed &&
                    x.Description == "Found 1 issues for DifferentIssueProviderName (Run 2)");

                result.ShouldContain(x =>
                    x.Name == "Issues-Another Issue Provider" &&
                    x.Genre == "Cake.Issues.Recipe" &&
                    x.State == PullRequestStatusState.Failed &&
                    x.Description == "Found 1 issues for Another Issue Provider");
            }

            [Fact]
            public void Should_Consider_Run()
            {
                // Given
                var issueProvider = new FakeIssueProvider(new FakeLog());
                var run = "Run 1";
                var issueProvidersAndRuns = new List<(IIssueProvider, string)>()
                {
                    new (issueProvider, run)
                };
                var issues = new List<IIssue>();
                var shouldSetPullRequestStatus = false;
                var shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun = true;
                string buildIdentifier = null;

                // When
                var result =
                    PullRequestStatusCalculator.GetPullRequestStates(
                        issueProvidersAndRuns,
                        issues,
                        shouldSetPullRequestStatus,
                        shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun,
                        buildIdentifier);

                // Then
                var status = result.ShouldHaveSingleItem();
                status.Name.ShouldBe("Issues-Fake Issue Provider (Run 1)");
                status.Genre.ShouldBe("Cake.Issues.Recipe");
                status.State.ShouldBe(PullRequestStatusState.Succeeded);
                status.Description.ShouldBe("No issues found for Fake Issue Provider (Run 1)");
            }
        }

        public sealed class TheBuildIdentifierParameter
        {
            [Fact]
            public void Should_Consider_Build_Identifier()
            {
                // Given
                var issueProvidersAndRuns = new List<(IIssueProvider, string)>();
                var issues = new List<IIssue>
                {
                    IssueBuilder
                        .NewIssue("Message", "ProviderType", "Provider Name")
                        .Create(),
                };
                var shouldSetPullRequestStatus = true;
                var shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun = false;
                string buildIdentifier = "Build Identifier";

                // When
                var result =
                    PullRequestStatusCalculator.GetPullRequestStates(
                        issueProvidersAndRuns,
                        issues,
                        shouldSetPullRequestStatus,
                        shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun,
                        buildIdentifier);

                // Then
                var status = result.ShouldHaveSingleItem();
                status.Name.ShouldBe("Issues-Build Identifier");
                status.Genre.ShouldBe("Cake.Issues.Recipe");
                status.State.ShouldBe(PullRequestStatusState.Failed);
                status.Description.ShouldBe("Found 1 issues in build Build Identifier");
            }
        }
    }
}