using FlexLauncher.Core;

namespace FlexLauncher.Data;

/// <summary>
/// A list of launch arguments
/// </summary>
public class ArgumentList
{
    private readonly List<Condition> conditions;
    private readonly List<string> arguments;

    public ArgumentList(List<Condition> conditions, List<string> arguments)
    {
        this.conditions = conditions;
        this.arguments = arguments;
    }

    public IEnumerable<string> EnumerateArguments(LaunchContext context)
    {
        if (!CheckConditions(context))
        {
            yield break;
        }

        foreach (var argument in arguments)
        {
            yield return context.ParseVariables(argument);
        }
    }
    
    private bool CheckConditions(LaunchContext context)
    {
        return conditions.All(condition => condition.Check(context));
    }
}