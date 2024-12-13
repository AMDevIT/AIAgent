using AMDevIT.AI.App.WPF.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace AMDevIT.AI.App.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Properties

        public IConfiguration? Configuration
        {
            get;
            private set;
        }

        public IServiceProvider Services
        {
            get;
            private set;
        }

        #endregion

        #region .ctor

        public App()
        {
            this.Services = BuildApplicationServices();
            this.Configuration = this.Services.GetService<IConfiguration>();
        }

        #endregion

        #region Methods

        private static ServiceProvider BuildApplicationServices()
        {
            var services = new ServiceCollection();

            string configurationFilename;
            IConfiguration configuration;

#if DEBUG
            configurationFilename = "appsettings.Development.json";
#else
                configurationFilename = "appsettings.json";
#endif
            configuration = new ConfigurationBuilder().AddJsonFile(configurationFilename,
                                                                   optional: false,
                                                                   reloadOnChange: true)
                                                      .AddUserSecrets<App>()
                                                      .Build();

            services.AddSingleton(configuration);
            services.AddLogging((builder) =>
            {
                builder.AddConfiguration(configuration.GetSection("Logging"));
#if DEBUG
                builder.AddDebug();
#endif                 
                builder.AddConsole();
            });
            
            services.AddSingleton<MainWindowViewModel>();

            return services.BuildServiceProvider();
        }

        #endregion
    }
}
