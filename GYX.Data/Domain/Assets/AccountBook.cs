using GYX.Core;
using System;
using System.Collections.Generic;
/// <summary>
/// �˱���
/// </summary>
namespace GYX.Data.Domain.Assets
{
    public partial class AccountBook
    {
        /// <summary>
        /// ���
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// �����˱����˱����ƴ��ţ�
        /// </summary>
        public string BookName { get; set; }
        /// <summary>
        /// �˵���֧���ͣ�֧��������
        /// </summary>
        public string BillType { get; set; }
        /// <summary>
        /// ֧��ʱ��
        /// </summary>
        public DateTime? PayTime { get; set; }
        /// <summary>
        /// ֧������
        /// </summary>
        public string PayType { get; set; }
        /// <summary>
        /// ���
        /// </summary>
        public decimal? Money { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public string CurrencyType { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime? CreateTime { get; set; }
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
