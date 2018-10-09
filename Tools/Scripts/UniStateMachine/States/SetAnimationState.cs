﻿using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.UniStateMachine;
using UnityEngine;

namespace GamePlay.States {
    
    [CreateAssetMenu(menuName = "States/States/SetAnimationState", fileName = "SetAnimationState")]
    public class SetAnimationState : UniStateBehaviour {

        #region inspector data
        
        [SerializeField]
        private string _animation;

        #endregion
        
        private int _animationId;

        protected override IEnumerator ExecuteState(IContext context) {

            var animator = context.Get<Animator>();
            _animationId = Animator.StringToHash(_animation);
            
            animator.Play(_animationId);
            yield break;

        }

    }

}