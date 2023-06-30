using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain;

[Table("Audits", Schema = "dbo")]
public class Audit : BaseEntity
{
    public string EntityName { get; set; }
    public string ActionType { get; set; }
    public string UserId { get; set; }
    public DateTime Date { get; set; }
    public string Details { get; set; }
    public string? OriginalValue { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public string ClientIP { get; set; }
    public string UserAgent { get; set; }
}