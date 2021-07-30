namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Parameters for reporting.
    /// </summary>
    public class IssuesParametersReporting
    {
        // TODO Requires Cake.Issues.Reporting.Generic working with Frosting https://github.com/cake-contrib/Cake.Issues.Reporting.Generic/issues/361

        /// <summary>
        /// Gets or sets a value indicating whether full issues report should be created.
        /// Default value is <c>true</c>.
        /// </summary>
        public bool ShouldCreateFullIssuesReport { get; set; } = false;
    }
}