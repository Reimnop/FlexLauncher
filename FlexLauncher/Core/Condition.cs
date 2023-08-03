namespace FlexLauncher.Core;

public abstract class Condition
{
    public abstract bool Check(LaunchContext context);
}