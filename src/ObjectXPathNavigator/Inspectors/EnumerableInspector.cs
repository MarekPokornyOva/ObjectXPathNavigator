#region using
using System.Collections;
using System.Linq;
#endregion using

namespace System.Xml.XPath.Object
{
	/// <summary>
	/// Enumerable inspector.
	/// </summary>
	public sealed class EnumerableInspector : IValueInspector
	{
		public static EnumerableInspector Instance { get; } = new EnumerableInspector();

		///<inheritdoc/>
		public ValueInfo GetValueInfo(object value)
			=> value is IEnumerable en
				? new ValueInfo(en.Cast<object>().Select(x => new NodeInfo("item", LazyValue.FromConstant(x))).ToArray())
				: ValueInfo.Empty;
	}
}
