using System.Security.Claims;
using System.Text;
using Core.Domain;
using Core.Persistence;
using Core.Utilities.Date;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Persistence.Context;

namespace Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly BaseDbContext _context;
    private readonly IHttpContextAccessor _contextAccessor;
    private bool _disposed;

    public UnitOfWork(BaseDbContext context, IHttpContextAccessor contextAccessor)
    {
        _context = context;
        _contextAccessor = contextAccessor;
    }

    public void Dispose()
    {
        Clean(true);
    }

    public void SaveChanges()
    {
        AddAuditLog();
        _context.SaveChanges();
    }

    private void Clean(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing && _context is not null)
            {
                _context.Dispose();
            }
        }

        _disposed = true;
        GC.SuppressFinalize(this);
    }
    
    private void AddAuditLog()
    {
        var modifiedEntities = _context.ChangeTracker.Entries().ToList();

        foreach (var audit in modifiedEntities.Select(CreateAuditEntry))
        {
            _context.Audits.Add(audit);
        }
    }

    private Audit CreateAuditEntry(EntityEntry entry)
    {
        return new Audit
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateHelper.GetCurrentDate(),
            CreatedBy = GetCurrentUser(),
            UpdatedAt = DateHelper.GetCurrentDate(),
            UpdatedBy = GetCurrentUser(),
            EntityName = entry.Entity.GetType().Name,
            ActionType = GetActionType(entry),
            UserId = GetUserId(),
            Date = DateHelper.GetCurrentDate(),
            Details = GetDetails(entry),
            OriginalValue = GetOriginalValue(entry),
            OldValue = GetOldValue(entry),
            NewValue = GetNewValue(entry),
            ClientIP = GetClientIp(),
            UserAgent = GetUserAgent()
        };
    }

    private string GetCurrentUser()
    {
        return _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name) ?? "Anonymous";
    }

    private string GetUserId()
    {
        return _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous";
    }

    private string GetClientIp()
    {
        return _contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Anonymous";
    }

    private string GetUserAgent()
    {
        return _contextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString() ?? "Anonymous";
    }

    private static string GetActionType(EntityEntry entry)
    {
        return entry.State switch
        {
            EntityState.Added => "Insert",
            EntityState.Modified => "Update",
            EntityState.Deleted => "Delete",
            EntityState.Detached => "Detached",
            EntityState.Unchanged => "Unchanged",
            _ => "Unknown"
        };
    }


    private static string GetDetails(EntityEntry entry)
    {
        return entry.State switch
        {
            EntityState.Added => "Inserted",
            EntityState.Modified => "Updated",
            EntityState.Deleted => "Deleted",
            EntityState.Detached => "Detached",
            EntityState.Unchanged => "Unchanged",
            _ => "Unknown"
        };
    }

    private static string GetOldValue(EntityEntry entry)
    {
        if (entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
        {
            var oldValueBuilder = new StringBuilder();

            foreach (var property in entry.Properties)
            {
                var propertyName = property.Metadata.Name;
                var originalValue = property.OriginalValue ?? string.Empty;

                oldValueBuilder.Append($"{propertyName}: {originalValue} | ");
            }

            return oldValueBuilder.ToString();
        }
        
        return string.Empty;
    }

    private static string GetNewValue(EntityEntry entry)
    {
        if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
        {
            var newValueBuilder = new StringBuilder();

            foreach (var property in entry.Properties)
            {
                var propertyName = property.Metadata.Name;
                var currentValue = property.CurrentValue ?? string.Empty;

                newValueBuilder.Append($"{propertyName}: {currentValue} | ");
            }

            return newValueBuilder.ToString();
        }

        return string.Empty;
    }
    
    private static string GetOriginalValue (EntityEntry entry)
    {
        if (entry.State != EntityState.Added)
        {
            var newValueBuilder = new StringBuilder();

            foreach (var property in entry.Properties)
            {
                var propertyName = property.Metadata.Name;
                var originalValue = property.OriginalValue ?? string.Empty;

                newValueBuilder.Append($"{propertyName}: {originalValue} | ");
            }

            return newValueBuilder.ToString();
        }

        return string.Empty;
    }
}