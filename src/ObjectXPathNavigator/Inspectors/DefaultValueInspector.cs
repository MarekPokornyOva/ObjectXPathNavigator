#region using
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
#endregion using

namespace System.Xml.XPath.Object
{
	/// <summary>
	/// Default value inspector.
	/// </summary>
	public sealed class DefaultValueInspector : IValueInspector
	{
		public static DefaultValueInspector Instance { get; } = new DefaultValueInspector();

		readonly bool _compileExpressions;

		public DefaultValueInspector() : this(null)
		{
		}

		public DefaultValueInspector(DefaultValueInspectorSettings settings)
		{
			_compileExpressions = settings == null ? true : settings.CompileExpressions;
		}

		///<inheritdoc/>
		public ValueInfo GetValueInfo(object value)
		{
			if (value == null)
				return ValueInfo.Empty;

			Type valueType = value.GetType();
			Type objectType = typeof(object);
			PropertyInfo[] props = valueType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(x => x.GetIndexParameters().Length == 0)
				.ToArray();
			NodeInfo[] nodes = new NodeInfo[props.Length];
			int index = 0;

			foreach (PropertyInfo prop in props)
			{
				Expression body = Expression.Property(Expression.Constant(value), prop);
				body = Expression.TypeAs(body, objectType);
				Func<object> getter = Expression.Lambda<Func<object>>(body).Compile(!_compileExpressions);
				nodes[index] = new NodeInfo(prop.Name, LazyValue.FromFactory(getter));
				index++;
			}
			return new ValueInfo(valueType.IsValueType || valueType == typeof(string) ? LazyValue.FromConstant(value) : null, nodes);
		}
	}
}
