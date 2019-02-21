﻿using System.Collections.Generic;
using UnityEngine;
using UniTools.UniUiSystem;

namespace UniUiSystem
{
    public class UiModule : UiViewBehaviour
    {
        #region inspector data
        
        [SerializeField]
        private UiModuleSlotsContainer _slots = new UiModuleSlotsContainer();

        [SerializeField]
        private UiTriggersContainer _triggers = new UiTriggersContainer();

        #endregion
    
        #region public properties

        public IContainer<IUiModuleSlot> Slots => _slots;
        
        public ITriggersContainer Triggers => _triggers;
        
        #endregion

        #region public methods

        public void AddTrigger(IInteractionTrigger trigger)
        {
            _triggers.Add(trigger);
        }

        public void AddSlot(IUiModuleSlot slot)
        {
            _slots.Add(slot);
        }
        
        #endregion

        protected override void OnInitialize()
        {
            _triggers.Initialize();
        }
    }
}