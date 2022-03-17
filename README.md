# ObjectXPathNavigator

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