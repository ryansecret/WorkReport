#region

using System.Collections.Generic;
using Microsoft.Practices.Prism.ViewModel;

#endregion

namespace WorkReport.Model
{

    public class WorkReportBuildingEntity : NotificationObject
    {
        private decimal? _result;


        public virtual decimal? WorkId { get; set; }

      
        public virtual decimal? SGroupId { get; set; }


        public virtual decimal? Result
        {
            get { return _result; }
            set
            {
                if (value == _result) return;
                _result = value;
                RaisePropertyChanged("Result");
            }
        }

         
        public virtual string BuildCode { get; set; }

       
        public virtual string BuildingName { get; set; }

        
        public virtual decimal? BuildingType { get; set; }

     
        public virtual decimal? BuildingUse { get; set; }

      
        public List<WorkReportFloorEntity> WorkReportFloors { get; set; }
    }
    public class WorkReportFloorEntity : NotificationObject
    {
        private decimal? _result;


        public virtual decimal? WorkId { get; set; }

       
        public virtual decimal? SGroupId { get; set; }


        public virtual decimal? Result
        {
            get { return _result; }
            set
            {
                if (value == _result) return;
                _result = value;
                RaisePropertyChanged("Result");
            }
        }
         

        public virtual string FloorsType { get; set; }

     
        public virtual string FloorsCode { get; set; }

        
        public virtual string FloorsName { get; set; }

        public List<WorkReprotLog> WorkReportLogs { get; set; }

      
    }
    public class WorkReprotLog:NotificationObject
    {
        private bool? _isSelected;

        public bool? IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value.Equals(_isSelected)) return;
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }
         
        public virtual decimal? WorkId { get; set; }

        public virtual decimal? SGroupId { get; set; }


        public virtual decimal? LogId { get; set; }


        public virtual string LogName { get; set; }


        public virtual string LogServertype { get; set; }

        public virtual decimal? Result { get; set; }

        public List<LogKpis> Kpis { get; set; }
    }

    public class LogKpis
    {
        public virtual string KpiName { get; set; }


        public virtual decimal? KpiValue { get; set; }


        public virtual string KpiRange { get; set; }
    }
}