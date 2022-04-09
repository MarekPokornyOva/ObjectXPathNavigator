#region using
using System.Collections.Generic;
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
		readonly IDictionary<Type, (string Name, Func<object, object> Getter)[]> _typeInfoCache;

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultValueInspector"/> class.
		/// </summary>
		public DefaultValueInspector() : this(DefaultValueInspectorSettings.Default)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultValueInspector"/> class.
		/// </summary>
		/// <param name="settings">The settings.</param>
		public DefaultValueInspector(DefaultValueInspectorSettings settings)
		{
			if (settings == null)
				throw new ArgumentNullException(nameof(settings));

			_compileExpressions = settings.CompileExpressions;
			_typeInfoCache = settings.TypeInfoCache ?? DefaultValueInspectorSettings.Default.TypeInfoCache;
		}

		readonly static Type _objectType = typeof(object);
		readonly static ParameterExpression parm = Expression.Parameter(_objectType, "x");

		///<inheritdoc/>
		public ValueInfo GetValueInfo(object value)
		{
			if (value == null)
				return ValueInfo.Empty;

			Type valueType = value.GetType();
			if (!_typeInfoCache.TryGetValue(valueType, out (string Name, Func<object, object> Getter)[] info))
			{
				IEnumerable<PropertyInfo> props = valueType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(x => x.GetIndexParameters().Length == 0);
				info = props.Select(prop =>
				{
					Expression body = Expression.Property(valueType.IsValueType ? Expression.Unbox(parm, valueType) : Expression.TypeAs(parm, valueType), prop);
					body = Expression.TypeAs(body, _objectType);
					Func<object, object> getter = Expression.Lambda<Func<object, object>>(body, parm).Compile(!_compileExpressions);
					return (prop.Name, getter);
				}).ToArray();
				_typeInfoCache.Add(valueType, info);
			}

			NodeInfo[] nodes = new NodeInfo[info.Length];
			int index = 0;
			foreach ((string name, Func<object, object> getter) in info)
				nodes[index++] = new NodeInfo(name, LazyValue.FromFactory(() => getter(value)));
			return new ValueInfo(valueType.IsValueType || valueType == typeof(string) ? LazyValue.FromConstant(value) : null, nodes);
		}
	}
}
