using System.IO.Ports;
using CommandLine;
using DCA.Core.Process;
using DCA.Pinger;

var config = ResolveConfig(args);
var configTerminalStates = DCAHelper.ReadTerminalStateFile(config.TerminalSpaceMappingFile);

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
