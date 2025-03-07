using System.Xml;
using System.Xml.XPath;

public static class XmlReadWithXlstDomApproach
{
    public static void Read(string path)
    {
        var doc = new XPathDocument(path);
        var nav = doc.CreateNavigator();
        var manager = new XmlNamespaceManager(nav.NameTable);
        manager.AddNamespace("x", "http://rejestrymedyczne.ezdrowie.gov.pl/rpl/eksport-danych-v1.0");

        Ex1(nav, manager);
    }

    private static void Ex1(XPathNavigator nav, XmlNamespaceManager manager)
    {
        XPathExpression query = nav.Compile(
            "/x:produktyLecznicze/x:produktLeczniczy[@postac='Krem' and @nazwaPowszechnieStosowana='Mometasoni furoas']");
        query.SetContext(manager);
        var count = nav.Select(query).Count;
        Console.WriteLine($"Liczba produktów leczniczych w postaci kremu, których nazwa powszechnie stosowana to Mometasoni furoas: {count}");
    }

    private static void Ex2(XPathNavigator nav, XmlNamespaceManager manager)
    {
        
        /*
        // maps "nazwaPowszechnieStosowana" to "postac" list
        var encountered = new Dictionary<string, HashSet<string>>();

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "produktLeczniczy")
            {
                if (reader.GetAttribute("nazwaPowszechnieStosowana") is not { } nazwaPowszechnieStosowana ||
                    reader.GetAttribute("postac") is not { } postac)
                    continue;

                if (encountered.TryGetValue(nazwaPowszechnieStosowana, out var list))
                {
                    if (!list.Contains(postac))
                    {
                        list.Add(postac);
                    }
                }
                else
                {
                    encountered[nazwaPowszechnieStosowana] = [postac];
                }
            }
        }
        Console.WriteLine($"Liczba produktów leczniczych o takiej samie nazwie powszechnej, pod różnymi postaciami: {encountered.Count(x => x.Value.Count > 1)}");
        */
    }
}
