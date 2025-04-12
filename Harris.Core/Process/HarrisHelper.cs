using System.Text;
using Harris.Core.Model;
using Newtonsoft.Json.Linq;

namespace Harris.Core.Process;
/// <summary>
///     Хэлпер для предметной области DCA
/// </summary>
public class HarrisHelper {
    /// <summary>
    ///     Прочитать файл с маппингом строк к типу состояния сессии DCA
    /// </summary>
    /// <param name="file">Путь к файлу</param>
    /// <returns>Маппинг</returns>
    public static Dictionary<string, HarrisTerminalState> ReadTerminalStateFile(string file) {
        var fileContent = File.ReadAllText(file, Encoding.UTF8);
        var fileJson = JObject.Parse(fileContent);
        var result = new Dictionary<string, HarrisTerminalState>();
        foreach (var entry in fileJson) {
            var entryString = entry.Value!.ToObject<string>()!;
            result[entry.Key] = ReadTerminalState(entryString);
        }
        return result;
    }
    /// <summary>
    ///     Прочитать состояние терминала из строки
    /// </summary>
    /// <param name="value">Строка</param>
    /// <returns>Состояние терминала</returns>
    /// <exception cref="HarrisException">Не удалось разрешить состояние</exception>
    public static HarrisTerminalState ReadTerminalState(string value) {
        if (value == "input") {
            return HarrisTerminalState.AwaitingInput;
        }
        if (value == "input alm") {
            return HarrisTerminalState.AwaitingAlarmCode;
        }
        if (value == "input cdr") {
            return HarrisTerminalState.AwaitingCdrInput;
        }
        if (value == "input edt") {
            return HarrisTerminalState.AwaitingEdtInput;
        }
        if (value == "input sts") {
            return HarrisTerminalState.AwaitingStsInput;
        }
        if (value == "input tel") {
            return HarrisTerminalState.AwaitingPhoneNumber;
        }
        if (value == "auth") {
            return HarrisTerminalState.AwaitingUserName;
        }
        if (value == "quit") {
            return HarrisTerminalState.Quit;
        }
        if (value == "connected") {
            return HarrisTerminalState.Connected;
        }
        if (value == "busy") {
            return HarrisTerminalState.LineBusy;
        }
        if (value == "fail call") {
            return HarrisTerminalState.CallFailed;
        }
        throw new HarrisException("Failed to read config");
    }
}
