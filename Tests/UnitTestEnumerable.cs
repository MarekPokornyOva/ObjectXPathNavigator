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
	public class UnitTestEnumerable
	{
		[TestMethod(nameof(TestEnumerable))]
		public void TestEnumerable()
		{
			XslCompiledTransform xslt = new XslCompiledTransform();
			xslt.Load(new XmlTextReader(new StringReader(@"<?xml version=""1.0"" encoding=""utf-8""?>
<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform""
	xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" exclude-result-prefixes=""msxsl""
>
	<xsl:output method=""xml"" indent=""no""/>

	<xsl:template match=""/"">
		<xsl:value-of select=""Ids/Length""/>
		<xsl:for-each select=""Ids/item"">
			<xsl:text>,</xsl:text>
			<xsl:value-of select="".""/>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>")));

			StringBuilder sb = new StringBuilder();
			xslt.Transform(new ObjectXPathNavigator(
				new SimpleDto
				{
					Ids = new[] { 123, 456, 789 },
				}), new XmlTextWriter(new StringWriter(sb)));

			Assert.AreEqual("3,123,456,789", sb.ToString());
		}

		public class SimpleDto
		{
			public int[] Ids { get; set; }
		}
	}
}
