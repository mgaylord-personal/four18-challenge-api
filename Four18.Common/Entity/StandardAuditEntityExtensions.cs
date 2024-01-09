using System;
using Four18.Common.Interfaces;

namespace Four18.Common.Entity;

public static class StandardAuditEntityExtensions
{
    public static void CreateStandardAuditEntity(this StandardAuditEntity entity, ITenantContextProvider provider)
    {
        AuditEntityCreate(entity, provider);
    }

    internal static void AuditEntityCreate(StandardAuditEntity entity, ITenantContextProvider provider)
    {
        entity.Deleted = false;
        entity.TenantId = provider.GetTenantId().GetValueOrDefault();
        entity.CreateDate = DateTimeOffset.Now;
        entity.UpdateStandardAuditEntity(provider, entity.Deleted);
    }

    public static void UpdateStandardAuditEntity(this StandardAuditEntity entity, ITenantContextProvider provider, 
        StandardAuditEntity? originalEntity = null, bool delete = false)
    {
        AuditEntityUpdate(entity, provider, originalEntity, delete);
    }

    internal static void AuditEntityUpdate(StandardAuditEntity entity, ITenantContextProvider provider,
        StandardAuditEntity? originalEntity, bool delete)
    {
        if (originalEntity != null)
        {
            entity.Deleted = originalEntity.Deleted;
            entity.TenantId = provider.GetTenantId().GetValueOrDefault();
            entity.CreateDate = originalEntity.CreateDate;
        }

        entity.UpdateStandardAuditEntity(provider, delete);
    }

    public static void UpdateStandardAuditEntity(this StandardAuditEntity entity, ITenantContextProvider provider, bool delete = false)
    {
        AuditEntityUpdate(entity, provider, delete);
    }

    internal static void AuditEntityUpdate(StandardAuditEntity entity, ITenantContextProvider provider, bool delete)
    {
        if (entity.CreateDate == default)
        {
            entity.CreateDate = DateTimeOffset.Now;
        }

        if (delete)
        {
            entity.Deleted = true;
        }

        entity.ModifiedDate = DateTimeOffset.Now;
        entity.ModifiedAuthId = provider.GetUserGuid().GetValueOrDefault();
    }
}