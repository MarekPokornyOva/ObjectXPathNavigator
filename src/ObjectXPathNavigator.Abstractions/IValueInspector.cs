#region using
using System.Collections.Generic;
using System.Linq;
#endregion using

namespace System.Xml.XPath.Object
{
	/// <summary>
	/// Value inspector.
	/// </summary>
	public interface IValueInspector
	{
		/// <summary>
		/// Provides value information.
		/// </summary>
		/// <param name="value">A value.</param>
		/// <returns>The value information.</returns>
		ValueInfo GetValueInfo(object value);
	}

	public sealed record ValueInfo
	{
		public ValueInfo(IReadOnlyCollection<NodeInfo> children) : this(null, children)
		{ }

		public ValueInfo(LazyValue valueProvider, IReadOnlyCollection<NodeInfo> children)
		{
			ValueProvider = valueProvider;
			Children = children;
		}

		public LazyValue ValueProvider { get; }
		public IReadOnlyCollection<NodeInfo> Children { get; }

		public static ValueInfo Empty { get; } = new ValueInfo(null, Array.Empty<NodeInfo>());

		public ValueInfo Combine(ValueInfo other)
			=> new ValueInfo(ValueProvider ?? other.ValueProvider, Children.Union(other.Children).ToArray());
	}

	public sealed record NodeInfo(string Name, LazyValue ValueProvider);
}
