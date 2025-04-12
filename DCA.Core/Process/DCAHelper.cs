using System.Text;
using DCA.Core.Model;
using Newtonsoft.Json.Linq;

namespace DCA.Core.Process;
/// <summary>
///     Хэлпер для предметной области DCA
/// </summary>
public class DCAHelper {
    /// <summary>
    ///     Прочитать файл с маппингом строк к типу состояния сессии DCA
    /// </summary>
    /// <param name="file">Путь к файлу</param>
    /// <returns>Маппинг</returns>
    public static Dictionary<string, DCATerminalState> ReadTerminalStateFile(string file) {
        var fileContent = File.ReadAllText(file, Encoding.UTF8);
        var fileJson = JObject.Parse(fileContent);
        var result = new Dictionary<string, DCATerminalState>();
        foreach (var entry in fileJson) {
            var entryString = entry.Value!.ToObject<string>()!;
            result[entry.Key] = DCAHelper.ReadTerminalState(entryString);
        }
        return result;
    }
    /// <summary>
    ///     Прочитать состояние терминала из строки
    /// </summary>
    /// <param name="value">Строка</param>
    /// <returns>Состояние терминала</returns>
    /// <exception cref="DCAException">Не удалось разрешить состояние</exception>
    public static DCATerminalState ReadTerminalState(string value) {
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
}
