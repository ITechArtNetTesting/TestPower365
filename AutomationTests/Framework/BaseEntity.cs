using System;
using log4net;
using log4net.Config;

namespace Product.Framework
{
	/// <summary>
	///     Class BaseEntity.
	/// </summary>
	public class BaseEntity
	{
		protected static ILog Log;

		protected BaseEntity()
		{
			XmlConfigurator.Configure();
			Log = LogManager.GetLogger(typeof(BaseEntity));
		}

		// Just logs current step number and name.
		/// <summary>
		///     Logs the step.
		/// </summary>
		/// <param name="step">The step.</param>
		/// <param name="message">The message.</param>
		public void LogStep(int step, string message)
		{
			Log.Info($"----------[ Step {step} ]: {message}");
		}

		// Logs Test Case name.
		/// <summary>
		///     Logs the case.
		/// </summary>
		/// <param name="message">The message.</param>
		public void LogCase(string message)
		{
			Log.Info("              ");
			Log.Info(string.Format(message));
			Log.Info("              ");
		}

	    public void LogHtml(string message)
	    {
	        Console.WriteLine(" ");
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine(" ");
	        Console.WriteLine(message);
	        Console.WriteLine(" ");
	        Console.WriteLine("----------------------------------------------------");
        }
	}
}