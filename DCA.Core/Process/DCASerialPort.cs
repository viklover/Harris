using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Text;
using DCA.Core.Extension;
using DCA.Core.Model;

namespace DCA.Core.Process;
/// <summary>
///     Клиент DCA
/// </summary>
public class DCASerialPort {
    private readonly SerialPort _serialPort;
    private readonly Dictionary<string, DCATerminalState> _config;
    /// <summary>
    ///     Конструктор DCA
    /// </summary>
    /// <param name="serialPort">Серийный порт</param>
    /// <param name="config">Конфигурация</param>
    public DCASerialPort(SerialPort serialPort, Dictionary<string, DCATerminalState> config) {
        _serialPort = serialPort;
        _config = config;
    }
    /// <summary>
    ///     Асинхронная задача на чтение состояния терминала
    /// </summary>
    /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
    /// <returns>Асинхронное перечисление с состояниями терминала</returns>
    public async IAsyncEnumerable<DCATerminalState> ReadStatesAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken
    ) {
        while (!cancellationToken.IsCancellationRequested) {
            var text = await ReadAsync(10000, 5000, cancellationToken);
            await Console.Out.WriteAsync(text);
            foreach (var (value, state) in _config) {
                if (text.Contains(value)) {
                    yield return state;
                }
            }
        }
    }
    /// <summary>
    ///     Прочитать данные с указанным timeout в асинхронной манере
    /// </summary>
    /// <param name="bufferLenght">Максимальная длина буффера</param>
    /// <param name="timeoutMs">Длительность в рамках которой порт считается активным</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
    /// <returns>Асинхронная задача на чтение данных</returns>
    public async Task<string> ReadAsync(int bufferLenght, int timeoutMs, CancellationToken cancellationToken) {
        var buffer = await ReadBytesAsync(bufferLenght, timeoutMs, cancellationToken);
        var bufferText = Encoding.ASCII.GetString(buffer);
        return bufferText;
    }
    /// <summary>
    ///     Прочитать данные с указанным timeout в асинхронной манере
    /// </summary>
    /// <param name="bufferLenght">Максимальная длина буффера</param>
    /// <param name="timeoutMs">Длительность в рамках которой порт считается активным</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
    /// <returns>Асинхронная задача на чтение данных</returns>
    public async Task<byte[]> ReadBytesAsync(int bufferLenght, int timeoutMs, CancellationToken cancellationToken) {
        using var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        tokenSource.CancelAfter(timeoutMs);
        var buffer = await _serialPort.ReadAsync(bufferLenght, tokenSource.Token);
        return buffer;
    }
}
