using System;
using System.Threading;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Starter.Data.Commands;
using Starter.Data.Entities;
using Starter.Data.Services;
using Starter.Framework.Extensions;

namespace Starter.Data.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class MainViewModel : IMainViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool AllowSave
        {
            get
            {
                if (DetailedCat == null)
                {
                    return false;
                }

                return DetailedCat.AbilityId != 0 && DetailedCat.Name.IsNotEmpty();
            }
        }

        public bool IsCatSelected => DetailedCat != null;

        public PropertyObservable<bool> IsCreating { get; set; }

        public PropertyObservable<bool> IsLoading { get; set; }

        public PropertyObservable<bool> IsNameFocused { get; set; }

        public ICommand CreateCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public List<object> Abilities { get; set; }

        public PropertyObservable<Cat> SelectedCat { get; set; }

        public Cat DetailedCat { get; set; }

        public ObservableCollection<IEntity> Cats
        {
            get => _cats;

            set
            {
                _cats = value;

                OnPropertyChanged(nameof(Cats));
            }
        }

        public MainViewModel(ICatService service)
        {
            _service = service;

            IsCreating = new PropertyObservable<bool>(false);
            IsLoading = new PropertyObservable<bool>(false);
            IsNameFocused = new PropertyObservable<bool>(false);

            SelectedCat = new PropertyObservable<Cat>(null);
            SelectedCat.PropertyChanged += OnSelectedCatPropertyChanged;

            CreateCommand = new CatCommand(Create, param => !IsCreating.Value);
            RefreshCommand = new CatCommand(async () => await GetAll(), param => !IsCreating.Value);
            SaveCommand = new CatCommand(Save, canExecute => AllowSave);
            DeleteCommand = new CatCommand(Delete, canExecute => IsCatSelected && !IsCreating.Value);
            CancelCommand = new CatCommand(ResetSelection, canExecute => IsCatSelected || IsCreating.Value);

            Abilities = new List<object>(typeof(Ability).ToNameValueList());

            Task.Run(GetAll);
        }

        public async Task GetAll()
        {
            IsLoading.Value = true;

            Cats = new ObservableCollection<IEntity>(await _service.GetAll());

            IsLoading.Value = false;
        }

        public async Task GetById(Guid id)
        {
            IsLoading.Value = true;

            DetailedCat = await _service.GetById(id);

            OnPropertyChanged(nameof(DetailedCat));
            OnPropertyChanged(nameof(IsCatSelected));

            IsLoading.Value = false;
        }

        public void Create()
        {
            IsCreating.Value = true;
            IsNameFocused.Value = false;

            DetailedCat = new Cat();

            IsNameFocused.Value = true;

            OnPropertyChanged(nameof(DetailedCat));
            OnPropertyChanged(nameof(IsCatSelected));
        }

        public async void Save()
        {
            IsLoading.Value = true;

            if (IsCreating.Value)
            {
                await _service.Create(DetailedCat);
            }
            else
            {
                await _service.Update(DetailedCat);
            }

            ResetSelection();

            // Hack: Make the thread sleep so the consumer
            // has time to consume the message
            Thread.Sleep(2000);

            await GetAll();
        }

        public async void Delete()
        {
            IsLoading.Value = true;

            await _service.Delete(DetailedCat.Id);

            ResetSelection();

            await GetAll();
        }

        private void OnSelectedCatPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var cat = ((PropertyObservable<Cat>)sender)?.Value;

            if (cat != null)
            {
                IsNameFocused.Value = false;
                IsCreating.Value = cat.Id.Equals(Guid.Empty);

                if (!IsCreating.Value)
                {
                    Task.Run(function: () => GetById(cat.Id));
                }
            }

            IsNameFocused.Value = true;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ResetSelection()
        {
            DetailedCat = null;
            SelectedCat.Value = null;
            IsCreating.Value = false;

            OnPropertyChanged(nameof(SelectedCat));
            OnPropertyChanged(nameof(IsCatSelected));
        }

        private ObservableCollection<IEntity> _cats;

        private readonly ICatService _service;
    }
}