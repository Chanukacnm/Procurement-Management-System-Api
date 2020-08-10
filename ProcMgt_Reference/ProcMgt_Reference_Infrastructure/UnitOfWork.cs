using ProcMgt_Reference_Core;
using ProcMgt_Reference_Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Infrastructure
{
    public class UnitOfWork : IUnitOfWorks
    {
        private readonly ReferenceContext _context;

        public UnitOfWork(ReferenceContext context)
        {
            _context = context;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
