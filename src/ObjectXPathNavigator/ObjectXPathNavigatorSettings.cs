namespace System.Xml.XPath.Object
{
	public class ObjectXPathNavigatorSettings
	{
		public virtual IValueFormatter ValueFormatter { get; set; }
		public virtual IValueInspector ValueInspector { get; set; }

		public static ObjectXPathNavigatorSettings Default { get; } = new ReadOnlyObjectXPathNavigatorSettings();

		class ReadOnlyObjectXPathNavigatorSettings : ObjectXPathNavigatorSettings
		{
			readonly static IValueFormatter _valueFormatter = SimpleValueFormatter.Instance;
			readonly static IValueInspector _valueInspector = DefaultValueInspectorFactory.Instance;

			public override IValueFormatter ValueFormatter { get => _valueFormatter; set => throw new NotSupportedException(); }
			public override IValueInspector ValueInspector { get => _valueInspector; set => throw new NotSupportedException(); }
		}
	}
}
