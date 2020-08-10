using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProcMgt_Reference_Infrastructure.Models;
using ProcMgt_Reference_Core.GenericRepoInter;

namespace ProcMgt_Reference_Infrastructure
{
    public class GenericRepository<T> : IGenericRepo<T> where T : class
    {
        private ReferenceContext _context = null;
        private DbSet<T> table = null;
        public GenericRepository()
        {
            this._context = new ReferenceContext();
            table = _context.Set<T>();
        }
        public GenericRepository(ReferenceContext _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            var AllList = await table.ToListAsync();
            return AllList;
           
            //return table.ToList();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await table.FindAsync(id);
        }
        public async Task<T> InsertAsync(T obj)
        {
            await table.AddAsync(obj);
            return obj;
        }
        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;

        }
        public void Delete(object id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }
        //public void DeleteRange (List<T> range)
        //{
        //    T existingRange = table.Where(obj => range.Contains(obj.));
        //}
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
