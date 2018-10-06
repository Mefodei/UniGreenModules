﻿using System;
using System.Collections;
using Assets.Tools.UnityTools.ActorEntityModel;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using Assets.Tools.UnityTools.StateMachine.UniStateMachine;
using UnityEngine;

public class ActorComponent : EntityComponent, IDisposable {
		
    private Actor _actor = new Actor();

    #region inspector data

    [SerializeField]
	private bool _launchOnStart = true;
    
    /// <summary>
    /// behaviour SO
    /// </summary>
    [SerializeField]
    private UniStateBehaviour _stateObject;

    /// <summary>
    /// behaviour component
    /// </summary>
    [SerializeField]
    private UniStateComponent _stateComponent;

    /// <summary>
    /// actor model data
    /// </summary>
    [SerializeField]
    private ActorInfo _actorInfo;

    #endregion

    public IContextState<IEnumerator> State { get; protected set; }

    public Actor Actor => _actor;

	public void Dispose() {

        Actor.SetEnabled(false);
	    Actor.Dispose();
        Entity?.Release();

	}
    
    protected IContextState<IEnumerator> GetState()
    {
        var model = Entity.Get<ActorModel>();
        var parameterBehaviour = _stateObject ? 
                (IContextState<IEnumerator>) _stateObject : 
                _stateComponent;

        if (model?.Behaviour == null && parameterBehaviour == null)
            return null;

        var state = model?.Behaviour == null ?
            parameterBehaviour:
            model.Behaviour.Value;

        return state;

    }

	protected virtual void Activate()
	{
        Actor.SetEnabled(true);
    }

    protected virtual void Deactivate() 
	{
	    Actor.SetEnabled(false);
	}

	private void OnDisable()
	{
        Deactivate();
	}

	private void OnEnable() {

        Activate();

	}

    private void Start()
    {
        if (_launchOnStart)
        {
            var state = GetState();
            Actor.SetBehaviour(state);
        }
    }

    // Use this for initialization
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        var model = _actorInfo?.Create();
        model?.AddContextData(Entity);
        InitializeContext();
        Actor.SetEntity(Entity);
    }

    protected virtual void InitializeContext(){}

    private void OnDestroy() {
		Dispose();
	}

}
