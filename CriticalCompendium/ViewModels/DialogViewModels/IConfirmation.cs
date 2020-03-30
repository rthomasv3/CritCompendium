using System;

namespace CritCompendium.ViewModels.DialogViewModels
{
    public interface IConfirmation
    {
		event EventHandler AcceptSelected;
		event EventHandler RejectSelected;
		event EventHandler CancelSelected;
	}
}
