﻿using Aki.Launcher.Helpers;
using ReactiveUI;

namespace Aki.Launcher.ViewModels.Dialogs
{
    public class ConfirmationDialogViewModel : ViewModelBase
    {
        public string Question { get; set; }
        public string ConfirmButtonText { get; set; }
        public string DenyButtonText { get; set; }

        /// <summary>
        /// A confirmation dialog
        /// </summary>
        /// <param name="Host">Set to null when <see cref="ViewModelBase.ShowDialog(object)"/> is used, since the dialog host is handling routing</param>
        /// <param name="Question"></param>
        /// <param name="ConfirmButtonText"></param>
        /// <param name="DenyButtonText"></param>
        public ConfirmationDialogViewModel(IScreen Host, string Question, string? ConfirmButtonText = null, string? DenyButtonText = null) : base(Host)
        {
            this.Question = Question;
            this.ConfirmButtonText = ConfirmButtonText ?? LocalizationProvider.Instance.yes;
            this.DenyButtonText = DenyButtonText ?? LocalizationProvider.Instance.no;
        }
    }
}
