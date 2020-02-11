using BuyOnline.Interface;
using BuyOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace BuyOnline.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private BuyOnlineEntities boEntities = new BuyOnlineEntities();

        public Repository(BuyOnlineEntities boEntities)
        {
            this.boEntities = boEntities;
        }

        public TEntity GetById(int id)
        {
            return boEntities.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return boEntities.Set<TEntity>().ToList();
        }

        public void Insert(TEntity entity)
        {
            boEntities.Set<TEntity>().Add(entity);
            boEntities.SaveChanges();
        }

        public void Delete(int id)
        {
            boEntities.Set<TEntity>().Remove(boEntities.Set<TEntity>().Find(id));
            boEntities.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            boEntities.Entry(entity).State = EntityState.Modified;
            
            boEntities.SaveChanges();
        }
    }
}