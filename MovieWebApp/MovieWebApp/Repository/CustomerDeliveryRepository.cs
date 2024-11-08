﻿using Microsoft.EntityFrameworkCore;
using MovieWebApp.Models;
using MovieWebApp.DTO;

namespace MovieWebApp.Repository
{
    public class CustomerDeliveryRepository : ICustomerDeliveryRepository
    {
        private readonly MovieDbContext _movieDbContext;

        public CustomerDeliveryRepository(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }

        public async Task<List<CustomerReturnDTO>> GetDvdstatuses()
        {
            var list = await (from sts in _movieDbContext.Dvdstatuses
                       join cus in _movieDbContext.CustomerSubscriptions
                       on sts.CustomerSubscriptionId equals cus.Id
                       join c in _movieDbContext.Customers on cus.CustomerId equals c.Id
                       join dvd in _movieDbContext.Dvdcatalogs on sts.DvdcatalogId equals dvd.Id
                       join u in _movieDbContext.Accounts on c.Email equals u.Email
                       where sts.ReturnedDate == null
                       select new { sts, c ,u,dvd}).ToListAsync();
            var dtos = new List<CustomerReturnDTO>();
            foreach (var d in list)
            {
                dtos.Add(new CustomerReturnDTO
                {
                    Code=d.sts.Dvdcode,
                    CustomerId=d.c.Id,
                    CustomerName=d.u.FirstName + " " + d.u.LastName,
                    CustomerSubscriptionId=d.sts.CustomerSubscriptionId,
                    DvdCatalogId=d.sts.DvdcatalogId,
                    Title=d.dvd.Title,
                    DeliveryDate=d.sts.DeliveredDate,
                    ReturnDate=d.sts.ReturnedDate
                });
            }
            return dtos;
        }

        public async Task<List<CustomerDeliveryDTO>> GetValidCustomerDelivery()
        {
            //var list = from c in _movieDbContext.CustomerSubscriptions
            //           join s in _movieDbContext.Subscriptions on c.SubscriptId equals s.Id
            //           join cust in _movieDbContext.Customers on c.CustomerId equals cust.Id
            //           join w in _movieDbContext.Watchlists on cust.Id equals w.CustomerId into Cust_Watchlist
            //           from cw in Cust_Watchlist.DefaultIfEmpty()
            //           join cat in _movieDbContext.Dvdcatalogs on cw.DvdcatalogId equals cat.Id into DVDCat_Watchlist
            //           from dw in DVDCat_Watchlist.DefaultIfEmpty()
            //           join cop in _movieDbContext.Dvdcopies on dw.Id equals cop.DvdcatalogId into Copy_DVDCat
            //           //join cop in _movieDbContext.Dvdcopies on new { dw.Id, false, "A" } equals new { cop.DvdcatalogId, cop.IsDeleted, cop.Status } into Copy_DVDCat
            //           from cdvd in Copy_DVDCat.DefaultIfEmpty()
            //           where cdvd != null && cdvd.IsDeleted == false && cdvd.Status == "A"
            //           && (DateTime.Now >= c.StartDate && DateTime.Now <= c.EndDate)
            //           && s.AtHomeDvd > (from ds in _movieDbContext.Dvdstatuses where ds.CustomerSubscriptionId == c.Id && ds.ReturnedDate != null select ds).Count()
            //           && s.NoDvdperMonth > (from ds in _movieDbContext.Dvdstatuses where ds.CustomerSubscriptionId == c.Id && ds.DeliveredDate != null select ds).Count()
            //           && cw.Rank >= 5
            //           select new { c, cust, cw, dw, cdvd } into x
            //           group x by new
            //           {
            //               x.c.CustomerId,
            //               x.c.SubscriptId,
            //               x.cust.FirstName,
            //               x.cust.LastName,
            //               x.cust.Gender,
            //               x.cdvd.DvdcatalogId,
            //               x.dw.Title,
            //               x.cdvd.Status,
            //               x.cdvd.Code,
            //               x.cw.Rank
            //           } into wholegroup

            //           select wholegroup.ToList();


            var customerDeliveryDtos = await _movieDbContext.CustomerDeliveryDtos
                .FromSql($"EXECUTE dbo.GetValidCustomerDelivery").ToListAsync();

            var dto = new List<CustomerDeliveryDTO>();

            foreach(var s in customerDeliveryDtos)
            {
                dto.Add(new CustomerDeliveryDTO{
                    customerId = s.CustomerId ?? 0,
                    DVDCatalogId = s.DvdcatalogId ?? 0,
                    firstName = s.FirstName ?? "",
                    lastName = s.LastName ?? "",
                    gender = s.Gender ?? "",
                    status = s.Status ?? "",
                    customerSubscriptionId = s.CustomerSubscriptionId ?? 0,
                    title = s.Title ?? "",
                    rank = s.Rank ?? 0,
                    code = s.Code ?? "",
                });
            }

            return dto;
        }

        public async Task ReturnDVDFromCustomer(int susubscriptionId, string code, int dvdCatalogId)
        {
            var tran = _movieDbContext.Database.BeginTransaction();
            try
            {
                var dvdStatus = await _movieDbContext.Dvdstatuses.Where(s => s.CustomerSubscriptionId==susubscriptionId && s.Dvdcode == code && s.DvdcatalogId==dvdCatalogId).SingleOrDefaultAsync();
                if(dvdStatus != null)
                {
                    dvdStatus.ReturnedDate = DateTime.Now;
                }

                var dvdCopy = await _movieDbContext.Dvdcopies.Where(s => s.Code == code && s.IsDeleted == false && s.DvdcatalogId == dvdCatalogId).SingleOrDefaultAsync();
                if(dvdCopy != null)
                {
                    dvdCopy.Status = "A";
                }
                await _movieDbContext.SaveChangesAsync();
                tran.Commit();
            }catch (Exception ex)
            {
                tran.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                tran.Dispose();
            }
        }

        public async Task SendDvdToCustomer(int susubscriptionId, string code,int dvdCatalogId)
        {
            var tran = _movieDbContext.Database.BeginTransaction();
            try
            {
                _movieDbContext.Dvdstatuses.Add(new Dvdstatus
                {
                    DeliveredDate = DateTime.Now,
                    CustomerSubscriptionId = susubscriptionId,
                    Dvdcode = code,
                    DvdcatalogId = dvdCatalogId
                });

                var dvdCopy = await _movieDbContext.Dvdcopies.Where(s => s.DvdcatalogId == dvdCatalogId && s.Code == code && s.IsDeleted==false).SingleOrDefaultAsync();
                if(dvdCopy != null)
                {
                    dvdCopy.Status = "O";//Occupied
                }

                await _movieDbContext.SaveChangesAsync();

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                tran.Dispose();
                throw new Exception(ex.Message);
            }
            finally
            {
                tran.Dispose ();
            }

        }
    }
}
