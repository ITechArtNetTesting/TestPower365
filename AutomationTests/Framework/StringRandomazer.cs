using System;
using System.Linq;

namespace Product.Framework
{
	/// <summary>
	///     Class StringRandomazer.
	/// </summary>
	public class StringRandomazer
	{
		private static readonly Random random = new Random();

		/// <summary>
		///     Makes the random string.
		/// </summary>
		/// <param name="length">The length.</param>
		/// <returns>String.</returns>
		public static string MakeRandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}

		public static string MakeRandomProjectName(string testName)
		{
			var date = DateTime.UtcNow.ToString("dd.MM.yy");
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var number = new string(Enumerable.Repeat(chars, 6)
				.Select(s => s[random.Next(s.Length)]).ToArray());
			return testName + "_" + date + "_" + number;
		}

		public static string MakeRandomClientName()
		{
			const string chars = "0123456789";
			return "TestClient" + GetRandomValue(chars, 6);
		}

		public static string MakeRandomFirstName()
		{
			const string chars = "0123456789";
			return "Binary" + GetRandomValue(chars, 2);
		}

		public static string MakeRandomLastName()
		{
			const string chars = "0123456789";
			return "Tree" + GetRandomValue(chars, 2);
		}

		public static string MakeRandomPhone()
		{
			const string chars = "0123456789";
			return "555-555-" + GetRandomValue(chars, 4);
		}

		public static string MakeRandomAddress()
		{
			const string chars = "0123456789";
			return GetRandomValue(chars, 3) + " JFK Blvd";
		}

		public static string MakeRandomZip()
		{
			const string chars = "0123456789";
			return GetRandomValue(chars, 5);
		}

		public static string GetRandomValue(string charset, int size)
		{
			var result = new string(Enumerable.Repeat(charset, size)
				.Select(s => s[random.Next(s.Length)]).ToArray());
			return result;
		}
	}
}