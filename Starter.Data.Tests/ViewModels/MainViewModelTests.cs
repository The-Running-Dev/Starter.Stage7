using System;
using System.Linq;
using System.Threading.Tasks;

using NUnit.Framework;
using FluentAssertions;

using Starter.Data.Entities;
using Starter.Data.ViewModels;

namespace Starter.Data.Tests.ViewModels
{
    /// <summary>
    /// Tests for the MainViewModel class
    /// </summary>
    public class MainViewModelTests : TestsBase
    {
        [Test]
        public void New_MainViewModel_Successful()
        {
            var viewModel = new MainViewModel(CatService);

            viewModel.Abilities.Should().NotBeNull();
            viewModel.IsCreating.Value.Should().BeFalse();
            viewModel.IsLoading.Value.Should().BeFalse();
            viewModel.IsNameFocused.Value.Should().BeFalse();
        }

        [Test]
        public async Task GetAll_Cats_Successful()
        {
            await ViewModel.GetAll();

            ViewModel.Cats.Count().Should().Be(Cats.Count);
            ViewModel.IsLoading.Value.Should().BeFalse();
        }

        [Test]
        public async Task GetById_ForCatId_Successful()
        {
            var lastCat = Cats.LastOrDefault();
            await ViewModel.GetById(lastCat.Id);

            ViewModel.DetailedCat.Should().Be(lastCat);
            ViewModel.IsCatSelected.Should().BeTrue();
        }

        [Test]
        public void Create_Cat_Successful()
        {
            ViewModel.Create();

            ViewModel.IsCreating.Value.Should().BeTrue();
            ViewModel.IsNameFocused.Value.Should().BeTrue();
            ViewModel.DetailedCat.AbilityId.Should().Be(0);
            ViewModel.IsCatSelected.Should().BeTrue();
        }

        [Test]
        public void Save_NewCat_Successful()
        {
            var cat = new Cat() { Id = Guid.NewGuid(), Name = Guid.NewGuid().ToString() };

            ViewModel.Create();
            ViewModel.DetailedCat = cat;
            ViewModel.Save();

            Cats.FirstOrDefault(x => x.Id == cat.Id).Should().BeEquivalentTo(cat);
        }

        [Test]
        public void Save_ExistingCat_Successful()
        {
            var cat = Cats.FirstOrDefault();
            var newName = Guid.NewGuid().ToString();

            cat.Name = newName;
            
            ViewModel.SelectedCat.Value = cat;
            ViewModel.Save();

            Cats.FirstOrDefault(x => x.Name == newName).Should().NotBeNull();
        }

        [Test]
        public void Delete_Cat_Successful()
        {
            var cat = Cats.FirstOrDefault();;
            
            ViewModel.DetailedCat = cat;
            ViewModel.Delete();

            Cats.FirstOrDefault(x => x.Id == cat.Id).Should().BeNull();
        }
    }
}