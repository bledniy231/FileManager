using PianoMentor.DAL.Models.DataSet;

namespace PianoMentor.BLL.Services.FilesUploadManagers;

/// <summary>
/// В случае успешной загрузки, возвращает новый DataSet и null вместо Errors,
/// В ином случае будет возвращен null вместо NewDataSet и массив ошибок
/// </summary>
/// <param name="NewDataSet">Результирующий объект типа DataSet</param>
/// <param name="Errors">Список ошибок в случае их наличия</param>
public record UploadingResponse(DataSet? NewDataSet, string[]? Errors = null);