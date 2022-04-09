using System.Collections.Generic;

namespace System.Xml.XPath.Object
{
	/// <summary>
	/// Provides <see cref="XPathNavigator"/> over an object.
	/// </summary>
	public sealed class ObjectXPathNavigator : XPathNavigator
	{
		readonly XmlNameTable _nameTable;
		readonly IValueFormatter _valueFormatter;
		readonly IValueInspector _valueInspector;

		/// <summary>
		/// Initializes a new instance of the <see cref="ObjectXPathNavigator"/> class.
		/// </summary>
		/// <param name="value">A value.</param>
		public ObjectXPathNavigator(object value) : this(value, ObjectXPathNavigatorSettings.Default)
		{ }

		Node _node;
		/// <summary>
		/// Initializes a new instance of the <see cref="ObjectXPathNavigator"/> class.
		/// </summary>
		/// <param name="value">A value.</param>
		/// <param name="settings">A settings.</param>
		public ObjectXPathNavigator(object value, ObjectXPathNavigatorSettings settings)
		{
			if (settings == null)
				throw new ArgumentNullException(nameof(settings));

			XmlNameTable nameTable = _nameTable = new NameTable();
			_node = new Node(nameTable.Add(""), XPathNodeType.Root, 0, null, LazyValue.FromConstant(value));

			_valueFormatter = settings.ValueFormatter ?? ObjectXPathNavigatorSettings.Default.ValueFormatter;
			_valueInspector = settings.ValueInspector ?? ObjectXPathNavigatorSettings.Default.ValueInspector;
		}

		ObjectXPathNavigator(ObjectXPathNavigator other)
		{
			_nameTable = other._nameTable;
			_node = other._node;
			_valueFormatter = other._valueFormatter;
			_valueInspector = other._valueInspector;
		}

		///<inheritdoc/>
		public override XPathNavigator Clone()
			=> new ObjectXPathNavigator(this);

		///<inheritdoc/>
		public override bool IsSamePosition(XPathNavigator other)
			=> other is ObjectXPathNavigator nav && nav._node == _node;

		///<inheritdoc/>
		public override bool MoveTo(XPathNavigator other)
		{
			if (other is ObjectXPathNavigator nav)
			{
				_node = nav._node;
				return true;
			}
			return false;
		}

		///<inheritdoc/>
		public override bool MoveToFirstAttribute()
			=> false;

		///<inheritdoc/>
		public override bool MoveToFirstChild()
		{
			Node[] children = GetChildren();
			if (children.Length == 0)
				return false;
			_node = children[0];
			return true;
		}

		///<inheritdoc/>
		public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
			=> false;

		///<inheritdoc/>
		public override bool MoveToId(string id)
			=> throw new NotImplementedException();

		///<inheritdoc/>
		public override bool MoveToNext()
		{
			Node[] children = _node.Parent.Children;
			int index = _node.Index;
			if (children.Length > index + 1)
			{
				_node = children[index + 1];
				return true;
			}
			return false;
		}

		///<inheritdoc/>
		public override bool MoveToNextAttribute()
			=> false;

		///<inheritdoc/>
		public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope)
			=> false;

		///<inheritdoc/>
		public override bool MoveToParent()
		{
			Node par = _node.Parent;
			if (par == null)
				return false;
			_node = par;
			return true;
		}

		///<inheritdoc/>
		public override bool MoveToPrevious()
		{
			Node[] children = _node.Parent.Children;
			int index = _node.Index;
			if (index > 0)
			{
				_node = children[index - 1];
				return true;
			}
			return false;
		}

		///<inheritdoc/>
		public override string BaseURI => "";
		///<inheritdoc/>
		public override bool IsEmptyElement => false;
		///<inheritdoc/>
		public override string LocalName => _node.Name;
		///<inheritdoc/>
		public override string Name => _node.Name;
		///<inheritdoc/>
		public override string NamespaceURI => "";
		///<inheritdoc/>
		public override XmlNameTable NameTable => _nameTable;
		///<inheritdoc/>
		public override XPathNodeType NodeType => _node.Type;
		///<inheritdoc/>
		public override string Prefix => "";
		///<inheritdoc/>
		public override string Value => _valueFormatter.Format(_node.ValueProvider.Value);
		/// <summary>
		/// Provides the real .NET object.
		/// </summary>
		public override object UnderlyingObject => _node.ValueProvider.Value;

		Node[] GetChildren()
		{
			Node[] children = _node.Children;
			if (children == null)
			{
				if (_node.Type == XPathNodeType.Text)
					children = Array.Empty<Node>();
				else
				{
					ValueInfo valueInfo = _valueInspector.GetValueInfo(_node.ValueProvider.Value);

					int index;
					IReadOnlyCollection<NodeInfo> childInfos = valueInfo.Children;
					int childrenCount = childInfos.Count;
					LazyValue valueProvider = valueInfo.ValueProvider;
					if (valueProvider != null)
					{
						children = new Node[childrenCount + 1];
						index = 1;
						children[0] = new Node(_nameTable.Add("#text"), XPathNodeType.Text, 0, _node, valueProvider);
					}
					else
					{
						children = new Node[childrenCount];
						index = 0;
					}

					foreach (NodeInfo childInfo in childInfos)
					{
						children[index] = new Node(_nameTable.Add(childInfo.Name), XPathNodeType.Element, index, _node, childInfo.ValueProvider);
						index++;
					}
				}
				_node.Children = children;
			}
			return children;
		}

		class Node
		{
			internal Node(string name, XPathNodeType type, int index, Node parent, LazyValue valueProvider)
			{
				Name = name;
				Type = type;
				Index = index;
				Parent = parent;
				ValueProvider = valueProvider;
			}

			internal string Name { get; }
			internal LazyValue ValueProvider { get; }
			internal int Index { get; }
			internal Node Parent { get; }
			internal XPathNodeType Type { get; }

			internal Node[] Children { get; set; }
		}
	}
}
