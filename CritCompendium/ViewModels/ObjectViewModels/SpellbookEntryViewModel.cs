using System;
using System.Windows.Input;
using CritCompendium.Services;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class SpellbookEntryViewModel : NotifyPropertyChanged
   {
      #region Fields

      private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();
      private readonly SpellbookEntryModel _spellbookEntryModel;
      private readonly ICommand _viewDetailsCommand;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="SpellbookEntryViewModel"/>
      /// </summary>
      public SpellbookEntryViewModel(SpellbookEntryModel spellbookEntryModel)
      {
         _spellbookEntryModel = spellbookEntryModel;

         _viewDetailsCommand = new RelayCommand(obj => true, obj => ViewSpellDetails());
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets id
      /// </summary>
      public Guid ID
      {
         get { return _spellbookEntryModel.ID; }
      }

      /// <summary>
      /// Gets spellbook entry model
      /// </summary>
      public SpellbookEntryModel SpellbookEntryModel
      {
         get { return _spellbookEntryModel; }
      }

      /// <summary>
      /// Gets spell
      /// </summary>
      public SpellModel Spell
      {
         get { return _spellbookEntryModel.Spell; }
      }

      /// <summary>
      /// Gets spell name
      /// </summary>
      public string SpellName
      {
         get { return _spellbookEntryModel.Spell.Name; }
      }

      /// <summary>
      /// Gets or sets prepared
      /// </summary>
      public bool Prepared
      {
         get { return _spellbookEntryModel.Prepared; }
         set { _spellbookEntryModel.Prepared = value; }
      }

      /// <summary>
      /// Gets or sets used
      /// </summary>
      public bool Used
      {
         get { return _spellbookEntryModel.Used; }
         set
         {
            _spellbookEntryModel.Used = value;
            OnPropertyChanged(nameof(Used));
         }
      }

      /// <summary>
      /// Gets view details command
      /// </summary>
      public ICommand ViewDetailsCommand
      {
         get { return _viewDetailsCommand; }
      }

      #endregion

      #region Private Methods

      private void ViewSpellDetails()
      {
         _dialogService.ShowDetailsDialog(new SpellViewModel(_spellbookEntryModel.Spell));
      }

      #endregion
   }
}
