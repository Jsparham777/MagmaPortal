using System.Windows;
using System.Windows.Input;

namespace Magma.Portal
{
    /// <summary>
    /// The view model for the custom flat window
    /// </summary>
    public class WindowViewModel : BaseViewModel
    {
        #region Private Members
        /// <summary>
        /// The window this view model controls
        /// </summary>
        private Window _Window;

        /// <summary>
        /// The window resizer helper that keeps the window size correct in various states
        /// </summary>
        private WindowResizer _WindowResizer;

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        private Thickness _OuterMarginSize = new Thickness(5);

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        private int _WindowRadius = 10;
        
        /// <summary>
        /// The last known dock position
        /// </summary>
        private WindowDockPosition _DockPosition = WindowDockPosition.Undocked;
        #endregion

        #region Properties
        /// <summary>
        /// The minimum width the window can be resized to
        /// </summary>
        public double WindowMinimumWidth { get; set; } = 800;

        /// <summary>
        /// The minimum height the window can be resized to
        /// </summary>
        public double WindowMinimumHeight { get; set; } = 500;

        /// <summary>
        /// True if the window is currently being moved/dragged
        /// </summary>
        public bool BeingMoved { get; set; }

        /// <summary>
        /// True if the window should be borderless because it is docked or maximized
        /// </summary>
        public bool Borderless => (_Window.WindowState == WindowState.Maximized || _DockPosition != WindowDockPosition.Undocked);

        /// <summary>
        /// The size of the resize border around the window
        /// </summary>
        public int ResizeBorder => _Window.WindowState == WindowState.Maximized ? 0 : 4;

        /// <summary>
        /// The size of the resize border around the window, taking into account the outer margin
        /// </summary>
        public Thickness ResizeBorderThickness => new Thickness(_OuterMarginSize.Left + ResizeBorder,
                                                                _OuterMarginSize.Top + ResizeBorder,
                                                                _OuterMarginSize.Right + ResizeBorder,
                                                                _OuterMarginSize.Bottom + ResizeBorder);
                /// <summary>
        /// The padding of the inner content of the main window
        /// </summary>
        public Thickness InnerContentPadding { get; set; } = new Thickness(0);

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        public Thickness OuterMarginSize
        {
            //If it is maximised or docked, no border
            get => _Window.WindowState == WindowState.Maximized ? _WindowResizer.CurrentMonitorMargin : (Borderless ? new Thickness(0) : _OuterMarginSize);
            set => _OuterMarginSize = value;
        }

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public int WindowRadius
        {
            // If it is maximized or docked, no border
            get => Borderless ? 0 : _WindowRadius;
            set => _WindowRadius = value;
        }

        /// <summary>
        /// The rectangle border around the window when docked
        /// </summary>
        public int FlatBorderThickness => Borderless && _Window.WindowState != WindowState.Maximized ? 1 : 0;

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public CornerRadius WindowCornerRadius => new CornerRadius(WindowRadius);

        /// <summary>
        /// The height of the title bar / caption of the window
        /// </summary>
        public int TitleHeight { get; set; } = 42;
        /// <summary>
        /// The height of the title bar / caption of the window
        /// </summary>
        public GridLength TitleHeightGridLength => new GridLength(TitleHeight + ResizeBorder);

        /// <summary>
        /// True if we should have a dimmed overlay on the window
        /// such as when a popup is visible or the window is not focused
        /// </summary>
        public bool DimmableOverlayVisible { get; set; }


        /// <summary>
        /// The current page of the application
        /// </summary>
        public ApplicationPage CurrentPage { get; set; } = ApplicationPage.Login;
        #endregion

        #region Commands
        /// <summary>
        /// The command to minimize the window
        /// </summary>
        public ICommand MinimizeCommand { get; set; }

        /// <summary>
        /// The command to maximize the window
        /// </summary>
        public ICommand MaximizeCommand { get; set; }

        /// <summary>
        /// The command to close the window
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// The command to show the system menu
        /// </summary>
        public ICommand MenuCommand { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public WindowViewModel(Window window)
        {
            _Window = window;

            // Listen out for the window resizing
            _Window.StateChanged += (sender, e) =>
            {
                // Fire off events for all properties that are affected by a resize
                WindowResized();
            };

            // Create commands
            MinimizeCommand = new RelayCommand(() => _Window.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => _Window.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => _Window.Close());
            MenuCommand = new RelayCommand(() => _Window.PointToScreen(Mouse.GetPosition(_Window)));

            // Fix window resize issue
            _WindowResizer = new WindowResizer(_Window);

            // Listen out for dock changes
            _WindowResizer.WindowDockChanged += (dock) =>
            {
                // Store last position
                _DockPosition = dock;

                // Fire off resize events
                WindowResized();
            };

            // On window being moved/dragged
            _WindowResizer.WindowStartedMove += () =>
            {
                // Update being moved flag
                BeingMoved = true;
            };

            // Fix dropping an undocked window at top which should be positioned at the
            // very top of screen
            _WindowResizer.WindowFinishedMove += () =>
            {
                // Update being moved flag
                BeingMoved = false;

                // Check for moved to top of window and not at an edge
                if (_DockPosition == WindowDockPosition.Undocked && _Window.Top == _WindowResizer.CurrentScreenSize.Top)
                    // If so, move it to the true top (the border size)
                    _Window.Top = -OuterMarginSize.Top;
            };
        }
        #endregion

        #region Private Helpers
        /// <summary>
        /// If the window resizes to a special position (docked or maximized)
        /// this will update all required property change events to set the borders and radius values
        /// </summary>
        private void WindowResized()
        {
            // Fire off events for all properties that are affected by a resize
            OnPropertyChanged(nameof(Borderless));
            OnPropertyChanged(nameof(FlatBorderThickness));
            OnPropertyChanged(nameof(ResizeBorderThickness));
            OnPropertyChanged(nameof(OuterMarginSize));
            OnPropertyChanged(nameof(WindowRadius));
            OnPropertyChanged(nameof(WindowCornerRadius));
        }
        #endregion
    }
}
