var xmlpath = Path.Combine("Assets", "data.xml");
Console.WriteLine("\nDOM approach:");
XmlReadWithDomApproach.Read(xmlpath);
Console.WriteLine("\nSAX approach:");
XmlReadWithSaxApproach.Read(xmlpath);
Console.WriteLine("\nXLST DOM approach:");
XmlReadWithXlstDomApproach.Read(xmlpath);
