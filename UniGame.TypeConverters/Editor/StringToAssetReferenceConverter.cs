﻿using System;

namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.TypeConverters
{
    using Abstract;
    using Core.EditorTools.Editor.AssetOperations;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [Serializable]
    [CreateAssetMenu(menuName = "UniGame/TypeConverters/StringToAssetReferenceConverter",fileName = nameof(StringToAssetReferenceConverter))]
    public class StringToAssetReferenceConverter : BaseTypeConverter
    {
        private Type _assetReferenceType = typeof(AssetReference);
        private Type _stringType = typeof(string);
        
        public sealed override bool CanConvert(Type fromType, Type toType)
        {
            if (!_assetReferenceType.IsAssignableFrom(toType))
                return false;
            if (!_assetReferenceType.IsAssignableFrom(fromType) && fromType != _stringType)
                return false;

            return true;
        }

        public sealed override (bool isValid, object result) TryConvert(object source, Type target)
        {
            if (source == null)
                return (false, source);
            var sourceType = source.GetType();
            var canConvert = CanConvert(sourceType, target);
            if(!canConvert)
                return (false, source);
            
            if(_assetReferenceType.IsAssignableFrom(sourceType))
                return (true, source);

            var filter = source as string;
            var asset = AssetEditorTools.GetAsset(filter);
            var guid = asset.GetGUID();
            if (string.IsNullOrEmpty(guid)) {
                return (false, source);
            }

            var args = new object[]{guid};
            var reference = Activator.CreateInstance(target, args);
            return (true, reference);
        }
        
    }
}
