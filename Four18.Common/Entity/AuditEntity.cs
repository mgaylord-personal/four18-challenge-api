using System;

namespace Four18.Common.Entity;

public class AuditEntity : StandardAuditEntity
{
    public Guid CreatedAuthId { get; set; }
}