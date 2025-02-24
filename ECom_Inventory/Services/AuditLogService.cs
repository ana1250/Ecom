using ECom_Inventory.Data;
using ECom_Inventory.Model;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;
using ECom_Inventory.Interfaces;

namespace ECom_Inventory.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly AppDbContext _context;

        public AuditLogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogChangeAsync(string tableName, int recordId, string action, string changedBy, object oldValues, object newValues)
        {
            try
            {
                var log = new AuditLogModel
                {
                    TableName = tableName,
                    RecordId = recordId,
                    Action = action,
                    ChangedBy = changedBy,
                    ChangeTimestamp = DateTime.UtcNow,
                    OldValues = oldValues != null ? JsonSerializer.Serialize(oldValues) : null,
                    NewValues = newValues != null ? JsonSerializer.Serialize(newValues) : null
                };

                _context.ProductAuditLog.Add(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error logging change for table {TableName}, record ID {RecordId}", tableName, recordId);
            }
        }
    }
}