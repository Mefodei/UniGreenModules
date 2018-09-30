﻿using UniStateMachine;
using UnityEngine;
using UnityToolsModule.Tools.UnityTools.ActorEntityModel;

namespace Tools.ActorModel
{
	public class UniActor : MonoBehaviour, IBehaviourObject
	{
		private ProxyActor _actor;
		private IEntity _entity;

		
		public bool IsActive => _actor.IsActive;
		
		
		public void SetEntity(IEntity entity) {

			_entity = entity;

		}

		public void SetBehaviour(UniStateBehaviour behaviour)
		{		
			behaviour.Initialize(_entity);
			_actor.SetBehaviour(behaviour);	
		}

		public void SetEnabled(bool state)
		{
			_actor.SetState(state);
		}

		#region private methods


		protected virtual void Awake()
		{

			_actor = new ProxyActor(Activate, Deactivate, OnDispose);

		}

		protected virtual void Start()
		{

		}

		protected virtual void Activate()
		{
		}

		protected virtual void Deactivate()
		{

		}

		protected virtual void OnDispose()
		{
		}

		public virtual void Dispose()
		{
			_actor.Dispose();
		}

		#endregion


	}
}