﻿using System;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;

namespace Assets.Tools.UnityTools.Common
{
    public class DisposableAction<TArg> : IDisposableItem
    {
        private Action<TArg> _onDisposed;
        private TArg _arg;
    
        public bool IsDisposed { get; protected set; }
    
        public void Initialize(Action<TArg> action, TArg arg)
        {
            IsDisposed = false;
            _onDisposed = action;
            _arg = arg;
        }

        public void Reset()
        {
            IsDisposed = true;
            _onDisposed = null;
            _arg = default(TArg);
        }
    
        public void Dispose()
        {

            if (!IsDisposed)
            {
                _onDisposed?.Invoke(_arg);
            }
            
            Reset();
        
            //return to pool
            this.Despawn();
        
        }
    }
}