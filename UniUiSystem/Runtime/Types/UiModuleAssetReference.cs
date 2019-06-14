﻿namespace UniGreenModules.UniUiSystem.Runtime.Types
{
    using System;
    using UnityEngine.AddressableAssets;

    [Serializable]
    public class UiModuleAssetReference : AssetReferenceT<UiModule>
    {
        public UiModuleAssetReference(string guid) : base(guid){}
    }
}
