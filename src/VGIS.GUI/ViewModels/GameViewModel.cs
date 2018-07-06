using System;
using System.Security.RightsManagement;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
using Prism.Commands;
using Prism.Mvvm;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;
using VGIS.Domain.Services;
using VGIS.GUI.Structs;

namespace VGIS.GUI.ViewModels
{
    public class GameViewModel : BindableBase
    {
        private readonly Game _game;
        private readonly IntroductionActivationService _introEditionService;

        private IntroductionStateEnum _introductionCurrentState;
        private string _introDesactivatedVisibility = UiVisibilityStruct.Hidden;
        private string _introEnabledVisibility = UiVisibilityStruct.Hidden;
        private string _introStateUnknownVisibility = UiVisibilityStruct.Hidden;

        #region Ctor
        public GameViewModel(Game game, IntroductionActivationService introEditionService)
        {
            _game = game;
            _introEditionService = introEditionService;
            ChangeStateCommand = new DelegateCommand(ChangeState);
            IntroductionCurrentState = _game.DetectionResult.IntroductionState;
        }
        #endregion

        public ICommand ChangeStateCommand { get; }

        public string IllustrationUrl => _game.IllustrationUrl;
        public string Name => _game.Name;
        public float Opacity => _game.IsDetected ? 1f : 0.2f;
        public bool IsDetected => _game.IsDetected;
        public bool IsEnabled;

        public string IntroDesactivatedVisibility
        {
            get => _introDesactivatedVisibility;
            set => SetProperty(ref _introDesactivatedVisibility, value);
        }

        public string IntroEnabledVisibility
        {
            get => _introEnabledVisibility;
            set => SetProperty(ref _introEnabledVisibility, value);
        }

        public string IntroStateUnknownVisibility
        {
            get => _introStateUnknownVisibility;
            set => SetProperty(ref _introStateUnknownVisibility, value);
        }

        public IntroductionStateEnum IntroductionCurrentState
        {
            get => _introductionCurrentState;
            set
            {
                switch (value)
                {
                    case IntroductionStateEnum.Disabled:
                        IntroDesactivatedVisibility = UiVisibilityStruct.Visible;
                        IntroEnabledVisibility = UiVisibilityStruct.Hidden;
                        IntroStateUnknownVisibility = UiVisibilityStruct.Hidden;
                        break;
                    case IntroductionStateEnum.Enabled:
                        IntroDesactivatedVisibility = UiVisibilityStruct.Hidden;
                        IntroEnabledVisibility = UiVisibilityStruct.Visible;
                        IntroStateUnknownVisibility = UiVisibilityStruct.Hidden;
                        break;
                    case IntroductionStateEnum.Unknown:
                        IntroDesactivatedVisibility = UiVisibilityStruct.Hidden;
                        IntroEnabledVisibility = UiVisibilityStruct.Hidden;
                        IntroStateUnknownVisibility = UiVisibilityStruct.Visible;
                        break;
                }

                SetProperty(ref _introductionCurrentState, value);
            }
        }

        private void ChangeState()
        {
            if (IntroductionCurrentState == IntroductionStateEnum.Enabled ||
                IntroductionCurrentState == IntroductionStateEnum.Unknown)
            {
                DisableIntro();
            }
            else if (IntroductionCurrentState == IntroductionStateEnum.Disabled)
            {
                EnableIntro();
            }
        }

        public void ChangeStateCommandTo(IntroductionStateEnum newState)
        {
            switch (newState)
            {
                case IntroductionStateEnum.Enabled:
                    EnableIntro();
                    break;
                case IntroductionStateEnum.Disabled:
                    DisableIntro();
                    break;
                default:
                    throw new Exception("Not Supported");
            }
        }

        private void EnableIntro()
        {
            var reenableTask = Task.Run(() => { _introEditionService.ReenableIntro(_game); });
            reenableTask.ContinueWith(p =>
            {
                IntroductionCurrentState = IntroductionStateEnum.Enabled;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            reenableTask.ContinueWith(p =>
            {
                Console.WriteLine("DisableTask didn't complete");
                IntroductionCurrentState = IntroductionStateEnum.Unknown;
            }, TaskContinuationOptions.NotOnRanToCompletion);
        }

        private void DisableIntro()
        {
            var disableTask = Task.Run(() => { _introEditionService.DisableIntro(_game); });
            disableTask.ContinueWith(p =>
            {
                IntroductionCurrentState = IntroductionStateEnum.Disabled;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            disableTask.ContinueWith(p =>
            {
                Console.WriteLine("DisableTask didn't complete");
                IntroductionCurrentState = IntroductionStateEnum.Unknown;
            }, TaskContinuationOptions.NotOnRanToCompletion);
        }
    }
}