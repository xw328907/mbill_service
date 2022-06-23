﻿namespace mbill.Service.Bill.Category;

public class CategorySvc : ApplicationSvc, ICategorySvc
{
    private readonly ICategoryRepo _categoryRepo;
    private readonly IFileRepo _fileRepo;
    private readonly IMapper _mapper;

    public CategorySvc(ICategoryRepo categoryRepo, IFileRepo fileRepo, IMapper mapper)
    {
        _categoryRepo = categoryRepo;
        _fileRepo = fileRepo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryGroupDto>> GetGroupsAsync(int? type)
    {
        List<CategoryEntity> entities = await _categoryRepo
            .Select
            .Where(c => c.IsDeleted == false)
            .WhereIf(type.HasValue, c => c.Type == type)
            .ToListAsync();
        List<CategoryEntity> parents = entities.FindAll(c => c.ParentId == 0).OrderBy(d => d.Sort).ToList();
        List<CategoryGroupDto> dtos = parents
            .Select(c =>
            {
                var dto = new CategoryGroupDto();
                dto.Name = c.Name;
                dto.Childs = entities
                    .FindAll(d => d.ParentId == c.Id)
                    .Select(d =>
                    {
                        var s = Mapper.Map<CategoryDto>(d);
                        s.IconUrl = _fileRepo.GetFileUrl(s.IconUrl);
                        return s;
                    }).OrderBy(d => d.Sort)
                    .ToList();
                return dto;
            })
            .ToList();
        return dtos;
    }

    public async Task<PagedDto<CategoryPageDto>> GetPageAsync(CategoryPagingDto pagingDto)
    {
        if (pagingDto.CreateStartTime != null && pagingDto.CreateEndTime == null) throw new KnownException("创建时间参数有误", ServiceResultCode.ParameterError);
        var parentIds = new List<string>();
        if (!string.IsNullOrWhiteSpace(pagingDto.ParentIds))
            parentIds = pagingDto.ParentIds.Split(",").ToList();
        pagingDto.Sort = pagingDto.Sort.IsNullOrEmpty() ? "id ASC" : pagingDto.Sort.Replace("-", " ");
        var categories = await _categoryRepo
            .Select
            .WhereIf(!string.IsNullOrWhiteSpace(pagingDto.CategoryName), c => c.Name.Contains(pagingDto.CategoryName))
            .WhereIf(parentIds != null && parentIds.Any(), c => parentIds.Contains(c.ParentId.ToString()))
            .WhereIf(!string.IsNullOrWhiteSpace(pagingDto.Type), c => c.Type.Equals(pagingDto.Type))
            .WhereIf(pagingDto.CreateStartTime != null, c => c.CreateTime >= pagingDto.CreateStartTime && c.CreateTime <= pagingDto.CreateEndTime)
            .OrderBy(pagingDto.Sort)
            .ToPageListAsync(pagingDto, out long totalCount);
        var categoryDtos = categories.Select(c =>
        {
            var dto = Mapper.Map<CategoryPageDto>(c);
            CategoryEntity category = null;
            if (c.ParentId != 0)
                category = _categoryRepo.Get(c.ParentId);
            dto.ParentName = category?.Name;
            dto.TypeName = SystemConst.Switcher.CategoryType(c.Type);
            dto.IconUrl = _fileRepo.GetFileUrl(c.IconUrl);
            return dto;
        }).ToList();

        return new PagedDto<CategoryPageDto>(categoryDtos, totalCount);
    }

    public async Task<IEnumerable<CategoryDto>> GetListAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<CategoryDto> GetAsync(long id)
    {
        var category = await _categoryRepo.GetCategoryAsync(id) ?? throw new KnownException("分类信息不存在或已删除！", ServiceResultCode.NotFound);
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<ServiceResult<List<CategoryDto>>> GetsAsync(int type)
    {
        var categories = await _categoryRepo.Select.Where(c => c.ParentId != 0 && c.Type == type).ToListAsync();
        var dtos = categories.Select(c => _mapper.Map<CategoryDto>(c)).ToList();
        return ServiceResult<List<CategoryDto>>.Successed(dtos);
    }

    public async Task<CategoryDto> GetParentAsync(long id)
    {
        var category = await _categoryRepo.GetCategoryAsync(id) ?? throw new KnownException("分类信息不存在或已删除！", ServiceResultCode.NotFound);
        var categoryParent = await _categoryRepo.GetCategoryAsync(category.ParentId) ?? throw new KnownException("分类父项信息不存在或已删除！", ServiceResultCode.NotFound);
        return _mapper.Map<CategoryDto>(categoryParent);
    }

    public async Task<IEnumerable<CategoryDto>> GetParentsAsync()
    {
        var assets = await _categoryRepo
            .Select
            .Where(a => a.ParentId == 0)
            .OrderBy(a => a.CreateTime)
            .ToListAsync();
        var categoryDtos = assets.Select(a => Mapper.Map<CategoryDto>(a)).ToList();
        return categoryDtos;
    }


    public async Task InsertAsync(CategoryEntity categroy)
    {
        if (!string.IsNullOrEmpty(categroy.Name))
        {
            bool isRepeatName = await _categoryRepo.Select.AnyAsync(r => r.Name == categroy.Name);
            if (isRepeatName)//分类名重复
            {
                throw new KnownException("分类名称重复，请重新输入", ServiceResultCode.RepeatField);
            }
        }
        await _categoryRepo.InsertAsync(categroy);
    }

    public async Task DeleteAsync(long id)
    {
        var exist = await _categoryRepo.Select.AnyAsync(s => s.Id == id && !s.IsDeleted);
        if (!exist) throw new KnownException("没有找到该账单分类信息", ServiceResultCode.NotFound);
        await _categoryRepo.DeleteAsync(id);
    }

    public async Task UpdateAsync(CategoryEntity categroy)
    {
        var exist = await _categoryRepo.Select.AnyAsync(s => s.Id == categroy.Id && !s.IsDeleted);
        if (!exist) throw new KnownException("没有找到该账单分类信息", ServiceResultCode.NotFound);
        Expression<Func<CategoryEntity, object>> ignoreExp = e => new { e.CreateUserId, e.CreateTime };
        await _categoryRepo.UpdateWithIgnoreAsync(categroy, ignoreExp);
    }
}
