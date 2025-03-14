import yaml
from deserialize_json import DeserializeJson
from serialize_json import SerializeJson
from convert_json_to_yaml import ConvertJsonToYaml

config = yaml.load(open('Assets/config.yaml', encoding="utf8"),
                   Loader=yaml.FullLoader)

# TODO: detect malformed config file

sd = config['source_directory']

for action in config['actions']:
    t = action['type']
    src = sd + action['source']
    dst = None

    if 'destination' in action:
        dst = sd + action['destination']

    print('executing step ' + t + ', src: ' + src + ', dst: ' + str(dst))

    if t == 'json_stats':
        DeserializeJson(src).somestats()
    elif t == 'json_select':
        SerializeJson.run(DeserializeJson(src), dst)
    elif t == 'json_object_convert_to_yaml':
        ConvertJsonToYaml.run(DeserializeJson(src), dst)
    elif t == 'json_path_convert_to_yaml':
        ConvertJsonToYaml.run2(src, dst)
    else:
        print('invalid step type, skipping')


"""
tempconffile = open('Assets/basic_config.yaml', encoding="utf8")
confdata = yaml.load(tempconffile, Loader=yaml.FullLoader)

newDeserializator = DeserializeJson(confdata['paths']['source_folder'] +
                                    confdata['paths']['json_source_file'])
newDeserializator.somestats()

SerializeJson.run(newDeserializator, confdata['paths']['source_folder'] +
                  confdata['paths']['json_destination_file'])

ConvertJsonToYaml.run(
        newDeserializator.data, confdata['paths']['source_folder'] +
        confdata['paths']['yaml_destination_file'])
"""

"""
from deserialize_json import DeserializeJson
from serialize_json import SerializeJson
from convert_json_to_yaml import ConvertJsonToYaml

newDeserializator = DeserializeJson('Assets/data.json')
newDeserializator.somestats()

SerializeJson.run(newDeserializator, 'Assets/data_mod.json')

ConvertJsonToYaml.run(newDeserializator.data, 'Assets/data.yaml')
ConvertJsonToYaml.run2('Assets/data_mod.json', 'Assets/data_mod.yaml')
"""
