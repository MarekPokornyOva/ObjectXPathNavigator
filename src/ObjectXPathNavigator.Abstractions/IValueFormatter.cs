namespace System.Xml.XPath.Object
{
	/// <summary>
	/// Value formatter.
	/// </summary>
	public interface IValueFormatter
	{
		/// <summary>
		/// Formates a value to string representation.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The string representation.</returns>
		string Format(object value);
	}
}
