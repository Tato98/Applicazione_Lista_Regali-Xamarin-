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

        public GiftListCell()
        {
            image = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "gift.png"
            };

            name = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontSize = 24,
                FontAttributes = Xamarin.Forms.FontAttributes.Bold
            };
            name.SetBinding(Label.TextProperty, "Nome");

            description = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
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
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = { image }
            };

            stackLayout2 = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { name, description }
            };

            stackLayout3 = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
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

            this.View = stackLayout;
        }
    }
}
