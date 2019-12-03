using System;
using System.Threading.Tasks;

using Starter.Bootstrapper;
using Starter.Data.Entities;
using Starter.Data.Repositories;
using Starter.Framework.Extensions;

namespace Starter.Azure.Populate
{
    public class Program
    {
        private static ICatRepository _repository;

        public static async Task Main(string[] args)
        {
            var isRelease = args.Length > 0 && args[0].IsEqualTo("-release");

            Setup.Bootstrap(isRelease ? SetupType.Release : SetupType.Debug);

            _repository = IocWrapper.Instance.GetService<ICatRepository>();

            await _repository.Create(new Cat("Widget", Ability.Eating));
            await _repository.Create(new Cat("Garfield", Ability.Engineering));
            await _repository.Create(new Cat("Mr. Boots", Ability.Napping));

            Console.WriteLine("Done...");
        }
    }
}