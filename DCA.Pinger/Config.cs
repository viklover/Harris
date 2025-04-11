using CommandLine;

namespace DCA.Pinger;
/// <summary>
///     Конфигурация
/// </summary>
public class Config {
     /// <summary>
    ///     Путь к серийному порту
    /// </summary>
    [Option("port", Required = true)]
    public string Port { get; }
    /// <summary>
    ///     Скорость передачи данных порта
    /// </summary>
    [Option("baud-rate", Required = true)]
    public int BaudRate { get; }
    /// <summary>
    ///     Путь к файлу с маппингом строки к типу терминального состояния
    /// </summary>
    [Option("terminal-map-file", Required = true)]
    public string TerminalSpaceMappingFile { get; }
    /// <summary>
    ///     Конструктор конфигурации
    /// </summary>
    /// <param name="port">Порт</param>
    /// <param name="baudRate">Скорость передачи данных</param>
    /// <param name="terminalSpaceMappingFile">Скорость передачи данных</param>
    public Config(string port, int baudRate, string terminalSpaceMappingFile) {
        Port = port;
        BaudRate = baudRate;
        TerminalSpaceMappingFile = terminalSpaceMappingFile;
    }
}
