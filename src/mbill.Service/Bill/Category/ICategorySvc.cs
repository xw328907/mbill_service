﻿namespace mbill.Service.Bill.Category;

public interface ICategorySvc
{
    /// <summary>
    /// 新增账单分类
    /// </summary>
    /// <param name="dto">数据源</param>
    /// <returns></returns>
    Task<ServiceResult<CategoryDto>> InsertAsync(ModifyCategoryDto dto);

    /// <summary>
    /// 删除账单分类信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(long id);

    /// <summary>
    /// 更新账单分类
    /// </summary>
    /// <param name="categroy">账单分类信息</param>
    /// <returns></returns>
    Task UpdateAsync(CategoryEntity categroy);

    /// <summary>
    /// 获取分级后的组合类别数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    Task<ServiceResult<IEnumerable<CategoryGroupDto>>> GetGroupsAsync(int? type);

    /// <summary>
    /// 获取账单分类分页
    /// </summary>
    /// <param name="pagingDto">分页参数</param>
    /// <returns></returns>
    Task<PagedDto<CategoryPageDto>> GetPageAsync(CategoryPagingDto pagingDto);


    Task<IEnumerable<CategoryDto>> GetListAsync();

    /// <summary>
    /// 获取分类
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<CategoryDto> GetAsync(long id);

    /// <summary>
    /// 获取群不分类
    /// </summary>
    /// <param name="type">分类类型 0 支出， 1 收入</param>
    /// <returns></returns>
    Task<ServiceResult<List<CategoryDto>>> GetsAsync(int type);

    /// <summary>
    /// 获取父项 by 子项 id
    /// </summary>
    /// <param name="id">分类子项Id</param>
    /// <returns></returns>
    Task<CategoryDto> GetParentAsync(long id);

    /// <summary>
    /// 获取账单分类父项集合
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<CategoryDto>> GetParentsAsync();

}
