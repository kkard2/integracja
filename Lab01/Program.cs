using System.Xml;

var xmlpath = Path.Combine("Assets", "data.xml");
Console.WriteLine("DOM approach:");
XmlReadWithDomApproach.Read(xmlpath);
Console.WriteLine("SAX approach:");
XmlReadWithSaxApproach.Read(xmlpath);
