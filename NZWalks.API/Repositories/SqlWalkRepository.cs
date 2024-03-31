using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Migrations;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SqlWalkRepository : IWalkRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;

        public SqlWalkRepository(NZWalksDBContext nZWalksDBContext)
        {
            this.nZWalksDBContext = nZWalksDBContext;
        }
        public async Task<Walk> CreateWalkAsync(Walk walk)
        {

            await nZWalksDBContext.Walks.AddAsync(walk);
            await nZWalksDBContext.SaveChangesAsync();
            return walk;
        }

        public async Task<List<Walk>> GetAllWalksAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true,int pageNumber = 1, int pageSize = 1000)
        {
            var walks = nZWalksDBContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrEmpty(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
            }

            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            var skipResults = (pageNumber - 1) * pageSize;

            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Walk?> GetWalkByIDAsync(Guid id)
        {
            return await nZWalksDBContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Walk?> UpdateAsync(Guid id, Walk updatedWalk)
        {
            var Walk = await nZWalksDBContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (Walk == null) { return null; }

            Walk.Id = id;
            Walk.DifficultyId = updatedWalk.DifficultyId;
            Walk.Description = updatedWalk.Description;
            Walk.Name = updatedWalk.Name;
            Walk.LengthInKm = updatedWalk.LengthInKm;
            Walk.RegionID = updatedWalk.RegionID;

            await nZWalksDBContext.SaveChangesAsync();

            return Walk;

        }
        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var Walk = await nZWalksDBContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (Walk == null) { return null; }

            nZWalksDBContext.Walks.Remove(Walk);
            await nZWalksDBContext.SaveChangesAsync();

            return Walk;
        }

    }
}
