using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace SendContact
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ContactPicker picker = new ContactPicker();
            picker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
            picker.SelectionMode = ContactSelectionMode.Fields;
            Contact contact = await picker.PickContactAsync();
            if (contact != null)
            {
                //set name title
                lblFullName.Text = contact.DisplayName;
                //set list of number
                listNumber.Items.Clear();
                foreach (var phone in contact.Phones)
                {
                    CheckBox cb = new CheckBox();
                    cb.Click += cb_Click;
                    cb.Content = phone.Number;
                    listNumber.Items.Add(cb);
                }

            }
        }

        void cb_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var cb in listNumber.Items)
            {
                CheckBox check = (CheckBox)cb;
                if ((bool)check.IsChecked)
                {
                    sb.Append(Environment.NewLine + check.Content.ToString());
                }
            }
            txtPreview.Text = string.IsNullOrEmpty(sb.ToString()) ? string.Empty : lblFullName.Text + sb.ToString();
        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPreview.Text))
            {
                await SendMessage(txtPreview.Text);
            }
        }
        public async Task SendMessage(string smsText)
        {
            Windows.ApplicationModel.Chat.ChatMessage msg = new Windows.ApplicationModel.Chat.ChatMessage();
            msg.Body = smsText;
            await Windows.ApplicationModel.Chat.ChatMessageManager.ShowComposeSmsMessageAsync(msg);
        }
    }
}
