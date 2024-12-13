using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMDevIT.AI.Core.Modules.Personality
{
    internal interface IPersonalityAIModule
       : IProviderAIModule
    {
        #region Properties

        string Name
        {
            get;
        }

        string[] Adjectives
        {
            get;
        }

        #endregion

        #region Methods

        string BuildPersonalityMessage();

        #endregion
    }
}
