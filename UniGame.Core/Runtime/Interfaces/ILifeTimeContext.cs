namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using DataFlow.Interfaces;

    public interface ILifeTimeContext
    {
        ILifeTime LifeTime { get; }
    }
}