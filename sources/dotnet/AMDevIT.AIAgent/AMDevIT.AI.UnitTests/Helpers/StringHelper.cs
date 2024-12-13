namespace AMDevIT.AI.UnitTests.Helpers;

internal static class StringHelper
{
    #region Methods

    public static string[] SplitAndTrim(string value, char separator)
    {
        return value.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToArray();
    }

    #endregion
}
