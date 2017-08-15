using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GYX.Core;
using GYX.Core.Caching;
using GYX.Data;
using GYX.Data.Domain.System;
using System.Linq.Expressions;
using GYX.Service.IServiceManger.System;

namespace GYX.Service.ServiceManger.System
{
    public partial class SysUserService : BaseService<SysUser>, ISysUserService
    {
        public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            //var curTable = GetQueryTable(objs);
            var curTable = this._entityStore.Table;
            if (objs != null)
            {
                var exp = ExpressionFactory(objs);
                curTable = curTable.Where(exp);
            }

            return new PagedList<SysUser>(curTable.OrderBy(c => c.UserName), pageIndex, pageSize, out count);
        }
    }
}