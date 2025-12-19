namespace TagsCloudContainer.Сlients.Console.Parsing;

public abstract class ArgStep
{
    public static readonly ArgStep Unhandled = new UnhandledStep();
    public static ArgStep Consumed(int count) => new HandledStep(count);

    public abstract ArgStep OrElse(Func<ArgStep> next);
    public abstract int NextIndex(int currentIndex);

    private sealed class UnhandledStep : ArgStep
    {
        public override ArgStep OrElse(Func<ArgStep> next) => next();
        public override int NextIndex(int currentIndex) => currentIndex + 1;
    }

    private sealed class HandledStep(int consumed) : ArgStep
    {
        public override ArgStep OrElse(Func<ArgStep> next) => this;
        public override int NextIndex(int currentIndex) => currentIndex + consumed;
    }
}