using MobilOnayService.Models;

namespace MobilOnayService
{
    public static class Properties
    {
        public static ApplicationSettings AppSettings { get; private set; } = new ApplicationSettings();

        public static void SetAppSettings(ApplicationSettings appSettings)
        {
            AppSettings = appSettings;
        }
    }
}
