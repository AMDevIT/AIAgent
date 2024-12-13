using AMDevIT.AI.Core.Modules.Personality;
using AMDevIT.AI.UnitTests.Helpers;
using System.Diagnostics;

namespace AMDevIT.AI.UnitTests
{
    [TestClass]
    public sealed class ModulesTests
    {
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            // This method is called once for the test class, before any tests of the class are run.
        }

        [TestInitialize]
        public void TestInit()
        {
            // This method is called before each test method.
        }

        [TestMethod]
        [DataRow("Saya", "solare, estatica, estroversa", PersonalityAnswerMode.Casual)]
        [DataRow("Gianluca", "pragmatico, preciso, conciso, diretto", PersonalityAnswerMode.Professional)]
        public void TestPersonalityModule(string name, string adjectives, PersonalityAnswerMode personalityAnswerMode)
        {
            string personalityDescriptionString;
            PersonalityAIModule module = PersonalityAIModule.FromCommaSeparatedAdjectives(name,
                                                                                          adjectives,
                                                                                          personalityAnswerMode);

            Assert.IsNotNull(module, "Personality module is null.");
            Assert.AreEqual(name, module.Name, $"Personality module name is not {name}");

            string[] adjectivesArray = StringHelper.SplitAndTrim(adjectives, ',');
            Assert.AreEqual(adjectivesArray.Length, module.Adjectives.Length, "Personality adjectives were not splitted correctly. Not same length.");

            for (int i = 0; i < adjectivesArray.Length; i++)
            {
                string currentModuleAdjective = module.Adjectives[i];
                string currentInputAdjective = adjectivesArray[i];
                Assert.AreEqual(currentInputAdjective,
                                currentModuleAdjective,
                                $"Input adjective {currentInputAdjective} is not equal to module adjective {currentModuleAdjective}");
            }

            Assert.AreEqual(personalityAnswerMode,
                            module.AnswerMode,
                            $"Personality answer mode is not the same. Expected {personalityAnswerMode}, found {module.AnswerMode}");

            personalityDescriptionString = module.BuildPersonalityMessage();
            Assert.IsFalse(string.IsNullOrEmpty(personalityDescriptionString), "Personality description string is null or empty.");
            Trace.WriteLine($"Personality description: {personalityDescriptionString}");
        }
    }
}
