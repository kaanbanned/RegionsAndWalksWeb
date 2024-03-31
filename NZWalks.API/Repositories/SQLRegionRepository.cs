using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository:IRegionRepository
    {
        private readonly NZWalksDBContext dBContext;

        public SQLRegionRepository(NZWalksDBContext nZWalksDBContext)
        {
            this.dBContext = nZWalksDBContext;
        }
        public async Task<List<Region>> GetAllAsync()
        {
            //Get Data from db -domain models
            return await dBContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIDAsync(Guid id)
        {
            return await dBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Region> CreateRegionAsync(Region region)
        {
            await dBContext.Regions.AddAsync(region);
            await dBContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var regionDomainModel = await dBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomainModel == null) return null;

            dBContext.Regions.Remove(regionDomainModel);

            await dBContext.SaveChangesAsync();
            return regionDomainModel;
        }



        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var regionDomainModel = await dBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomainModel == null) return null;

            regionDomainModel.Code = region.Code;
            regionDomainModel.Name = region.Name;
            regionDomainModel.RegionImageUrl = region.RegionImageUrl;

            await dBContext.SaveChangesAsync();
            return region;
        }
    }
}
