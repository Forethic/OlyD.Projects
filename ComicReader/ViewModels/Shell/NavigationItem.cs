﻿using OlyD.Infrastructure;
using System;

namespace ComicReader.ViewModels
{
    public class NavigationItem : ObservableObject
    {
        public readonly string Glyph;
        public readonly string Label;
        public readonly Type ViewModel;


        public NavigationItem(Type viewModel)
        {
            ViewModel = viewModel;
        }

        public NavigationItem(int glyph, string label, Type viewModel)
            : this(viewModel)
        {
            Label = label;
            Glyph = Char.ConvertFromUtf32(glyph).ToString();
        }
    }
}