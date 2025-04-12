using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Text;
using Harris.Core.Extension;
using Harris.Core.Model;

namespace Harris.Core.Process;
/// <summary>
///     Клиент DCA
/// </summary>
public class HarrisSerialPort : IHarrisStateSource {
    private readonly Dictionary<string, HarrisTerminalState> _config;
    /// <summary>
    ///     Серийный порт
    /// </summary>
    public SerialPort SerialPort { get; }
    /// <summary>
    ///     Конструктор DCA
    /// </summary>
    /// <param name="serialPort">Серийный порт</param>
    /// <param name="config">Конфигурация</param>
    public HarrisSerialPort(SerialPort serialPort, Dictionary<string, HarrisTerminalState> config) {
        SerialPort = serialPort;
        _config = config;
    }
    /// <summary>
    ///     Асинхронная задача на чтение состояния терминала
    /// </summary>
    /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
    /// <returns>Асинхронное перечисление с состояниями терминала</returns>
    public async IAsyncEnumerable<HarrisTerminalState> ReadStatesAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken
    ) {
        while (!cancellationToken.IsCancellationRequested) {
            var text = await ReadAsync(10000, 10000, cancellationToken);
            foreach (var (value, state) in _config) {
                if (text.Contains(value)) {
                    yield return state;
                }
            }
        }
    }
    /// <summary>
    ///     Отправить массив байтов в серийный порт в асинхронной манере 
    /// </summary>
    /// <param name="message">Сообщение представленное массивом байтов</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
    /// <returns>Асинхронная задача на отправку сообщения в серийный порт</returns>
    public ValueTask WriteAsync(byte[] message, CancellationToken cancellationToken) {
        return SerialPort.BaseStream.WriteAsync(message, cancellationToken);
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
        var buffer = await SerialPort.ReadAsync(bufferLenght, tokenSource.Token);
        return buffer;
    }
}
