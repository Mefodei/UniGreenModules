﻿namespace UniGame.UiSystem.ModelViews.Editor.PostProcessors
{
    using System;
    using System.Linq;
    using ModelViewsMap.Runtime.Settings;
    using UiSystem.Runtime;
    using UiSystem.Runtime.Abstracts;
    using UniGreenModules.UniCore.EditorTools.Editor.AssetOperations;
    using UniGreenModules.UniCore.Runtime.ReflectionUtils;
    using UnityEditor;

    public class UpdateModelViewsSettingsProcessor : AssetModificationProcessor
    {
        public static string[] OnWillSaveAssets(string[] paths)
        {
            var settingsAssets = AssetEditorTools.
                GetAssets<ModelViewsModuleSettings>();
            
            foreach (var asset in settingsAssets) {
                if(!ValidateTarget(asset,paths))
                    continue;
                
                Rebuild(asset);
                EditorUtility.SetDirty(asset);
            }
            Rebuild();
            return paths;
        }        
        
        [MenuItem("Rebuild", menuItem = "UniGame/UiSystem/ModelsViewsSettings")]
        public static void Rebuild()
        {
            var settings = AssetEditorTools.
                GetAssets<ModelViewsModuleSettings>();
            
            foreach (var setting in settings) {
                Rebuild(setting);
                EditorUtility.SetDirty(setting);
            }
        }
        
        public static void Rebuild(ModelViewsModuleSettings settings)
        {
            settings.CleanUp();
            
            var baseViewType  = typeof(IUiView<>);
            var baseModelType = typeof(IViewModel);
            
            var modelTypes = baseModelType.GetAssignableTypes();
            var typeArs    = new Type[1];
            //get all views
            foreach (var modelType in modelTypes) {
                typeArs[0] = modelType;
                var targetType = baseViewType.MakeGenericType(typeArs);
                var viewTypes  = targetType.GetAssignableTypes();

                settings.UpdateValue(modelType,viewTypes);

            }
            
        }

        private static bool ValidateTarget(ModelViewsModuleSettings asset, string[] paths)
        {
            if (!asset || asset.isRebuildActive == false) return false;
            if (asset.updateTargets.Count == 0) return true;
            
            foreach (var targetPath in asset.updateTargets) {
                if (paths.Any(x => x.IndexOf(targetPath, StringComparison.OrdinalIgnoreCase) >= 0)) {
                    return true;
                }
            }

            return false;
        }

    }
}