#region using
using System.Collections.Generic;
#endregion using

namespace System.Xml.XPath.Object
{
	public class DefaultValueInspectorSettings
	{
		public virtual bool CompileExpressions { get; set; }
		public virtual IDictionary<Type, (string Name, Func<object, object> Getter)[]> TypeInfoCache { get; set; }

		public static DefaultValueInspectorSettings Default { get; } = new ReadOnlyDefaultValueInspectorSettings();

		class ReadOnlyDefaultValueInspectorSettings : DefaultValueInspectorSettings
		{
			readonly static IDictionary<Type, (string Name, Func<object, object> Getter)[]> _typeInfoCache = new Dictionary<Type, (string Name, Func<object, object> Getter)[]>();
			public override bool CompileExpressions { get => true; set => throw new NotSupportedException(); }
			public override IDictionary<Type, (string Name, Func<object, object> Getter)[]> TypeInfoCache { get => _typeInfoCache; set => throw new NotSupportedException(); }
		}
	}
}
