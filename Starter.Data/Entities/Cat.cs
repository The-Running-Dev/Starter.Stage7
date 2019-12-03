using System;

namespace Starter.Data.Entities
{
    /// <summary>
    /// Implements the Cat entity
    /// </summary>
    public class Cat : Entity
    {
        public Cat()
        {
        }

        public Cat(string name, Ability abilityId)
        {
            Id = Guid.NewGuid();
            SecondaryId = Guid.NewGuid();

            PartitionKey = ((int)abilityId).ToString();
            RowKey = Id.ToString();

            Name = name;
            AbilityId = ((int)abilityId);
            Ability = abilityId;
        }

        public string Name { get; set; }

        public int AbilityId { get; set; }

        public Ability Ability
        {
            get => (Ability)AbilityId;
            set => AbilityId = (int) value;
        }

        public Guid SecondaryId { get; set; }
    }
}