namespace System.Xml.XPath.Object
{
	/// <summary>
	/// Provides support for lazy initialization.
	/// </summary>
	public sealed class LazyValue
	{
		object _value;
		Func<object> _valueFactory;
		bool _gotValue;

		LazyValue(object value)
		{
			_value = value;
			_gotValue = true;
		}
		/// <summary>
		/// Creates <see cref="LazyValue"/> from a constant value.
		/// </summary>
		/// <param name="value">A value.</param>
		/// <returns>The <see cref="LazyValue"/>.</returns>
		public static LazyValue FromConstant(object value)
			=> new LazyValue(value);

		LazyValue(Func<object> valueFactory)
		{
			_valueFactory = valueFactory;
		}
		/// <summary>
		/// Creates lazy evaluated <see cref="LazyValue"/>.
		/// </summary>
		/// <param name="valueFactory">The factory.</param>
		/// <returns>The <see cref="LazyValue"/>.</returns>
		public static LazyValue FromFactory(Func<object> valueFactory)
			=> new LazyValue(valueFactory);

		/// <summary>
		/// Returns the value.
		/// </summary>
		public object Value
		{
			get
			{
				if (!_gotValue)
				{
					_value = _valueFactory();
					_gotValue = true;
				}
				return _value;
			}
		}
	}
}
