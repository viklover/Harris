using Harris.Core.Process;
using Microsoft.Extensions.Logging;

namespace Harris.Pinger;
/// <summary>
///     Программа для проверки доступности DCA
/// </summary>
public class Pinger {
    private readonly HarrisSerialPort _port;
    private readonly ILogger _logger;
    /// <summary>
    ///     Конструктор программы
    /// </summary>
    /// <param name="port">Порт</param>
    /// <param name="logger">Логгер</param>
    public Pinger(HarrisSerialPort port, ILogger logger) {
        _port = port;
        _logger = logger;
    }
    /// <summary>
    ///     Запустить в асинхронной манере
    /// </summary>
    /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
    /// <returns>Асинхронная задача на выполнение жизнненного цикла программы</returns>
    public async Task RunAsync(CancellationToken cancellationToken) {
        _port.SerialPort.Open();
        var stateEnumerable = _port.ReadStatesAsync(CancellationToken.None);
        await foreach (var state in stateEnumerable) {
            
            
            
            await Console.Out.WriteLineAsync(state.ToString());
        }
    }
}
