using System.Collections.Generic;

using Starter.Data.Entities;

namespace Starter.Mocks
{
    public static class TestData
    {
        public static List<Cat> Cats = new List<Cat>
            {
                new Cat("Widget", Ability.Eating),
                new Cat("Garfield",Ability.Engineering),
                new Cat("Mr. Boots", Ability.Lounging)
            };
    }
}