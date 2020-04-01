namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class TableElementViewModel
   {
      #region Fields

      private readonly string _text;
      private readonly bool _isHeader;
      private readonly int _column;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates a new instance of <see cref="TableElementViewModel"/>
      /// </summary>
      public TableElementViewModel(string text, bool isHeader, int column)
      {
         _text = text;
         _isHeader = isHeader;
         _column = column;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets text
      /// </summary>
      public string Text
      {
         get { return _text; }
      }

      /// <summary>
      /// Gets isHeader
      /// </summary>
      public bool IsHeader
      {
         get { return _isHeader; }
      }

      /// <summary>
      /// Gets column
      /// </summary>
      public int Column
      {
         get { return _column; }
      }

      #endregion
   }
}
