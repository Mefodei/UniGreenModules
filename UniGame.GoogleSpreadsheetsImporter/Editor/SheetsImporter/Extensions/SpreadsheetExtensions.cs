﻿namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter.Extensions
{
    using System;
    using System.Collections.Generic;
    using TypeConverters.Editor;
    using UniGreenModules.UniCore.Runtime.Utils;
    using Object = UnityEngine.Object;

    public static class SpreadsheetExtensions
    {

        private static System.Func<Type, SheetSyncValue> _syncCache =
            MemorizeTool.Create<Type, SheetSyncValue>(x => DefaultProcessor.CreateSyncItem(x));
        
        
        public static readonly AssetSheetDataProcessor DefaultProcessor = new AssetSheetDataProcessor();

        public static SheetData UpdateSheetValue(this object source, SheetData data)
        {
            return DefaultProcessor.UpdateSheetValue(source, data);
        }
        
        public static SheetData UpdateSheetValue(this object source, object keyValue, SheetSyncValue schemaValue, SheetData data)
        {
            return DefaultProcessor.UpdateSheetValue(source, keyValue, schemaValue, data);
        }
        
        public static object ApplyBySheetFieldValue(this object source,string sheetField,object value)
        {
            var syncScheme = source.ToSpreadsheetSyncedItem();
            foreach (var field in syncScheme.fields) {
                if (string.Equals(field.sheetValueField, sheetField, StringComparison.OrdinalIgnoreCase))
                    field.ApplyValue(source, value);
            }

            return source;
        }

        public static object GetSyncValueId(this object source)
        {
            var syncScheme = source.ToSpreadsheetSyncedItem();
            syncScheme.keyField.GetValue(source);
            return source;
        }

        public static List<Object> SyncFolderAssets(
            this Type filterType, 
            string folder,
            SpreadsheetData spreadsheetData,
            Object[] assets = null,
            bool createMissing = true, 
            int maxItemsCount = -1,
            string overrideSheetId = "")
        {
            return DefaultProcessor.SyncFolderAssets(filterType,folder, spreadsheetData,assets, createMissing,maxItemsCount,overrideSheetId);
        }
        
        public static List<Object> SyncFolderAssets(
            this Type type, 
            string folder,
            bool createMissing, 
            SpreadsheetData spreadsheetData)
        {
            return DefaultProcessor.SyncFolderAssets(type, folder,createMissing, spreadsheetData);
        }

        public static SheetSyncValue ToSpreadsheetSyncedItem(this object asset)
        {
            return _syncCache(asset.GetType());
        }
        
        public static SheetSyncValue ToSpreadsheetSyncedItem(this Type type) => _syncCache(type);
        
        public static object ApplySpreadsheetData(this object asset, SpreadsheetData data)
        {
            var syncAsset = asset.GetType().ToSpreadsheetSyncedItem();
            DefaultProcessor.ApplyDataByAssetKey(asset,syncAsset,data);
            return asset;
        }
        
        public static object ApplySpreadsheetData(
            this object asset,
            Type type,
            object keyValue,
            SheetData sheetData)
        {
            var syncAsset = type.ToSpreadsheetSyncedItem();
            
            var result = DefaultProcessor.ApplyData(asset,keyValue,syncAsset,sheetData);
            return result;
        }

        public static object ConvertType(this object source, Type target)
        {
            if (source == null)
                return null;
            
            if (target.IsInstanceOfType(source))
                return source;

            return ObjectTypeConverter.TypeConverters.ConvertValue(source, target);
        }
        
    }
}