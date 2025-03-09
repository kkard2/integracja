using System.Xml;

public static class XmlReadWithDomApproach
{
    public static void Read(string path)
    {
        var doc = new XmlDocument();
        doc.Load(path);

        var drugs = doc.GetElementsByTagName("produktLeczniczy")
            .Cast<XmlNode>()
            .ToArray();

        Ex1(drugs);
        Ex2(drugs);
        Ex3(drugs);
        Ex4(drugs);
    }

    private static void Ex1(IEnumerable<XmlNode> drugs)
    {
        var mometasoniFuroasCount = 0;
        foreach (var d in drugs)
        {
            if (d.Attributes?.GetNamedItem("postac")?.Value == "Krem" &&
                d.Attributes?.GetNamedItem("nazwaPowszechnieStosowana")?.Value == "Mometasoni furoas")
                mometasoniFuroasCount++;
        }
        Console.WriteLine($"Liczba produktów leczniczych w postaci kremu, których nazwa powszechnie stosowana to Mometasoni furoas: {mometasoniFuroasCount}");
    }

    private static void Ex2(IEnumerable<XmlNode> drugs)
    {
        // maps "nazwaPowszechnieStosowana" to "postac" list
        var encountered = new Dictionary<string, HashSet<string>>();

        foreach (var d in drugs)
        {
            if (d.Attributes?.GetNamedItem("nazwaPowszechnieStosowana")?.Value is not { } nazwaPowszechnieStosowana ||
                d.Attributes?.GetNamedItem("postac")?.Value is not { } postac)
                continue;

            if (encountered.TryGetValue(nazwaPowszechnieStosowana, out var list))
            {
                if (!list.Contains(postac))
                    list.Add(postac);
            }
            else
            {
                encountered[nazwaPowszechnieStosowana] = [postac];
            }
        }
        Console.WriteLine($"Liczba produktów leczniczych o takiej samie nazwie powszechnej, pod różnymi postaciami: {encountered.Count(x => x.Value.Count > 1)}");
    }

    private static void Ex3(IEnumerable<XmlNode> drugs)
    {
        // maps "podmiotOdpowiedzialny" to encountered counts
        var productionCount = new Dictionary<string, (int KremCount, int TabletkiCount)>();
        foreach (var d in drugs)
        {
            if (d.Attributes?.GetNamedItem("podmiotOdpowiedzialny")?.Value is not { } podmiotOdpowiedzialny ||
                d.Attributes?.GetNamedItem("postac")?.Value is not { } postac)
                continue;

            if (productionCount.TryGetValue(podmiotOdpowiedzialny, out var counts))
            {
                if (postac == "Krem")
                    counts.KremCount++;
                else if (postac == "Tabletki")
                    counts.TabletkiCount++;

                productionCount[podmiotOdpowiedzialny] = counts;
            }
            else
            {
                if (postac == "Krem")
                    productionCount[podmiotOdpowiedzialny] = (KremCount: 1, TabletkiCount: 0);
                else if (postac == "Tabletki")
                    productionCount[podmiotOdpowiedzialny] = (KremCount: 0, TabletkiCount: 1);
            }
        }

        var maxKremCount = int.MinValue;
        var maxTabletkiCount = int.MinValue;

        foreach (var value in productionCount.Values)
        {
            maxKremCount = Math.Max(maxKremCount, value.KremCount);
            maxTabletkiCount = Math.Max(maxTabletkiCount, value.TabletkiCount);
        }

        var maxKremPodmiots = productionCount
            .Where(x => x.Value.KremCount == maxKremCount)
            .Select(x => x.Key);
        var maxTabletkiPodmiots = productionCount
            .Where(x => x.Value.TabletkiCount == maxTabletkiCount)
            .Select(x => x.Key);

        Console.WriteLine($"Podmiot(y) produkujący/e najwięcej ({maxKremCount}) kremów: {string.Join(", ", maxKremPodmiots)}");
        Console.WriteLine($"Podmiot(y) produkujący/e najwięcej ({maxTabletkiCount}) tabletek: {string.Join(", ", maxTabletkiPodmiots)}");
    }

    private static void Ex4(IEnumerable<XmlNode> drugs)
    {
        var result = drugs
            .Where(x => x.Attributes?.GetNamedItem("postac")?.Value == "Krem")
            .Select(x => x.Attributes?.GetNamedItem("podmiotOdpowiedzialny")?.Value ?? string.Empty)
            .GroupBy(x => x)
            .Select(x => new { Podmiot = x.Key, Count = x.Count() })
            .OrderByDescending(x => x.Count)
            // TODO: this is technically wrong, there can be ties
            .Take(3)
            .Zip(Enumerable.Range(1, 3));

        Console.WriteLine("Podmioty produkujące najwięcej kremów:");
        foreach (var (Value, Index) in result)
            Console.WriteLine($"{Index}. {Value.Podmiot} ({Value.Count})");
    }
}
