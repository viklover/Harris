namespace Harris.Core;
/// <summary>
///     Базовое исключение приложения
/// </summary>
public class HarrisException : Exception {
    /// <summary>
    ///     Конструктор исключения
    /// </summary>
    /// <param name="message">Сообщение</param>
    public HarrisException(string message) : base(message) {}
}
