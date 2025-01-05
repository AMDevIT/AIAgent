using Microsoft.SemanticKernel;

namespace AMDevIT.AI.Core.Modules
{
    public interface ISemanticKernelAIModule
       : IProviderAIModule
    {
        #region Properties      

        bool RegisterKernelFunctions
        {
            get;
        }

        #endregion

        #region Methods

        IKernelBuilderPlugins AddToKernelBuilder(IKernelBuilder kernelBuilder);

        #endregion
    }
}
