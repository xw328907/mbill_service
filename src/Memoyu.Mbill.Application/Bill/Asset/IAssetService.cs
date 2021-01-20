﻿/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Asset
*   文件名称 ：AssetService.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 21:05:24
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Domain.Entities.Bill.Asset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Bill.Asset
{
    public interface IAssetService
    {
        /// <summary>
        /// 新增资产分类
        /// </summary>
        /// <param name="input">数据源</param>
        /// <returns></returns>
        Task InsertAsync(AssetEntity asset);
    }
}
