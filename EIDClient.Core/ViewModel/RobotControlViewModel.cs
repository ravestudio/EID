using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace EIDClient.Core.ViewModel
{
    public class RobotControlViewModel : ViewModelBase
    {
        private INavigationService _navigationService = null;
        private IMainCommandBar _commandBar = null;

        public RelayCommand StartRobot { get; set; }

        private Robot.Robot _robot = null;

        public RobotControlViewModel(INavigationService navigationService, IMainCommandBar commandBar)
        {
            this._navigationService = navigationService;
            this._commandBar = commandBar;

            this.StartRobot = new RelayCommand(() =>
            {
                this._robot = new Robot.Robot(new Robot.Strategy());

                this._robot.Run();

            });
        }

        public void LoadData()
        {

            _commandBar.Clear();
            _commandBar.AddCommandButton(new AppBarButton()
            {
                Icon = new SymbolIcon(Symbol.Play),
                Command = this.StartRobot
            });

        }
    }
}
