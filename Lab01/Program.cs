var xmlpath = Path.Combine("Assets", "data.xml");
Console.WriteLine("DOM approach:");
XmlReadWithDomApproach.Read(xmlpath);
Console.WriteLine("SAX approach:");
XmlReadWithSaxApproach.Read(xmlpath);
Console.WriteLine("XLST DOM approach:");
XmlReadWithXlstDomApproach.Read(xmlpath);
