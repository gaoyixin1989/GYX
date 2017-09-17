using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GYX.Core;
using GYX.Core.Caching;
using GYX.Data;
using GYX.Data.Domain.Assets;
using System.Linq.Expressions;
using GYX.Service.IServiceManger.Assets;

namespace GYX.Service.ServiceManger.Assets
{
    public partial class CreditCardInfoService : BaseService<CreditCardInfo>, ICreditCardInfoService
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

            return new PagedList<CreditCardInfo>(curTable.OrderBy(c=>c.UserObj.RealName).ThenBy(c => c.RepaymentDay), pageIndex, pageSize, out count);
        }

    }
}