﻿//using AgLibrary.ViewModels;
using AgOpenGPS.Core.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
﻿using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AgOpenGPS.Core.ViewModels
{
    public partial class FieldTableViewModel : ViewModelBase
    {
        protected readonly ApplicationModel _appModel;
       //private ObservableCollection<FieldDescriptionViewModel> _fieldDescriptions;
       public ObservableCollection<FieldDescriptionViewModel> FieldDescriptionViewModels {get; set;}
       // protected FieldDescriptionViewModel _localSelectedField;

        public FieldTableViewModel(ApplicationModel appModel)
        {
            _appModel = appModel;
            SelectFieldCommand = new RelayCommand(SelectField);
            SortCommand = new RelayCommand(Sort);
        }

        public ICommand SelectFieldCommand { get; }
        public ICommand SortCommand { get; }
        
        
        //public ObservableCollection<FieldDescriptionViewModel> FieldDescriptionViewModels;
        /***
        {
            get { return _fieldDescriptions; }
            set
            {
                _fieldDescriptions = value;
                NotifyPropertyChanged();
            }
        }
         ****/
        // The field that is selected in the table (if any). 
        // Probably different from the field that is currently selected in the application
       [ObservableProperty]
        public FieldDescriptionViewModel _localSelectedField;
        /***
        {
            get { return _localSelectedField; }
            set
            {
                if (value != _localSelectedField)
                {
                    _localSelectedField = value;
                    NotifyPropertyChanged();
                }
            }
        }
        ****/
        
        public void UpdateFields()
        {
            //ObservableCollection<FieldDescriptionViewModel> viewModels = new ObservableCollection<FieldDescriptionViewModel>();
            FieldDescriptionViewModels = new ObservableCollection<FieldDescriptionViewModel>();
            var descriptions = _appModel.Fields.GetFieldDescriptions();
            foreach (FieldDescription description in descriptions)
            {
                FieldDescriptionViewModel viewModel = new FieldDescriptionViewModel(
                    description.FieldDirectory,
                    description.Area.HasValue ? description.Area.Value.ToString() : "Error",
                    description.Wgs84Start.HasValue ? _appModel.CurrentLatLon.DistanceInKiloMeters(description.Wgs84Start.Value).ToString() : "Error");
                FieldDescriptionViewModels.Add(viewModel);
            }
            // The Winforms views do not update when elements inside the ObservableCollection are changed.
            // Therefore change the ObservableCollection as a whole.
           // FieldDescriptionViewModels = viewModels;
        }

        protected virtual void SelectField()
        {
            var selectedField = LocalSelectedField;
            if (null != selectedField)
            {
                LocalSelectedField = null;
                _appModel.Fields.SelectField(selectedField.DirectoryInfo);
            }
        }

        private void Sort()
        {
            // TODO implement sorting
        }
    }

}
