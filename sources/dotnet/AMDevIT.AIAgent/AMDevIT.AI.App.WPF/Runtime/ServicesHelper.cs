using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace AMDevIT.AI.App.WPF.Runtime
{
    public static class ServicesHelper
    {
        #region Methods

        public static T? GetService<T>()
        {
            if (Application.Current is not App currentApplication)
                return default;

            return currentApplication.Services.GetService<T>();
        }

        #endregion
    }
}
