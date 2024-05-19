namespace Cake.Frosting.Issues.Recipe
{
    using Cake.Common.Diagnostics;
    using Cake.Core;
    using System.Diagnostics;
    using System.Reflection;

    /// <summary>
    /// Base class for parameters and state of the build run.
    /// </summary>
    /// <typeparam name="TParameters">Type of the parameters description.</typeparam>
    /// <typeparam name="TState">Type of the build state description.</typeparam>
    public abstract class IssuesContext<TParameters, TState> : FrostingContext, IIssuesContext
        where TParameters: IIssuesParameters
        where TState: IIssuesState
    {
        private readonly Lazy<TParameters> parameters;
        private readonly Lazy<TState> state;

        /// <summary>
        /// Gets input parameters.
        /// </summary>
        public TParameters Parameters => this.parameters.Value;

        /// <summary>
        /// Gets the mutable state of the build run.
        /// </summary>
        public TState State => this.state.Value;

        /// <inheritdoc />
        IIssuesParameters IIssuesContext.Parameters => this.Parameters;

        /// <inheritdoc />
        IIssuesState IIssuesContext.State => this.State;

        /// <summary>
        /// Creates a new instance of the <see cref="IssuesContext{TParameters, TState}"/> class.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        protected IssuesContext(ICakeContext context)
            : base(context)
        {
            var assembly = Assembly.GetAssembly(typeof(IssuesTask));
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            context.Information("Initializing Cake.Frosting.Issues.Recipe (Version {0})...", versionInfo.FileVersion);

            this.parameters = new Lazy<TParameters>(() => this.CreateIssuesParameters());
            this.state = new Lazy<TState>(() => this.CreateIssuesState());
        }

        /// <summary>
        /// Factory method to instantiate <see cref="Parameters"/>.
        /// </summary>
        /// <returns>Object to store input parameters.</returns>
        protected abstract TParameters CreateIssuesParameters();

        /// <summary>
        /// Factory method to instantiate <see cref="State"/>.
        /// </summary>
        /// <returns>Object to store mutable state of the build run.</returns>
        protected abstract TState CreateIssuesState();
    }
}
