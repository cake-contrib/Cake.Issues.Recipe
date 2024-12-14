namespace Cake.Frosting.Issues.Recipe.Tests;

using System.Collections.Generic;
using Cake.Core.Diagnostics;

/// <summary>
/// Additional Implementation of a <see cref="FakeIssueProvider"/> for use in test cases.
/// </summary>
internal class FakeIssueProvider2 : FakeIssueProvider
{

    /// <summary>
    /// Initializes a new instance of the <see cref="FakeIssueProvider2"/> class.
    /// </summary>
    /// <param name="log">The Cake log instance.</param>
    public FakeIssueProvider2(ICakeLog log)
        : base(log)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FakeIssueProvider2"/> class.
    /// </summary>
    /// <param name="log">The Cake log instance.</param>
    /// <param name="issues">Issues which should be returned by the issue provider.</param>
    public FakeIssueProvider2(ICakeLog log, IEnumerable<IIssue> issues)
        : base(log, issues)
    {
    }

    /// <inheritdoc/>
    public override string ProviderName => "Fake Issue Provider 2";
}
