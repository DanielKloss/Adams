namespace TheClockEnd.UI.ViewModels
{
    public class ConfirmContentDialogViewModel : BaseViewModel
    {
        private string _title;
        public string title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(title));
            }
        }

        private string _text;
        public string text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged(nameof(text));
            }
        }

        public ConfirmContentDialogViewModel(string Title, string Text)
        {
            title = Title;
            text = Text;
        }
    }
}
