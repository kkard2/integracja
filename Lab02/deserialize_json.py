import json


class DeserializeJson:
    def __init__(self, filename):
        tempdata = open(filename, encoding="utf8")
        self.data = json.load(tempdata)

    def somestats(self):
        print("DeserializeJson somestats:")
        result = {}

        for dep in self.data:
            woj_name = dep['Województwo'].strip()
            woj = result[woj_name] = result.get(woj_name, {})
            woj[dep['typ_JST']] = woj.get(dep['typ_JST'], 0) + 1

        for woj, types in result.items():
            print(woj + ':')
            for t, count in types.items():
                print('\t' + t + ': ' + str(count))

        print("done")
