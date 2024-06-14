using PianoMentor.Contract.Models.DataSet;

namespace PianoMentor.BLL.Services.FilesUploadManagers;

public interface IUploadFilesManager
{
    /// <summary>
    /// Загрзит файл(-ы) из тела запроса в хранилище согласно установленным настройкам.
    /// Изначально устанавливаются настройки:
    /// Проверка на наличие только одного файла = false.
    /// Проверка на соответствие расширениям = FileExtensionsCollectionsEnum.AllExcludeExecutable.
    /// По окончанию выполнения метода файл загружен, а его данные сохранены в БД
    /// </summary>
    /// <param name="ownerId">Id пользователя, которому нужно добавить файл</param>
    /// <param name="contentType">Content type из запроса</param>
    /// <param name="requestBody">Тело запроса с файлами</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат сохранения: DataSet или ошибки</returns>
    public Task<UploadingResponse> UploadAsync(long ownerId, BinaryTypeEnum binaryType, string contentType, Stream requestBody, CancellationToken cancellationToken);
    
    /// <summary>
    /// Указывает нужно ли проверять загрузчику наличие только одного файла в теле запроса.
    /// </summary>
    /// <param name="isAllowed">Флаг, указывающий на необходимость проверки</param>
    void AllowOnlyOneFileUpload(bool isAllowed = true);
    
    /// <summary>
    /// Указывает нужно ли проверять загружаемые файлы на соответствие расширениям.
    /// Если хотя бы одним из параметров будет передан FileExtensionsCollectionsEnum.AllExcludeExecutable, то все остальные параметры будут проигнорированы.
    /// </summary>
    /// <param name="option">Указание какие типы расширений нужно разрешить</param>
    void AddCheckingFileExtensions(params FileExtensionsCollectionsEnum[] option);
}