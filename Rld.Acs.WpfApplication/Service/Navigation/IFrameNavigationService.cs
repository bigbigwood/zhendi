using GalaSoft.MvvmLight.Views;

namespace Rld.Acs.WpfApplication.Service.Navigation
{
    public interface IFrameNavigationService : INavigationService
    {
        object Parameter { get; }
    }
}
