// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Elastic.Apm.Api;

namespace MaaCopilotServer.Api.Wrappers
{
    /// <summary>
    /// The wrapper class of <see cref="Elastic.Apm.Agent"/>.
    /// </summary>
    public interface IElasticApmAgentWrapper
    {
        /// <summary>
        /// Gets the value of <see cref="Elastic.Apm.Agent.Tracer"/>.
        /// </summary>
        ITracer Tracer { get; }
    }
}
