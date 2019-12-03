using System;

using Microsoft.Azure.Cosmos.Table;

namespace Starter.Data.Entities
{
    /// <summary>
    /// Implements a generic entity
    /// </summary>
    public class Entity: TableEntity, IEntity
    {
        public Guid Id { get; set; }

        public Entity()
        {
            Id = Guid.Empty;
        }
    }
}
