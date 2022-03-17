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
	public class UnitTestString
	{
		[TestMethod(nameof(TestString))]
		public void TestString()
		{
			XslCompiledTransform xslt = new XslCompiledTransform();
			xslt.Load(new XmlTextReader(new StringReader(@"<?xml version=""1.0"" encoding=""utf-8""?>
<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform""
	xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" exclude-result-prefixes=""msxsl""
>
	<xsl:output method=""xml"" indent=""no""/>

	<xsl:template match=""/"">
		<xsl:value-of select=""Name""/>
		<xsl:text>,</xsl:text>
		<xsl:value-of select=""Name/Length""/>
		<xsl:for-each select=""Name/item"">
			<xsl:text>,</xsl:text>
			<xsl:value-of select="".""/>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>")));

			StringBuilder sb = new StringBuilder();
			xslt.Transform(new ObjectXPathNavigator(
			new SimpleDto
			{
				Name = "Hello world"
			}), new XmlTextWriter(new StringWriter(sb)));

			Assert.AreEqual("Hello world,11,H,e,l,l,o, ,w,o,r,l,d", sb.ToString());
		}

		public class SimpleDto
		{
			public string Name { get; set; }
		}
	}
}
