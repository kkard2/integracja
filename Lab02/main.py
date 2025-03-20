import yaml
from deserialize_json import DeserializeJson
from serialize_json import SerializeJson
from convert_json_to_yaml import ConvertJsonToYaml

config = yaml.load(open('Assets/config.yaml', encoding="utf8"),
                   Loader=yaml.FullLoader)

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
        ConvertJsonToYaml.run(DeserializeJson(src).data, dst)
    elif t == 'json_path_convert_to_yaml':
        ConvertJsonToYaml.run2(src, dst)
    else:
        print('invalid step type, skipping')
