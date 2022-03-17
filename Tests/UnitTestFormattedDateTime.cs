#region using
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath.Object;
using System.Xml.Xsl;
#endregion using

namespace Tests
{
	[TestClass]
	public class UnitTestFormattedDateTime
	{
		[TestMethod(nameof(TestFormattedDateTime))]
		public void TestFormattedDateTime()
		{
			XslCompiledTransform xslt = new XslCompiledTransform();
			xslt.Load(new XmlTextReader(new StringReader(@"<?xml version=""1.0"" encoding=""utf-8""?>
<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform""
	xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" exclude-result-prefixes=""msxsl""
>
	<xsl:output method=""xml"" indent=""no""/>

	<xsl:template match=""/"">
		<xsl:value-of select=""Dt""/>
	</xsl:template>
</xsl:stylesheet>")));

			DateTime dt = DateTime.Now;
			SpecificValueFormater formatter = new SpecificValueFormater();
			StringBuilder sb = new StringBuilder();
			xslt.Transform(new ObjectXPathNavigator(
				new SimpleDto
				{
					Dt = dt
				}, new ObjectXPathNavigatorSettings { ValueConverter = formatter, ValueExplorer = DefaultValueInspectorFactory.Instance }), new XmlTextWriter(new StringWriter(sb)));

			Assert.AreEqual(formatter.Format(dt), sb.ToString());
		}

		public class SimpleDto
		{
			public DateTime Dt { get; set; }
		}

		class SpecificValueFormater : IValueFormatter
		{
			public string Format(object value)
				=> value == null ? "" : value is DateTime dt ? dt.ToString("s") : value.ToString();
		}
	}
}
