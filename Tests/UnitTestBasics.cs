#region using
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath.Object;
using System.Xml.Xsl;
#endregion using

namespace Tests
{
	[TestClass]
	public class UnitTestBasics
	{
		[TestMethod(nameof(TestBasics))]
		public void TestBasics()
		{
			XslCompiledTransform xslt = new XslCompiledTransform();
			xslt.Load(new XmlTextReader(new StringReader(@"<?xml version=""1.0"" encoding=""utf-8""?>
<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform""
	xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" exclude-result-prefixes=""msxsl""
>
	<xsl:output method=""xml"" indent=""no""/>

	<xsl:template match=""@* | node()"">
		<xsl:copy>
			<xsl:apply-templates select=""@* | node()""/>
		</xsl:copy>
	</xsl:template>
</xsl:stylesheet>")));

			StringBuilder sb = new StringBuilder();
			xslt.Transform(new ObjectXPathNavigator(
				new SimpleDto
				{
					Id = 7,
					Name = "Hello",
					Child = new ChildDto { Id = 21, Name = "12" }
				}), new XmlTextWriter(new StringWriter(sb)));

			Assert.AreEqual("<Id>7</Id><Name>Hello<item>H</item><item>e</item><item>l</item><item>l</item><item>o</item><Length>5</Length></Name><Child><Id>21</Id><Name>12<item>1</item><item>2</item><Length>2</Length></Name></Child>", sb.ToString());
		}

		public class SimpleDto
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public ChildDto Child { get; set; }
		}

		public class ChildDto
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}
	}
}
