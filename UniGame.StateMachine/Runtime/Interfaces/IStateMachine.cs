﻿namespace UniGreenModules.UniStateMachine.Runtime.Interfaces
{
    using UniCore.Runtime.Interfaces;

    public interface IStateMachine<TState> : 
        ICommonExecutor<TState>
    {

    }
}