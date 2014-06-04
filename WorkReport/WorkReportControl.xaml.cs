#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Practices.Prism.Commands;
using WorkReport.Annotations;
using WorkReport.Model;

#endregion

namespace WorkReport
{
    public partial class WorkReportControl:INotifyPropertyChanged
    {
        private ObservableCollection<SysDictionaryEntity> _buildingLevels;
        private bool _canExport;
        private DelegateCommand _exportCommand;
        private Visibility _loadingVisibility;
        private ObservableCollection<SysDictionaryEntity> _sysDictionarys;
        private DelegateCommand _computeResultCommand;

        public WorkReportControl()
        {
            InitializeComponent();
            Header = "室内测试简报";
            LoadingVisibility = Visibility.Collapsed;
            WorkReportBuildingEntity building = new WorkReportBuildingEntity();
            building.BuildingName = "oldstar";
            building.BuildingType = 1;
            building.BuildingUse = 3;
            building.Result = 1;
            WorkReportFloorEntity floor = new WorkReportFloorEntity();
            floor.FloorsName = "new star";
            floor.FloorsType = "new begining";
            floor.WorkReportLogs = new List<WorkReprotLog>();
            floor.Result = 1;
            WorkReprotLog log = new WorkReprotLog();
            log.LogName = "sdfsdf";
            log.LogServertype = "数据";
            log.Result = 2;
            WorkReprotLog log1 = new WorkReprotLog();
            log1.LogName = "sdfsdf1";
            log1.LogServertype = "数据";
            log1.Result = 1;
            
            LogKpis logKpis = new LogKpis() {KpiName = "kpi test", KpiRange = ">5", KpiValue = 12.2m};
            log.Kpis = new List<LogKpis>();
            log.Kpis.AddRange(Enumerable.Repeat(logKpis,10));
           // floor.WorkReportLogs.AddRange(Enumerable.Repeat(log,15));
            floor.WorkReportLogs.AddRange(new []{log,log1});
            building.WorkReportFloors=new List<WorkReportFloorEntity>();
            building.WorkReportFloors.AddRange(Enumerable.Repeat(floor,5));


            Initial(new List<WorkReportBuildingEntity>(){building});
        }

        

        public DelegateCommand ComputeResultCommand
        {
            get { return _computeResultCommand??(_computeResultCommand=new DelegateCommand(ComputeResult)); }
           
        }

        private void ComputeResult()
        {
            if (_logResult==null)
            {
                 return;
            }
            var building = _logResult.First();
            if (building!=null)
            {
                building.WorkReportFloors.First().Result = 3;
                foreach (var floor in building.WorkReportFloors)
                {
                    var selectedLogs = floor.WorkReportLogs.Where(d => d.IsSelected == true);
                    if (selectedLogs!=null&&selectedLogs.Count()>0)
                    {
                        floor.Result = selectedLogs.Max(d => d.Result);
                    }
                }
                var  buildingResult= building.WorkReportFloors.Max(d => d.Result);
                if (buildingResult>(int)Quality.中)
                {
                    building.Result = buildingResult;
                }
                else
                {
                    int goodCount = building.WorkReportFloors.Where(d => d.Result == (int) Quality.优秀).Count();
                    int fineCount = building.WorkReportFloors.Where(d => d.Result == (int) Quality.良).Count();
                    int totalCount = building.WorkReportFloors.Count;
                    if ((double)goodCount/totalCount>=0.8)
                    {
                        building.Result = (int) Quality.优秀;
                    }
                    else if ((double)fineCount / totalCount >= 0.8)
                    {
                        building.Result = (int)Quality.良;
                    }
                    else
                    {
                        building.Result = (int)Quality.中;
                    }
                }
            }
        }


        public DelegateCommand ExportCommand
        {
            get
            {
                return _exportCommand ?? (_exportCommand = new DelegateCommand(Export));
            }
        }

        private void Export()
        {
            
        }

        public Visibility LoadingVisibility
        {
            get
            {
                return _loadingVisibility;
            }
            set
            {
                _loadingVisibility = value;
                OnPropertyChanged("LoadingVisibility");
            }
        }

        public bool CanExport
        {
            get
            {
                return _canExport;
            }
            set
            {
                _canExport = value;
                OnPropertyChanged("CanExport");
            }
        }

        public string Header
        {
            get;
            set;
        }

        private List<WorkReportBuildingEntity> _logResult;

        private void SelectLog(WorkReportBuildingEntity workReport)
        {
            foreach (var floor in workReport.WorkReportFloors)
            {
                var bestResult = floor.WorkReportLogs.Min(d => d.Result);
                floor.WorkReportLogs.First(d => d.Result == bestResult).IsSelected = true;
            }
        }

        void Initial(List<WorkReportBuildingEntity> result)
        {
          
            _logResult = result;
            var nodeRoot = new TreeViewItem
            {
                Header = Header,
                IsExpanded = true
            };
            foreach (var workReportBuildingEntity in result)
            {
                SelectLog(workReportBuildingEntity);
                
                var stackPanelBuilding = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    FlowDirection = FlowDirection.LeftToRight
                };
                stackPanelBuilding.Children.Add(new TextBlock
                {
                    Text = workReportBuildingEntity.BuildingName,
                    Margin = new Thickness(10, 0, 10, 0),VerticalAlignment = VerticalAlignment.Center
                });
                //SysDictionaryEntity dt =
                //    _sysDictionarys.FirstOrDefault(x => x.CodeId == workReportBuildingEntity.BuildingUse);

                stackPanelBuilding.Children.Add(new TextBlock
                {
                   // Text = dt != null ? dt.Cnname : ""
                    Text = "BuildingUse"
                    ,
                    Margin = new Thickness(10, 0, 10, 0),
                    VerticalAlignment = VerticalAlignment.Center
                });
                stackPanelBuilding.Children.Add(new TextBlock
                {
                    Text = "BuildingType",
                        //_buildingLevels.FirstOrDefault(
                        //    d => d.CodeId == workReportBuildingEntity.BuildingType).Cnname,
                    Margin = new Thickness(10, 0, 10, 0),VerticalAlignment = VerticalAlignment.Center
                });

                var qc = new BuildingQuality();
                qc.SetBinding(BuildingQuality.QuilityProperty,
                    new Binding()
                    {
                        Source = workReportBuildingEntity,
                        Path = new PropertyPath("Result"),
                        Converter = new QualityConverter()
                    });
                qc.VerticalAlignment=VerticalAlignment.Center;
                
                stackPanelBuilding.Children.Add(qc);

                var nodeBuilding = new TreeViewItem
                {
                    Header = stackPanelBuilding,
                    IsExpanded = true
                };
                nodeRoot.Items.Add(nodeBuilding);
                GetFloorReport(workReportBuildingEntity, nodeBuilding);

            }
            tvKpiQuality.Items.Add(nodeRoot);

        }


        private void GetFloorReport(WorkReportBuildingEntity building,TreeViewItem buildingNode)
        {
            var length = new GridLength(90);

            var border = CreateBorder();
            var grid = new Grid
            {
                Background = GetColor(5),
                ShowGridLines = false,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            var kpiCount = 0;
            var logs = building.WorkReportFloors.First().WorkReportLogs.FirstOrDefault();
            if (logs!=null)
            {
                kpiCount = logs.Kpis.Count;
            }
            var floorCount = building.WorkReportFloors.Count();
 
            var count = 0;
            for (int j = 0; j < floorCount; j++)
            {
                count++;

                #region floorkpi

                StackPanel floorstStackPanel = new StackPanel(){Orientation = Orientation.Horizontal};
                var floorReport = building.WorkReportFloors.First();
                var text = new TextBlock
                {
                    Text = floorReport.FloorsName,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                };

                text.SetValue(ToolTipService.ToolTipProperty, floorReport.FloorsName);

                var borderlabelName = new Border
                {
                    Child = text,Margin = new Thickness(10, 0, 10, 0)
                };

               floorstStackPanel.Children.Add(borderlabelName);
              

                var borderType = new Border
                {
                    Child = new TextBlock
                    {
                        Text = floorReport.FloorsType,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center
                    },
                    Margin = new Thickness(10, 0, 10, 0)
                };

                floorstStackPanel.Children.Add(borderType);
                var txt = new TextBlock
                {
                    
                    MaxWidth = 80,
                    TextWrapping = TextWrapping.Wrap,
                    
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                txt.SetBinding(TextBlock.TextProperty, new Binding()
                {
                    Source = floorReport,
                    Path = new PropertyPath("Result"),
                    Converter = new QualityConverter()
                });
                txt.SetBinding(TextBlock.ForegroundProperty, new Binding()
                {
                    Source = floorReport,
                    Path = new PropertyPath("Result"),
                    Converter = new ColorConverter()
                });
 
                var borderQuality = new Border
                {
                    Child = txt
                   ,Margin = new Thickness(10, 0, 10, 0)
                };
                floorstStackPanel.Children.Add(borderQuality);
                TreeViewItem nodeFloor = new TreeViewItem() {Header = floorstStackPanel, IsExpanded = true};
                buildingNode.Items.Add(nodeFloor);
                #endregion
 
                count++;
                nodeFloor.Items.Add(GetLogControl(floorReport.WorkReportLogs.ToList(), count, kpiCount, floorCount, length));
            }
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            border.Child = grid;
             
        }

        private UIElement GetLogControl(List<WorkReprotLog> logs, int rowIndex, int columnCount, int logCount,
            GridLength length)
        {
            int margin = 2;
            string groupName = Guid.NewGuid().ToString();
            var userLogControl = new LogGrid
            {
                Header = "LOG质量"
            };

       
            var gridLog = new Grid
            {
                ShowGridLines = false,
                
                Background = GetColor(5)
            };

            var logGroup = logs;
            if (logGroup.Count > 0)
            {
                for (int i1 = 0; i1 < columnCount + margin; i1++)
                {
                    if (i1==0)
                    {
                       gridLog.ColumnDefinitions.Add(new ColumnDefinition
                       {
                        Width =new GridLength(115),
                       });
                    }
                    else
                    {
                         gridLog.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width =new GridLength(100),
                    });
                    }
                   
                    Rectangle rec = CreateVertLine(i1, logGroup.Count + 1);
                
                    gridLog.Children.Add(rec);
                }

                for (int i1 = 0; i1 < logGroup.Count + 1; i1++)
                {
                    gridLog.RowDefinitions.Add(new RowDefinition
                    {
                        Height = GridLength.Auto,
                        MinHeight = 40
                    });
                    Rectangle rectSub = CreateHorLine(i1, columnCount + margin);
                    if (i1 == logGroup.Count() && rowIndex != logCount  * 2)
                    {
                        rectSub.Height = 2;
                        rectSub.Fill = new SolidColorBrush(Colors.Black);
                        rectSub.Margin = new Thickness(0, 1, 0, -1);
                    }
                    gridLog.Children.Add(rectSub);
                }

                Rectangle rectlogo = CreateRectangler(2);
                rectlogo.Margin = new Thickness(1);
                Grid.SetColumn(rectlogo, 0);
                Grid.SetColumnSpan(rectlogo,2);
                Grid.SetRow(rectlogo, 0);
                gridLog.Children.Add(rectlogo);

                var lableLogName = new Border
                {
                    Child = new TextBlock
                    {
                        Text = "LOG名称",
                        FontSize = 12,
                        FontWeight = FontWeights.Bold,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(20, 0, 0, 0)
                    },
                    Margin = new Thickness(1, 0, 1, 1)
                };
                lableLogName.SetValue(Canvas.ZIndexProperty, 10);
                Grid.SetColumn(lableLogName, 0);
               
                Grid.SetRow(lableLogName, 0);
                lableLogName.HorizontalAlignment = HorizontalAlignment.Stretch;
                lableLogName.VerticalAlignment = VerticalAlignment.Stretch;
                gridLog.Children.Add(lableLogName);
 
                var lableLogQuality = new Border
                {
                    Child = new TextBlock
                    {
                        Text = "Log质量",
                        FontSize = 12,
                        FontWeight = FontWeights.Bold,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(20, 0, 0, 0)
                    },
                    Margin = new Thickness(1, 0, 1, 1)
                };
                lableLogName.SetValue(Canvas.ZIndexProperty, 10);
                Grid.SetColumn(lableLogQuality, 1);

                Grid.SetRow(lableLogQuality, 0);
                lableLogName.HorizontalAlignment = HorizontalAlignment.Stretch;
                lableLogName.VerticalAlignment = VerticalAlignment.Stretch;
                gridLog.Children.Add(lableLogQuality);


                for (int m = 0; m < columnCount; m++)
                {
                    var kpi = logs[0].Kpis[m];
                    var lableLog = new TextBlock
                    {
                        Text = kpi.KpiName + (kpi.KpiName.Contains("FTP") ? "" : "(%)"),
                        FontSize = 12,
                        FontWeight = FontWeights.Bold,
                        MaxWidth = 80,
                        TextWrapping = TextWrapping.Wrap
                    };
                    lableLog.SetValue(ToolTipService.ToolTipProperty,
                        kpi.KpiName + (kpi.KpiName.Contains("FTP") ? "" : "(%)"));
                    Grid.SetColumn(lableLog, m + margin);
                    lableLog.VerticalAlignment = VerticalAlignment.Center;
                    lableLog.HorizontalAlignment = HorizontalAlignment.Center;
                    Grid.SetRow(lableLog, 0);
                    lableLog.SetValue(Canvas.ZIndexProperty, 10);
                    gridLog.Children.Add(lableLog);
                }
 
                var countLog = 0;
                for (int i = 0; i < logGroup.Count(); i++)
                {
                    countLog++;
                    StackPanel stackPanelLog = new StackPanel() {Orientation = Orientation.Horizontal};

                    var rb = new RadioButton() { Tag = logGroup[i], VerticalAlignment = VerticalAlignment.Center, GroupName = groupName+logGroup[i].LogServertype };
                    rb.SetBinding(RadioButton.IsCheckedProperty,
                        new Binding() { Source = rb.Tag, Path = new PropertyPath("IsSelected"), Mode = BindingMode.TwoWay });
                     
          
                    stackPanelLog.Children.Add(rb);
                    var logtxt = new TextBlock
                    {
                        FontSize = 12,
                        FontWeight = FontWeights.Bold,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(20, 0, 0, 0)
                    };
                    var run = new Run
                    {
                        Text = logGroup[i].LogName,
                    };
                    logtxt.Inlines.Add(run);
                    var br = new LineBreak();
                    logtxt.Inlines.Add(br);

                    var dataType = new Underline();

                    dataType.Inlines.Add(new Run
                    {
                        Text = logGroup[i].LogServertype + "(打点图)"
                    });
                    logtxt.Inlines.Add(dataType);
                    stackPanelLog.Children.Add(logtxt);
                    var lableNameLog1 = new Border
                    {
                        Child = stackPanelLog,
                        Margin = new Thickness(1, 0, 1, 1),
                        Cursor = Cursors.Hand
                    };

                    lableNameLog1.MouseLeftButtonDown +=
                        (s, e) =>
                        {
                        };
                    lableNameLog1.SetValue(Canvas.ZIndexProperty, 10);
                    Grid.SetColumn(lableNameLog1, 0);
                
                    Grid.SetRow(lableNameLog1, countLog);
                    lableNameLog1.VerticalAlignment = VerticalAlignment.Stretch;
                    lableNameLog1.HorizontalAlignment = HorizontalAlignment.Stretch;
                    gridLog.Children.Add(lableNameLog1);


                    Rectangle rectbg = CreateGridRowBackground(1, countLog);
                    gridLog.Children.Add(rectbg);

                    TextBlock logQuality = new TextBlock()
                    {
                        Foreground = GetColor(logGroup[i].Result),
                        FontFamily = new FontFamily("Microsoft YaHei"),
                        FontSize = 14,
                        FontWeight = FontWeights.Bold,

                    };
                    
                    logQuality.Text = GetResult(logGroup[i].Result);
                    Grid.SetColumn(logQuality, 1);
                    Grid.SetRow(logQuality, countLog);
                    logQuality.VerticalAlignment = VerticalAlignment.Center;
                    logQuality.HorizontalAlignment = HorizontalAlignment.Center;
                    gridLog.Children.Add(logQuality);


                    if (logs[i].Kpis==null)
                    {
                        continue;
                    }
                    for (int m = 0; m < columnCount; m++)
                    {
                       
                        var kpi = logs[i].Kpis[m];
                       

                        Rectangle newrect = CreateRectangler(1);
                        newrect.Margin = new Thickness(1, 1, 1, 1);
                        Grid.SetColumn(newrect, m + margin);
                        Grid.SetRow(newrect, 0);
                        gridLog.Children.Add(newrect);

                        var borderlog = new TextBlock
                        {
                            Text =
                                kpi.KpiName.Contains("FTP")
                                    ? kpi.KpiValue.ToString()
                                    : (kpi.KpiValue.Value * 100).ToString("00.0"),
                            Foreground =GetColor(logs[i].Result),
                            FontFamily = new FontFamily("Microsoft YaHei"),
                            FontSize = 12,
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center
                        };

                        borderlog.SetValue(ToolTipService.ToolTipProperty,
                            kpi.KpiRange);
                        borderlog.SetValue(Canvas.ZIndexProperty, 100);
                        Grid.SetColumn(borderlog, m + margin);
                        Grid.SetRow(borderlog, countLog);
                        gridLog.Children.Add(borderlog);
                    }
                }
            }
            userLogControl.Content = new Border() { Child = gridLog,BorderBrush = new SolidColorBrush(Colors.Black),BorderThickness = new Thickness(1)}; ;

            return userLogControl;
        }

        private string GetResult(decimal? result)
        {
            return result.HasValue ? ((Quality)result).ToString() : string.Empty;
        }

        private Border CreateBorder()
        {
            var border = new Border();
            border.BorderThickness = new Thickness(1, 1, 1, 1);
            //  border.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x99, 0x99, 0x99));
            border.BorderBrush = new SolidColorBrush(Colors.Green);
            border.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xee, 0xee, 0xee));
            border.HorizontalAlignment = HorizontalAlignment.Left;
            border.VerticalAlignment = VerticalAlignment.Center;
            return border;
        }

        private Rectangle CreateRectangler(int column)
        {
            var rect = new Rectangle();
            if (column > 0)
            {
                rect.SetValue(Grid.ColumnSpanProperty, column);
            }
            rect.SetValue(Canvas.ZIndexProperty, -1);

            rect.SetValue(Grid.RowProperty, 0);

            var colls = new GradientStopCollection();
            var stop1 = new GradientStop();
            stop1.Color = Color.FromArgb(0xff, 0xe9, 0xed, 0xf0);
            stop1.Offset = 0;

            var stop2 = new GradientStop();
            stop2.Color = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
            stop2.Offset = 1;
            colls.Add(stop1);
            colls.Add(stop2);
            var linebrush = new LinearGradientBrush(colls, 0);
            linebrush.StartPoint = new Point(0, 0);
            linebrush.EndPoint = new Point(0, 1);
            rect.Fill = linebrush;
            return rect;
        }


        private Rectangle CreateGridRowBackground(int column, int index)
        {
            var rect = new Rectangle();
            rect.Margin = new Thickness(1);
            rect.SetValue(Canvas.ZIndexProperty, 0);
            if (column > 0)
                rect.SetValue(Grid.ColumnSpanProperty, column);
            rect.SetValue(Grid.RowProperty, index);

            var linebrush = new SolidColorBrush();
            if (index % 2 == 0)
                linebrush.Color = Colors.White;
            else
                linebrush.Color = Color.FromArgb(0xff, 0xee, 0xee, 0xee);
            rect.Fill = linebrush;
            return rect;
        }

        private Rectangle CreateVertLine(int column, int colCount)
        {
            var rect = new Rectangle();
            rect.SetValue(Canvas.ZIndexProperty, 0);
            rect.SetValue(Grid.RowSpanProperty, colCount);
            rect.SetValue(Grid.ColumnProperty, column);
            rect.Width = 1;
            rect.HorizontalAlignment = HorizontalAlignment.Right;
            rect.Fill = new SolidColorBrush(Color.FromArgb(0xff, 0x99, 0x99, 0x99));
            return rect;
        }

        private Rectangle CreateHorLine(int column, int colCount, double height = 1)
        {
            var rect = new Rectangle();
            rect.SetValue(Canvas.ZIndexProperty, 0);
            rect.SetValue(Grid.ColumnSpanProperty, colCount);
            rect.SetValue(Grid.RowProperty, column);
            rect.Height = height;
            rect.VerticalAlignment = VerticalAlignment.Bottom;
            rect.Fill = new SolidColorBrush(Color.FromArgb(0xff, 0x99, 0x99, 0x99));
            //rect.Fill = new SolidColorBrush(Colors.Red);
            return rect;
        }


        private SolidColorBrush GetColor(decimal? quality)
        {
            var value = quality.HasValue ? (int)quality.Value : 5;
            return GetColor(value);
        }

        private SolidColorBrush GetColor(int quality)
        {
            #region  优良中差的颜色区分

            switch (quality)
            {
                case 1:
                    return new SolidColorBrush
                    {
                        Color = Color.FromArgb(255, 0, 90, 0)
                    };
                    break;
                case 2:
                    return new SolidColorBrush
                    {
                        Color = Colors.Blue
                    };
                    break;
                case 3:
                    return new SolidColorBrush
                    {
                        Color = Color.FromArgb(255, 255, 155, 0)
                    };
                    break;
                case 4:
                    return new SolidColorBrush
                    {
                        Color = Color.FromArgb(255, 255, 0, 0)
                    };
                    break;
                case 5:
                    return new SolidColorBrush
                    { 
                        Color = Color.FromArgb(0, 0, 0, 0)
                    };
                    break;
                default:
                    return new SolidColorBrush
                    {
                        Color =Colors.Black
                    };
                    break;
            }

            #endregion
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class QualityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value!=null)
            {
                return ((Quality)int.Parse(value.ToString())).ToString();
            }
            return  string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class ColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value!=null)
            {
                switch (value.ToString())
                {
                    case "1":
                        return new SolidColorBrush
                        {
                            Color = Color.FromArgb(255, 0, 90, 0)
                        };
                        break;
                    case "2":
                        return new SolidColorBrush
                        {
                            Color = Colors.Blue
                        };
                        break;
                    case "3":
                        return new SolidColorBrush
                        {
                            Color = Color.FromArgb(255, 255, 155, 0)
                        };
                        break;
                    case "4":
                        return new SolidColorBrush
                        {
                            Color = Color.FromArgb(255, 255, 0, 0)
                        };
                        break;
                    case "5":
                        return new SolidColorBrush
                        {
                            Color = Color.FromArgb(0, 0, 0, 0)
                        };
                        break;
                    default:
                        return new SolidColorBrush
                        {
                            Color = Colors.Black
                        };
                        break;
                }
            }
            return new SolidColorBrush() {Color = Colors.Black};
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class SysDictionaryEntity
    {
    }

    public enum Quality
    {
        优秀 = 1,
        良 = 2,
        中 = 3,
        差 = 4
    }
}