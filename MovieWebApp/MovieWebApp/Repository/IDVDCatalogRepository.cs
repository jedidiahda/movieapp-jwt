using MovieWebApp.Models;

namespace MovieWebApp.Repository
{
    public interface IDVDCatalogRepository
    {
        public Task<Dvdcatalog> Save(Dvdcatalog dvdcatalog);
        public Task<Dvdcatalog> GetById(int id);
        public Task<Dvdcatalog> Update(int dvdCatalogId,Dvdcatalog dvdcatalog);
        public Task UpdateDVDFileUrl(int dvdCatalogId, Dvdcatalog dvdcatalog);

        public Task<IEnumerable<Dvdcatalog>> GetAll(string title, int limit, int pageNumber);
        public Task<IEnumerable<Dvdcatalog>> GetAll(int catalogId);
        public Task Delete(Dvdcatalog dvdcatalog);

        public Task AddDVDCopy(int dvdCatalogId, Dvdcopy dvdcopy);
        public Task<IEnumerable<Dvdcopy>> GetDVDCopies(int dvdCatalogId,string code, int limit,int pageNumber);
        public Task<Dvdcopy> GetDvdcopy(int dvdCopyId);
        public Task<Dvdcopy> GetDvdcopy(string code);
        public Task DeleteDVDCopy(int dvdCopyId);
        public Task<Dvdcopy> UpdateDvdCopy(int id,Dvdcopy dvdcopy);

    }
}
