namespace ECom_Inventory.Model
{
    public class AuditLogModel
    {
            public int Id { get; set; }
            public string TableName { get; set; }
            public int RecordId { get; set; }
            public string Action { get; set; }
            public string ChangedBy { get; set; }
            public DateTime ChangeTimestamp { get; set; } = DateTime.UtcNow;
            public string OldValues { get; set; }
            public string NewValues { get; set; }
    }
}
