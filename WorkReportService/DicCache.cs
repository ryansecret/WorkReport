using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using WorkReportService.WorkReport;

namespace WorkReportService
{
    public class DicCache
    {

        public static readonly DicCache Instance = new DicCache();
        private DicCache()
        {
            
        }
        private Dictionary<string, List<SysDictionary>> _localDb = new Dictionary<string, List<SysDictionary>>();

        public void GetDic(Action<List<SysDictionary>> callback, string groupName)
        {
            if (_localDb.ContainsKey(groupName))
            {
                callback(_localDb[groupName]);
            }
            else
            {
               QualityReportClient client = new QualityReportClient();
                client.QueryDictAsync(groupName);
                client.QueryDictCompleted += (s, e) =>
                {
                    if (e.Error==null)
                    {
                        callback(e.Result.ToList());
                    }
                };
            }
        }
    }
}
