using NUnit.Framework;

using Starter.Mocks;
using Starter.Bootstrapper;
using Starter.API.Controllers;

namespace Starter.API.Tests
{
    /// <summary>
    /// Implements tests setup
    /// </summary>
    public class TestsBase
    {
        protected CatController CatController;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Setup.Bootstrap(SetupType.Test);

            CatController = new CatController(new CatRepositoryMock().Instance);
        }
    }
}