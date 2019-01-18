﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Tools.UnityTools.Interfaces;
using UniStateMachine.Nodes;
using UnityEngine;
using UnityTools.UniNodeEditor.Connections;

namespace UniStateMachine.CommonNodes
{
    public class UniModuleNode : UniNode
    {
        /// <summary>
        /// target module adapter
        /// </summary>
        [SerializeField] private NodeModuleAdapter _adapter;

        [HideInInspector] [SerializeField] private List<string> _modulePortValues = new List<string>();

        public List<string> ModulePortValues => _modulePortValues;

        public INodeModuleAdapter Adapter => _adapter;

        public override void UpdatePortsCache()
        {
            base.UpdatePortsCache();

            UpdateModulePorts();
        }

        /// <summary>
        /// update adapter for target context
        /// </summary>
        protected override IEnumerator ExecuteState(IContext context)
        {
            yield return base.ExecuteState(context);

            var lifeTime = GetLifeTime(context);
            _adapter.Bind(context, lifeTime);

            while (IsActive(context))
            {
                yield return null;
                _adapter.Execute(context, lifeTime);
            }
        }

        private void UpdateModulePorts()
        {
            //register all module ports
            _modulePortValues.Clear();

            if (!_adapter)
            {
                return;
            }

            _adapter.Initialize();
            _modulePortValues.AddRange(_adapter.Ports.Select(x => x.Name));

            foreach (var port in _adapter.Ports)
            {
                var item = this.UpdatePortValue(port.Name,port.Direction);
                port.Bind(item.value);
            }
        }
    }
}