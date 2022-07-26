﻿namespace mbill.Controllers.Bill;

/// <summary>
/// 资产分类管理
/// </summary>
[Authorize]
[Route("api/asset")]
public class AssetController : ApiControllerBase
{
    private readonly IMapper _mapper;
    private readonly IAssetSvc _assetService;

    public AssetController(IAssetSvc assetService, IMapper mapper)
    {
        _mapper = mapper;
        _assetService = assetService;
    }

    /// <summary>
    /// 新增资产分组/分类
    /// </summary>
    /// <param name="input">资产分类</param>
    [Logger("用户新建了一个资产分类")]
    [HttpPost]
    [LocalAuthorize("新增", "资产分类")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
    public async Task<ServiceResult> CreateAsync([FromBody] CreateAssetInput input)
    {      
        return await _assetService.InsertAsync(_mapper.Map<AssetEntity>(input));
    }

    /// <summary> 
    /// 删除资产分组/分类
    /// </summary>
    /// <param name="id">资产分类id</param>
    [HttpDelete]
    [LocalAuthorize("删除", "资产分类")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
    public async Task<ServiceResult> DeleteAsync([FromQuery] long id)
    {
        
        return await _assetService.DeleteAsync(id);
    }

    /// <summary>
    /// 更新资产分组/分类
    /// </summary>
    /// <param name="input">资产分类信息</param>
    [HttpPut]
    [LocalAuthorize("更新", "资产分类")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
    public async Task<ServiceResult> EditAsync([FromBody] EditAssetInput input)
    {
        
        return await _assetService.EditAsync(_mapper.Map<AssetEntity>(input));
    }

    /// <summary>
    /// 获取资产分组/分类
    /// </summary>
    /// <param name="id">资产id</param>
    [HttpGet]
    [LocalAuthorize("获取详情", "资产分类")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<AssetDto>> GetAsync([FromQuery] long id)
    {
        return await _assetService.GetAsync(id);
    }

    /// <summary>
    /// 获取资产父项
    /// </summary>
    /// <param name="id">资产id</param>
    [HttpGet("parent")]
    [LocalAuthorize("获取父项详情", "资产分类")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<AssetDto>> GetParentAsync([FromQuery] long id)
    {
        return await _assetService.GetParentAsync(id);
    }

    /// <summary>
    /// 获取资产父项集合
    /// </summary>
    [HttpGet("parents")]
    [LocalAuthorize("获取父项集合", "资产分类")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<IEnumerable<AssetDto>>> GetParentsAsync()
    {
        return await _assetService.GetParentsAsync();
    }

    /// <summary>
    /// 获取分组后的资产
    /// </summary>
    /// <param name="type">资产类型</param>
    [HttpGet("groups")]
    [LocalAuthorize("获取分组", "资产分类")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<IEnumerable<AssetGroupDto>>> GetGroupAsync([FromQuery] int? type)
    {
        return await _assetService.GetGroupsAsync(type);
    }

    /// <summary>
    /// 获取资产分类分页
    /// </summary>
    /// <param name="pagingDto">分页参数</param>
    [HttpGet("pages")]
    [LocalAuthorize("获取分页", "管理员")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
    public async Task<ServiceResult<PagedDto<AssetPageDto>>> GetPageAsync([FromQuery] AssetPagingDto pagingDto)
    {
        return await _assetService.GetPageAsync(pagingDto);
    }
}
