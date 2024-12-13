using AMDevIT.AI.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AMDevIT.AI.Core.Modules.Utils
{
    public class TimeAIModule(ILogger? logger)
        : SemanticKernelAIModule(logger)

    {
        #region Consts

        protected const string CurrentModuleName = "TimeModule";

        #endregion

        #region Properties


        public override string ModuleName
        {
            get;
            protected set;
        } = CurrentModuleName;
      
        #endregion

        #region .ctor

        #endregion

        #region Methods      

        [KernelFunction]
        [Description("Return current time, including time zone data.")]
        [return: Description("This is the current time, including time zone data. Pay attention to the plurality of 'hours' based on the time.")]
        public Task<TimeResponse> GetCurrentTimeAsync()
        {
            TimeResponse response = new()
            {
                CurrentTime = DateTimeOffset.Now
            };

            this.Logger?.LogDebug("Requested current time. Current time is: {currentTime}",
                                  response.CurrentTime);

            return Task.FromResult(response);
        }

        #endregion
    }
}
