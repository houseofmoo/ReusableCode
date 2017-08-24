﻿using Common.Animations; // user define
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Common.Pages
{
    /// <summary>
    /// Base page for inherited page functionality
    /// </summary>
    /// <typeparam name="VM">The view model for this page</typeparam>
    public class BasePage<VM> : Page where VM : ViewModels.BaseViewModel, new()
    {
        #region private fields
        private VM _viewModel;

        private PageAnimation _pageLoadAnimation = PageAnimation.FadeIn;
        private PageAnimation _pageUnloadAnimation = PageAnimation.FadeOut;

        private float _animationTime = 0.5f;
        #endregion

        #region public properties
        /// <summary>
        /// The view model for this page
        /// </summary>
        public VM ViewModel
        {
            get { return _viewModel; }
            set
            {
                if (_viewModel == value)
                    return;
                _viewModel = value;
                this.DataContext = _viewModel;
            }
        }

        /// <summary>
        /// The pages load animation
        /// </summary>
        public PageAnimation PageLoadAnimation
        {
            get { return _pageLoadAnimation; }
            set { _pageLoadAnimation = value; }
        }

        /// <summary>
        /// The pages unload animation
        /// </summary>
        public PageAnimation PageUnloadAnimation
        {
            get { return _pageUnloadAnimation; }
            set { _pageUnloadAnimation = value; }
        }
        #endregion

        #region constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public BasePage()
        {
            // if we're animating, start out hidden
            if (this.PageLoadAnimation != PageAnimation.None)
                this.Visibility = Visibility.Collapsed;

            // listen for page loading event
            this.Loaded += BasePage_Loaded;

            // load viewmodel
            this.ViewModel = new VM();
        }
        #endregion

        /// <summary>
        /// Once the page is loaded, perform animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            await AnimateIn();
        }

        /// <summary>
        /// Animates this page into view
        /// </summary>
        /// <returns></returns>
        private async Task AnimateIn()
        {
            // if no animation is defined, return
            if (this.PageLoadAnimation == PageAnimation.None)
                return;

            // add and play the appropriate animation
            switch (this.PageLoadAnimation)
            {
                case PageAnimation.FadeIn:
                    await this.FadeIn(this._animationTime); 
                    break;

                case PageAnimation.SlideInFromRight:
                    await this.SlideInFromRight(this._animationTime);
                    break;

                case PageAnimation.SlideAndFadeInFromRight:
                    await this.SlideAndFadeInFromRight(this._animationTime);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Animtes this page out of view
        /// </summary>
        /// <returns></returns>
        private async Task AnimateOut()
        {
            // if no animation is defined, return
            if (this.PageUnloadAnimation == PageAnimation.None)
                return;

            // add and play the appropriate animation
            switch (this.PageUnloadAnimation)
            {
                case PageAnimation.FadeOut:
                    await this.FadeOut(this._animationTime);
                    break;

                case PageAnimation.SlideOutToLeft:
                    await this.SlideOutToLeft(this._animationTime);
                    break;
                case PageAnimation.SlideAndFadeOutToLeft:
                    await this.SlideAndFadeOutToLeft(this._animationTime);
                    break;

                default:
                    break;
            }
        }
    }
}
