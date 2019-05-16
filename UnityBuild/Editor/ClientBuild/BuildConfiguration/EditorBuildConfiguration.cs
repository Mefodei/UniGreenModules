﻿using UniGreenModules.UnityBuild.Editor.ClientBuild.Interfaces;

namespace UniGreenModules.UnityBuild.Editor.ClientBuild.BuildConfiguration
{
    using System;

    [Serializable]
    public class EditorBuildConfiguration : IUniBuilderConfiguration
    {
        private readonly IArgumentsProvider arguments;
        private readonly IBuildParameters   buildParameters;

        public EditorBuildConfiguration(IArgumentsProvider argumentsProvider, IBuildParameters parameters)
        {
            arguments       = argumentsProvider;
            buildParameters = parameters;
        }
    
        public IArgumentsProvider Arguments => arguments;

        public IBuildParameters BuildParameters => buildParameters;
        
    }
}
