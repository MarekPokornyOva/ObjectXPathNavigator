# ObjectXPathNavigator

[![Package Version](https://img.shields.io/nuget/v/ObjectXPathNavigator.svg)](https://www.nuget.org/packages/ObjectXPathNavigator)
[![NuGet Downloads](https://img.shields.io/nuget/dt/ObjectXPathNavigator.svg)](https://www.nuget.org/packages/ObjectXPathNavigator)
[![License](https://img.shields.io/github/license/MarekPokornyOva/ObjectXPathNavigator.svg)](https://github.com/MarekPokornyOva/ObjectXPathNavigator/blob/master/LICENSE)

### Description
ObjectXPathNavigator is XPathNavigator implementation over System.Object.
Its purpose is to provide object's properties to XSL transformation as nodes.

### Usage
```
XslCompiledTransform xslt = new XslCompiledTransform();
xslt.Load(...);
xslt.Transform(new ObjectXPathNavigator(new SomeDto()), new XmlTextWriter(new StringWriter()));
```

Look at the included unit tests for detailed examples.

### Release notes
[See](./ReleaseNotes.md)