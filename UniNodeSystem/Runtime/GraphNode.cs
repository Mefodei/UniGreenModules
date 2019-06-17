﻿namespace UniGreenModules.UniNodeSystem.Runtime
{
    using System.Collections;
    using Extensions;
    using Interfaces;
    using Runtime;
    using UniCore.Runtime.AsyncOperations;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool;
    using UniCore.Runtime.Rx.Extensions;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UniTools.UniRoutine.Runtime;

    public class GraphNode : UniNode
    {
        private IUniGraph graphInstance;
        
#region inspector data

        [Tooltip("should we await target graph before pass data to output or not")]
        [SerializeField]
        public bool awaitGraph = false;
        
        /// <summary>
        /// cached addressable asset name
        /// </summary>
        [HideInInspector]
        public string graphName;
        
        /// <summary>
        /// target graph resource
        /// </summary>
        public AssetReferenceGameObject graphReference;

#endregion

        /// <summary>
        /// graph addressable instance loading handle
        /// </summary>
        public AsyncOperationHandle<GameObject> GraphInstanceHandle => graphReference.LoadAssetAsync();

        public override string GetName()
        {
            return string.IsNullOrEmpty(graphName) ? base.GetName() : graphName;
        }

        
        protected override IEnumerator OnExecuteState(IContext context)
        {         
            
            //await graph loading
            yield return GraphInstanceHandle.Task.AwaitTask();
            //spawn graph
            graphInstance = GraphInstanceHandle.Result.Spawn<IUniGraph>();
            
            //despawn when execution finished
            var lifeTime = LifeTime;
            lifeTime.AddCleanUpAction(() => {
                                          graphInstance.AssetInstance.Despawn();
                                          graphInstance = null;
                                      });
            
            graphInstance.UpdatePortsCache();
            
            //bind node ports to graph nodes
            BindGraphPorts(graphInstance);
            
            if (awaitGraph) {
                yield return graphInstance.Execute(context);
            }
            else {
                graphInstance.Execute(context).
                    RunWithSubRoutines(graphInstance.RoutineType).
                    AddTo(lifeTime);
            }
            
            yield return base.OnExecuteState(context);
        }

        protected override void OnUpdatePortsCache()
        {
            base.OnUpdatePortsCache();

            return;
#if !UNITY_EDITOR
            return;
#endif

            var graphAsset = graphReference.editorAsset as GameObject;
            //if target addressable is empty
            if (!graphAsset)
                return;
            
            var targetGraph = graphAsset.GetComponent<IUniGraph>();
            if (targetGraph == null)
                return;
            
            targetGraph.UpdatePortsCache();
            
            foreach (var port in targetGraph.GraphInputs) {
                this.UpdatePortValue(port.ItemName,PortIO.Input);
            }
            
            foreach (var port in targetGraph.GraphOuputs) {
                this.UpdatePortValue(port.ItemName,PortIO.Output);
            }
        }

        private void BindGraphPorts(IUniGraph targetGraph)
        {
            var lifetime = LifeTime;
            foreach (var node in targetGraph.GraphInputs) {
                BindGraphPort(node,lifetime);
            }
            
            foreach (var node in targetGraph.GraphOuputs) {
                BindGraphPort(node, lifetime);
            }
        }

        private void BindGraphPort(IGraphPortNode graphNode, ILifeTime lifeTime)
        {
            
            var portName = graphNode.ItemName;
            var graphPort = graphNode.PortValue;
            var port = GetPortValue(portName);

            var source = graphNode.Direction == PortIO.Input ? port : graphPort;
            var target = graphNode.Direction == PortIO.Input ? graphPort : port;

            source.Connect(target);
            lifeTime.AddCleanUpAction(() => source.Disconnect(target));

        }

    }
}