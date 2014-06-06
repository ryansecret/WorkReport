using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Expression.Interactivity.Core;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using WorkReportService;
using WorkReportService.WorkReport;

namespace WorkReport
{
    public class OrderListVm:NotificationObject
    {
        private bool _loading;
        private string _searchText;
        private DelegateCommand _searchCommand;
        private List<WorkDetailInfoEntity> _workOrders;
        private WorkDetailInfoEntity _selectedWorkOrder;
        private DelegateCommand _qualityShowCommand;

        public bool Loading
        {
            get { return _loading; }
            set { _loading = value; RaisePropertyChanged("Loading"); }
        }


        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; RaisePropertyChanged("SearchText"); }
        }

        public DelegateCommand QualityShowCommand
        {
            get { return _qualityShowCommand??(_qualityShowCommand=new DelegateCommand(QualityShow)); }
           
        }

        public WorkDetailInfoEntity ReportRelatedOrder
        {
            get { return _reportRelatedOrder; }
            set
            {
                if (Equals(value, _reportRelatedOrder)) return;
                _reportRelatedOrder = value;
                RaisePropertyChanged("ReportRelatedOrder");
            }
        }
         

        private void QualityShow()
        {
            ReportRelatedOrder = SelectedWorkOrder;
        }
        public ICommand PageChangeCommand { get; set; }
        public DelegateCommand SearchCommand
        {
            get { return _searchCommand??(_searchCommand=new DelegateCommand(Search)); }
        }

        private const int PageSize = 20;
        private int _currentPageIndex = 1;
        private int _reCordCount;
        private WorkDetailInfoEntity _reportRelatedOrder;

        private void Search()
        {

            Loading = true;
             
            QualityReportClient client = new QualityReportClient();
            client.GetAllWorkOrderAsync(new OrderClause() { WorkId = SearchText, PageSize = PageSize, PageIndex = _currentPageIndex });
            client.GetAllWorkOrderCompleted += (s, e) =>
            {
                Loading = false;
                if (e.Error==null)
                {
                    WorkOrders = e.Result.ToList();
                    if (WorkOrders.Any())
                    {
                        ReCordCount = WorkOrders.First().WorkSubItemGroup.RecordCount;
                    }
                    else
                    {
                        ReCordCount = 0;
                    }
                }
                else
                {
                    ReCordCount = 0;
                }
            };
        }

        public int ReCordCount
        {
            get { return _reCordCount; }
            set
            {
                if (value == _reCordCount) return;
                _reCordCount = value;
                RaisePropertyChanged("ReCordCount");
            }
        }
         
        public List<WorkDetailInfoEntity> WorkOrders
        {
            get { return _workOrders; }
            set { _workOrders = value; 
                RaisePropertyChanged("WorkOrders"); }
        }

        public WorkDetailInfoEntity SelectedWorkOrder
        {
            get { return _selectedWorkOrder; }
            set
            {
                if (Equals(value, _selectedWorkOrder)) return;
                _selectedWorkOrder = value;
                RaisePropertyChanged("SelectedWorkOrder");
            }
        }
         
        public OrderListVm()
        {
            Loading = false;
            DicCache.Instance.GetDic(null, "BULIDING_LEVEL");
            Search();
            PageChangeCommand = new ActionCommand(PageChange);
        }

        private void PageChange(object obj)
        {
            int pageIndex = Convert.ToInt32(obj);
            if (pageIndex > 0)
            {
                _currentPageIndex = pageIndex;
                Search();
            }
        }
    }
}
