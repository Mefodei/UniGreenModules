﻿using System;
using Assets.Tools.Utils;

public class DisposableAction<TArg> : IDisposable
{
    private Action<TArg> _onDisposed;
    private TArg _arg;
    
    public bool IsDisposed { get; protected set; }
    
    public void Initialize(Action<TArg> action, TArg arg)
    {
        IsDisposed = false;
        _onDisposed = action;
    }

    public void Reset()
    {
        IsDisposed = true;
        _onDisposed = null;
        _arg = default(TArg);
    }
    
    public void Dispose()
    {
        
        if (IsDisposed) return;
        _onDisposed?.Invoke(_arg);
        Reset();
        
        //return to pool
        this.Despawn();
        
    }
}