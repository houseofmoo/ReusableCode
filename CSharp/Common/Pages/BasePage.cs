using Common.Animations; // user define
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Common.Pages
{
    /// <summary>
    /// Base page for inherited page functionality
    /// </summary>
    /// <typeparam name="VM">The view model for this page</typeparam>
    public class BasePage<VM> : Page where VM : Common.ViewModels.BaseViewModel, new()
    {
        #region private fields
        private VM _viewModel;

        private Common.Animations.PageAnimation _pageLoadAnimation = Common.Animations.PageAnimation.FadeIn;
        private Common.Animations.PageAnimation _pageUnloadAnimation = Common.Animations.PageAnimation.FadeOut;

        private float _animationTime = 0.5f;
        #endregion

        #region public properties
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

        public Common.Animations.PageAnimation PageLoadAnimation
        {
            get { return _pageLoadAnimation; }
            set { _pageLoadAnimation = value; }
        }

        public Common.Animations.PageAnimation PageUnloadAnimation
        {
            get { return _pageUnloadAnimation; }
            set { _pageUnloadAnimation = value; }
        }
        #endregion

        #region constructors
        public BasePage()
        {
            // if we're animating, start out hidden
            if (this.PageLoadAnimation != Common.Animations.PageAnimation.None)
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
            if (this.PageLoadAnimation == Common.Animations.PageAnimation.None)
                return;

            switch (this.PageLoadAnimation)
            {
                case Animations.PageAnimation.FadeIn:
                    await this.FadeIn(this._animationTime); 
                    break;

                case Animations.PageAnimation.SlideAndFadeInFromRight:
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
            if (this.PageUnloadAnimation == Animations.PageAnimation.None)
                return;

            switch (this.PageUnloadAnimation)
            {
                case Animations.PageAnimation.FadeOut:
                    await this.FadeOut(this._animationTime);
                    break;

                case Animations.PageAnimation.SlideAndFadeOutToLeft:
                    await this.SlideAndFadeOutToLeft(this._animationTime);
                    break;

                default:
                    break;
            }
        }
    }
}
