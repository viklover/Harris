using System.IO.Ports;
using System.Text;
using CommandLine;
using DCA.Core;
using DCA.Core.Model;
using DCA.Core.Process;
using DCA.Pinger;
using Newtonsoft.Json.Linq;

var config = ResolveConfig(args);
var configTerminalStates = ReadTerminalStateFile(config.TerminalSpaceMappingFile);

var serialPort = new SerialPort(config.Port, config.BaudRate);
var serialPortWrapper = new DCASerialPort(serialPort, configTerminalStates);

serialPort.Open();

var stateEnumerable = serialPortWrapper.ReadStatesAsync(CancellationToken.None);
await foreach (var state in stateEnumerable) {
    await Console.Out.WriteLineAsync(state.ToString());
}

static Config ResolveConfig(string[] args) {
    var parser = new Parser(_ => _.IgnoreUnknownArguments = true);
    var parsed = parser.ParseArguments<Config>(args);
    if (parsed.Value == null) {
        throw new Exception("Failed to resolve config");
    }
    return parsed.Value;
}

static Dictionary<string, DCATerminalState> ReadTerminalStateFile(string file) {
    var fileContent = File.ReadAllText(file, Encoding.UTF8);
    var fileJson = JObject.Parse(fileContent);
    var result = new Dictionary<string, DCATerminalState>();
    foreach (var entry in fileJson) {
        var entryString = entry.Value!.ToObject<string>()!;
        result[entry.Key] = ResolveTerminalState(entryString);
    }
    return result;
}

static DCATerminalState ResolveTerminalState(string value) {
    if (value == "input") {
        return DCATerminalState.AwaitingInput;
    }
    if (value == "input alm") {
        return DCATerminalState.AwaitingAlarmCode;
    }
    if (value == "input cdr") {
        return DCATerminalState.AwaitingCdrInput;
    }
    if (value == "input edt") {
        return DCATerminalState.AwaitingEdtInput;
    }
    if (value == "input sts") {
        return DCATerminalState.AwaitingStsInput;
    }
    if (value == "input tel") {
        return DCATerminalState.AwaitingPhoneNumber;
    }
    if (value == "auth") {
        return DCATerminalState.AwaitingUserName;
    }
    if (value == "quit") {
        return DCATerminalState.Quit;
    }
    if (value == "connected") {
        return DCATerminalState.Connected;
    }
    if (value == "busy") {
        return DCATerminalState.LineBusy;
    }
    if (value == "fail call") {
        return DCATerminalState.CallFailed;
    }
    throw new DCAException("Failed to read config");
}
