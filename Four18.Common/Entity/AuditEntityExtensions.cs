using Four18.Common.Interfaces;

namespace Four18.Common.Entity;

public static class AuditEntityExtensions
{
    public static void CreateAuditEntity(this AuditEntity entity, ITenantContextProvider provider)
    {
        StandardAuditEntityExtensions.AuditEntityCreate(entity, provider);
        entity.CreatedAuthId = provider.GetUserGuid().GetValueOrDefault();
        entity.UpdateAuditEntity(provider, entity.Deleted);
    }

    public static void UpdateAuditEntity(this AuditEntity entity, ITenantContextProvider provider, 
        AuditEntity? originalEntity = null, bool delete = false)
    {
        StandardAuditEntityExtensions.AuditEntityUpdate(entity, provider, originalEntity, delete);
        if (originalEntity != null)
        {
            entity.CreatedAuthId = originalEntity.CreatedAuthId;
        }
    }

    public static void UpdateAuditEntity(this AuditEntity entity, ITenantContextProvider provider, bool delete = false)
    {
        StandardAuditEntityExtensions.AuditEntityUpdate(entity, provider, delete);
        if (entity.CreatedAuthId == default)
        {
            entity.CreatedAuthId = provider.GetUserGuid().GetValueOrDefault();
        }
    }
}