using GYX.Core;
using System;
using System.Collections.Generic;
/// <summary>
/// �ֵ��
/// </summary>
namespace GYX.Data.Domain.System
{
    public partial class SysDict
    {
        /// <summary>
        /// ���
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// �ֵ����
        /// </summary>
        public string DictCode { get; set; }
        /// <summary>
        /// �ֵ�����
        /// </summary>
        public string DictText { get; set; }
        /// <summary>
        /// ����Id
        /// </summary>
        public int? ParentId { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public int? OrderId { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// �Ƿ�Ĭ��ֵ����ֵ�����ֵ�
        /// </summary>
        public bool? IsDefalut { get; set; }
        /// <summary>
        /// �Ƿ����ã���ֵ�����ֵ�
        /// </summary>
        public bool? IsUse { get; set; }
        /// <summary>
        /// ����״̬��0������1ɾ��
        /// </summary>
        public int? DataState { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        public string Remark { get; set; }
    }
}
