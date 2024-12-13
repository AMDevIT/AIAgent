using Microsoft.SemanticKernel;

namespace AMDevIT.AI.Core.Modules
{
    public interface ISemanticKernelAIModule
       : IProviderAIModule
    {
        #region Properties      

        #endregion

        #region Methods

        IKernelBuilderPlugins AddToKernelBuilder(IKernelBuilder kernelBuilder);

        #endregion
    }
}
