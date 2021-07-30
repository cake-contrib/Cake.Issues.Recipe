using Cake.Core;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Parameters and state for the build run.
    /// </summary>
    public class IssuesContext : FrostingContext
    {
        /// <summary>
        /// Input parameters.
        /// </summary>
        public IssuesParameters Parameters { get; }

        /// <summary>
        /// Mutable state of the build run.
        /// </summary>
        public IssuesState State { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="IssuesContext"/> class.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        public IssuesContext(ICakeContext context)
            : base(context)
        {
            this.Parameters = new IssuesParameters();
            this.State = new IssuesState(this);
        }
    }
}
