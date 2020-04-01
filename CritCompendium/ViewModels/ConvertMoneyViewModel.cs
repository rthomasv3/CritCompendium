using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritCompendium.ViewModels.ObjectViewModels;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Business;

namespace CritCompendium.ViewModels
{
   public sealed class ConvertMoneyViewModel : ObjectViewModel
   {
      #region Fields

      private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();
      private readonly BagModel _bagModel;
      private readonly List<KeyValuePair<Currency, string>> _currencyOptions = new List<KeyValuePair<Currency, string>>();
      private readonly Dictionary<Tuple<Currency, Currency>, float> _conversionRates = new Dictionary<Tuple<Currency, Currency>, float>()
        {
            { new Tuple<Currency, Currency>(Currency.Copper, Currency.Silver), 0.1f },
            { new Tuple<Currency, Currency>(Currency.Copper, Currency.Electrum), 0.02f },
            { new Tuple<Currency, Currency>(Currency.Copper, Currency.Gold), 0.01f },
            { new Tuple<Currency, Currency>(Currency.Copper, Currency.Platinum), 0.001f },
            { new Tuple<Currency, Currency>(Currency.Silver, Currency.Electrum), 0.25f },
            { new Tuple<Currency, Currency>(Currency.Silver, Currency.Gold), 0.1f },
            { new Tuple<Currency, Currency>(Currency.Silver, Currency.Platinum), 0.01f },
            { new Tuple<Currency, Currency>(Currency.Electrum, Currency.Gold), 0.5f },
            { new Tuple<Currency, Currency>(Currency.Electrum, Currency.Platinum), 0.05f },
            { new Tuple<Currency, Currency>(Currency.Gold, Currency.Platinum), 0.1f },
        };
      private KeyValuePair<Currency, string> _selectedFromCurrencyOption;
      private KeyValuePair<Currency, string> _selectedToCurrencyOption;
      private int _toConvert;
      private int _conversionResult;
      private int _remaining;
      private int _newTotal;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="ConvertMoneyViewModel"/>
      /// </summary>
      public ConvertMoneyViewModel(BagModel bagModel)
      {
         _bagModel = bagModel;

         foreach (Currency currency in Enum.GetValues(typeof(Currency)))
         {
            if (currency != Currency.None)
            {
               _currencyOptions.Add(new KeyValuePair<Currency, string>(currency, _stringService.GetString(currency)));
            }
         }

         _selectedFromCurrencyOption = _currencyOptions[0];
         _selectedToCurrencyOption = _currencyOptions[0];

         UpdateConversionResults();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets currency options
      /// </summary>
      public List<KeyValuePair<Currency, string>> CurrencyOptions
      {
         get { return _currencyOptions; }
      }

      /// <summary>
      /// Gets or sets selected from currency option
      /// </summary>
      public KeyValuePair<Currency, string> SelectedFromCurrencyOption
      {
         get { return _selectedFromCurrencyOption; }
         set
         {
            _selectedFromCurrencyOption = value;
            UpdateConversionResults();
            OnPropertyChanged(nameof(FromCurrency));
         }
      }

      /// <summary>
      /// Gets or sets selected to currency option
      /// </summary>
      public KeyValuePair<Currency, string> SelectedToCurrencyOption
      {
         get { return _selectedToCurrencyOption; }
         set
         {
            _selectedToCurrencyOption = value;
            UpdateConversionResults();
            OnPropertyChanged(nameof(ToCurrency));
         }
      }

      /// <summary>
      /// Gets from currency
      /// </summary>
      public string FromCurrency
      {
         get { return _selectedFromCurrencyOption.Value; }
      }

      /// <summary>
      /// Gets to currency
      /// </summary>
      public string ToCurrency
      {
         get { return _selectedToCurrencyOption.Value; }
      }

      /// <summary>
      /// Gets or sets to convert
      /// </summary>
      public int ToConvert
      {
         get { return _toConvert; }
         set
         {
            _toConvert = value;
            UpdateConversionResults();
         }
      }

      /// <summary>
      /// Gets conversion result
      /// </summary>
      public int ConversionResult
      {
         get { return _conversionResult; }
      }

      /// <summary>
      /// Gets remaining
      /// </summary>
      public int Remaining
      {
         get { return _remaining; }
      }

      /// <summary>
      /// Gets new total
      /// </summary>
      public int NewTotal
      {
         get { return _newTotal; }
      }

      #endregion

      #region Private Methods

      private void UpdateConversionResults()
      {
         if (_selectedFromCurrencyOption.Key == _selectedToCurrencyOption.Key)
         {
            int amount = GetAmount(_selectedFromCurrencyOption.Key);
            _conversionResult = _toConvert;
            _remaining = amount;
            _newTotal = amount;
         }
         else
         {
            float rate = 1.0f;

            Tuple<Currency, Currency> conversionPair = new Tuple<Currency, Currency>(_selectedFromCurrencyOption.Key, _selectedToCurrencyOption.Key);
            if (_conversionRates.ContainsKey(conversionPair))
            {
               rate = _conversionRates[conversionPair];
            }
            else
            {
               conversionPair = new Tuple<Currency, Currency>(_selectedToCurrencyOption.Key, _selectedFromCurrencyOption.Key);
               if (_conversionRates.ContainsKey(conversionPair))
               {
                  rate = 1 / _conversionRates[conversionPair];
               }
            }

            int toConvertMax = GetAmount(_selectedFromCurrencyOption.Key);
            _toConvert = Math.Min(_toConvert, toConvertMax);

            _conversionResult = (int)(_toConvert * rate);
            _remaining = toConvertMax - (int)Math.Ceiling(_conversionResult / rate);
            _newTotal = GetAmount(_selectedToCurrencyOption.Key) + _conversionResult;
         }

         OnPropertyChanged(nameof(ToConvert));
         OnPropertyChanged(nameof(ConversionResult));
         OnPropertyChanged(nameof(Remaining));
         OnPropertyChanged(nameof(NewTotal));
      }

      private int GetAmount(Currency currency)
      {
         int amount = 0;
         switch (currency)
         {
            case Currency.Copper:
               amount = _bagModel.Copper;
               break;

            case Currency.Silver:
               amount = _bagModel.Silver;
               break;

            case Currency.Electrum:
               amount = _bagModel.Electrum;
               break;

            case Currency.Gold:
               amount = _bagModel.Gold;
               break;

            case Currency.Platinum:
               amount = _bagModel.Platinum;
               break;
         }
         return amount;
      }

      #endregion

      #region Protected Methods

      protected override void OnAccept()
      {
         switch (_selectedFromCurrencyOption.Key)
         {
            case Currency.Copper:
               _bagModel.Copper = _remaining;
               break;

            case Currency.Silver:
               _bagModel.Silver = _remaining;
               break;

            case Currency.Electrum:
               _bagModel.Electrum = _remaining;
               break;

            case Currency.Gold:
               _bagModel.Gold = _remaining;
               break;

            case Currency.Platinum:
               _bagModel.Platinum = _remaining;
               break;
         }

         switch (_selectedToCurrencyOption.Key)
         {
            case Currency.Copper:
               _bagModel.Copper = _newTotal;
               break;

            case Currency.Silver:
               _bagModel.Silver = _newTotal;
               break;

            case Currency.Electrum:
               _bagModel.Electrum = _newTotal;
               break;

            case Currency.Gold:
               _bagModel.Gold = _newTotal;
               break;

            case Currency.Platinum:
               _bagModel.Platinum = _newTotal;
               break;
         }

         Accept();
      }

      #endregion
   }
}
