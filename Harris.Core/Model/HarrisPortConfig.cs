namespace Harris.Core.Model;
/// <summary>
///     Объектное представление порта
/// </summary>
/// <param name="Port">Порт</param>
/// <param name="BaudRate">Скорость передачи данных</param>
public record HarrisPortConfig(FileInfo Port, int BaudRate);