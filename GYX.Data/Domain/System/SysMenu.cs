using GYX.Core;
using System;
using System.Collections.Generic;
/// <summary>
/// �˵���
/// </summary>
namespace GYX.Data.Domain.System
{
    public partial class SysMenu
    {
        /// <summary>
        /// ID���
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// �˵���ʾ����
        /// </summary>
        public string MenuText { get; set; }
        /// <summary>
        /// �˵�URL
        /// </summary>
        public string MenuUrl { get; set; }
        /// <summary>
        /// �˵����ͣ�menu���˵���item���˵����ֵ�����ֵ�
        /// </summary>
        public string MenuType { get; set; }
        /// <summary>
        /// �����˵�ID
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public int? OrderId { get; set; }
        /// <summary>
        /// �˵�Сͼ��URL
        /// </summary>
        public string ImgUrl_Small { get; set; }
        /// <summary>
        /// �˵���ͼ��URL
        /// </summary>
        public string ImgUrl_Big { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public Nullable<DateTime> CreateTime { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// �Ƿ���ʾ����ֵ�����ֵ�
        /// </summary>
        public bool? IsShow { get; set; }
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
