namespace System.Xml.XPath.Object
{
	/// <summary>
	/// Default value inspector factory.
	/// </summary>
	public sealed class DefaultValueInspectorFactory : IValueInspector
	{
		public static DefaultValueInspectorFactory Instance { get; } = new DefaultValueInspectorFactory(EnumerableInspector.Instance, DefaultValueInspector.Instance);

		readonly IValueInspector[] _inspectors;
		public DefaultValueInspectorFactory(params IValueInspector[] inspectors)
		{
			_inspectors = inspectors;
		}

		///<inheritdoc/>
		public ValueInfo GetValueInfo(object value)
		{
			ValueInfo result = ValueInfo.Empty;
			foreach (IValueInspector explorer in _inspectors)
				result = result.Combine(explorer.GetValueInfo(value));
			return result;
		}
	}
}
