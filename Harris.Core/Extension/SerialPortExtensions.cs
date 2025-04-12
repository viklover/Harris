using System.IO.Ports;

namespace Harris.Core.Extension;
/// <summary>
///     Методы расширения для <see cref="SerialPort"/>
/// </summary>
public static class SerialPortExtensions {
    /// <summary>
    ///     Прочитать данные из порта в асинхронной манере
    /// </summary>
    /// <param name="serialPort">Серийный порт</param>
    /// <param name="buffer">Буффер для складирования результата</param>
    /// <param name="offset">Смещение относительно ожидаемого количества байт</param>
    /// <param name="count">Размер буфера</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
    /// <returns>Асинхронная задача на чтение данных из серийного порта</returns>
    public async static Task ReadAsync(this SerialPort serialPort, 
        byte[] buffer, int offset, int count, CancellationToken cancellationToken
    ) {
        var bytesToRead = count;
        var temp = new byte[count];
        while (bytesToRead > 0) {
            var readBytes = await serialPort.BaseStream.ReadAsync(
                temp.AsMemory(0, bytesToRead), 
                cancellationToken
            );
            Array.Copy(temp, 0, buffer, offset + count - bytesToRead, readBytes);
            bytesToRead -= readBytes;
        }
    }
    /// <summary>
    ///     Прочитать данные из серийного порта в асинхронной манере
    /// </summary>
    /// <param name="serialPort">Серийный порт</param>
    /// <param name="count">Количество байт на чтение</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
    /// <returns>Асинхронная задача на чтение данных из серийного порта</returns>
    public async static Task<byte[]> ReadAsync(this SerialPort serialPort, int count, 
        CancellationToken cancellationToken
    ) {
        var buffer = new byte[count];
        try {
            await serialPort.ReadAsync(buffer, 0, count, cancellationToken);
        } catch (TaskCanceledException) {
            if (cancellationToken.IsCancellationRequested) {
                return buffer;
            }
        }
        return buffer;
    }
}
