namespace System.Xml.XPath.Object
{
	public class ObjectXPathNavigatorSettings
	{
		public virtual IValueFormatter ValueConverter { get; set; }
		public virtual IValueInspector ValueExplorer { get; set; }
	}
}
