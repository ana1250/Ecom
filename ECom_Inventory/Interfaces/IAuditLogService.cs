namespace ECom_Inventory.Interfaces
{
    public interface IAuditLogService
    {
        Task LogChangeAsync(string tableName, int recordId, string action, string changedBy, object oldValues, object newValues);
    }
}
