using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using NasobeniEdhouse.Models;
using NasobeniEdhouse.Models.EdhouseTask;
using NasobeniEdhouse.Views;

namespace NasobeniEdhouse.ViewModels
{
    public class NasobeniViewModel : BaseViewModel
    {
        public NasobeniViewModel()
        {
            processedValuesHolder = new ProcessedValuesHolder();
            graphViewModel = new GraphViewModel(processedValuesHolder);
            loadingViewModel = new  LoadingViewModel();
            activeInputViewModel = new ActiveInputViewModel(this);
            processedInputViewModel = new ProcessedInputViewModel();
            messageViewModel = new MessageViewModel();
            infoViewModel = new InfoViewModel(processedValuesHolder);
            startUpViewModel = new StartUpViewModel();
            selectedOrderMethod = OrderBy.Sestupně;

            graphPartViewModel = startUpViewModel;
            InputPartViewModel = activeInputViewModel;

            ChangeState(State.Edit);
        }

        private BaseViewModel graphPartViewModel;
        private BaseViewModel inputPartViewModel;
        private GraphViewModel graphViewModel;
        private LoadingViewModel loadingViewModel;
        private ActiveInputViewModel activeInputViewModel;
        private ProcessedInputViewModel processedInputViewModel;
        private MessageViewModel messageViewModel;
        private string actionButtonText = "Najdi násobek";
        private InfoViewModel infoViewModel;
        private ProcessedValuesHolder processedValuesHolder;
        private StartUpViewModel startUpViewModel;
        

        private int neighbouringNumberRange = 13;

        private enum State
        {
            InvalidInput,
            Proccesing,
            Proccesed,
            Stopped,
            Edit
        }

        private State currentState;

        public  enum OrderBy
        {
            Vzestupně,
            Sestupně,
            podlePořadí
        }

        public IList<OrderBy> OrderByEnum
        {
            get
            {
                return Enum.GetValues(typeof(OrderBy)).Cast<OrderBy>().ToList<OrderBy>();
            }
        }

        private OrderBy selectedOrderMethod;

        public OrderBy SelectedOrderMethod
        {
            get { return selectedOrderMethod; }
            set
            {
                selectedOrderMethod = value;
                NotifyOfPropertyChange(()=> SelectedOrderMethod);
                processedValuesHolder.ChangeSeriesCollectionOrder(value);
            }
        }


        public string ActionButtonText
        {
            get { return actionButtonText;}
            set
            {
                actionButtonText = value;
                NotifyOfPropertyChange(() => ActionButtonText);
            }
        }

        public int NeighbouringNumberRange
        {
            get { return neighbouringNumberRange; }
            set
            {
                neighbouringNumberRange = value;
                NotifyOfPropertyChange(() =>NeighbouringNumberRange);
            }
        }

        private bool isNotProcessing;

        public bool IsNotProcessing
        {
            get { return isNotProcessing; }
            set
            {
                isNotProcessing = value;
                NotifyOfPropertyChange(()=> isNotProcessing);
            }
        }

        public BaseViewModel GraphPartViewModel
        {
            get { return graphPartViewModel; }
            set { graphPartViewModel = value; }
        }

        public BaseViewModel InputPartViewModel
        {
            get { return inputPartViewModel; }
            set { inputPartViewModel = value; }
        }
        
        

        public void ActButtonClick()
        {
            switch (currentState)
            {
                case State.Edit:
                    ChangeState(State.Proccesing);
                    break;
                case State.Proccesing:
                    ChangeState(State.Stopped);
                    break;
                case State.InvalidInput:
                    ChangeState(State.Proccesing);
                    break;
                case State.Proccesed:
                case State.Stopped:
                    ChangeState(State.Edit);
                    break;
            }

        }

        private async Task ChangeState(State changeTo)
        {
            switch (changeTo)
            {
                case State.Proccesed:
                {
                    currentState = State.Proccesed;
                    ActionButtonText = "Edit";
                }
                    break;
                case State.Proccesing:
                {
                    IsNotProcessing = false;
                    infoViewModel.InterruptionVisibility = Visibility.Collapsed;
                    currentState = State.Proccesing;
                    ActionButtonText = "Stop";
                    try
                    {
                        GraphPartViewModel = graphViewModel;
                        NotifyOfPropertyChange(() => GraphPartViewModel);
                        InputPartViewModel = infoViewModel;
                        NotifyOfPropertyChange(() => InputPartViewModel);
                        await processedValuesHolder.StartProcessing(activeInputViewModel.InputValues, neighbouringNumberRange, selectedOrderMethod);
                        ChangeState(State.Proccesed);
                    }
                    catch (FormatException e)
                    {
                        ChangeState(State.InvalidInput);
                    }
                    catch (OperationCanceledException)
                    {
                        ChangeState(State.Stopped);
                    }

                }
                    break;
                case State.Stopped:
                {
                    if (await processedValuesHolder.StopProcessing())
                    {
                        infoViewModel.InterruptionVisibility = Visibility.Visible;
                        currentState = State.Stopped;
                        ActionButtonText = "Edit";
                        GraphPartViewModel = startUpViewModel;
                        NotifyOfPropertyChange(()=>GraphPartViewModel);
                        InputPartViewModel = infoViewModel;
                        NotifyOfPropertyChange(() => InputPartViewModel);
                    }
                }
                    break;
                case State.Edit:
                {
                    isNotProcessing = true;
                        currentState = State.Edit;
                        ActionButtonText = "Najdi násobek";
                        GraphPartViewModel = startUpViewModel;
                        NotifyOfPropertyChange(()=> GraphPartViewModel);
                        InputPartViewModel = activeInputViewModel;
                        NotifyOfPropertyChange(()=> InputPartViewModel);
                    }
                    break;
                case State.InvalidInput:
                {
                    currentState = State.InvalidInput;
                    ActionButtonText = "Najdi násobek";
                    GraphPartViewModel = messageViewModel;
                    NotifyOfPropertyChange(() => GraphPartViewModel);
                    InputPartViewModel = activeInputViewModel;
                    NotifyOfPropertyChange(() => InputPartViewModel);
                    }
                    break;
            }




            

        }

        
    }
}
