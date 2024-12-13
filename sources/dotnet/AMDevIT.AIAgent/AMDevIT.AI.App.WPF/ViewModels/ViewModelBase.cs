using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AMDevIT.AI.App.WPF.ViewModels;

public partial class ViewModelBase(ILogger logger,
                                IConfiguration configuration)
 : ObservableObject
{
    #region Properties

    protected ILogger Logger
    {
        get;
        private set;
    } = logger;

    public IConfiguration Configuration
    {
        get;
        private set;
    } = configuration;

    #endregion
}
