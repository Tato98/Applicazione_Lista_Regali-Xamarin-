using Applicazione_Lista_Regali.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Applicazione_Lista_Regali.Cell
{
    public class GiftListCell : ViewCell
    {
        Image image;
        Label name, description, budget;
        StackLayout stackLayout, stackLayout1, stackLayout2, stackLayout3;
        MenuItem deleteAction, modifyAction;
        IMenuItem menuItem;

        public GiftListCell(IMenuItem menuItem)
        {
            this.menuItem = menuItem;

            deleteAction = new MenuItem
            {
                Text = "Elimina"
            };
            deleteAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            deleteAction.Clicked += Delete_Clicked;

            modifyAction = new MenuItem
            {
                Text = "Modifica"
            };
            modifyAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            modifyAction.Clicked += Modify_Clicked;

            image = new Image
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Source = "gift.png",
                WidthRequest = 60,
                HeightRequest = 60,
                BackgroundColor = Color.Transparent
            };

            name = new Label
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                FontSize = 24,
                FontAttributes = Xamarin.Forms.FontAttributes.Bold
            };
            name.SetBinding(Label.TextProperty, "Nome");

            description = new Label
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                FontSize = 18
            };
            description.SetBinding(Label.TextProperty, "Descrizione");

            budget = new Label
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End
            };
            budget.SetBinding(Label.TextProperty, "Budget");

            stackLayout1 = new StackLayout
            {
                Padding = 10,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children = { image }
            };

            stackLayout2 = new StackLayout
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { name, description }
            };

            stackLayout3 = new StackLayout
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                Children = { budget }
            };

            stackLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal,
                Padding = 15,
                Children = { stackLayout1, stackLayout2, stackLayout3 }
            };

            this.ContextActions.Add(deleteAction);
            this.ContextActions.Add(modifyAction);
            this.View = stackLayout;
        }

        private void Modify_Clicked(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            var listaRegali = (ListaRegali)mi.CommandParameter;
            menuItem.ModifyItem(listaRegali);
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            var listaRegali = (ListaRegali)mi.CommandParameter;
            menuItem.RemoveItem(listaRegali);
        }

        public interface IMenuItem
        {
            void RemoveItem(ListaRegali listaRegali);
            void ModifyItem(ListaRegali listaRegali);
        }
    }
}
