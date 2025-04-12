namespace DCA.Core.Model;
/// <summary>
///     Источник состояний сессии с DCA
/// </summary>
public interface IDCAStateSource {
    /// <summary>
    ///     Асинхронно перечислить состояния сессий 
    /// </summary>
    /// <param name="cancellationToken">Токен отмены асинхронной операци</param>
    /// <returns>Асинхронное перечисление с состояниями DCA</returns>
    IAsyncEnumerable<DCATerminalState> ReadStatesAsync(CancellationToken cancellationToken);
}
