using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Magma.Portal
{
    /// <summary>
    /// The view model for a login screen
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        #region Private members


        #endregion

        #region Properties
        /// <summary>
        /// The user's email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The user's password
        /// </summary>
        public SecureString Password { get; set; }

        #endregion

        #region Commands
        /// <summary>
        /// The command to login
        /// </summary>
        public ICommand LoginCommand { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginViewModel()
        {
            //Create commands
            LoginCommand = new RelayParameterizedCommand(async (parameter) => await Login(parameter));            
        }

        /// <summary>
        /// Attmpets to log the user in
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/> passed in from the view for the users password</param>
        /// <returns></returns>
        public async Task Login(object parameter)
        {
            await Task.Delay(500);
        }
        #endregion
    }
}
