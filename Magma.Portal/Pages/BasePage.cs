using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Magma.Portal
{
    /// <summary>
    /// A base page for all pages to gain base functionality 
    /// </summary>
    public class BasePage<VM> : Page
        where VM : BaseViewModel, new()
    {
        #region Private members
        /// <summary>
        /// The page's View Model
        /// </summary>
        private VM _ViewModel;
        #endregion

        #region Properties
        /// <summary>
        /// The animation to play when the page is loaded
        /// </summary>
        public PageAnimation PageLoadAnimation { get; set; } = PageAnimation.SlideAndFadeInFromRight;

        /// <summary>
        /// The animation to play when the page is unloaded
        /// </summary>
        public PageAnimation PageUnloadAnimation { get; set; } = PageAnimation.SlideAndFadeOutToLeft;

        /// <summary>
        /// The time any slide animation takes to complete
        /// </summary>
        public float SlideSeconds { get; set; } = 0.8f;

        /// <summary>
        /// The View Model associated with this page
        /// </summary>
        public VM ViewModel
        {
            get { return _ViewModel; }
            set
            {
                if (_ViewModel == value)
                    return;

                //Update the View Model
                _ViewModel = value;

                //Set the data context for this page
                DataContext = _ViewModel;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Deafult constructor
        /// </summary>
        public BasePage()
        {
            //Hide the page if animating in 
            if (PageLoadAnimation != PageAnimation.None)
                Visibility = Visibility.Collapsed;

            //Listen out for the page loading
            Loaded += BasePage_Loaded;

            //Create a default View Model
            DataContext = new VM();
        }
        #endregion

        #region Animation load/unload
        /// <summary>
        /// Once the page is loaded, perform any required animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BasePage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //Animate the page in
            AnimateIn();
        }
        
        /// <summary>
        /// Animates this page in
        /// </summary>
        /// <returns></returns>
        public async Task AnimateIn()
        {
            //Check if an animation is to be started
            if (PageLoadAnimation == PageAnimation.None)
                return;

            switch (PageLoadAnimation)
            {
                case PageAnimation.SlideAndFadeInFromRight:
                    //Start the animation
                    await this.SlideAndFadeInFromRight(SlideSeconds);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Animates this page out
        /// </summary>
        /// <returns></returns>
        public async Task AnimateOut()
        {
            //Check if an animation is to be started
            if (PageUnloadAnimation == PageAnimation.None)
                return;

            switch (PageUnloadAnimation)
            {                
                case PageAnimation.SlideAndFadeOutToLeft:
                    //Start the animation
                    await this.SlideAndFadeOutToLeft(SlideSeconds);
                    break;

                default:
                    break;
            }
        }
        #endregion
    }
}
