using System.Windows;
using System.Windows.Controls;

namespace YTDownloader

{
    public class TextBoxWithValidation: TextBox
    {
        public string ValidationError
        {
            get { return (string)GetValue(ValidationErrorProperty); }
            set { SetValue(ValidationErrorProperty, value); }
        }
        public static readonly DependencyProperty ValidationErrorProperty = DependencyProperty.Register("ValidationError", typeof(string), typeof(TextBoxWithValidation), new PropertyMetadata(null, OnValidationErrorChanged));

        private static void OnValidationErrorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TextBoxWithValidation)d).UpdateErrorNotification();
        }    

        public bool HasValidationError
        {
            get { return (bool)GetValue(HasValidationErrorProperty); }
            set { SetValue(HasValidationErrorProperty, value); }
        }
        public static readonly DependencyProperty HasValidationErrorProperty = DependencyProperty.Register("HasValidationError", typeof(bool), typeof(TextBoxWithValidation), new PropertyMetadata(false));


        private void UpdateErrorNotification()
        {
            HasValidationError = !string.IsNullOrEmpty(ValidationError);
        }

    }
}
