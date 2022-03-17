namespace System.Xml.XPath.Object
{
	/// <summary>
	/// Simple value formatter.
	/// </summary>
	public sealed class SimpleValueFormatter : IValueFormatter
	{
		public static SimpleValueFormatter Instance { get; } = new SimpleValueFormatter();

		/// <summary>
		/// Formates the value in with ToString() method.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The string representation.</returns>
		public string Format(object value)
			=> value == null ? "" : value.ToString();
	}
}
