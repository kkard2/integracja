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
        Ex2(nav, manager);
        Ex3(nav, manager);
    }

    private static void Ex1(XPathNavigator nav, XmlNamespaceManager manager)
    {
        var query = nav.Compile(
            "/x:produktyLecznicze/x:produktLeczniczy[@postac='Krem' and @nazwaPowszechnieStosowana='Mometasoni furoas']");
        query.SetContext(manager);
        var count = nav.Select(query).Count;
        Console.WriteLine($"Liczba produktów leczniczych w postaci kremu, których nazwa powszechnie stosowana to Mometasoni furoas: {count}");
    }

    private static void Ex2(XPathNavigator nav, XmlNamespaceManager manager)
    {
        var (kremPodmiots, kremCount) = GetMax("Krem");
        Console.WriteLine($"Podmiot(y) produkujący/e najwięcej ({kremCount}) kremów: {string.Join(", ", kremPodmiots)}");
        var (tabletkiPodmiots, tabletkiCount) = GetMax("Tabletki");
        Console.WriteLine($"Podmiot(y) produkujący/e najwięcej ({tabletkiCount}) tabletek: {string.Join(", ", tabletkiPodmiots)}");

        (List<string> Podmiots, int Count) GetMax(string postac)
        {
            var query = nav.Compile(
                $"/x:produktyLecznicze/x:produktLeczniczy[@postac='{postac}']/@podmiotOdpowiedzialny");
            query.SetContext(manager);
            var counts = nav
                .Select(query)
                .Cast<XPathNavigator>()
                .GroupBy(x => x.Value);

            var maxCount = int.MinValue;
            var maxPodmiots = new List<string>();

            foreach (var c in counts)
            {
                var cur = c.Count();

                if (cur > maxCount)
                {
                    maxCount = cur;
                    maxPodmiots = [c.Key];
                }
                else if (cur == maxCount)
                {
                    maxPodmiots.Add(c.Key);
                }
            }

            return (maxPodmiots, maxCount);
        }
    }

    private static void Ex3(XPathNavigator nav, XmlNamespaceManager manager)
    {
        var query = nav.Compile(
            $"/x:produktyLecznicze/x:produktLeczniczy[@postac='Krem']/@podmiotOdpowiedzialny");
        query.SetContext(manager);

        var counts = nav
            .Select(query)
            .Cast<XPathNavigator>()
            .GroupBy(x => x.Value)
            .Select(x => new { Podmiot = x.Key, Count = x.Count() })
            .OrderByDescending(x => x.Count)
            // TODO: this is technically wrong, there can be ties
            .Take(3)
            .Zip(Enumerable.Range(1, 3));

        Console.WriteLine("Podmioty produkujące najwięcej kremów:");
        foreach (var (Value, Index) in counts)
            Console.WriteLine($"{Index}. {Value.Podmiot} ({Value.Count})");
    }
}
