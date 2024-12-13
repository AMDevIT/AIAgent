using AMDevIT.AI.Core.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMDevIT.AI.Core.Modules.Personality
{
    [UniqueModule]
    public class PersonalityAIModule
     : IPersonalityAIModule, IInitAgentAIModule
    {
        #region Consts

        protected const char AdjectivesSeparator = ',';
        protected const string CurrentModuleName = "PersonalityModule";

        #endregion

        #region Property

        public ILogger? Logger
        {
            get;
            protected set;
        }

        public string ModuleName
        {
            get;
            protected set;
        } = CurrentModuleName;        

        public string Name
        {
            get;
            protected set;
        }

        public string[] Adjectives
        {
            get;
            protected set;
        }

        public PersonalityAnswerMode AnswerMode
        {
            get;
            protected set;
        } = PersonalityAnswerMode.Neutral;

        #endregion

        #region .ctor

        public PersonalityAIModule(string name,
                                 string[] adjectives,
                                 ILogger? logger)
        {
            this.Name = name;
            this.Adjectives = adjectives;
            this.Logger = logger;
        }

        public PersonalityAIModule(string name,
                                 string[] adjectives,
                                 PersonalityAnswerMode answerMode,
                                 ILogger? logger)
        {
            this.Name = name;
            this.Adjectives = adjectives;
            this.AnswerMode = answerMode;
            this.Logger = logger;
        }

        #endregion

        #region Methods

        public string BuildPersonalityMessage()
        {
            string personalityString = $"{PersonalityText.PersonalityYourNameIs} {this.Name}.";

            if (this.Adjectives.Length > 0)
            {
                personalityString += $" {PersonalityText.PersonalityCharacterCanBeDescribedAs} ";

                for (int i = 0; i < this.Adjectives.Length; i++)
                {
                    if (i == this.Adjectives.Length - 1)
                        personalityString += $"{this.Adjectives[i]}.";
                    else
                        personalityString += $"{this.Adjectives[i]}, ";
                }
            }

            personalityString = this.AnswerMode switch
            {
                PersonalityAnswerMode.Professional => $"{personalityString} {PersonalityText.PersonalityProfessionalModeOfAnswer}",
                PersonalityAnswerMode.Casual => $"{personalityString} {PersonalityText.PersonalityCasualModeOfAnswer}",
                PersonalityAnswerMode.Neutral => $"{personalityString} {PersonalityText.PersonalityNeutralModeOfAnswer}",
                _ => personalityString
            };

            return personalityString;
        }

        public string BuildInitAgentMessage()
        {
            return this.BuildPersonalityMessage();
        }

        public static PersonalityAIModule FromCommaSeparatedAdjectives(string name,
                                                                     string? commaSeparatedAdjectives,
                                                                     PersonalityAnswerMode answerMode = PersonalityAnswerMode.Neutral,
                                                                     ILogger? logger = null)
        {
            PersonalityAIModule personalityModule;
            string[] adjectives;

            if (!string.IsNullOrWhiteSpace(commaSeparatedAdjectives))
                adjectives = commaSeparatedAdjectives.Split([AdjectivesSeparator],
                                                            StringSplitOptions.RemoveEmptyEntries)
                                                     .Select(x => x.Trim())
                                                     .ToArray();
            else
                adjectives = [];

            personalityModule = new PersonalityAIModule(name,
                                                      adjectives,
                                                      answerMode,
                                                      logger);

            return personalityModule;
        }

        #endregion
    }

}
