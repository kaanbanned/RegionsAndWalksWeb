﻿using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {

        Task<Walk> CreateWalkAsync(Walk region);
        Task<List<Walk>> GetAllWalksAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true,int pageNumber = 1, int pageSize = 1000);
        Task<Walk> GetWalkByIDAsync(Guid id);
        Task<Walk?> UpdateAsync(Guid id, Walk walk);
        Task<Walk?> DeleteAsync(Guid id);

    }
}
