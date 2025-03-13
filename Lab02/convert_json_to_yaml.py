import yaml
import json


class ConvertJsonToYaml:
    @staticmethod
    def run(deserializeddata, destinationfilelocaiton):
        print("ConvertJsonToYaml run:")
        with open(destinationfilelocaiton, 'w', encoding='utf-8') as f:
            yaml.dump(deserializeddata, f, allow_unicode=True)
        print("done")

    @staticmethod
    def run2(source_file_path, destination_file_path):
        print("ConvertJsonToYaml run2:")
        tempdata = open(source_file_path, encoding="utf8")
        data = json.load(tempdata)
        with open(destination_file_path, 'w', encoding='utf-8') as f:
            yaml.dump(data, f, allow_unicode=True)
        print("done")
