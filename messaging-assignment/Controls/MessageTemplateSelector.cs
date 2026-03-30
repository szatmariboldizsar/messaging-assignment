using Microsoft.Maui.Controls;
using DAL.Models;
using messaging_assignment.ViewModels;

namespace messaging_assignment.Controls
{
    public class MessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate IncomingTemplate { get; set; }
        public DataTemplate OutgoingTemplate { get; set; }

        //// Make this a BindableProperty so XAML binding works
        //public static readonly BindableProperty LoggedInUserIdProperty =
        //    BindableProperty.Create(
        //        nameof(LoggedInUserId),
        //        typeof(long),
        //        typeof(MessageTemplateSelector),
        //        default(long));

        //public long LoggedInUserId { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is Message msg && container.BindingContext is MessageViewModel vm)
            {
                return msg.FromUserId == vm.LoggedInUser.Id ? OutgoingTemplate : IncomingTemplate;
            }
            return IncomingTemplate;
        }
    }
}