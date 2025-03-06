using System.Xml;

public static class XmlReadWithSaxApproach
{
    public static void Read(string path)
    {
        var settings = new XmlReaderSettings
        {
            IgnoreComments = true,
            IgnoreProcessingInstructions = true,
            IgnoreWhitespace = true
        };

        var reader = XmlReader.Create(path, settings);
        reader.MoveToContent();
        Ex1(reader);

        reader = XmlReader.Create(path, settings);
        reader.MoveToContent();
        Ex2(reader);

        reader = XmlReader.Create(path, settings);
        reader.MoveToContent();
        Ex3(reader);
    }

    private static void Ex1(XmlReader reader)
    {
        var mometasoniFuroasCount = 0;

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "produktLeczniczy")
            {
                if (reader.GetAttribute("postac") == "Krem" &&
                    reader.GetAttribute("nazwaPowszechnieStosowana") == "Mometasoni furoas")
                    mometasoniFuroasCount++;
            }
        }
        Console.WriteLine($"Liczba produktów leczniczych w postaci kremu, których nazwa powszechnie stosowana to Mometasoni furoas: {mometasoniFuroasCount}");
    }

    private static void Ex2(XmlReader reader)
    {
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
    }

    private static void Ex3(XmlReader reader)
    {
        // maps "podmiotOdpowiedzialny" to encountered counts
        var productionCount = new Dictionary<string, (int KremCount, int TabletkiCount)>();
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "produktLeczniczy")
            {
                if (reader.GetAttribute("podmiotOdpowiedzialny") is not { } podmiotOdpowiedzialny ||
                    reader.GetAttribute("postac") is not { } postac)
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
}
