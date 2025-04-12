namespace Harris.Core.Model;
/// <summary>
///     Состояние терминала DCA
/// </summary>
public enum HarrisTerminalState {
    /// <summary>
    ///     Состояние отсутствует
    /// </summary>
    None = 0,
    /// <summary> 
    ///     Ожидание ввода данных (общий случай)
    /// </summary>
    AwaitingInput = 1,
    /// <summary>
    ///     Ожидание ввода телефонного номера 
    /// </summary>
    AwaitingPhoneNumber = 2,
    /// <summary>
    ///     Линия занята
    /// </summary>
    LineBusy = 3,
    /// <summary>
    ///     Успешное соединение
    /// </summary>
    Connected = 4,
    /// <summary>
    ///     Ошибка вызова
    /// </summary>
    CallFailed = 5,
    /// <summary>
    ///     Ожидание ввода аварийного кода (alarm)
    /// </summary>
    AwaitingAlarmCode = 6,
    /// <summary>
    ///     Ожидание ввода CDR (Call Detail Record)
    /// </summary>
    AwaitingCdrInput = 7,
    /// <summary>
    ///     Ожидание ввода EDT
    /// </summary>
    AwaitingEdtInput = 8,
    /// <summary>
    ///     Ожидание ввода STS
    /// </summary>
    AwaitingStsInput = 9,
    /// <summary>
    ///     Ожидание ввода Username
    /// </summary>
    AwaitingUserName = 10,
    /// <summary>
    ///     Выход
    /// </summary>
    Quit = 11
}
