using System;

namespace Four18.Common.Entity;

public class StandardAuditEntity
{
    public bool Deleted { get; set; }
    public DateTimeOffset CreateDate { get; set; }
    public DateTimeOffset ModifiedDate { get; set; }
    public Guid ModifiedAuthId { get; set; }
    public int TenantId { get; set; }
}