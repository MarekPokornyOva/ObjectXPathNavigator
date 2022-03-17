#region using
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath.Object;
using System.Xml.Xsl;
#endregion using

namespace Tests
{
	[TestClass]
	public class UnitTestDictionary
	{
		[TestMethod(nameof(TestDictionary))]
		public void TestDictionary()
		{
			XslCompiledTransform xslt = new XslCompiledTransform();
			xslt.Load(new XmlTextReader(new StringReader(@"<?xml version=""1.0"" encoding=""utf-8""?>
<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform""
	xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" exclude-result-prefixes=""msxsl""
>
	<xsl:output method=""xml"" indent=""no""/>

	<xsl:template match=""/"">
		<xsl:value-of select=""Index/Count""/>
		<xsl:for-each select=""Index/item"">
			<xsl:text>,</xsl:text>
			<xsl:value-of select=""./Key""/>
			<xsl:text>=</xsl:text>
			<xsl:value-of select=""./Value/Name""/>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>")));

			StringBuilder sb = new StringBuilder();
			xslt.Transform(new ObjectXPathNavigator(
				new SimpleDto
				{
					Index = new Dictionary<int, ItemCls>
					{
						{ 123, new ItemCls { Id = 123, Name = "One-Two-Three" } },
						{ 456, new ItemCls { Id = 456, Name = "Four-Five-Six" } },
						{ 789, new ItemCls { Id = 789, Name = "Seven-Eight-Nine" } },
					},
				}), new XmlTextWriter(new StringWriter(sb)));

			Assert.AreEqual("3,123=One-Two-Three,456=Four-Five-Six,789=Seven-Eight-Nine", sb.ToString());
		}

		public class SimpleDto
		{
			public Dictionary<int, ItemCls> Index { get; set; }
		}

		public class ItemCls
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}
	}
}
