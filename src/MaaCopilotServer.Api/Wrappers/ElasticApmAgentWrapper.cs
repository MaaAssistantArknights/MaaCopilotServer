// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using Elastic.Apm.Api;

namespace MaaCopilotServer.Api.Wrappers
{
    /// <inheritdoc cref="IElasticApmAgentWrapper"/>
    [ExcludeFromCodeCoverage]
    public class ElasticApmAgentWrapper : IElasticApmAgentWrapper
    {
        /// <inheritdoc/>
        public ITracer Tracer => Elastic.Apm.Agent.Tracer;
    }
}
