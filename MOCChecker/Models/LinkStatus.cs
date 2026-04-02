namespace MOCChecker.Models
{
    public enum LinkStatus
    {
        Pending,    // Еще не проверялась
        Valid,      // Файл существует / HTTP 200 OK
        Broken,     // Файл не найден / HTTP 404, 500
        Timeout     // Внешний ресурс не ответил
    }
}