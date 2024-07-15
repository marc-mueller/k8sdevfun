using System.Collections.Concurrent;
using ReaFx.Core.Common.Licensing;

namespace DevFun.Api.Extensions
{
    public static class ModularServiceCollectionExtensions
    {
        private static readonly ConcurrentDictionary<string, LicenseData> licenseDataCache = new();

        public static IServiceCollection AddLicensing(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
            {
                return services;
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            string licenseFile = configuration["License:LicenseFile"];
            string licenseData = configuration["License:LicenseData"];
            if (!string.IsNullOrEmpty(licenseFile))
            {
                LoadLicenseFile(licenseFile).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else if (!string.IsNullOrEmpty(licenseData))
            {
                LoadLicenseData(licenseData).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else if (File.Exists("testing.license.debug.lic"))
            {
                LoadLicenseFile("testing.license.debug.lic").ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else
            {
                throw new InvalidOperationException("Invalid license parameter.");
            }

            return services;
        }

        private static async Task<IEnumerable<ValidationFailure>> LoadLicenseFile(string licenseFile)
        {
            List<ValidationFailure> validationFailures = new();
            validationFailures.AddRange(await LicenseManager.LoadLicenseFile(licenseFile).ConfigureAwait(false));

            try
            {
                LicenseData licenseData = LicenseManager.ValidateAndGetLicense();
                licenseDataCache.AddOrUpdate(nameof(LicenseData), key => licenseData, (key, old) => licenseData);
            }
            catch (LicenseException)
            {
                // ignore, is already part of response
            }

            return validationFailures;
        }

        private static async Task<IEnumerable<ValidationFailure>> LoadLicenseData(string licenseDataContent)
        {
            List<ValidationFailure> validationFailures = new();
            validationFailures.AddRange(await LicenseManager.LoadLicenseData(licenseDataContent).ConfigureAwait(false));

            try
            {
                LicenseData licenseData = LicenseManager.ValidateAndGetLicense();
                licenseDataCache.AddOrUpdate(nameof(LicenseData), key => licenseData, (key, old) => licenseData);
            }
            catch (LicenseException)
            {
                // ignore, is already part of response
            }

            return validationFailures;
        }
    }
}
