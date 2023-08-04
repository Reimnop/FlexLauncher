const fs = require('fs');
const path = require('path');

const arguments = process.argv.slice(2);

if (arguments.length === 0) {
    console.log("Error: No file to convert!");
} else {
    let inherits = null;
    do {
        inherits = convert(inherits ? `${inherits}.json` : arguments[0]);
    } while (inherits)
}

function convert(filePath) {
    const fileName = path.basename(filePath, path.extname(filePath));
    let json = JSON.parse(fs.readFileSync(filePath, 'utf8'));
    
    let converted = {
        inherits: json.inheritsFrom ?? undefined,
        type: json.type ?? undefined,
        main_class: json.mainClass ?? undefined,
        asset_index: json.assets ? {
            name: json.assets,
            url: json.assetIndex.url
        } : undefined,
        main_jar: json.downloads?.client?.url ? {
            path: `${fileName}.jar`,
            url: json.downloads.client.url
        } : undefined,
        libraries: json.libraries ? convertLibraries(json.libraries) : undefined,
        jvm_arguments: json.arguments?.jvm ? convertArguments(json.arguments.jvm) : undefined,
        game_arguments: json.arguments?.game ? convertArguments(json.arguments.game) : undefined
    };

    // write converted
    fs.writeFileSync(`${fileName}-converted.json`, JSON.stringify(converted, null, 4));
    console.log(`Conversion succeeded for version name '${fileName}'`);
    
    if (json.inheritsFrom) {
        return json.inheritsFrom;
    }
}

function convertArguments(arguments) {
    let converted = [];
    let argStrings = [];
    for (let arg of arguments) {
        if (typeof arg === "string") {
            argStrings.push(arg);
        } else {
            if (argStrings.length > 0) {
                converted.push({
                    values: argStrings.map(mapVariableName)
                });
                argStrings = [];
            }
            converted.push({
                rules: convertRules(arg.rules),
                values: typeof arg.value === "string" ? [mapVariableName(arg.value)] : arg.value.map(mapVariableName)
            });
        }
    }
    if (argStrings.length > 0) {
        converted.push({
            values: argStrings.map(mapVariableName)
        });
    }
    return converted;
}

function mapVariableName(value) {
    let map = {
        "clientid": "client_id",
        "classpath": "libraries",
        "quickPlayPath": "quick_play_path",
        "quickPlaySingleplayer": "quick_play_identifier",
        "quickPlayMultiplayer": "quick_play_identifier",
        "quickPlayRealms": "quick_play_identifier"
    };
    
    return value.replace(/\$\{([a-zA-Z0-9_]+)\}/g, (match, p1) => {
        let mapped = map[p1];
        if (mapped) {
            return `\$\{${mapped}\}`;
        }
        return match;
    });
}

function convertLibraries(libraries) {
    let result = [];
    for (let lib of libraries) {
        let convertedLib;
        if (lib.downloads) {
            convertedLib = {
                rules: lib.rules ? convertRules(lib.rules) : undefined,
                url: lib.downloads.artifact.url,
                path: lib.downloads.artifact.path
            };
        } else {
            let download = getMavenDownload(lib.name, lib.url);
            convertedLib = {
                rules: lib.rules ? convertRules(lib.rules) : undefined,
                url: download.url,
                path: download.path
            };
        }

        result.push(convertedLib);
    }
    return result;
}

function getMavenDownload(name, mavenUrl) {
    let firstColonIndex = name.indexOf(':');
    let libNamespace = name.substring(0, firstColonIndex);
    let libName = name.substring(firstColonIndex + 1);
    let fileName = libName.replaceAll(':', '-') + ".jar";
    let path = libNamespace.replaceAll('.', '/') + "/" + libName.replaceAll(':', '/') + "/" + fileName;
    let url = mavenUrl + path;
    
    return {
        url: url,
        path: path
    };
}

function convertRules(rules) {
    let converted = {};
    for (let rule of rules) {
        if (rule.os) {
            let osName = undefined;
            let osArch = undefined;
            if (rule.os.name) {
                osName = rule.os.name;
                if (osName === "osx") {
                    osName = "macos";
                }
            }
            if (rule.os.arch) {
                osArch = rule.os.arch;
            }
            converted.environment = {
                os: osName,
                arch: osArch
            };
        }
        if (rule.features) {
            const demo = rule.features.is_demo_user;
            const customRes = rule.features.has_custom_resolution;
            const quickPlaysSupport = rule.features.has_quick_plays_support;
            const quickPlaySingleplayer = rule.features.is_quick_play_singleplayer;
            const quickPlayMultiplayer = rule.features.is_quick_play_multiplayer;
            const quickPlayRealms = rule.features.is_quick_play_realms;
            converted.features = {
                demo: demo,
                custom_resolution: customRes,
                quick_plays_support: quickPlaysSupport,
                quick_play_singleplayer: quickPlaySingleplayer,
                quick_play_multiplayer: quickPlayMultiplayer,
                quick_play_realms: quickPlayRealms
            };
        }
    }
    return converted;
}