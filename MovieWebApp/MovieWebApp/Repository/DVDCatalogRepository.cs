using Microsoft.EntityFrameworkCore;
using MovieWebApp.Models;

namespace MovieWebApp.Repository
{
    public class DVDCatalogRepository : IDVDCatalogRepository
    {
        private readonly MovieDbContext _movieDbContext;

        public DVDCatalogRepository(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }

        public async Task Delete(Dvdcatalog dvdcatalog)
        {
            dvdcatalog.IsDeleted = true;
            await _movieDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Dvdcatalog>> GetAll(string title, int limit, int pageNumber)
        {
            return await _movieDbContext.Dvdcatalogs
                .Where(s => s.IsDeleted == false && (string.IsNullOrEmpty(title) || s.Title.Contains(title)))
                .OrderBy(s=>s.Title)
                .Take(limit)
                .Skip((pageNumber-1) * limit)
                .ToListAsync();
        }

        public async Task<Dvdcatalog> GetById(int id) => await _movieDbContext.Dvdcatalogs.Where(s => s.Id == id).SingleOrDefaultAsync();

        public async Task<Dvdcatalog> Save(Dvdcatalog dvdcatalog)
        {
            _movieDbContext.Dvdcatalogs.Add(dvdcatalog);
            await _movieDbContext.SaveChangesAsync();
            return dvdcatalog;
        }

        public async Task<Dvdcatalog> Update(int dvdCatalogId, Dvdcatalog dvdcatalog)
        {
            var saveDVDCatalog = await _movieDbContext.Dvdcatalogs.Where(s=>s.Id == dvdCatalogId).SingleOrDefaultAsync();
            if(saveDVDCatalog != null)
            {
                saveDVDCatalog.Title = dvdcatalog.Title;
                saveDVDCatalog.Description = dvdcatalog.Description;
                saveDVDCatalog.Genre = dvdcatalog.Genre;
                saveDVDCatalog.StockQty = dvdcatalog.StockQty;
                saveDVDCatalog.ReleasedDate = dvdcatalog.ReleasedDate;
                saveDVDCatalog.Language = dvdcatalog.Language;
                saveDVDCatalog.NoDisk = dvdcatalog.NoDisk;

                await _movieDbContext.SaveChangesAsync();
            }
            return dvdcatalog;
        }

        public async Task AddDVDCopy(int dvdCatalogId, Dvdcopy dvdcopy)
        {
            var dvdCatalog = await GetById(dvdCatalogId);
            if (dvdCatalog != null)
            {
                dvdCatalog.Dvdcopies.Add(dvdcopy);
                await _movieDbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteDVDCopy(int dvdCopyId)
        {
            var dvdCopy = await _movieDbContext.Dvdcopies.Where(s => s.Id == dvdCopyId).SingleOrDefaultAsync();
            if (dvdCopy != null)
            {
                dvdCopy.IsDeleted = true;
                await _movieDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Dvdcopy>> GetDVDCopies(int dvdCatalogId, string code, int limit, int pageNumber)
            => await _movieDbContext.Dvdcopies
                .Where(s => s.DvdcatalogId == dvdCatalogId 
                && (string.IsNullOrEmpty(code) || s.Code.Contains(code)) 
                && s.IsDeleted == false).ToListAsync();
        

        public async Task<Dvdcopy?> GetDvdcopy(string code) => await _movieDbContext.Dvdcopies.Where(s => s.Code == code && s.IsDeleted == false).SingleOrDefaultAsync() ;
        
        public async Task<Dvdcopy?> GetDvdcopy(int dvdCopyId) => await _movieDbContext.Dvdcopies.Where(s => s.Id == dvdCopyId && s.IsDeleted == false).SingleOrDefaultAsync() ;
        

        public async Task<Dvdcopy?> UpdateDvdCopy(int id, Dvdcopy dvdcopy)
        {
            var copy = await GetDvdcopy(id);
            if(copy != null)
            {
                copy.Status = dvdcopy.Status;
                copy.Code = dvdcopy.Code;
                await _movieDbContext.SaveChangesAsync();
                return copy;
            }
            return null;
        }

        public async Task<IEnumerable<Dvdcatalog>> GetAll(int catalogId) => await _movieDbContext.Dvdcatalogs.Where(s => s.Id != catalogId).ToListAsync();
        
    }
}
