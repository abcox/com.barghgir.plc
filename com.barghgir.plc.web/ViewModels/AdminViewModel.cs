using com.barghgir.plc.data.Models;
using com.barghgir.plc.web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.barghgir.plc.web.ViewModels;

public class AdminViewModel
{
    public RangeEnabledObservableCollection<Member> MemberList { get; } = new();


}