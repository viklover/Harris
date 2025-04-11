namespace DCA.Core;
/// <summary>
///     Базовое исключение приложения
/// </summary>
public class DCAException : Exception {
    /// <summary>
    ///     Конструктор исключения
    /// </summary>
    /// <param name="message">Сообщение</param>
    public DCAException(string message) : base(message) {}
}
