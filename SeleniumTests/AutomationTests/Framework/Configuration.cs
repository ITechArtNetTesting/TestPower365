using System.Configuration;

namespace Product.Framework
{
	/// <summary>
	///     Class Configuration.
	/// </summary>
	public class Configuration
	{
		//Settings
		private const string Timeout = "Timeout";


		private static string GetParameterValue(string key)
		{
			return ConfigurationManager.AppSettings.Get(key);
		}

		private static void SetParameterValue(string key, string value)
		{
			ConfigurationManager.AppSettings.Set(key, value);
		}

#region [settings]
		/// <summary>
		///     Gets the timeout.
		/// </summary>
		/// <returns>System.String.</returns>
		public static string GetTimeout()
		{
			return GetParameterValue(Timeout);
		}
	}
#endregion
}