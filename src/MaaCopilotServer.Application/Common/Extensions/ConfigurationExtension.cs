// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Attributes;
using MaaCopilotServer.Domain.Extensions;
using Microsoft.Extensions.Configuration;

namespace MaaCopilotServer.Application.Common.Extensions;

public static class ConfigurationExtension
{
    /// <summary>
    ///     获取配置项实例
    /// </summary>
    /// <param name="configuration">IConfiguration 实现</param>
    /// <typeparam name="T">Options 类</typeparam>
    /// <returns><typeparamref name="T" /> 的实例</returns>
    /// <exception cref="ArgumentNullException"><typeparamref name="T" /> 无 <see cref="OptionNameAttribute" /> 修饰</exception>
    public static T GetOption<T>(this IConfiguration configuration) where T : class, new()
    {
        var option = new T();
        var attr = typeof(T).ReadAttribute<OptionNameAttribute>();
        if (attr is null)
        {
            throw new ArgumentNullException(typeof(T).FullName);
        }

        configuration.GetSection(attr.OptionName).Bind(option);
        return option;
    }
}
